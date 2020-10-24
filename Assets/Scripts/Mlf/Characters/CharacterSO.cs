using System;
using Mlf.Traffic;
using UnityEngine;

namespace Mlf.Characters
{

    public enum CharacterAgeTypes
    {
        Child, Teen, Adult, Elderly
    }

    public enum CharacterSex
    {
        Male, Femaile
    }


    public enum ScheduleEventTypes
    {
        GoHome, GoWork
    }

    [Serializable]
    public struct ScheduleEvent
    {
        public CharacterLocations destination;
        public CharacterLocations requiredCurrentLocation;
        public int hour;
        public int minute;
        public bool exactTimeDateRequired;
        [Help("If not exact time, you can choose a buffer")]
        public int bufferHours;

    }


    [CreateAssetMenu(menuName = "Mlf/City/Character")]
    public class CharacterSO : ScriptableObject
    {

        public string firstName;
        public string lastName;
        public string nickName;

        public CharacterAgeTypes ageType;
        public CharacterSex sex;
        public int age;

        [SerializeField]
        public ScheduleEvent[] scheduleEvents;


        public GameObject prefab;

    }

}
