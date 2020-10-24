using UnityEngine;

namespace Mlf.Controllers.ThirdPerson
{
    using Mlf.Dialogue;
    using UnityEngine;

    // WASD to move, Space to sprint
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class CharacterMovement : MonoBehaviour
    {
        //public Transform InvisibleCameraOrigin;

        //public float StrafeSpeed = 0.1f;
        public float walkSpeed = 3;
        public float runSpeed = 5; //Todo implement speed or strife
        public float TurnSpeed = 3;
        public float Damping = 0.2f;
        public float VerticalRotMin = -80;
        public float VerticalRotMax = 80;
        public KeyCode sprintJoystick = KeyCode.JoystickButton2;
        public KeyCode sprintKeyboard = KeyCode.Space;

        private bool isSprinting;
        private Animator anim;
        private Vector2 currentVelocity;
        float currentSpeed;

        public AudioSource audioSource;
        public AudioClip walk;
        public AudioClip run;

        [SerializeField] private GameObject dialogueMidPoint;

        private CharacterController m_CharacterController;
        bool isInDialog = false;

        private void Start()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            m_CharacterController = GetComponent<CharacterController>();


            currentVelocity = Vector2.zero;
            isSprinting = false;
        }

        private void Update()
        {
            if (isInDialog) return;

            Interact();
        }

        void FixedUpdate()
        {
            if (isInDialog) return;

            Move();
        }



        void Interact()
        {
            // Check input
            if (Input.GetButtonDown("Interact"))
            {

                // Check if NPC is active and not already talking
                if (DialogueNPCCmp.ActiveNPC && !isInDialog)
                {
                    OnDialogueStart();

                }

                //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }


        public void OnDialogueStart()
        {
            // Start dialog
            isInDialog = true;
            DialogUI.Instance.dialogueRunner.StartDialogue(DialogueNPCCmp.ActiveNPC.YarnStartNode);
            anim.SetBool("Talk", true);
            anim.SetTrigger("startTalk");
            DialogueNPCCmp.ActiveNPC.startDialogue(this.transform.position);

            dialogueMidPoint.transform.position =
             (transform.position + DialogueNPCCmp.ActiveNPC.transform.position) / 2 +
             new Vector3(0, 1f, 0);


            transform.LookAt(DialogueNPCCmp.ActiveNPC.transform);
        }

        public void OnDialogEnd()
        {
            Debug.Log("On Dialogue END......");
            anim.SetBool("Talk", false);
            isInDialog = false;
            DialogueNPCCmp.ActiveNPC.endDialogue();

        }





        private void Move()
        {
            currentSpeed = Input.GetAxis("Vertical");
            currentSpeed = Mathf.Clamp(currentSpeed, -1f, 1f);

            isSprinting = (Input.GetKey(sprintJoystick) ||
                Input.GetKey(sprintKeyboard));

            anim.SetBool("Run", isSprinting);

            currentSpeed = isSprinting ? currentSpeed * runSpeed : currentSpeed * walkSpeed;

            currentSpeed = Mathf.SmoothDamp(anim.GetFloat("Speed"), currentSpeed,
                ref currentVelocity.y, Damping);
            //Debug.Log("Current Velocity:: " + currentVelocity);
            anim.SetFloat("Speed", currentSpeed);
            //Debug.Log("Speed:: " + currentSpeed);
            //anim.SetFloat("Direction", speed);

            // set sprinting



            //anim.SetBool("isSprinting", isSprinting);

            // strafing
            //currentSpeed = Mathf.SmoothDamp(
            //   currentStrafeSpeed, input.x * StrafeSpeed, ref currentVelocity.x, Damping);
            //Debug.Log("Straffle:: " + currentStrafeSpeed);



            var rotInput = new Vector2(Input.GetAxis("Horizontal"), 0);
            anim.SetFloat("Turn", Input.GetAxis("Horizontal"));
            Vector3 rot = transform.eulerAngles;
            rot.y += rotInput.x * TurnSpeed;
            transform.rotation = Quaternion.Euler(rot);

            //m_CharacterController.Move(transform.forward * currentSpeed * Time.fixedDeltaTime);
            m_CharacterController.SimpleMove(transform.forward * currentSpeed);
            // transform.position += transform.TransformDirection(Vector3.right) *
            //    currentStrafeSpeed;


            //sound effects
            return;
            if (currentSpeed > 0 && isSprinting)
            {
                if (audioSource.clip != run)
                    audioSource.clip = run;


                //audioSource.clip = run;
                audioSource.Play();
            }
            else if (currentSpeed > 0)
            {
                if (audioSource.clip != walk)
                    audioSource.clip = walk;
                //audioSource.clip = walk;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }


        }



    }


}
