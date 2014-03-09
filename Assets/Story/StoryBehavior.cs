using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contorls currnet story progressing, dialog and simple animation.
/// </summary>
public class StoryBehavior : MonoBehaviour
{

    /// <summary>
    /// Singleton instance of StoryBehavior
    /// </summary>
    static public StoryBehavior Instance { get; set; }

    static public Matrix4x4 GUIMatrix { get; private set; }

    /// <summary>
    /// Is there any animation playing?
    /// </summary>
    public bool IsReady { get; set; }

    /// <summary>
    /// Should the story pause for input?
    /// </summary>
    public bool HaltForInput { get; set; }

    /// <summary>
    /// If animation should play?
    /// </summary>
    public bool Skipping { get; set; }

    /// <summary>
    /// If current stroy is rewinding to load position.
    /// </summary>
    public bool IsRewinding { get; set; }

    public List<Action> ActionList { get; set; }

    public GUISkin GUISkin;
    public Vector2 VirtualSize;

    Dictionary<string, Character> _characterMap;

    int _stActionId;
    int _currActionID;
    string _storyScript;

    DialogWindow _dialogWindow;

    SelectionWindow _selectionWindow;
    

    #region # Mono behavior

    void Awake()
    {
        
        Instance = this;

        ActionList = new List<Action>();

        _characterMap = new Dictionary<string, Character>();
    }

    // Use this for initialization
    void Start()
    {

        UnpackProgress(StoryData.Progress);

        Story.StorySection storySection = this.gameObject.AddComponent(_storyScript) as Story.StorySection;

        print("sb start");

        _dialogWindow = this.gameObject.GetComponent<DialogWindow>() as DialogWindow;

        _selectionWindow = this.gameObject.GetComponent<SelectionWindow>() as SelectionWindow;

        GUIStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady)
        {
            ProcessAction();

            ProcessInput();
        }
    }

    void ProcessInput()
    {
        if (!HaltForInput) return;

        if (Input.GetButtonDown("Skip"))
        {
            Skipping = true;
        }
        else if (Input.GetButtonUp("Skip"))
        {
            Skipping = false;
        }

        HaltForInput = !Input.GetButtonDown("Confirm") && !Skipping;

        /*
        if (_selectionItems != null)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                _selectionId += (Input.GetAxis("Vertical") < 0) ? 1 : -1;

                _selectionId = Mathf.Clamp(_selectionId, 0, _selectionItems.Length - 1);
            }

            if (_selectionId >= _selectionItems.Length || _selectionId < 0)
            {
                _haltForInput = true;
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // FIXME: test code for leave
            GUIEnd();
        }
    }

    void ProcessAction()
    {
        while (!HaltForInput && IsReady)
        {
            ActionList[_currActionID].Invoke();

            ++_currActionID;

            if (_currActionID >= ActionList.Count)
            {
                _currActionID = 0; break;
            }
        }
    }

    #endregion


    #region # GUI control

    void GUIStart()
    {
        IsReady = false;

        GUIMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            new Vector3(Screen.width / VirtualSize.x,
                Screen.height / VirtualSize.y,
                1.0f));

        Hashtable args;

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeOutExpo},
            {"time", 1f},
            {"amount", 1f},
        };

        GameObject camera = iTween.CameraFadeAdd();
        iTween.CameraFadeFrom(args);

    }

    void GUIEnd()
    {
        IsReady = false;

        _dialogWindow.Leave();


        iTween.CameraFadeTo(new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInCubic},
            {"time", 1f},
            {"delay", 0.25f},
            {"amount", 1f},
            {"oncomplete", "OnEndLeave"},
            {"oncompletetarget", gameObject},
        });

        foreach (Character character in _characterMap.Values)
        {
            character.Leave();
        }
    }

    void OnGUI()
    {
        GUI.matrix = GUIMatrix;
        GUI.skin = GUISkin;
        /*
        if (_selectionItems != null)
        {
            for (int i = 0; i < _selectionItems.Length; ++i)
            {
                GUI.SetNextControlName("item" + i);
                if (GUI.Button(_selectionRects[i], _selectionItems[i]) && !_skip)
                {
                    _selectionId = i;
                    _haltForInput = false;
                }
            }

            GUI.FocusControl("item" + _selectionId);
        }*/

        GUI.Label(new Rect(0, VirtualSize.y - 30, 300, 30), "w: " + VirtualSize.x + " h: " + VirtualSize.y);
    }

    void OnEndAnim()
    {
        IsReady = true;
    }

    void OnEndLeave()
    {
        Application.LoadLevel(0);
    }

    #endregion


    #region # Methods for stroy section handling

    public void AddCharacter(string name, string face, int pos)
    {
        if (!_characterMap.ContainsKey(name))
        {
            GameObject newCharObj =
                Instantiate(Resources.Load<GameObject>("character/Character")) as GameObject;

            Character newChar = newCharObj.GetComponent<Character>();
            _characterMap.Add(name, newChar);

            newChar.Initialize(face, pos);
        }
    }

    public void RemoveCharacter(string name)
    {
        var charMap = _characterMap;
        if (charMap.ContainsKey(name))
        {
            Character character = charMap[name];

            character.Leave();

            charMap.Remove(name);
        }
    }

    public void MoveCharacter(string name, int newPos, float speed, iTween.EaseType easeType)
    {
        Character character = _characterMap[name];

        character.MoveTo(newPos, speed, easeType);

        print(PackProgress() + " : " + name + newPos);
    }

    public void ChangeCharacterFace(string name, string face)
    {
        Character character = _characterMap[name];

        character.ChangeFace(face);

        print(PackProgress() + " : " + face);
    }

    public void SendDialog(string name, string dialog)
    {
        HaltForInput = true;
        _dialogWindow.ShowDialog(name, dialog);

        print(PackProgress() + " : " + dialog);
    }

    public void StartSelection(string[] items)
    {
        // TODO: save state and record
        // TODO: show selection menu

        _selectionWindow.ShowSelection(items);

        print(PackProgress() + " : " + items);
    }

    public void ResolveSelection(Action[] results, string selectionName)
    {

        // TODO: insert results after current iterator
        // TODO: save state
        // TODO: decide if this is to pass

        _selectionWindow.ResolveSelection(results, selectionName);

        print(PackProgress() + " : " + selectionName);
    }

    public void SetBackground(string bg)
    {

    }

    public void SetBGM(string bgm)
    {

    }

    public void PlayAudio(string audio)
    {

    }

    public void ToNextScene(string scene)
    {

    }

    public void Init()
    {
        ActionList.Clear();

        HaltForInput = false;
    }

    public void EndInit()
    {
        _currActionID = 0;

        if (_stActionId != 0)
        {
            IsRewinding = true;
            
            for (_currActionID = 0; _currActionID < _stActionId; ++_currActionID )
            {
                ActionList[_currActionID].Invoke();
            }

            IsRewinding = false;
            HaltForInput = false;
        }
    }

    public void AddAction(Action action)
    {
        ActionList.Add(action);
    }

    public void InsertAction(Action actionPack)
    {
        List<Action> tempActionList = ActionList;
        ActionList = new List<Action>();

        actionPack.Invoke();

        tempActionList.InsertRange(_currActionID + 1, ActionList);
        ActionList = tempActionList;
    }

    #endregion

    #region Static position methods

    static public Rect GetPostionRect(int pos)
    {
        float x = GetPostionX(pos);
        float y = Instance.VirtualSize.y - 900;
        float width = 600;
        float height = 900;
        return new Rect(x, y, width, height);
    }

    static public float GetPostionX(int pos)
    {
        return pos * (Instance.VirtualSize.x) / 6.0f;
    }

    #endregion

    #region # Progress string processing

    /// <summary>
    /// Unpack incoming progress string.
    /// </summary>
    /// <param name="progress">Progress string form StoryData</param>
    public void UnpackProgress(string progress)
    {
        string[] tokens = progress.Split(':');

        _storyScript = tokens[0];

        if (!Int32.TryParse(tokens[1], out _stActionId))
        {
            _stActionId = 0;
        }
    }

    /// <summary>
    /// Pack up current data into progress string
    /// </summary>
    /// <returns>Progress string for StroyData</returns>
    public String PackProgress()
    {
        StringBuilder sb = new StringBuilder(_storyScript);
        sb.AppendFormat(":{0}", _currActionID);

        return sb.ToString();
    }

    #endregion
}
