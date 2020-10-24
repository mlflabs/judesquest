// Created by Ronis Vision. All rights reserved
// 11.10.2019.

using System;
using System.Diagnostics;

namespace RVModules.RVUtilities
{
    // usefull to allow set frequency for monobehaviours 
    public class UpdateCounter
    {
        public UpdateCounter(int _desiredFrequency, int _expectedCallFrequency = 60)
        {
            allowUpdatePerCalls = (_expectedCallFrequency * 1.0f / _desiredFrequency);
        }

        public float allowUpdatePerCalls;
        private int counter;

        public bool Update()
        {
            counter++;
            if (counter >= allowUpdatePerCalls)
            {
                counter = 0;
                return true;
            }
            return false;
        }
    }
}