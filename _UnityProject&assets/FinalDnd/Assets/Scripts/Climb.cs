using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour {

    GeneralManager manager;
    AudioClip voice;
    AudioClip effect;

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
                if (manager.GameFlow.CheckInteractionName("Use_Lever") &&
                    !manager.GameFlow.Progress["canClimb"])
                {
                    manager.GameFlow.Progress["canClimb"] = true;
                    manager.Importer.AddHiddenItemToTree("Door4-1", "Climb");
                }

                if (manager.GameFlow.CheckInteractionName("Check") ||
                    manager.GameFlow.CheckInteractionName("Inspect"))
                {
                    if (!manager.GameFlow.Progress["canClimb"])
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(1);
                    }
                    else
                    {
                        manager.SoundManager.PlaySpecificSound_Indexed(2);
                    }
                }

                else
                {
                    manager.SoundManager.PlaySpecificSound_Indexed(1);
                }

                if (manager.GameFlow.CheckInteractionName("Climb") ||
                    manager.GameFlow.CheckInteractionName("Go_Down"))
                {
                    StartCoroutine(WaitToTraverseLadder(other.gameObject));
                }
            }

            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Vector3 newLocation = manager.FindTeleportSpot(manager.Mechanics.CurrentRoom.name);
                manager.TeleportWithFootsteps(other.gameObject, newLocation);
            }
        }
    }

    IEnumerator WaitToTraverseLadder(GameObject toTeleoprt)
    {
        manager.AllowInput = false;
        yield return new WaitForSeconds(5);
        Vector3 newLocation = manager.MapForDoors[this.name];
        newLocation.y += 2;
        manager.TeleportWithFootsteps(toTeleoprt, newLocation);
        yield break;
    }
}
