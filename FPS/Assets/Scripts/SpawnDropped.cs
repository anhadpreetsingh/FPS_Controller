using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDropped : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject item;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void SpawnObject()
    {
        Instantiate(item, player.transform.position + new Vector3(0f, 0f, 3f), Quaternion.identity);
    }
}
