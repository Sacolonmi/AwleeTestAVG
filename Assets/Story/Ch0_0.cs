using UnityEngine;
using System.Collections;
using System;

namespace Story
{

    public class Ch0_0 : StorySection
    {
        public override void Story()
        {
            Character e = new Character("E", this);

            e.Enter(2, "e_normal");

            e.Say("talk 1");

            e.MoveTo(3);

            e.Say("talk 2");

            e.MoveTo(0);

            e.Say("talk 3");

            e.MoveTo(1, 0.2f, iTween.EaseType.spring);

            e.Face("e_dock");

            e.Say("talk 4");

            SendDialog("", "selection test");

            ShowSelection(
                new string[] { "選項一", "選項二", "字數很多很多很多很多很多很多很多的選項三", "極限的選項四" },
                "ExampleSelection",
                new Action[] { 
                    () => {
                        e.Say("1");
                    },
                    () => {
                        e.Say("2");
                    }
                }
                );

            e.Say("end talk");

            e.Say("test");

            e.Say("test2");
        }
    }
}