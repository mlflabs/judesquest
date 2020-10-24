using System;
using Mlf.Characters;
using Mlf.TimeDate;
using UnityEngine;

namespace Mlf.City.Actions
{
    public static class CharactersEventActions
    {

        public static void CharacterEvents(CharacterContext context, int hour, int minute)
        {
            //Debug.Log("CharacterEvents");

            for (int i = 0; i < context.characterSO.scheduleEvents.Length; i++)
            {
                //are we in required state
                if (context.characterSO.scheduleEvents[i].requiredCurrentLocation !=
                context.currentLocation)
                    continue;

                //do we have exact time required
                if (context.characterSO.scheduleEvents[i].exactTimeDateRequired == true &&
                  (context.characterSO.scheduleEvents[i].hour != hour ||
                  context.characterSO.scheduleEvents[i].minute != minute))
                    continue;


                if (context.characterSO.scheduleEvents[i].exactTimeDateRequired == false)
                {
                    if (context.characterSO.scheduleEvents[i].bufferHours != 0)
                    {
                        if (context.characterSO.scheduleEvents[i].hour > hour +
                            context.characterSO.scheduleEvents[i].bufferHours)
                            continue;
                    }

                    if (context.characterSO.scheduleEvents[i].hour > hour)
                        continue;

                    if (context.characterSO.scheduleEvents[i].hour == hour &&
                        context.characterSO.scheduleEvents[i].minute > minute)
                        continue;


                }





                context.actions.Enqueue(new ScheduleActionStates
                {
                    action = context.characterSO.scheduleEvents[i],
                    state = ActionState.Added
                });

                CityManager.instance.instantiateCharacter(
                    context, context.getCurrentLocation().transform);

            }
        }
    }
}
