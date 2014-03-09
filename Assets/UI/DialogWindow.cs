using UnityEngine;
using System.Collections;

public class DialogWindow : MonoBehaviour {

    public Rect DialogLayout;

    public Rect DialogSize
    {
        get
        {
            float x = _story.VirtualSize.x * DialogLayout.x;
            float y = _story.VirtualSize.y * DialogLayout.y;
            float width = _story.VirtualSize.x * DialogLayout.width;
            float height = _story.VirtualSize.y * DialogLayout.height;

            return new Rect(x, y, width, height);
        }
    }

    StoryBehavior _story;
    GUISkin _guiSkin;

    Rect _dialogBox;
    string _dialogContent;
    string _dialogSpeaker;
    float _dialogAlpha;

	// Use this for initialization
	void Start () {
        _story = StoryBehavior.Instance;
        _guiSkin = _story.GUISkin;

        _dialogContent = "";
        _dialogSpeaker = "";

        _dialogBox = DialogSize;

        _dialogBox.y += 500;

        iTween.ValueTo(gameObject, new Hashtable()
        {
            {"easeType", iTween.EaseType.easeOutExpo},
            {"time", 1.0f},
            {"delay", 0.5f},
            {"from", _dialogBox},
            {"to", DialogSize},
            {"onupdate", "OnUpdateDialogWindow"},
            {"oncomplete", "OnEndAnim"},
            {"oncompletetarget", _story.gameObject},
        }
        );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.matrix = StoryBehavior.GUIMatrix;
        GUI.skin = _guiSkin;

        GUIStyle st;
        st = _guiSkin.box;
        Color textClr = st.normal.textColor;
        textClr.a = _dialogAlpha;
        st.normal.textColor = textClr;

        GUI.Box(_dialogBox, _dialogSpeaker, st);

        st = _guiSkin.label;
        textClr = st.normal.textColor;
        textClr.a = _dialogAlpha;
        st.normal.textColor = textClr;
        
        GUI.Label(_dialogBox, _dialogContent, st);
    }

    void OnUpdateDialogWindow(Rect rect)
    {
        _dialogBox = rect;
    }

    void OnUpdateDialogAlpha(float alpha)
    {
        _dialogAlpha = alpha;
    }

    public void ShowDialog(string name, string dialog)
    {
        _dialogContent = dialog;
        _dialogSpeaker = name;


        if (!_story.Skipping)
        {
            Hashtable args;

            args = new Hashtable()
            {
                {"easeType", iTween.EaseType.easeOutExpo},
                {"time", 0.25f},
                {"from", 0},
                {"to", 1f},
                {"onupdate", "OnUpdateDialogAlpha"},
            };

            iTween.ValueTo(gameObject, args);
        }
    }

    public void Leave()
    {
        iTween.ValueTo(gameObject, new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInExpo},
            {"time", 0.25f},
            {"from", 1},
            {"to", 0},
            {"onupdate", "OnUpdateDialogAlpha"},
        });


        Rect dest = DialogSize;

        dest.y += 500;

        _dialogBox = DialogSize;

        iTween.ValueTo(gameObject, new Hashtable()
        {
            {"easeType", iTween.EaseType.easeInExpo},
            {"time", 0.5f},
            {"from", _dialogBox},
            {"to", dest},
            {"onupdate", "OnUpdateDialogWindow"},
        });
    }
}
