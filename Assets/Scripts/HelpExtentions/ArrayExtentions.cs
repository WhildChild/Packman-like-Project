using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ArrayExtentions
{
    public static List<T> ToList<T>(this T[,] array)
    {
        var result = new List<T>();
        foreach (var item in array) 
        {
            result.Add(item);
        }
        return result;
    } 
}
