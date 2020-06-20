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
        [SerializeField] Transform CamRecoilAssist;
        public static Vector3 testEulerAngles;
        public static Vector3 assistEulerAngles;

        float yThrow;
        float xThrow;
        float xRotation;
        float yRotation;

        private void Start()
        {
            assistEulerAngles = testEulerAngles;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            ProcessLookAround();
            
        }

        private void ProcessLookAround()
        {

            yThrow = Input.GetAxis("Mouse Y");
            xThrow = Input.GetAxis("Mouse X");
            
            yRotation = xThrow * sensitivity;
            xRotation = yThrow * sensitivity;

            testEulerAngles = new Vector3(Mathf.Clamp(testEulerAngles.x - xRotation, -85, 85), testEulerAngles.y, testEulerAngles.z);

            if (!Firing.isShooting)
            {
                assistEulerAngles = new Vector3(Mathf.Clamp(assistEulerAngles.x - xRotation, -85, 85), assistEulerAngles.y, assistEulerAngles.z);
            }
            else if (Firing.isShooting && yThrow > 0)
            {
                assistEulerAngles = new Vector3(Mathf.Clamp(assistEulerAngles.x - xRotation, -85, 85), assistEulerAngles.y, assistEulerAngles.z);
            }
            else if (Firing.isShooting && yThrow < 0 && testEulerAngles.x >= assistEulerAngles.x)
            {
                assistEulerAngles = new Vector3(Mathf.Clamp(assistEulerAngles.x - xRotation, -85, 85), assistEulerAngles.y, assistEulerAngles.z);
            }


            transform.Rotate(0, yRotation, 0, Space.Self);
         
            myCamera.transform.localRotation = Quaternion.Euler(testEulerAngles);
            CamRecoilAssist.localRotation = Quaternion.Euler(assistEulerAngles);
        }

    }

    

}
