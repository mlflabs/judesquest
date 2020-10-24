using System.Collections;
using System.Collections.Generic;
using Mlf.Dialogue;
using UnityEngine;


namespace Mlf.City.UserInput
{

    public class MovementCmp : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private float playerSpeed = 3.0f;
        private float jumpHeight = 1.0f;
        private float gravityValue = -9.81f;

        private Vector3 moveValue;

        bool isInDialog = false;

        private void Start()
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        private void Update()
        {
            if (isInDialog) return;

            GetMovementInput();
            Interact();
        }

        void FixedUpdate()
        {
            if (isInDialog) return;

            Move();
        }

        void GetMovementInput()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            moveValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        }



        void Interact()
        {
        }

        public void OnDialogEnd()
        {
            isInDialog = false;
        }

        private void Move()
        {
            controller.Move(moveValue * Time.deltaTime * playerSpeed);
            if (moveValue != Vector3.zero)
            {
                gameObject.transform.forward = moveValue;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }


    }
}