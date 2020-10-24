using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using SensorToolkit;
using Mlf.Utils;

namespace Mlf.Dialogue
{
    public class DialogUI : Singleton<DialogUI>
    {

        [SerializeField] Image speakerPortrait;
        [SerializeField] TextMeshProUGUI txt_Dialog, txt_SpeakerName;




        public DialogueRunner dialogueRunner;
        DialogueUI dialogueUI;

        Dictionary<string, SpeakerData> speakerDatabase = new Dictionary<string, SpeakerData>();

        bool autoContinue = false;


        private void Awake()
        {
            dialogueRunner = GetComponent<DialogueRunner>();
            dialogueRunner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
            dialogueRunner.AddCommandHandler("AutoContinue", (string[] s) => autoContinue = true);
            dialogueUI = GetComponent<DialogueUI>();
            dialogueUI.onLineFinishDisplaying.AddListener(AutoContinueCheck);
        }

        void AutoContinueCheck()
        {
            if (autoContinue)
            {
                dialogueUI.MarkLineComplete();
                autoContinue = false;
            }
        }

        public void AddSpeaker(SpeakerData data)
        {
            if (speakerDatabase.ContainsKey(data.speakerName))
            {
                Debug.LogWarningFormat("Attempting to add {0} into speaker database, but it already exists!", data.speakerName);
                return;
            }
            // Add
            speakerDatabase.Add(data.speakerName, data);
        }

        void SetSpeakerInfo(string[] info)
        {
            string speaker = info[0];
            string emotion = info.Length > 1 ? info[1].ToLower() : SpeakerData.EMOTION_NEUTRAL;

            if (speakerDatabase.TryGetValue(speaker, out SpeakerData data))
            {
                speakerPortrait.sprite = data.GetEmotionPortrait(emotion);
                txt_SpeakerName.text = data.speakerName;
                return;
            }
            Debug.LogErrorFormat("Could not set speaker info for unknown speaker {0}", speaker);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }



        public void onNPCViewEnter(GameObject gb, Sensor s)
        {
            Debug.Log("On Enter");
            if (DialogueNPCCmp.ActiveNPC != null) return;

            var d = gb.GetComponent<DialogueNPCCmp>();

            if (d == null)
            {
                Debug.LogError("Npc doesn't have dialog component");
                return;
            }


            SetActiveNPC(true, d);

        }


        public void onNPCViewExit(GameObject gb, Sensor s)
        {
            Debug.Log("On Exit");
            var d = gb.GetComponent<DialogueNPCCmp>();

            if (d == null)
            {
                Debug.LogError("Npc doesn't have dialog component");
                return;
            }


            if (DialogueNPCCmp.ActiveNPC != d) return;


            SetActiveNPC(false, d);

        }

        void SetActiveNPC(bool set, DialogueNPCCmp target)
        {
            target.SetActive(set);
        }

    }
}