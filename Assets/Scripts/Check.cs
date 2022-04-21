using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Check
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
			throw new UnityException($"Attempt to add duplicate of {element}");
        }
    }
}