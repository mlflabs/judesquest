﻿using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
