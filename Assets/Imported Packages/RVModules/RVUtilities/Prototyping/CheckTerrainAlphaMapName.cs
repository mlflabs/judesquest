// Created by Ronis Vision. All rights reserved
// 13.11.2016.

using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class CheckTerrainAlphaMapName : ScreenToWorldRaycaster
    {
        #region Not public methods

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                RaycastHit hit;
                if (RaycastCheck(out hit))
                {
                    //Debug.Log(FindObjectOfType<Terrain>().transform.parent.GetComponent<ITerrainTypeGetter>().GetTerrainTypeAtPos(hit.point));
                }
            }
        }

        #endregion
    }
}