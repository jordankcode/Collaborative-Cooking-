using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 5f;
        public float gravity = -15f;

        Vector3 velocity;

        void Update()
        {
            Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

            float x = moveInput.x;
            float z = moveInput.y;

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}