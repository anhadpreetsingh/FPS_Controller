using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTestRecoil : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Quaternion camRecoilX = Quaternion.Euler(Camera.main.transform.localRotation.x - 500f, Camera.main.transform.localRotation.y,
                Camera.main.transform.localRotation.z);
            Camera.main.transform.localRotation = Quaternion.Lerp(transform.localRotation, camRecoilX, 5f * 0.02f);

        }
        
    }
}
