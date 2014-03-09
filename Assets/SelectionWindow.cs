using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class SelectionWindow : MonoBehaviour {

    StoryBehavior _story;
    GUISkin _guiSkin;

    public int SelectionId { get; set; }
    public bool IsActive { get { return _selectionItems != null; } }

    /// <summary>
    /// The upper-most item Rect
    /// </summary>
    Rect SelectionItemSize
    {
        get
        {
            if (IsActive)
            {
                float width = 
                    _selectionItems.Max((str) => { return str.Length; }) * 50;
                float height = 100;

                float x = (_story.VirtualSize.x - width) / 2;
                float y = _story.VirtualSize.y * 0.5f - _selectionItems.Length * height / 2;
                
                return new Rect(x, y, width, height);
            }
            else
            {
                return new Rect();
            }

        }
    }

    string[] _selectionItems;
    Rect[] _selectionRects;

	// Use this for initialization
	void Start () {
        _story = StoryBehavior.Instance;
        _guiSkin = _story.GUISkin;

        _selectionItems = null;
        _selectionRects = null;
        SelectionId = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if (IsActive)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                SelectionId += (Input.GetAxis("Vertical") < 0) ? 1 : -1;

                SelectionId = Mathf.Clamp(SelectionId, 0, _selectionItems.Length - 1);
            }

            if (SelectionId >= _selectionItems.Length || SelectionId < 0)
            {
                _story.HaltForInput = true;
            }
        }
	}

    void OnGUI()
    {
        if (IsActive)
        {
            GUI.matrix = StoryBehavior.GUIMatrix;
            GUI.skin = _guiSkin;

            for (int i = 0; i < _selectionItems.Length; ++i)
            {
                GUI.SetNextControlName("item" + i);
                if (GUI.Button(_selectionRects[i], _selectionItems[i]) && 
                    !_story.Skipping)
                {
                    SelectionId = i;
                    _story.HaltForInput = false;
                }
            }

            GUI.FocusControl("item" + SelectionId);
        }
    }

    public void ShowSelection(string[] items)
    {
        _story.HaltForInput = true;
        _selectionItems = items;
        _selectionRects = new Rect[items.Length];
        SelectionId = -1;

        for(int i = 0; i < _selectionRects.Length; ++i)
        {
            _selectionRects[i] = SelectionItemSize;
            _selectionRects[i].y += _selectionRects[i].height * i;
        }

        if (_story.IsRewinding)
        {
            _selectionItems = null;
        }

        if (_story.Skipping) return;

        _story.IsReady = false;

        // NOTE: code form hell
        for (int i = 0; i < items.Length; ++i)
        {
            float[] pos = { Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity };
            
            pos[i] = _selectionRects[i].x;
            Rect to = new Rect(pos[0], pos[1], pos[2], pos[3]);

            pos[i] = _story.VirtualSize.x;
            Rect form = new Rect(pos[0], pos[1], pos[2], pos[3]);
            _selectionRects[i].x = pos[i];
            

            Hashtable args = new Hashtable()
            {
                {"easeType", iTween.EaseType.easeOutExpo},
                {"time", 0.5f},
                {"delay", 0.0f + i * 0.2f},
                {"from", form},
                {"to", to},
                {"onupdate", "OnUpdateSelection"},
            };

            if (i + 1 == items.Length)
            {
                args["oncomplete"] = "OnEndAnim";
                args["oncompletetarget"] = _story.gameObject;
            }

            iTween.ValueTo(gameObject, args);
        }
    }

    public void ResolveSelection(Action[] results, string selectionName)
    {
        if (StoryData.SelectionTable.ContainsKey(selectionName) && StoryData.IsPresent)
        {
            SelectionId = StoryData.GetNewestSelectionResult(selectionName);
        }

        if (results != null && results.Length > SelectionId && SelectionId >= 0)
        {
            _story.InsertAction(results[SelectionId]);
        }

        StoryData.UpdateSelectionResult(selectionName, SelectionId);

        if (_story.Skipping || !IsActive)
        {
            _selectionItems = null;
            return;
        }

        float delay = 0;

        _story.HaltForInput = true;
        _story.IsReady = false;

        // NOTE: code form hell
        for (int i = 0; i < _selectionItems.Length && !_story.Skipping; ++i)
        {
            float[] pos = { Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity };
            pos[i] = _selectionRects[i].x;

            Rect form = new Rect(pos[0], pos[1], pos[2], pos[3]);

            pos[i] = -_selectionRects[i].width;
            Rect to = new Rect(pos[0], pos[1], pos[2], pos[3]);


            Hashtable args = new Hashtable()
            {
                {"easeType", iTween.EaseType.easeOutExpo},
                {"time", 0.5f},
                {"delay", delay},
                {"from", form},
                {"to", to},
                {"onupdate", "OnUpdateSelection"},
            };

            if (i == SelectionId)
            {
                args["delay"] = 1.0f;
                args["onstart"] = "OnSelectionResult";
                args["oncomplete"] = "OnEndSelection";
            }
            else
            {
                delay += 0.2f;
            }

            iTween.ValueTo(gameObject, args);
        }
    }

    void OnSelectionResult()
    {


    }

    void OnUpdateSelection(Rect rect)
    {
        float[] pos = { rect.x, rect.y, rect.width, rect.height };

        for (int i = 0; i < _selectionRects.Length; ++i)
        {
            if (pos[i] > -_story.VirtualSize.x)
            {
                _selectionRects[i].x = pos[i];
            }
        }
    }

    void OnEndSelection()
    {
        _selectionItems = null;
        _story.HaltForInput = false;
        _story.IsReady = true;
    }
}
