using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mlf.TimeDate
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Light Settings", menuName = "Mlf/Light/Settings")]
    public class LightSettings : ScriptableObject
    {
        public Gradient AmbientColor;
        public Gradient DirectionalColor;
        public Gradient FogColor;
    }
}

