using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mlf.TimeDate
{
    public class SkyboxModule : DNModuleBase
    {

        [SerializeField]
        private Gradient skyColor;
        [SerializeField]
        private Gradient horizonColor;

        public override void UpdateModule(float intensity)
        {
            RenderSettings.skybox.SetColor("_SkyTint", skyColor.Evaluate(intensity));
            RenderSettings.skybox.SetColor("_GroundColor", horizonColor.Evaluate(intensity));
        }
    }
}