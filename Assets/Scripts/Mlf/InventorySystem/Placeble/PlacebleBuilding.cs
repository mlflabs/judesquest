

using Mlf.Gm.InputTypes;
using Mlf.InventorySystem.Items;
using UnityEngine;

namespace Mlf.InventorySystem.Placeble {

  public class PlacebleBuilding : BaseUserDragInput {

    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private BoxCollider2D mainCollider;

    public BaseItem baseItem;

    private void Start() {
      


      if(mainSprite == null) {
        Debug.LogError("MainSprite cannot be null, component mising");
      }

      if(baseItem == null) {
        Debug.LogError("BaseItem cannot be null");
      }

      mainSprite.sprite = baseItem.icon;

      
    }
  }
}