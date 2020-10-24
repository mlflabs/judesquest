using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Mlf.Traffic
{
    public enum WaypointTypes { normal, home, destination };

    public class Waypoint : MonoBehaviour
    {


        public List<WaypointLink> neighbours = new List<WaypointLink>();
        [SerializeField] public WaypointTypes type = WaypointTypes.normal;


        public Waypoint previous;
        public float distance;

        [Range(0, 5)] public float width = 1f;

        public GameObject AddLinkWaypoint;
        public bool Bidirectional = true;

        private void Start()
        {

            PathManager.instance.AddWaypoint(this);
        }



        public Vector3 GetPosition()
        {
            //Vector3 minBound = transform.position + transform.right * width / 2f;
            //Vector3 maxBount = transform.position - transform.right * width / 2f;

            //return Vector3.Lerp(minBound, maxBount, Random.Range(0f, 1f));
            return transform.position;
        }

        public void AddLink(Waypoint target)
        {

            if (target == this) return; //can't connect to itself


            foreach (var link in neighbours)
            {
                if (link.neighbour == target) return; //already have this link
            }

            neighbours.Add(new WaypointLink(this, target));
        }

        public void CheckDuplicateLinks()
        {
            for (var x = 0; x < neighbours.Count; x++)
            {
                for (var y = x + 1; y < neighbours.Count; y++)
                {
                    if (neighbours[x].neighbour == neighbours[y].neighbour)
                    {
                        Debug.Log("Duplicate at: " + x + ", " + y);
                        neighbours.Remove(neighbours[x]);

                    }
                }
            }
        }



    }


}

