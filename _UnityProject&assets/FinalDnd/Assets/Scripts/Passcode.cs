using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passcode : MonoBehaviour {

    GeneralManager manager;

    bool PasscodeMode = false;
    Item OldBox;
    int numbersInserted;
    List<string> codeInserted = new List<string>();

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // current item was set before the teleport.
            // setting the index

            manager.Mechanics.Index = 0;
            manager.SoundManager.PlaySpecificSound_Indexed(0);

            //manager.Mechanics.Index = manager.Mechanics.CurrentItem.children.Count - 1;
            //manager.SoundManager.PlaySpecifiedEffect("cling");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && manager.AllowInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                manager.Mechanics.DecrementIndex();
                manager.SoundManager.PlaySpecificSound_Indexed(0);

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                manager.Mechanics.IncrementIndex();
                manager.SoundManager.PlaySpecificSound_Indexed(0);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (PasscodeMode && manager.GameFlow.IsThisNumberInteraction())
                {
                    codeInserted.Add(manager.Mechanics.GetItemAtIndex(manager.Mechanics.CurrentItem.children).name);
                    numbersInserted++;

                    // inform the player what he had just inserted
                    manager.SoundManager.PlaySpecificSound_Indexed(1);

                    if (PasscodeMode && numbersInserted == 4)
                    {
                        PasscodeMode = false;
                        if (CheckIfThePasscodeIsCorrect())
                        {
                            manager.GameFlow.Progress["correctPasscode"] = true;
                        }
                        manager.Mechanics.CurrentItem = OldBox;
                        manager.Mechanics.Index = manager.Mechanics.CurrentItem.children.Count - 1;

                        if (manager.GameFlow.Progress["correctPasscode"])
                        {
                            // congrats, the box is opened
                            manager.SoundManager.PlaySpecificSound_Indexed(2);
                            manager.Importer.AddHiddenItemToTree("Old_Box", "Open");
                        }
                        else
                        {
                            manager.SoundManager.PlayVoice(manager.Effects["sorryPasscode"]);
                        }
                        codeInserted.Clear();
                    }
                }

                else if (manager.GameFlow.CheckInteractionName("Description"))
                {
                    if (manager.GameFlow.Progress["correctPasscode"])
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(2);
                    }
                    else
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(1);

                    }
                }

                else if (!PasscodeMode && manager.GameFlow.CheckInteractionName("Insert_Passcode"))
                {
                    if (manager.GameFlow.Progress["correctPasscode"])
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(2);
                    }

                    else
                    {
                        PasscodeMode = true;
                        numbersInserted = 0;

                        // play passCode tutorial voice
                        manager.SoundManager.PlaySpecificSound_Indexed(1);

                        // remember the item box
                        OldBox = manager.Mechanics.CurrentItem;

                        // the passcode is now the item
                        manager.Mechanics.CurrentItem = manager.Mechanics.CurrentItem.children[manager.Mechanics.Index];
                        manager.Mechanics.Index = manager.Mechanics.CurrentItem.children.Count - 1;
                    }
                }

                else
                {
                    manager.SoundManager.PlaySpecificSound_Indexed(1);
                }

                if (manager.GameFlow.CheckInteractionName("Open") &&
                    !manager.GameFlow.Progress["ultimateEvidence"])
                {
                    manager.GameFlow.Progress["ultimateEvidence"] = true;
                    manager.Importer.AddHiddenItemToTree("Pathos", "Accuse");
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                PasscodeMode = false;

                Vector3 newLocation = manager.FindTeleportSpot(manager.Mechanics.CurrentRoom.name);
                manager.TeleportWithFootsteps(other.gameObject, newLocation);
            }
        }
    }

    bool CheckIfThePasscodeIsCorrect()
    {
        return codeInserted[0] == "Four" &&
            codeInserted[1] == "Five" &&
            codeInserted[2] == "Two" &&
            codeInserted[3] == "Four";
    }
}
