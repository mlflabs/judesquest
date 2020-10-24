using System;
using Mlf.Characters;
using Mlf.Traffic;
using UnityEngine;

namespace Mlf.City.Buildings
{
    [CreateAssetMenu(menuName = "Mlf/City/Buildings/Building")]
    public class BuildingSO : ScriptableObject
    {

        [SerializeField] Waypoint enterenceWaypoint;
        [SerializeField] GameObject buildingPrefab;

        [SerializeField] CharacterSO[] residents;



    }

}
