using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contorls currnet story progressing, dialog and simple animation.
/// </summary>
public class StoryBehavior : MonoBehaviour {

    /// <summary>
    /// Singleton instance of StoryBehavior
    /// </summary>
    static public StoryBehavior Instance { get; set; }

    public List<Action> ActionList { get; set; }
    public GUISkin GUISkin;


    IEnumerator<Action> _currAction;
    bool _ready;
    bool _haltForInput;
    int _lineId;
    string _storyScript;

    float virtualWidth = 1920.0f;
    float virtualHeight = 1080.0f;
    Matrix4x4 myGUIMatrix;

    Rect _dialogBox;
    string _dialogContent;
    string _dialogSpeaker;
    float _dialogAlpha;

    Dictionary<string, Character> _characterMap;

    #region # Mono behavior

    void Awake()
    {
        Instance = this;

        ActionList = new List<Action>();

        _characterMap = new Dictionary<string, Character>();

        _dialogSpeaker = "";
        _dialogContent = "";
    }

	// Use this for initialization
	void Start () {

        UnpackProgress(StoryData.Progress);

        Component storySection = this.gameObject.AddComponent(_storyScript);

        GUIStart();
	}
	
	// Update is called once per frame
	void Update () {

        if (_ready)
        {
            ProcessAction();

            ProcessInput();
        }
	}

    void ProcessInput()
    {
        if (!_haltForInput) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _haltForInput = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIEnd(); 
        }
    }

    void ProcessAction()
    {
        if (_currAction == null) return;

        while (!_haltForInput && _currAction.Current != null)
        {
            _currAction.Current.Invoke();

            if (!_currAction.MoveNext())
            {
                // end of action
            }
        }

        
    }

    #endregion


    #region # GUI control

    void GUIStart()
    {
        _ready = false;

        myGUIMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            new Vector3(Screen.width / virtualWidth,
                Screen.height / virtualHeight,
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
        
        

        float x = virtualWidth * 0.1f;
        float y = virtualHeight * 0.75f;
        float width = virtualWidth * 0.8f;
        float height = virtualHeight * 0.2f;

        _dialogBox = new Rect(x, y + 500, width, height);

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeOutExpo},
            {"time", 1.0f},
            {"delay", 0.5f},
            {"from", _dialogBox},
            {"to", new Rect(x, y, width, height)},
            {"onupdate", "OnUpdateDialogWindow"},
            {"oncomplete", "OnEndAnim"},
        };

        iTween.ValueTo(gameObject, args);
    }

    void GUIEnd()
    {
        _ready = false;

        Hashtable args;

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInExpo},
            {"time", 0.25f},
            {"from", 1},
            {"to", 0},
            {"onupdate", "OnUpdateDialogAlpha"},
        };

        iTween.ValueTo(gameObject, args);


        float x = virtualWidth * 0.1f;
        float y = virtualHeight * 0.75f;
        float width = virtualWidth * 0.8f;
        float height = virtualHeight * 0.2f;

        _dialogBox = new Rect(x, y, width, height);

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInExpo},
            {"time", 0.5f},
            {"from", _dialogBox},
            {"to", new Rect(x, y + 500, width, height)},
            {"onupdate", "OnUpdateDialogWindow"},
            
        };

        iTween.ValueTo(gameObject, args);

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInCubic},
            {"time", 1f},
            {"delay", 0.5f},
            {"amount", 1f},
            {"oncomplete", "OnEndLeave"},
            {"oncompletetarget", gameObject},
        };

        iTween.CameraFadeTo(args);
        
    }

    void OnGUI()
    {
        GUI.matrix = myGUIMatrix;
        GUI.skin = GUISkin;

        foreach (Character character in _characterMap.Values)
        {
            float x = character.Postion;
            float y = virtualHeight - 900;
            float width = 600;
            float height = 900;
            Rect rect = new Rect(x, y, width, height);

            GUI.DrawTexture(rect, character.FaceTexture);
        }

        GUIStyle st;
        st = GUISkin.box;
        Color textClr = st.normal.textColor;
        textClr.a = _dialogAlpha;
        st.normal.textColor = textClr;
        
        GUI.Box(_dialogBox, _dialogSpeaker, st);

        st = GUISkin.label;
        textClr = st.normal.textColor;
        textClr.a = _dialogAlpha;
        st.normal.textColor = textClr;

        GUI.Label(_dialogBox, _dialogContent, st);

        GUI.Label(new Rect(0, virtualHeight - 30, 100, 30), "w: " + virtualWidth + " h: " + virtualHeight);
    }

    void OnUpdateDialogWindow(Rect rect)
    {
        _dialogBox = rect;
    }

    void OnUpdateDialogAlpha(float alpha)
    {
        _dialogAlpha = alpha;
    }

    void OnEndAnim()
    {
        _ready = true;
    }

    void OnEndLeave()
    {
        print("!");
        Application.LoadLevel(0);
    }

    #endregion


    #region # Static method for stroy section handling

    static public void AddCharacter(string name, string face, int pos)
    {
        Character newChar = new Character(name, face);

        Instance._characterMap.Add(name, newChar);
        ChangeCharacterFace(name, face);
    }

    static public void RemoveCharacter(string name)
    {

    }

    static public void MoveCharacter(string name, int newPos)
    {

    }

    static public void ChangeCharacterFace(string name, string face)
    {
        Character character = Instance._characterMap[name];
        Texture2D faceTex = Resources.Load<Texture2D>("character/" + face);
        print(faceTex);
        character.Face = face;
        character.FaceTexture = faceTex;
    }

    static public void SendDialog(string name, string dialog)
    {
        print(dialog);
        Instance._dialogContent = dialog;
        Instance._dialogSpeaker = name;
        Instance._haltForInput = true;

        Hashtable args;

        if (Instance._lineId != 0)
        {
            args = new Hashtable()
            {
                {"easeType", iTween.EaseType.easeOutExpo},
                {"time", 0.25f},
                {"from", 1},
                {"to", 0},
                {"onupdate", "OnUpdateDialogAlpha"},
            };

            iTween.ValueTo(Instance.gameObject, args);
        }

        args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeOutExpo},
            {"time", 0.25f},
            {"delay", 0.25f},
            {"from", 0},
            {"to", 1f},
            {"onupdate", "OnUpdateDialogAlpha"},
        };

        iTween.ValueTo(Instance.gameObject, args);

        ++Instance._lineId;
    }

    static public void SetBackground(string bg)
    {

    }

    static public void SetBGM(string bgm)
    {

    }

    static public void PlayAudio(string audio)
    {

    }

    static public void Init()
    {
        Instance.ActionList.Clear();

        Instance._haltForInput = false;

        Instance._lineId = 0;
    }

    static public void EndInit()
    {
        Instance._currAction = Instance.ActionList.GetEnumerator();
        Instance._currAction.MoveNext();
    }

    static public void AddAction(Action action)
    {
        Instance.ActionList.Add(action);
    }

    #endregion


    #region # Progress string processing

    /// <summary>
    /// Unpack incoming progress string.
    /// </summary>
    /// <param name="progress">Progress string form StoryData</param>
    void UnpackProgress(string progress)
    {
        string[] tokens = progress.Split(':');
        
        _storyScript = tokens[0];

        if(!Int32.TryParse(tokens[1], out _lineId))
        {
            _lineId = 0;
        }
    }

    /// <summary>
    /// Pack up current data into progress string
    /// </summary>
    /// <returns>Progress string for StroyData</returns>
    String PackProgress()
    {
        StringBuilder sb = new StringBuilder(_storyScript);
        sb.AppendFormat(":{0}", _lineId);

        return sb.ToString();
    }

    #endregion
}
