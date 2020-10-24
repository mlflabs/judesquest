using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.TimeDate
{

    public delegate void TimeEvent(int hour, int min);

    public class GameTimeManager : MonoBehaviour
    {

        int currentHour;
        int currentMin;

        public event TimeEvent hourInterval;
        public event TimeEvent minInterval;




        public static GameTimeManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
                Debug.LogError("Duplicate Singleton");
            }

        }

        public void Start()
        {
            TimeManager.instance.onTimeChange += onTimeTick;
        }

        private void OnDestroy()
        {
            TimeManager.instance.onTimeChange -= onTimeTick;
        }


        private void onTimeTick()
        {
            if (currentMin != TimeManager.instance.minNumber)
            {
                currentMin = TimeManager.instance.minNumber;
                minInterval?.Invoke(TimeManager.instance.hourNumber, currentMin);
            }

            if (currentHour != TimeManager.instance.hourNumber)
            {
                currentHour = TimeManager.instance.hourNumber;
                hourInterval?.Invoke(currentHour, currentMin);
            }
        }
    }
}



