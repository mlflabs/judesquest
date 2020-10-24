// Created by Ronis Vision. All rights reserved
// 01.01.2020.

using System;
using RVModules.RVSmartAI.RpgPack;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class CompareNumber : GetProperty
    {
        [SmartAiExposeField]
        public ValueComparison valueComparison;

        [SmartAiExposeField]
        public float value;

        [SmartAiExposeField]
        public float scoreFalse;

        public override float Score(float _deltaTime)
        {
            var v = GetPropertyValue;

            float usedValue = 0;
            if (GetPropertyValue is int)
                usedValue = (int) v;
            else
                usedValue = (float) v;

            float s = 0;
            switch (valueComparison)
            {
                case ValueComparison.Equals:
                    s = value == usedValue ? score : scoreFalse;
                    break;
                case ValueComparison.Lower:
                    s = value > usedValue ? score : scoreFalse;
                    break;
                case ValueComparison.Higher:
                    s = value < usedValue ? score : scoreFalse;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return s;
        }
    }
}