using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] float speed;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endPos.position, speed);
    }

}
