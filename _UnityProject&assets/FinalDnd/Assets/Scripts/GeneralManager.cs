using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeneralManager : MonoBehaviour {

    private Item root = new Item("root");
    private bool allowInput = true;

    // Organised resources
    private Dictionary<string, Vector3> map = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> mapForDoors = new Dictionary<string, Vector3>();
    private Dictionary<Item, List<string>> soundDatabase = new Dictionary<Item, List<string>>();
    private Dictionary<Item, bool> roomsExplored = new Dictionary<Item, bool>();
    private Dictionary<string, AudioClip> effects = new Dictionary<string, AudioClip>();

    // Efentials
    private GameObject player;
    private Mechanics mechanics;
    private ImportData importer;
    private GameFlow gameFlow;
    private SoundManager soundManager;

    private void Awake()
    {
        LocateTheComponents();
        LoadAllTheInfo();
    }

    void Start () {
        //print(map["Library"]);
	}

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SoundManager.PlayRoomDescription();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            SoundManager.PlayControls();
        }

        if (SoundManager.Voice.isPlaying)
        {
            //print("voice is playing");
            //AllowInput = false;
        }

        else if (SoundManager.SoundEffects.isPlaying)
        {
            //print("Effects are playing");
            //AllowInput = false;
        }

        else
        {
            //AllowInput = true;
        }
    }

    void LocateTheComponents()
    {
        Importer = GameObject.Find("General").GetComponent<ImportData>();
        Mechanics = GameObject.Find("General").GetComponent<Mechanics>();
        GameFlow = GameObject.Find("General").GetComponent<GameFlow>();
        SoundManager = GameObject.Find("General").GetComponent<SoundManager>();
        Player = GameObject.Find("Player").gameObject;
    }
  
    void LoadAllTheInfo()
    {
        Importer.PopulateTree();
    }

    public IEnumerator WaitTheWalkingTime(GameObject toTeleport, Vector3 newLocation)
    {
        allowInput = false;
        SoundManager.PlaySpecifiedEffect("footsteps");
        yield return new WaitForSeconds(SoundManager.WalkingTime);
        // i also have to disable controls here
        toTeleport.transform.position = newLocation;
        allowInput = true;
        yield break;
    }

    public void TeleportWithFootsteps(GameObject toTeleport, Vector3 newLocation)
    {
        
        SoundManager.ShowString("Walking ...");
        StartCoroutine(WaitTheWalkingTime(toTeleport, newLocation));
    }
    

    public Vector3 FindTeleportSpot(string name)
    {
        Vector3 position = Map[name];

        position.x += 2;
        position.y += 3f;

        return position;
    }

    // function tries to find the item in the tree
    // if it cannot find it, the return is null
    public Item TryToFindItem(string name)
    {
        if(FindItem(name, Root))
        {
            return Mechanics.FoundItem;
        }
        else
        {
            return null;
        }
    }

    public bool FindItem(string name, Item root)
    {
        bool result = false;

        if (root.name == name)
        {
            Mechanics.FoundItem = root;
            return true;
        }

        foreach (Item item in root.children)
        {
            result = result || FindItem(name, item);
        }

        return result;
    }

    public Item Root
    {
        get
        {
            return root;
        }

        set
        {
            root = value;
        }
    }
  
    public Dictionary<string, Vector3> Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public Dictionary<Item, List<string>> SoundDatabase
    {
        get
        {
            return soundDatabase;
        }

        set
        {
            soundDatabase = value;
        }
    }

    public Dictionary<Item, bool> RoomsExplored
    {
        get
        {
            return roomsExplored;
        }

        set
        {
            roomsExplored = value;
        }
    }

    public Dictionary<string, AudioClip> Effects
    {
        get
        {
            return effects;
        }

        set
        {
            effects = value;
        }
    }

    public Mechanics Mechanics
    {
        get
        {
            return mechanics;
        }

        set
        {
            mechanics = value;
        }
    }

    public Dictionary<string, Vector3> MapForDoors
    {
        get
        {
            return mapForDoors;
        }

        set
        {
            mapForDoors = value;
        }
    }

    public GameFlow GameFlow
    {
        get
        {
            return gameFlow;
        }

        set
        {
            gameFlow = value;
        }
    }

    public ImportData Importer
    {
        get
        {
            return importer;
        }

        set
        {
            importer = value;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public SoundManager SoundManager
    {
        get
        {
            return soundManager;
        }

        set
        {
            soundManager = value;
        }
    }

    public bool AllowInput
    {
        get
        {
            return allowInput;
        }

        set
        {
            allowInput = value;
        }
    }
}