using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Shooting;

namespace FPS.Control
{
    
    public class LookAround : MonoBehaviour
    {
        [SerializeField] float sensitivity = 3f;
        [SerializeField] Camera myCamera;
        [SerializeField] Transform camRecoilAssist;
        public static Vector3 testEulerAngles;
        public static Vector3 assistEulerAngles;
        public static Vector3 playerEulerAngles;

        float yThrow;
        float xThrow;

        private void Start()
        {
            assistEulerAngles = new Vector3(testEulerAngles.x, playerEulerAngles.y, 0);
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            ProcessLookAround();
        }

        private void ProcessLookAround()
        {

            yThrow = Input.GetAxis("Mouse Y") * sensitivity;
            xThrow = Input.GetAxis("Mouse X") * sensitivity;

            testEulerAngles = new Vector3(Mathf.Clamp(testEulerAngles.x - yThrow, -85, 85), testEulerAngles.y, testEulerAngles.z);

            playerEulerAngles = new Vector3(playerEulerAngles.x, playerEulerAngles.y + xThrow, playerEulerAngles.z);
            assistAdjustor();


            transform.rotation = Quaternion.Euler(playerEulerAngles);

            myCamera.transform.localRotation = Quaternion.Euler(testEulerAngles);
            camRecoilAssist.localRotation = Quaternion.Euler(assistEulerAngles);
        }

        private void assistAdjustor()
        {
            if(!Firing.assistAdjust)
            {
                assistEulerAngles = new Vector3(Mathf.Clamp(assistEulerAngles.x - yThrow, -85, 85), assistEulerAngles.y, assistEulerAngles.z);
                assistEulerAngles += new Vector3(0f, xThrow, 0f);
            }
            if(Firing.assistAdjust && yThrow != 0)
            {
                assistEulerAngles = new Vector3(testEulerAngles.x + RecoilHandler.xMovedRot, assistEulerAngles.y, assistEulerAngles.z); 
                assistEulerAngles = new Vector3(assistEulerAngles.x, playerEulerAngles.y - RecoilHandler.yMovedRot, assistEulerAngles.z); 
            }
            
        }
    }

    

}
