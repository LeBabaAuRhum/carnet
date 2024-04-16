using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Surface : MonoBehaviour
{
    public int monIndex;

   [SerializeField] newDict newDict;

   Dictionary<int, Vector3> directions;

   public Transform repere;

   private void Start()
   {
        directions = newDict.ToDictionary();
   }

   private void Update()
   {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(repere.position.normalized - transform.position.normalized);
        }
   }

   public Vector3 SetDestination(int destinationIndex)
   {
        return directions[destinationIndex];
   }
}

[Serializable]

public class NewSurface
{
    [SerializeField] public int index;
    [SerializeField] public Vector3 direction;
}

[Serializable]

public class newDict
{
    [SerializeField] NewSurface[] surfaceTableau;

    public Dictionary<int,Vector3> ToDictionary()
    {
        Dictionary<int,Vector3> newDict = new Dictionary<int, Vector3>();

        foreach (var item in surfaceTableau)
        {
            newDict.Add(item.index, item.direction);
        }

        return newDict;
    }
}

