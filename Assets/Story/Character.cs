using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {


    Texture2D _face;

    string _defaultFace;
    Rect _rect;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.matrix = StoryBehavior.GUIMatrix;


        GUI.DrawTexture(_rect, _face);
    }

    public void Initialize(string face, int pos)
    {
        _defaultFace = face;

        ChangeFace(face);

        int oriPos = (pos > 2) ? 6 : -2;

        _rect = StoryBehavior.GetPostionRect(oriPos);

        MoveTo(pos, 1f);
    }

    public void ChangeFace(string face)
    {
        Texture2D faceTex = Resources.Load<Texture2D>("character/" + face);

        if (faceTex == null)
        {
            faceTex = Resources.Load<Texture2D>("character/" + _defaultFace);
        }

        _face = faceTex;
        
    }

    public void MoveTo(int pos, float speed = 0.5f, iTween.EaseType easeType = iTween.EaseType.easeOutSine)
    {
        iTween.Stop(gameObject);

        float st = _rect.x;
        float dest = StoryBehavior.GetPostionX(pos);
        float t = Mathf.Abs(st - dest) / StoryBehavior.Instance.VirtualSize.y / speed;

        iTween.ValueTo(
            gameObject, 
            new Hashtable()
            {
                {"easeType", easeType},
                {"time", t},
                {"from", st},
                {"to", dest},
                {"onupdate", "OnRectXUpdate"},
            });
    }

    public void Leave(float speed = 1.0f, iTween.EaseType easeType = iTween.EaseType.easeOutSine)
    {
        iTween.Stop(gameObject);

        float width = StoryBehavior.Instance.VirtualSize.y;
        float st = _rect.x;
        float dest = _rect.x > (width / 2.0f) ? width * 2 : -width;

        float t = Mathf.Abs(st - dest) / width / speed;

        iTween.ValueTo(
            gameObject,
            new Hashtable()
            {
                {"easeType", easeType},
                {"time", t},
                {"from", st},
                {"to", dest},
                {"onupdate", "OnRectXUpdate"},
                {"oncomplete", "OnEndLeave"},
            });

    }

    void OnRectXUpdate(float x)
    {
        _rect.x = x;
    }

    void OnEndLeave()
    {
        Destroy(gameObject);
    }
}
