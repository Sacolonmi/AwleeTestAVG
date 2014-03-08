using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Base class of all story section.
/// </summary>
public abstract class StorySection : MonoBehaviour {

    void Awake()
    {
        StoryBehavior.Init();
    }

	// Use this for initialization
    protected abstract void Start();
	
	// Update is called once per frame
	void Update () {
	    // do nothing for now
	}

    /// <summary>
    /// To next section, chpater or bettle.
    /// </summary>
    /// <param name="target">Next level name.</param>
    protected void ToNext(string target)
    {

    }

    /// <summary>
    /// Set current background.
    /// </summary>
    /// <param name="bg">Asset name</param>
    protected void SetBG(string bg)
    {

    }

    /// <summary>
    /// End stroy definition. This is essential for initialization.
    /// </summary>
    protected void EndStory()
    {
        StoryBehavior.EndInit();
    }

}

class Character
{

    public Character(string name, string defaultFace)
    {
        Name = name;
        Face = defaultFace;
    }

    public string Name { get; set; }
    public string Face { get; set; }
    public Texture FaceTexture { get; set; }
    public int Postion { get; set; }

    /// <summary>
    /// Enter the scene.
    /// </summary>
    /// <param name="pos">Postion on scene</param>
    public void Enter(int pos)
    {
        string face = Face;

        this.Postion = pos;
        StoryBehavior.AddAction(
            () => StoryBehavior.AddCharacter(this.Name, face, pos));
    }

    /// <summary>
    /// Say something.
    /// </summary>
    /// <param name="dialog">Content</param>
    public void Say(string dialog)
    {
        StoryBehavior.AddAction(
            () => StoryBehavior.SendDialog(this.Name, dialog));
    }

    /// <summary>
    /// Change face.
    /// </summary>
    /// <param name="faceId">Asset of the face.</param>
    public void FaceChange(string faceId)
    {
        Face = faceId;

        StoryBehavior.AddAction(
            () => StoryBehavior.ChangeCharacterFace(this.Name, faceId));
    }

    /// <summary>
    /// Leave the scene.
    /// </summary>
    public void Leave()
    {

    }
}