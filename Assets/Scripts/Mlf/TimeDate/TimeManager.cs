using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Mlf.TimeDate
{
    [System.Serializable]
    public struct TimeDateStruct
    {
        public int minute;
        public int hour;
        public int day;
        public int year;
    }

    public class TimeManager : MonoBehaviour
    {

        [Header("Time")]
        [Help("Day Length in Minutes")]
        [SerializeField] private float _targetDayLength = 0.5f;
        [Help("Update clock interval, if 5, will update clock txt every 5 mins")]
        [SerializeField] private int _minsSizeUpdate = 5;

        [SerializeField] private int runUpdateEveryHowManyFrames = 10;
        private int currentFrameCount = 0;

        [SerializeField] private bool use24Clock = true;

        [SerializeField] [Range(0, 1)] private float _timeOfDay;
        [SerializeField] float elapsedTime;
        [SerializeField] private int _dayNumber;

        [SerializeField] private int _yearNumber;

        [SerializeField] private float _timeScale = 100f;

        [SerializeField] private int _yearLength = 100;

        [Header("Output")]
        [SerializeField] private int elapsedMins;
        [SerializeField] private int elapsedHours;
        public event System.Action onTimeChange;

        [Header("UI")]
        [SerializeField] private TMP_Text clockText;

        public bool pause = false;

        public float targetDayLength { get => _targetDayLength; }
        public float timeOfDay { get => _timeOfDay; }
        public int minNumber { get => elapsedMins; }
        public int hourNumber { get => elapsedHours; }
        public int dayNumber { get => _dayNumber; }
        public int yearNumber { get => _yearNumber; }
        public float timeScale { get => _timeScale; }
        public int yearLength { get => _yearLength; }


        public static TimeManager instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
            }
        }

        public static TimeDateStruct getTimeDateStruct()
        {
            return new TimeDateStruct
            {
                minute = instance.minNumber,
                hour = instance.hourNumber,
                day = instance.dayNumber,
                year = instance.yearNumber
            };
        }


        private void Start()
        {
            //first time run right away
            currentFrameCount = runUpdateEveryHowManyFrames;
            lastCheckTime = Time.time;
            UpdateTimeScale();
        }

        private void Update()
        {

            if (currentFrameCount <= runUpdateEveryHowManyFrames)
            {
                currentFrameCount++;
                return;
            }

            currentFrameCount = -runUpdateEveryHowManyFrames;

            if (!pause)
            {
                UpdateTime();
                UpdateClock();
            }
        }


        private void UpdateTimeScale()
        {
            _timeScale = 24 / (_targetDayLength / 60);
        }


        private float lastCheckTime;
        private float timeDiff;
        private void UpdateTime()
        {
            timeDiff = Time.time - lastCheckTime;
            //Debug.Log(timeDiff);


            lastCheckTime = Time.time;
            _timeOfDay += timeDiff * _timeScale / 86400; //seconds in day;

            //Debug.Log("Update Day");
            elapsedTime += timeDiff;

            if (_timeOfDay > 1)
            {
                elapsedTime = 0;
                _dayNumber++;
                _timeOfDay -= 1;
            }

            if (_dayNumber > _yearLength)
            {
                _yearNumber++;
                _dayNumber = 0;
            }


        }


        //TODO no need to update every frame, maybe every 100 frames?????
        private string hourString;
        private string minuteString;
        private void UpdateClock()
        {
            float time = elapsedTime / (targetDayLength * 60);
            int hour = Mathf.FloorToInt(time * 24);
            int minute = Mathf.FloorToInt(((time * 24) - hour) * 60);

            int rounded = Mathf.FloorToInt((minute / _minsSizeUpdate) * _minsSizeUpdate);


            if (elapsedHours == rounded) return;

            elapsedMins = rounded;
            elapsedHours = hour;


            //broadcast change

            onTimeChange?.Invoke();


            //// TODO: Move this to new compoenent


            if (!use24Clock && hour > 12)
                hour -= 12;

            if (hour < 10)
                hourString = "0" + hour.ToString();
            else
                hourString = hour.ToString();

            if (elapsedMins < 10)
                minuteString = "0" + elapsedMins.ToString();
            else
                minuteString = elapsedMins.ToString();

            if (clockText == null) return;

            if (use24Clock)
                clockText.text = hour + " : " + minuteString;
            else if (time > 0.5f)
                clockText.text = hour + " : " + minuteString + " pm";
            else
                clockText.text = hour + " : " + minuteString + " am";

        }






    }

}
