using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    GeneralManager manager;
    
    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && manager.AllowInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (manager.GameFlow.CheckInteractionName("Open") || 
                    manager.GameFlow.CheckInteractionName("Climb") ||
                    manager.GameFlow.CheckInteractionName("Go_Down"))
                {
                    StartCoroutine(WaitForDoorToOpen(other.gameObject));
                }
            }
        }
    }

    IEnumerator WaitForDoorToOpen(GameObject toTeleoprt)
    {
        manager.AllowInput = false;
        yield return new WaitForSeconds(4);
        Vector3 newLocation = manager.MapForDoors[this.name];
        newLocation.y += 2;
        manager.TeleportWithFootsteps(toTeleoprt, newLocation);
        yield break;
    }
}
