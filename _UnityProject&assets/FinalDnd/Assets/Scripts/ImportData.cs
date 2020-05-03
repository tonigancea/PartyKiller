using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImportData : MonoBehaviour {

    GeneralManager manager;
    private List<InputItem> hiddenItems = new List<InputItem>();

    public void PopulateTree()
    {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();

        string path = Application.streamingAssetsPath + "/allobjects.json";
        string jsonString = File.ReadAllText(path);
        InputItem[] items = JsonHelper.FromJson<InputItem>(jsonString);

        foreach (InputItem item in items)
        {
            if (item.observations == "hidden")
            {
                HiddenItems.Add(item);
            }

            else
            {
                Item newItem = new Item(item.name);

                // add in the tree
                manager.Root.AddItem(item.parent, newItem);

                // register the location only if the object is not abstract
                if (item.observations == "placeable")
                {
                    Vector3 location = GameObject.Find(item.name).transform.position;
                    manager.Map.Add(item.name, location);
                }

                if (item.parent == "root")
                // if item is a room
                {
                    // make new room as unexplored
                    manager.RoomsExplored.Add(newItem, false);
                }

                // map the sounds for this item
                manager.SoundDatabase.Add(newItem, new List<string>());
                manager.SoundDatabase[newItem].Add(item.sound1);
                manager.SoundDatabase[newItem].Add(item.sound2);
                manager.SoundDatabase[newItem].Add(item.sound3);
            }
        }

        // where does door lead?
        manager.MapForDoors.Add("Door1-1", manager.Map["Study"]);
        manager.MapForDoors.Add("Door1-2", manager.Map["Dining_Room"]);
        manager.MapForDoors.Add("Door2-1", manager.Map["Bathroom"]);
        manager.MapForDoors.Add("Door2-2", manager.Map["Dining_Room"]);
        manager.MapForDoors.Add("Door3-1", manager.Map["Library"]);
        manager.MapForDoors.Add("Door3-2", manager.Map["Study"]);
        manager.MapForDoors.Add("Door4-1", manager.Map["Attic"]);
        manager.MapForDoors.Add("Door4-2", manager.Map["Library"]);
    }

    public void AddHiddenItemToTree(string parent, string name)
    {
        foreach (InputItem item in hiddenItems)
        {
            if (item.name == name && item.parent == parent)
            {
                // create and add item to tree
                Item newItem = new Item(item.name);
                manager.Root.AddItem(item.parent, newItem);
                
                // map the sounds for this item
                manager.SoundDatabase.Add(newItem, new List<string>());
                manager.SoundDatabase[newItem].Add(item.sound1);
                manager.SoundDatabase[newItem].Add(item.sound2);
                manager.SoundDatabase[newItem].Add(item.sound3);
                return;
            }
        }
        print("The " + name + " is not in the hiddenItems list!");
    }

    public List<InputItem> HiddenItems
    {
        get
        {
            return hiddenItems;
        }

        set
        {
            hiddenItems = value;
        }
    }
}

public class Item
{
    public List<Item> children;
    public string name;

    public Item()
    {
        children = new List<Item>();
    }

    public Item(string name)
    {
        children = new List<Item>();
        this.name = name;
    }

    public void RemoveItem(string parent, string toRemove)
    {
        if (parent == name)
        {
            List<Item> newChildren = new List<Item>();
            foreach (Item child in children)
            {
                if (child.name != toRemove)
                {
                    newChildren.Add(child);
                }
            }
            children = newChildren;
            return;
        }

        else
        {
            foreach (Item child in children)
            {
                child.RemoveItem(parent, toRemove);
            }
        }
    }

    public void AddItem(string parent, Item newItem)
    {
        if (parent == name)
        {
            children.Add(newItem);
            return;
        }

        foreach (Item child in children)
        {
            child.AddItem(parent, newItem);
        }
    }

    public bool FindItem(string name)
    {
        bool result = false;

        if (this.name == name)
        {
            result = true;
            return result;
        }

        else
        {
            foreach (Item item in children)
            {
                result = result || item.FindItem(name);
            }
        }

        return result;
    }

    override
    public string ToString()
    {
        string result = name + " ";

        result += "[ ";
        foreach (Item child in children)
        {
            result += child + " "; 
        }
        result += "]";

        return result;
    }
}

[System.Serializable]
public class InputItem
{
    public string name;
    public string parent;
    public string observations;
    public string sound1;
    public string sound2;
    public string sound3;

    override
    public string ToString()
    {
        string result = name + " : ";
        result += parent + " : ";
        result += observations + " : ";
        result += sound1 + ", ";
        result += sound2 + ", ";
        result += sound3;
        return result;
    }
}

// this class is used to deserialize the objects from json
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
