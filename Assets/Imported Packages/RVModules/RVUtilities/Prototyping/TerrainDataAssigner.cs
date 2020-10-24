// Created by Ronis Vision. All rights reserved
// 13.11.2016.

using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    [ExecuteInEditMode] public class TerrainDataAssigner : MonoBehaviour
    {
        #region Fields

        public TerrainData terrainDataAsset;
        public bool work;

        #endregion

        #region Not public methods

        // Update is called once per frame
        private void Update()
        {
            if (work)
            {
                work = false;
                Work();
            }
        }

        private void Work()
        {
            Terrain terrain = GetComponent<Terrain>();
            terrain.terrainData = terrainDataAsset;
            Debug.Log("done!");
        }

        #endregion
    }
}