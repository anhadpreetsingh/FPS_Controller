using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public void OnDrop()
    {
        foreach(Transform child in transform)
        {
            if(child.tag == "Item")
            {
                child.GetComponent<SpawnDropped>().SpawnObject();
                Destroy(child.gameObject);
            }
        }
    }
}
