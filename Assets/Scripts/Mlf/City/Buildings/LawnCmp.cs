using System;
using System.Collections;
using System.Collections.Generic;
using Mlf.Characters;
using Mlf.TimeDate;
using UnityEngine;



namespace Mlf.City.Buildings
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LawnCmp : MonoBehaviour
    {

        [Header("Properties")]
        private int _lawnGrassLevel = 0;
        private int _levels;
        public int growthRateInSec = 5;

        [SerializeField] LawnSO props;

        [Header("Components")]
        [SerializeField] MeshFilter _mesh;
        [SerializeField] MeshRenderer _renderer;




        private void Start()
        {

            if (props == null)
            {
                Debug.LogError("Missing BuildingSO");
                return;
            }

            _levels = (props.meshes.Length < props.materials.Length) ?
                props.meshes.Length : props.materials.Length;


            _mesh = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();

            EventManager.instance.oneSecInterval += oneSecUpdate;
        }

        private int _secondsPassed = 0;

        private void oneSecUpdate()
        {
            _secondsPassed++;

            if (_secondsPassed < growthRateInSec)
                return;

            _secondsPassed = 0;

            if (lawnGrassLevel < _levels)
            {
                lawnGrassLevel++;
            }


        }





        private void OnDestroy()
        {

        }



        #region List Setters

        public int lawnGrassLevel
        {
            get => _lawnGrassLevel;
            set
            {
                if (value >= _levels)
                {
                    Debug.LogWarning("Level out of range");
                    return;
                }

                if (_lawnGrassLevel != value)
                {
                    _mesh.mesh = props.meshes[value];
                    _renderer.material = props.materials[value];
                }
                _lawnGrassLevel = value;
            }
        }

        #endregion


    }


}

