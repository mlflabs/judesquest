using System.Collections.Generic;
using Mlf.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Mlf.Gm
{


    public class PanelManager : MonoBehaviour
    {

        public bool panelOpened = false;

        [SerializeField] private RectTransform panelRecTransform;


        [SerializeField] public Sprite openStateSprite;
        [SerializeField] public Sprite closeStateSprite;
        [SerializeField] public Image buttonImage;


        public string exitString = "Back To Main Menu";

        void Start()
        {

            togglePanel(panelOpened);
            panelRecTransform = GetComponent<RectTransform>();
        }

        public void togglePanel(bool open)
        {
            panelOpened = open;


            if (open)
            {
                panelRecTransform.transform.localScale = new Vector3(1, 1, 1);
                if (openStateSprite != null && buttonImage != null)
                {
                    buttonImage.sprite = openStateSprite;
                }

            }
            else
            {
                panelRecTransform.transform.localScale = new Vector3(0, 0, 0);
                if (closeStateSprite != null && buttonImage != null)
                {
                    buttonImage.sprite = closeStateSprite;
                }

            }
        }

        public void playOpenSound()
        {
            if (panelOpened)
                SoundManager.instance.PlayInteractSound("openPanel");
            else
                SoundManager.instance.PlayInteractSound("closePanel");
        }

    }
}