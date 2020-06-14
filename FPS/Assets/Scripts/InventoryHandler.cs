using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public bool[] isFull;

    private void Start()
    {
        
    }

    private void Update()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            for (int j = 0; j < inventorySlots[i].transform.childCount; j++)
            {
                if(inventorySlots[i].transform.GetChild(j).tag == "Item")
                {
                    isFull[i] = true;
                    break;
                }
                else
                {
                    isFull[i] = false;
                }
            }
        }
    }
}
