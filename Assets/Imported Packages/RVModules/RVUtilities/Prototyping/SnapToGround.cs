using UnityEngine;

namespace RVModules.RVUtilities.PrototypingAndTesting
{
    public class SnapToGround : MonoBehaviour
    {
        public LayerMask raymask;

        public Vector3 targetPosOffet;

        public Vector3 startPointOffset;

        // Use this for initialization
        void Start()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + startPointOffset, Vector3.down, out hit, float.MaxValue, raymask))
            {
                transform.position = hit.point + targetPosOffet;
            } 
        }
    }
}