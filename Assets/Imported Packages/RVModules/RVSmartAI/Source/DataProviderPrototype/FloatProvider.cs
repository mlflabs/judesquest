// Created by Ronis Vision. All rights reserved
// 11.07.2020.

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    public abstract class FloatProvider : DataProvider<float>
    {
        [SmartAiExposeField]
        public float number;
        
        public static implicit operator float(FloatProvider _floatProvider)
        {
            return _floatProvider.number;
        }

    }
}