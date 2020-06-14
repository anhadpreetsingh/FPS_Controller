using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Control
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] float sensitivity = 3f;
        [SerializeField] Camera myCamera;


        float yRot = 0f;
        float xRot = 0f;

        private void Start()
        {
           Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            ProcessLookAround();
        }

        private void ProcessLookAround()
        {
            float yThrow = Input.GetAxis("Mouse Y") * sensitivity;
            float xThrow = Input.GetAxis("Mouse X") * sensitivity;

            yRot -= yThrow;
            xRot += xThrow;

            float clampedYRot = Mathf.Clamp(yRot, -85f, 85f);

            myCamera.transform.localRotation = Quaternion.Euler(clampedYRot, transform.localRotation.y, transform.localRotation.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, xRot, transform.rotation.z);
        }
    }
}
