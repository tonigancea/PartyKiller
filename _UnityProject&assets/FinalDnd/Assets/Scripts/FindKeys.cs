using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindKeys : MonoBehaviour {

    GeneralManager manager;

    bool deleteGetKey = false;

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
                
                if (manager.GameFlow.CheckInteractionName("Inspect") &&
                    !manager.GameFlow.Progress["foundKey"])
                {
                    manager.GameFlow.Progress["foundKey"] = true;
                    manager.Importer.AddHiddenItemToTree("Desk", "Get_Key");
                }

                if (manager.GameFlow.CheckInteractionName("Get_Key") &&
                    !manager.GameFlow.Progress["haveKey"])
                {
                    manager.GameFlow.Progress["haveKey"] = true;
                    manager.Importer.AddHiddenItemToTree("Door3-1", "Open");
                }
                

                if (manager.GameFlow.CheckInteractionName("Inspect"))
                {
                    if (manager.GameFlow.Progress["haveKey"])
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

                if (manager.GameFlow.Progress["foundKey"] &&
                    manager.GameFlow.Progress["haveKey"])
                {

                    if (deleteGetKey == false)
                    {
                        manager.Root.RemoveItem("Desk", "Get_Key");
                        manager.Mechanics.Index--;
                        deleteGetKey = true;
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
