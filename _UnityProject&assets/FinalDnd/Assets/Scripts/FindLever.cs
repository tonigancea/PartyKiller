using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLever : MonoBehaviour {

    GeneralManager manager;

    bool deletePickLever = false;

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


            //print(manager.Mechanics.Index);
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
                if (manager.GameFlow.CheckInteractionName("Open") &&
                    !manager.GameFlow.Progress["foundLever"])
                {
                    manager.GameFlow.Progress["foundLever"] = true;
                    manager.Importer.AddHiddenItemToTree("Chest", "Pick");
                }

                if (manager.GameFlow.CheckInteractionName("Pick") &&
                    !manager.GameFlow.Progress["haveLever"])
                {
                    manager.GameFlow.Progress["haveLever"] = true;
                    manager.Importer.AddHiddenItemToTree("Door4-1", "Use_Lever");
                }

                if (manager.GameFlow.CheckInteractionName("Open"))
                {
                    if (manager.GameFlow.Progress["haveLever"])
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(2);
                    }

                    else
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(1);
                    }
                }

                else
                {
                    manager.SoundManager.PlaySpecificSound_Indexed(1);
                }
                
                if (manager.GameFlow.Progress["foundLever"] &&
                    manager.GameFlow.Progress["haveLever"])
                {
                    if (deletePickLever == false)
                    {
                        manager.Root.RemoveItem("Chest", "Pick");
                        //manager.Mechanics.Index--;
                        deletePickLever = true;
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Vector3 newLocation = manager.FindTeleportSpot(manager.Mechanics.CurrentRoom.name);
                manager.TeleportWithFootsteps(other.gameObject, newLocation);
            }
        }
    }
}
