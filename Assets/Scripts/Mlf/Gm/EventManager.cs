using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm
{
    public class EventManager : MonoBehaviour
    {

        //play time to real world time 10 min to 1 day 0.0069
        public float timeRatio = 0.007f;

        public int delay1MinCount = 0;
        public int delay5MinCount = 0;


        public bool isCoroutine15SecExecuting = false;
        public static EventManager instance;

        //For the initial conversation setup

        private void Awake()
        {
            if (EventManager.instance == null)
                EventManager.instance = this;
            else
            {
                Debug.LogWarning("Spanwer singleton has a double instance");
                Destroy(this);
            }

            Debug.Log("-----------------Spawn Timer-------------");
        }



        private void Update()
        {
            StartCoroutine(ExecuteAfter15SecTime());
        }

        IEnumerator ExecuteAfter15SecTime()
        {
            if (isCoroutine15SecExecuting) yield break;

            isCoroutine15SecExecuting = true;

            yield return new WaitForSeconds(1);

            delay1MinCount++;
            delay5MinCount++;

            if (delay1MinCount >= 4)
            {
                execute1Min();
                delay1MinCount = 0;

            }

            if (delay5MinCount >= 20)
            {
                execute5Min();
                delay5MinCount = 0;

            }


            isCoroutine15SecExecuting = false;

            // Code to execute after the delay
        }

        private void execute1Min()
        {

        }

        private void execute5Min()
        {

        }



    }
}



