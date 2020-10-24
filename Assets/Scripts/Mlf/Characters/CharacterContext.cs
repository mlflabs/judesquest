using System;
using System.Collections.Generic;
using Mlf.Traffic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mlf.Characters
{
    //In City is in play
    [Serializable]
    public enum CharacterLocations { Home, Work }

    [Serializable]
    public enum ActionState { Added, InProgress, Finished }

    [Serializable]
    public class ScheduleActionStates
    {
        public ScheduleEvent action;
        public ActionState state;
    }

    [Serializable]
    public class LocationWaypointLink
    {
        public CharacterLocations location;
        public Waypoint waypoint;
    }

    [Serializable]
    public class CharacterContext
    {

        public int Level = 1;
        public CharacterLocations currentLocation = CharacterLocations.Home;
        public bool onStage = false;
        public CharacterSO characterSO = null;

        [SerializeField]
        public LocationWaypointLink[] locations;
        [SerializeField]
        public Queue<ScheduleActionStates> actions = new Queue<ScheduleActionStates>();


        public Waypoint getCurrentLocation()
        {
            for (int i = 0; i < locations.Length; i++)
            {
                if (currentLocation == locations[i].location)
                    return locations[i].waypoint;
            }


            return null;
        }

        public Waypoint getNextEventLocation()
        {
            for (int i = 0; i < locations.Length; i++)
            {
                if (actions.Peek().action.destination == locations[i].location)
                    return locations[i].waypoint;
            }


            return null;
        }



    }
}