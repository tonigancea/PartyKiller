using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{

    private int index;
    private Item currentItem;
    private Item currentRoom;
    private Item foundItem;

    // Increments index in relation to the CurrentItem's children
    public void IncrementIndex()
    {
        if (Index >= CurrentItem.children.Count - 1)
        {
            Index = 0;
        }
        else
        {
            Index++;
        }
    }

    // Decrements index in relation to the CurrentItem's children
    public void DecrementIndex()
    {
        if (Index == 0)
        {
            Index = CurrentItem.children.Count - 1;
        }
        else
        {
            Index--;
        }
    }

    public Item GetItemAtIndex(List<Item> list)
    {
        if (list.Count == 0)
        {
            print("Lista nu are elemente.");
            return null;
        }
        else if (index >= 0)
        {
            return list[index];
        }
        else
        {
            print("Index negativ");
            return null;
        }

    }

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public Item CurrentItem
    {
        get
        {
            return currentItem;
        }

        set
        {
            currentItem = value;
        }
    }

    public Item CurrentRoom
    {
        get
        {
            return currentRoom;
        }

        set
        {
            currentRoom = value;
        }
    }

    public Item FoundItem
    {
        get
        {
            return foundItem;
        }

        set
        {
            foundItem = value;
        }
    }
}
