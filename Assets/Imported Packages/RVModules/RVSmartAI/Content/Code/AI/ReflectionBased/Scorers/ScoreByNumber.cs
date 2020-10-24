// Created by Ronis Vision. All rights reserved
// 01.01.2020.

using System;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class ScoreByNumber : GetProperty
    {
        public override float Score(float _deltaTime)
        {
            var v = GetPropertyValue;

            float usedValue = 0;
            if (GetPropertyValue is int)
                usedValue = (int) v;
            else
                usedValue = (float) v;

            return usedValue * score;
        }
    }
}