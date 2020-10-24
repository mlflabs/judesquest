using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mlf.Dialogue;
using Mlf.Controllers.ThirdPerson;
using Mlf.InventorySystem.Items;
using Mlf.Sound;

namespace Mlf.Gm
{
    public class GameStaticEventsManager : MonoBehaviour
    {


        public static GameStaticEventsManager instance;

        [SerializeField] private CharacterMovement player;
        [SerializeField] private DialogueNPCCmp firstConversationNPC;

        [SerializeField] private string itemGoalName;
        [SerializeField] private int itemGoalAmmount;

        [SerializeField] private RectTransform winPanelRecTransform;


        private void Start()
        {
            winPanelRecTransform.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(ExecuteAfterTime(1));

            GameInventoryManager.instance.onUserInventoryChanged += inventoryChange;
        }

        private void inventoryChange()
        {
            Debug.Log("************************************ Checking Win conditions");
            for (int i = 0; i < GameInventoryManager.instance.UserInventory.items.Count; i++)
            {
                Debug.Log("Loop item Name:::: " + GameInventoryManager.instance.UserInventory.items[i].item.name);
                if (GameInventoryManager.instance.UserInventory.items[i].item.name == itemGoalName)
                {
                    Debug.Log("Found ITEM  Amount: " + GameInventoryManager.instance.UserInventory.items[i].amount);
                    if (GameInventoryManager.instance.UserInventory.items[i].amount >= itemGoalAmmount)
                    {
                        WinGame();
                    }
                }
            }
        }

        IEnumerator ExecuteAfterTime(float time)
        {
            if (firstConversationNPC == null) yield break;

            yield return new WaitForSeconds(time);

            if (firstConversationNPC != null && player != null)
            {
                firstConversationNPC.SetActive(true);
                player.OnDialogueStart();
            }


        }



        private void WinGame()
        {
            Debug.Log("WWWWWWWWWWWWOOOOOOOOOOOOOOONNNNNNNNNNN");
            SoundManager.instance.PlayInteractSound("won");
            winPanelRecTransform.transform.localScale = new Vector3(1, 1, 1);
        }






        private void Awake()
        {
            if (GameStaticEventsManager.instance == null)
                GameStaticEventsManager.instance = this;
            else
            {
                Debug.LogWarning("Spanwer singleton has a double instance");
                Destroy(this);
            }

        }




    }
}



