using Mlf.Characters;
using Mlf.Traffic;
using UnityEngine;



namespace Mlf.City.Buildings
{

    public class BuildingCmp : MonoBehaviour, ILocation
    {

        [SerializeField] public Waypoint waypoint;
        [SerializeField] public BuildingSO props;
        [SerializeField] private CharacterContext[] _residents;
        [SerializeField] private LawnCmp lawn = null;

        public Waypoint WaypointEntry { get => waypoint; set => waypoint = value; }



        private void Start()
        {

            if (props == null)
            {
                Debug.LogError("Missing BuildingSO");
                return;
            }

            if (waypoint == null)
                Debug.LogWarning("Home enterence waypoint is null");

            CityManager.instance.AddBuilding(this);
            for (var i = 0; i < _residents.Length; i++)
            {
                CityManager.instance.AddCharacter(_residents[i]);
            }

            //EventManager.instance.oneSecInterval += oneSecFun;
        }

        private bool spawned = false;


        private void oneSecFun()
        {
            if (spawned) return;

            if (_residents == null || _residents.Length == 0) return;

            for (int i = 0; i < _residents.Length; i++)
            {
                //GameObject c = Instantiate(_residents[i].prefab);
                //c.transform.position = waypoint.transform.position;
            }

            spawned = true;
        }

        public void executeCharacterEvent()
        {

        }




        #region List Setters

        #endregion


    }


}

