using UnityEngine;

public class Room : MonoBehaviour {

    GeneralManager manager;

    AudioClip voice;
    AudioClip effect;
    Item item;

    private void Start()
    {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();

        // corelate this object with its item in the tree
        item = manager.TryToFindItem(this.name);
        //print(this.name);
        if (item == null)
        {
            print("WARNING! Item not found in the tree.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // setting the current item, resetting the index
            manager.Mechanics.CurrentItem = manager.TryToFindItem(name);
            manager.Mechanics.CurrentRoom = manager.Mechanics.CurrentItem;
            manager.Mechanics.Index = manager.Mechanics.CurrentItem.children.Count - 1;

            if (manager.RoomsExplored[item])
            {
                //plays Sound2 (short description) for this item
                manager.SoundManager.PlaySpecificSound_Nearby(1);
            }
            else
            {
                //plays Sound1 (long description) for this item
                manager.SoundManager.PlaySpecificSound_Nearby(0);
                manager.RoomsExplored[item] = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && manager.AllowInput)
        {
            //print("OnTriggerStay");
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                manager.Mechanics.DecrementIndex();
                manager.SoundManager.PlaySpecificSound_Indexed(0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                manager.Mechanics.IncrementIndex();
                manager.SoundManager.PlaySpecificSound_Indexed(0);
                //print("down");

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                // the indexed object is now the current item
                // this should be done in Obiect.cs
                manager.Mechanics.CurrentItem = manager.Mechanics.CurrentItem.children[manager.Mechanics.Index];

                //walk to it
                Vector3 newLocation = manager.FindTeleportSpot(manager.Mechanics.CurrentItem.name);
                manager.TeleportWithFootsteps(other.gameObject, newLocation);
            }
        }
    }
}
