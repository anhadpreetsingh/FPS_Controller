using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickups : MonoBehaviour
{
    InventoryHandler inventoryHandler; 
    [SerializeField] Button button;

    private void Start()
    {
        inventoryHandler = FindObjectOfType<InventoryHandler>();
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < inventoryHandler.inventorySlots.Length; i++)
            {
                if(!inventoryHandler.isFull[i])
                {
                    Instantiate(button, inventoryHandler.inventorySlots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
