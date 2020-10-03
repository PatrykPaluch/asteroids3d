using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace DefaultNamespace {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour {

        public float speed = 10;
        public float rotationSpeed = 1f;
        public float rotationMouseSpeed = 0.1f;
        
        private Vector3 inputMovement;
        private Vector3 inputRotation;

        private Rigidbody rb;
        private new Transform transform;

        private void Start() {
            rb = GetComponent<Rigidbody>();
            transform = GetComponent<Transform>();
            
            if (rb == null) {
                Debug.LogError("Missing Rigidbody", this);
                enabled = false;
            }

            //Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnMovement(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
            inputMovement.x = input.x;
            inputMovement.z = input.y;
        }

        public void OnMovementVertical(InputAction.CallbackContext context) {
            float input = context.ReadValue<float>();
            inputMovement.y = input;
        }
        
        public void OnRotation(InputAction.CallbackContext context) {
            float input = context.ReadValue<float>();
            inputRotation.z = -input;
        }
        
        public void OnMouse(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
            inputRotation.y = input.x;
            inputRotation.x = -input.y;
        }


        private void FixedUpdate() {
            rb.velocity = (transform.forward * inputMovement.z 
                          + transform.right * inputMovement.x
                          + transform.up * inputMovement.y
                          ) * speed;

            
            transform.Rotate(inputRotation.z * rotationSpeed * Vector3.forward, Space.Self);
            transform.Rotate(inputRotation.x * rotationMouseSpeed * Vector3.right, Space.Self);
            transform.Rotate(inputRotation.y * rotationMouseSpeed * Vector3.up, Space.Self);
            
            // rb.angularVelocity = (
            //     Quaternion.AngleAxis(inputRotation.z * rotationSpeed, transform.forward)
            //     * Quaternion.AngleAxis(inputRotation.x * rotationMouseSpeed, transform.right)
            //     * Quaternion.AngleAxis(inputRotation.y * rotationMouseSpeed, transform.up)
            //     ).eulerAngles;

        }
    }

}
