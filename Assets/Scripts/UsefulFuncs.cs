using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsefulFuncs
{
    public static void AddToListWithoutDuplicates<T>(List<T> list, T element) where T : Object
    {
        if (!list.Contains(element))
        {
            list.Add(element);
        }
        else
        {
            Debug.LogError("Adding duplicates is not allowed\n" + list);
        }
    }
}