using Mlf.Dialogue;
using Mlf.Gm;
using Mlf.InventorySystem;
using Mlf.RvAi.Components;
using Mlf.RvAi.Contexts;
using RVModules.RVSmartAI;
using UnityEngine;

namespace Mlf.Characters
{

    [RequireComponent(typeof(MlfWaypointMovementCmp))]
    public class CharacterCmp : MonoBehaviour, IContext, ICharacterContextProvider, IContextProvider,
        IMlfWaypointMovementProvider, IMlfDialogueCmpProvider, IInventory
    {
        [SerializeField]
        private CharacterContext _characterContext;

        public CharacterContext characterContext
        {
            get => _characterContext;
            set => _characterContext = value;
        }


        // IContextProvider implementation
        public virtual IContext GetContext() => this;
        public IMlfWaypointMovement Movement { get; private set; }
        public DialogueNPCCmp DialogueCmp { get; set; }
        public InventoryData Inventory { get => _inventory; set => _inventory = value; }

        public InventoryData _inventory = new InventoryData();



        private void Awake()
        {
            // RV AI needs these items, needs to be initiated before startß
            Movement = GetComponent<MlfWaypointMovementCmp>();
            DialogueCmp = GetComponent<DialogueNPCCmp>();

        }
        private void Start()
        {
            //Debug.LogWarning("Inventory::: " + GameInventoryManager.instance);



            //GameInventoryManager.instance.UserInventory = Inventory;

            if (_characterContext == null)
            {
                Debug.LogError("Missing Character Context");
            }
        }


    }


}

