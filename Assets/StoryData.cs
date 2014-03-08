using UnityEngine;
using System.Collections;

/// <summary>
/// Stores data about progress and some story state.
/// </summary>
public class StoryData : MonoBehaviour {

    /// <summary>
    /// Returns whether the current progress is present time or the past time.
    /// </summary>
    public static bool IsPresent { get; set; }

    /// <summary>
    /// Current progress string according to IsPresent
    /// </summary>
    public static string Progress
    {
        get { return IsPresent ? CurrentProgress : PastProgress; }
        set
        {
            if (IsPresent) CurrentProgress = value;
            else PastProgress = value;
        }

    }

    /// <summary>
    /// Currnet stroy progress in present time.
    /// This value will be used to load stroy script, so please don't mess it up.
    /// Format: {chapter}_{section}:{line}, ex: Ch0_0:1
    /// </summary>
    public static string CurrentProgress { get; set; }

    /// <summary>
    /// *Currnet* story progress in the past time with the same format with CurrentStory.
    /// It will be undefined when in present time mode. It's only valid in the past time.
    /// </summary>
    public static string PastProgress { get; set; }


    static StoryData()
    {
        IsPresent = true;
        CurrentProgress = "Ch0_0:0";
        PastProgress = null;
    }

    static void LoadData()
    {
        
    }

    static void UpdateProgress()
    {

    }
}
