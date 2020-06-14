using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS.Control
{
    public class WeaponSway : MonoBehaviour
    {
        [SerializeField] float maxSway = 5f;
        [SerializeField] float swaySpeed = 1f;
        [SerializeField] float smoothAmount = 1f;
        [SerializeField] float zSwaySpeed = 5f;

        Quaternion initRot;

        private void Start()
        {
            initRot = transform.localRotation;
        }

        private void FixedUpdate()
        {
            float xThrow = Input.GetAxis("Mouse X");
            float yThrow = Input.GetAxis("Mouse Y");

            float horizontalThrow = Input.GetAxis("Horizontal");

            xThrow = Mathf.Clamp(xThrow * swaySpeed, -maxSway, maxSway);
            yThrow = Mathf.Clamp(yThrow * swaySpeed, -maxSway, maxSway);

            horizontalThrow = Mathf.Clamp(horizontalThrow * zSwaySpeed, -maxSway, maxSway);

            Quaternion finalRot = Quaternion.Euler(yThrow + initRot.eulerAngles.x, xThrow + initRot.eulerAngles.y, horizontalThrow + initRot.eulerAngles.z);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, finalRot, smoothAmount * Time.fixedDeltaTime);




        }
    }
}

