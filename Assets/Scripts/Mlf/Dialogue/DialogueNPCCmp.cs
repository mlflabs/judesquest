using System;
using Mlf.RvAi.Components;
using Mlf.RvAi.Contexts;
using SensorToolkit;
using UnityEngine;

namespace Mlf.Dialogue
{


    [RequireComponent(typeof(MlfWaypointMovementCmp))]
    public class DialogueNPCCmp : MonoBehaviour
    {
        public string YarnStartNode { get { return yarnStartNode; } }


        [SerializeField] GameObject chatBubble;
        [SerializeField] string yarnStartNode = "Start";
        [SerializeField] YarnProgram yarnDialog;
        [SerializeField] SpeakerData speakerData;
        private Animator anim;

        public static DialogueNPCCmp ActiveNPC { get; private set; }

        protected IMlfWaypointMovement movement;

        public bool inDialogue { get; private set; }
        public string dialogueAnimationState { get; set; }

        private void Start()
        {
            movement = GetComponent<MlfWaypointMovementCmp>();
            anim = GetComponent<Animator>();
            chatBubble.SetActive(false);
            DialogUI.Instance.dialogueRunner.Add(yarnDialog);
            DialogUI.Instance.AddSpeaker(speakerData);
        }



        public void startDialogue(Vector3 pos)
        {
            Debug.LogWarning("NPC Start Dialogue");
            movement.target = pos;
            inDialogue = true;
            anim.SetBool("Talk", true);
            anim.SetTrigger("startTalk");

        }

        public void endDialogue()
        {
            Debug.LogWarning("NPC End Dialogue");
            movement.target = Vector3.zero;
            inDialogue = false;
            anim.SetBool("Talk", false);
        }

        public void SetActive(bool set)
        {
            chatBubble.SetActive(set);

            if (set)
            {
                ActiveNPC = this;
            }
            else
            {
                ActiveNPC = null;
            }

        }





    }
}

