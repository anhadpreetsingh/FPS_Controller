using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.control
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float gravity = -10f;
        [SerializeField] float radius = 0.4f;
        [SerializeField] float jumpVelocity = 2f;
        [SerializeField] Transform groundCheck; 
        [SerializeField] LayerMask ground;


        
        CharacterController controller;
        float velocity;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        private void Update()
        {
            ProcessVerticalVelocity();
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            float xThrow = Input.GetAxis("Horizontal") * movementSpeed;
            float yThrow = Input.GetAxis("Vertical") * movementSpeed;

            controller.Move(transform.right * xThrow * Time.deltaTime + transform.forward * yThrow * Time.deltaTime +
                transform.up * velocity * Time.deltaTime);
        }

        private void ProcessVerticalVelocity()
        {
            if (IsGrounded())
            {
                velocity = -6f;
            }

            else
            {
                velocity += gravity * Time.deltaTime;
            }

            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                velocity = jumpVelocity;
            }
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, radius, ground);
        }
    }
}
