using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.TimeDate
{

    public class EventManager : MonoBehaviour
    {

        float timeDelay = 0f;
        int delaySeconds = 0;
        int delayMins = 0;

        public event Action oneSecInterval;
        public event Action oneMinInterval;
        public event Action fiveMinInterval;

        public static EventManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
            }
        }


        private void Update()
        {
            timeDelay += Time.deltaTime;

            if (timeDelay > 1)
            {
                execute1Sec();
                timeDelay -= 1f;
            }


        }

        private void execute1Sec()
        {
            //Debug.Log("Sec");
            delaySeconds++;

            if (delaySeconds >= 60)
            {
                executeMin();
                delaySeconds -= 60;
            }


            oneSecInterval?.Invoke();


        }

        private void executeMin()
        {
            //Debug.Log("Min");
            delayMins++;
            if (delayMins >= 5)
            {
                execute5Min();
                delayMins -= 5;
            }

            oneMinInterval?.Invoke();

        }

        private void execute5Min()
        {
            fiveMinInterval?.Invoke();

        }



    }
}



