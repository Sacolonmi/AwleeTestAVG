using UnityEngine;
using System.Collections;

public class Ch0_0 : StorySection
{
    protected override void Start()
    {
        Character e = new Character("E", "e_normal");

        e.Enter(1);
        // test
        e.Say("talk 1");

        e.Say("talk 2");

        e.Say("talk 3");

        e.FaceChange("e_dock");

        e.Say("talk 4");
        
        EndStory();
	}
}
