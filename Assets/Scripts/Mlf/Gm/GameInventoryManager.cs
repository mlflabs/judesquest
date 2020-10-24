using System;
using Mlf.InventorySystem;
using Mlf.InventorySystem.Base;
using Mlf.InventorySystem.GameObjects;
using Mlf.InventorySystem.Items;
using Mlf.Sound;
using UnityEngine;
using Yarn.Unity;

namespace Mlf.Gm
{


    public class GameInventoryManager : MonoBehaviour
    {
        public static GameInventoryManager instance;

        public event System.Action onUserInventoryChanged;
        public event System.Action onSecondaryInventoryChanged;

        [SerializeField] private InventoryData userInventory;
        [SerializeField] private InventoryData secondaryInventory;


        //Here we store reference to all items that could be accepted
        [SerializeField] private BaseItem[] usableItemList;

        [Header("To Connect to Dialogue System")]
        [SerializeField] Yarn.Unity.DialogueRunner dialogueRunner;

        private void Start()
        {
            GameInputManager.instance.OnColliderHit += onObjectHit;

            AddDialogueYarnFunctions();

            UserInventory = userInventory;
        }



        private void AddDialogueYarnFunctions()
        {
            dialogueRunner.AddFunction("userHasItemAmount", 2, delegate (Yarn.Value[] parameters)
            {

                Debug.Log("Parameters::: " + parameters.Length);
                Debug.LogWarning("Do we have the items: " + parameters[0].AsString + ", " +
                    parameters[1].AsNumber);
                int i = GetUserItemIndexByName(parameters[0].AsString);

                if (i < 0) return false;

                if (userInventory.items[i].amount < parameters[1].AsNumber)
                    return false;

                Debug.Log("YES WE DO");
                return true;



                //var nodeName = parameters[0];
                //return _visitedNodes.Contains(nodeName.AsString);
                // run check inventory function here
            });

            dialogueRunner.AddCommandHandler("addItemToUser", AddItemToUser);
            dialogueRunner.AddCommandHandler("removeItemFromUser", RemoveItemFromUser);
            dialogueRunner.AddCommandHandler("playSound", PlaySound);
        }


        public void PlaySound(string[] parameters)
        {
            if (parameters[0] == null)
            {
                Debug.Log("PlaySound missing name argument");
            }
            SoundManager.instance.PlayInteractSound(parameters[0]);
        }


        private void AddItemToUser(string[] parameters)
        {
            Debug.Log("AddItem:: ");
            Debug.Log(parameters[0]);
            Debug.Log(parameters[1]);
            AddItemToUser(parameters[0], Int32.Parse(parameters[1]));
        }

        private void RemoveItemFromUser(string[] parameters)
        {
            Debug.Log("RemoveItem:: ");
            Debug.Log(parameters[0]);
            Debug.Log(parameters[1]);
            RemoveItemFromUser(parameters[0], Int32.Parse(parameters[1]));
        }


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
                Debug.LogError("Duplicate Singleton");
            }
        }


        public InventoryData SecondaryInventory
        {
            get => secondaryInventory;
            set
            {
                secondaryInventory.onInventoryUpdate -= onSecondaryInventoryChangedFunc;
                secondaryInventory = value;
                onSecondaryInventoryChanged?.Invoke();
                secondaryInventory.onInventoryUpdate += onSecondaryInventoryChangedFunc;
            }
        }



        public InventoryData UserInventory
        {
            get => userInventory;
            set
            {
                Debug.LogWarning("===========================================    Changing User Inventory");
                userInventory.onInventoryUpdate -= onUserInventoryChangedFunc;
                userInventory = value;
                onUserInventoryChanged?.Invoke();
                userInventory.onInventoryUpdate += onUserInventoryChangedFunc;
            }
        }

        private void onUserInventoryChangedFunc()
        {
            Debug.Log("------- onUserInventoryChangedFunc");
            onUserInventoryChanged?.Invoke();
        }
        private void onSecondaryInventoryChangedFunc()
        {
            onSecondaryInventoryChanged?.Invoke();
        }



        private void onObjectHit(Collider collider)
        {
            Debug.Log("OnCollider HIT");
            Debug.Log(collider.tag);
            if (collider.tag == "Pickable")
            {
                InventoryPickableComp cmp = collider.GetComponent<InventoryPickableComp>();

                if (cmp == null)
                {
                    Debug.Log("Pickable object is null, no InventoryPickableComp");
                }

                Debug.Log("Adding items");
                while (cmp.inventory.hasMoreItems())
                {
                    Debug.Log("About to add item");
                    if (userInventory.MaxInventoryReached())
                    {
                        Debug.Log("Max Reached::: " + userInventory.maxItemCapacity);
                        return;
                    }
                    userInventory.AddItem(cmp.inventory.removeTopItem(), 1);
                }
                Debug.Log("Destroy Object");
                //if we are here, we took out all the items
                cmp.destoryItem();

                SoundManager.instance.PlayInteractSound("item");
            }

        }



        public void AddItemToUser(string name, int a)
        {
            Debug.Log("AddItemToUser");
            int itemIndex = -1;
            for (int i = 0; i < usableItemList.Length; i++)
            {
                if (usableItemList[i].name == name)
                {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex == -1)
            {
                Debug.LogError("Trying to add item thats not in usable item list");
                return;
            }

            UserInventory.AddItem(usableItemList[itemIndex], a);

        }


        public void RemoveItemFromUser(string name, int a)
        {
            Debug.Log("RemoveItemFromUser");
            int itemIndex = -1;
            for (int i = 0; i < UserInventory.items.Count; i++)
            {
                if (UserInventory.items[i].item.name == name)
                {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex == -1)
            {
                Debug.LogWarning("Trying to remove item that is not found");
                return;
            }

            UserInventory.removeItemAmountByIndex(itemIndex, a);


        }

        public int GetUserItemIndexByName(string name)
        {
            for (int i = 0; i < UserInventory.items.Count; i++)
            {
                if (UserInventory.items[i].item.name == name)
                {
                    return i;
                }
            }

            return -1;
        }










    }
}



