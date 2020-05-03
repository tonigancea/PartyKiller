using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour {

    GeneralManager manager;
    Dictionary<string, bool> progress = new Dictionary<string, bool>();

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();
        initProgress();
    }

    void initProgress()
    {
        Progress.Add("foundKey", false);
        Progress.Add("haveKey", false);

        Progress.Add("foundLever", false);
        Progress.Add("haveLever", false);

        Progress.Add("canClimb", false);

        Progress.Add("foundBook", false);

        Progress.Add("correctPasscode", false);

        Progress.Add("ultimateEvidence", false);
    }

    public bool CheckInteractionName(string desired)
    {
        return desired == manager.Mechanics.GetItemAtIndex(manager.Mechanics.CurrentItem.children).name;
    }

    public bool IsThisNumberInteraction()
    {
        string interaction = manager.Mechanics.GetItemAtIndex(manager.Mechanics.CurrentItem.children).name;

        switch (interaction)
        {
            case "One":
                return true;
            case "Two":
                return true;
            case "Three":
                return true;
            case "Four":
                return true;
            case "Five":
                return true;
            case "Six":
                return true;
            case "Seven":
                return true;
            case "Eight":
                return true;
            case "Nine":
                return true;
            case "Zero":
                return true;
            default:
                return false;
        }
    }

    public Dictionary<string, bool> Progress
    {
        get
        {
            return progress;
        }

        set
        {
            progress = value;
        }
    }
}
