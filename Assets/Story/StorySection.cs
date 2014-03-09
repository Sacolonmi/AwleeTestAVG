using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace Story
{
    /// <summary>
    /// Base class of all story section.
    /// </summary>
    public abstract class StorySection : MonoBehaviour
    {
        public StoryBehavior StoryBehavior { get; set; }

        /// <summary>
        /// This property is to prevent unnecessary operation when 
        /// this object is just used for section reference.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Modidfier list for player refernece.
        /// </summary>
        public StoryModifier[] SectionModidfiers { get; set; }

        public class SelectionItem
        {
            public string Context;
            public Action Result;
            public StoryModifier Modifier;
        }


        void Awake()
        {
            StoryBehavior = StoryBehavior.Instance;
            StoryBehavior.Init();

            IsStatic = false;
            print("ss awake");
        }

        // Use this for initialization
        void Start()
        {
            Story();

            if (!IsStatic)
            {
                StoryBehavior.EndInit();
            }else
            {
                StoryBehavior.ActionList.Clear();
            }

            print("ss start");
        }

        // Update is called once per frame
        void Update()
        {
            // do nothing for now
        }

        /// <summary>
        /// Defines the body of this story section.
        /// </summary>
        public abstract void Story();

        /// <summary>
        /// To next section, chpater or bettle.
        /// </summary>
        /// <param name="target">Next level name.</param>
        protected void ToNext(string target)
        {
            StoryBehavior.AddAction(
                () => StoryBehavior.ToNextScene(target));
        }

        /// <summary>
        /// Set current background.
        /// </summary>
        /// <param name="bg">Asset name</param>
        protected void SetBG(string bg)
        {
            StoryBehavior.AddAction(
                () => StoryBehavior.SetBackground(bg));
        }

        /// <summary>
        /// Send a dialog.
        /// </summary>
        /// <param name="name">Speaker's name</param>
        /// <param name="dialog">Contend</param>
        protected void SendDialog(string name, string dialog)
        {
            StoryBehavior.AddAction(
                () => StoryBehavior.SendDialog(name, dialog));
        }

        /// <summary>
        /// Define a selection with items and their results.
        /// </summary>
        /// <param name="items">Context of items</param>
        /// If not exist it will be regrad as doing nothing</param>
        /// <param name="selectionName">Name in StoryData. 
        /// This is only used to create branch across mutiple sections.
        /// </param>
        /// <param name="results">Corresponding results.
        protected void ShowSelection(string[] items, string selectionName, Action[] results = null)
        {
            // TODO: add start and end selection action
            // TODO: create section modifier list

            StoryBehavior.AddAction(
                () => StoryBehavior.StartSelection(items));

            StoryBehavior.AddAction(
                () => StoryBehavior.ResolveSelection(results, selectionName));
        }
    }

    class Character
    {
        StoryBehavior _story;
        string _name;


        public Character(string name, StorySection section)
        {
            _name = name;
            _story = section.StoryBehavior;
        }


        /// <summary>
        /// Enter the scene.
        /// </summary>
        /// <param name="pos">Postion on scene. Range form 0 ~ 4.</param>
        /// <param name="face">Default face</param>
        public void Enter(int pos, string face)
        {
            _story.AddAction(
                () => _story.AddCharacter(this._name, face, pos));
        }

        /// <summary>
        /// Say something.
        /// </summary>
        /// <param name="dialog">Content</param>
        public void Say(string dialog)
        {
            _story.AddAction(
                () => _story.SendDialog(this._name, dialog));
        }

        /// <summary>
        /// Change face.
        /// </summary>
        /// <param name="faceId">Asset of the face.</param>
        public void Face(string faceId)
        {

            _story.AddAction(
                () => _story.ChangeCharacterFace(this._name, faceId));
        }

        /// <summary>
        /// Move to postion
        /// </summary>
        /// <param name="p">Postion to move. Range form 0 ~ 4.</param>
        public void MoveTo(int p, float speed = 0.5f, iTween.EaseType easeType = iTween.EaseType.easeOutSine)
        {
            _story.AddAction(
                () => _story.MoveCharacter(this._name, p, speed, easeType));
        }

        /// <summary>
        /// Leave the scene.
        /// </summary>
        public void Leave()
        {
            _story.AddAction(
                () => _story.RemoveCharacter(this._name));
        }

        
    }

}