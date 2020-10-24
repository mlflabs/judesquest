using System;
using System.Collections.Generic;
using Mlf.Gm;
using UnityEngine;
using UnityEngine.UI;

namespace Mlf.InventorySystem.Panel
{
    public class InventoryPanelUi : MonoBehaviour
    {
        public GameObject gridObject;
        public GameObject itemObject;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button openButton;

        [SerializeField] private RectTransform inventoryRec;
        [SerializeField] private RectTransform openButtonRec;
        public InventoryData inventory = new InventoryData();


        public PanelItemUi[] slots;


        public bool panelOpened = false;

        public Action<PanelItemUi> onSelectedItemChanged;
        [SerializeField] public PanelItemUi selectedItem;


        private void Start()
        {
            if (gridObject == null)
                Debug.LogError("GridObject cannot be null");

            slots = gridObject.GetComponentsInChildren<PanelItemUi>();
            onInventoryChanged();
            GameInventoryManager.instance.onUserInventoryChanged += onInventoryChanged;


            //reset everyones view
            onSelectedItemChanged?.Invoke(null);
            closeButton.onClick.AddListener(panelOpen);
            openButton.onClick.AddListener(panelOpen);

            panelOpen(panelOpened);

        }

        public void panelOpen(bool open)
        {
            panelOpened = open;
            if (open)
            {
                inventoryRec.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                inventoryRec.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        public void panelOpen()
        {
            panelOpen(!panelOpened);
        }




        public void setSelectedItem(PanelItemUi item)
        {
            this.selectedItem = item;
            onSelectedItemChanged?.Invoke(item);
        }

        private void onInventoryChanged()
        {
            Debug.Log("Inventory update.............................");

            Debug.Log(slots.Length);
            this.inventory = GameInventoryManager.instance.UserInventory;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].inventoryPanel = this;
                    slots[i].InitFromParent();
                    slots[i].setItem(inventory.items[i]);
                }
                else
                {
                    slots[i].hide();
                }
            }

        }

    }
}
