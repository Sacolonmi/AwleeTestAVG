using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    /// <summary>
    /// Selection made by player.
    /// If the option is made in newest progress the flag is set. If it is made before the flag is reset.
    /// Other it is null;
    /// </summary>
    public static Dictionary<string, bool?[]> SelectionTable { get; private set; }

    static StoryData()
    {
        IsPresent = true;
        CurrentProgress = "Ch0_0:0";
        PastProgress = null;

        SelectionTable = new Dictionary<string, bool?[]>();
        //UpdateSelectionResult("ExampleSelection", 1);
    }

    static public  int GetNewestSelectionResult(string selectionName)
    {
        bool?[] decList = SelectionTable[selectionName];
        for (int i = 0; i < decList.Length; ++i)
        {
            if (decList[i] != null && decList[i].Value)
            {
                return i;
            }
        }

        Debug.LogError("Currupted selection data");
        return -1;
    }

    static public void UpdateSelectionResult(string selectionName, int decisition)
    {
        if (!SelectionTable.ContainsKey(selectionName))
        {
            SelectionTable.Add(selectionName, new bool?[4]);
        }

        bool?[] decList = SelectionTable[selectionName];
        for (int i = 0; i < decList.Length; ++i)
        {
            if (i == decisition)
            {
                decList[i] = true;
            }
            else if (decList[i] != null && decList[i].Value)
            {
                decList[i] = false;
            }
        }
    }

    static void LoadData()
    {
        
    }

    static void UpdateProgress()
    {

    }
}

public struct StoryModifier
{
    public enum Type
    {
        ChangeStatus,
        GetItem,
        GetMoney
    }

    public Type ModifierType;
    public string Target;
    public int Value;
}