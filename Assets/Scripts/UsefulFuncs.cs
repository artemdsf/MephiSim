using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsefulFuncs
{
    //returns true if succeed
    public static bool AddToListWithoutDuplicates<T>(List<T> list, T element)
    {
        if (!list.Contains(element))
        {
            list.Add(element);
            return true;
        }
        else
        {
            Debug.LogWarning($"Attemp to add duplicate of {element}");
            return false;
        }
    }
}