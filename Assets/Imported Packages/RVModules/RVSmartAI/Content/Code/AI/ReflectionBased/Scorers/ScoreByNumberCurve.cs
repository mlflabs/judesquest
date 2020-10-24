// Created by Ronis Vision. All rights reserved
// 01.01.2020.

using System;
using RVModules.RVSmartAI.RpgPack;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class ScoreByNumberCurve : GetProperty
    {
        [SmartAiExposeField("Value(vertical axis) is returned value multiplied by score, time(horizontal axis) is number from property divided by maxValue")]
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        [SmartAiExposeField("Your property value will be divided by this to evaluate curve")]
        public float maxValue = 1;

        public override float Score(float _deltaTime)
        {
            var v = GetPropertyValue;

            float usedValue = 0;
            if (GetPropertyValue is int)
                usedValue = (int) v;
            else
                usedValue = (float) v;

            return curve.Evaluate(usedValue / maxValue) * score;
        }
    }
}