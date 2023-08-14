
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct PeasePoolObjectsComponent 
{
    public List<GameObject> PeasePool;
    public GameObject PeasePoolGO;
    public GameObject PeasePrefab;

    public GameObject Get() 
    {
        GameObject result;
        var freeObjects = PeasePool.Where(x=>!x.activeSelf);
        if (freeObjects.Any())
        {
            result = freeObjects.First();
            result.SetActive(true);
        }
        else
        {
            result = GameObject.Instantiate(PeasePrefab,PeasePoolGO.transform);
            PeasePool.Add(result);
        }
        return result;
    }

    public void Return(GameObject pease)
    {
        pease.SetActive(false);
    }

}
