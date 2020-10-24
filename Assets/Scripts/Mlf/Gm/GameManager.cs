using System;
using Mlf.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private GameObject _selectedObject;
        //public BasicSm selectedSm;
        public event Action<GameObject> onSelectedObjectChanged;
        //public event Action<BasicSm> onSelectedSmChanged;

        public static GameManager instance;

        [SerializeField] public GameObject[] waterLocations;
        [SerializeField] public GameObject[] grassLocations;


        public GameObject selectedObject
        {
            get => _selectedObject; set
            {
                _selectedObject = value;

                /*BasicSm sm = _selectedObject.GetComponent<BasicSm>();
                if (sm != null)
                {
                    selectedSm = sm;
                }
                else
                {
                    sm = null;
                }
                */
                onSelectedObjectChanged?.Invoke(selectedObject);
                //onSelectedSmChanged?.Invoke(selectedSm);
            }
        }


        private void Awake()
        {
            if (GameManager.instance == null)
                GameManager.instance = this;
            else
                Debug.LogError("Multiple GameManagers");
            // lets find all our waters
            waterLocations = GameObject.FindGameObjectsWithTag("Water");
            grassLocations = GameObject.FindGameObjectsWithTag("Grass");
            Debug.Log("--------------------------------------------------------");
        }



    }
}



