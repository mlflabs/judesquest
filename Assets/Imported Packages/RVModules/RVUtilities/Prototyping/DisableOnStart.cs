// Created by Ronis Vision. All rights reserved
// 21.01.2019.

using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class DisableOnStart : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}