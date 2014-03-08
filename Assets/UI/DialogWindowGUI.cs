using UnityEngine;
using System.Collections;

public class DialogWindowGUI : MonoBehaviour {

    public GUISkin advSkin;

    public string str = "testtest";

    Rect boxRect;

	// Use this for initialization
	void Start () {
        boxRect = new Rect(-100, -100, 100, 90);

        Hashtable args = new Hashtable()
        {
            {"easeType", iTween.EaseType.easeOutExpo},
            {"time", 1.5f},
            {"delay", 1f},
            {"from", new Rect(50, 500, 700, 200)},
            {"to", new Rect(50, 200, 700, 200)},
            {"onupdate", "UpdateRect"},
        };

        iTween.ValueTo(gameObject, args);
        /*
        args["time"] = 5f;

        iTween.ValueTo(gameObject, args);*/

        
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}

    void UpdateRect(Rect curr)
    {
        boxRect = curr;
    }

    void OnGUI()
    {
        GUI.skin = advSkin;
        //boxRect = iTween.RectUpdate(boxRect, new Rect(10, 10, 100, 90), 1.0f);

        // Make a background box
        GUI.Box(boxRect, "Loader Menu");
        
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 80, 20), "reset"))
        {
            Application.LoadLevel(0);
        }

        // Make the second button.
        if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2"))
        {
            Application.LoadLevel(2);
        }

        GUI.Label(new Rect(100, 100, 300, 300), "TextArea　中文測試 Test long seq ................................................long");

        GUI.Label(new Rect(0, Screen.height - 30, 100, 30), "w: " + Screen.width + " h: " + Screen.height);
    }

}

