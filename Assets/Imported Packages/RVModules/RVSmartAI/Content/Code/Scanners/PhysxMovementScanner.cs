// Created by Ronis Vision. All rights reserved
// 27.10.2019.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RVModules.RVSmartAI.Content.Code.Scanners
{
    public class PhysxMovementScanner : MonoBehaviour, IMovementScanner
    {
        #region Fields

        /// <summary>
        /// Mask used for getting walkable positions
        /// </summary>
        [FormerlySerializedAs("walkableLayerMask")]
        [SerializeField]
        private LayerMask obstaclesLayerMask;

        /// <summary>
        /// Mask used for checking ground height
        /// </summary>
        [SerializeField]
        private LayerMask groundLayerMask;

        /// <summary>
        /// At which height shoot raycasts to scan positions for FindWalkablePositions
        /// </summary>
        [SerializeField]
        private float rayHeight = 3f;

        /// <summary>
        /// Defines how many SphereCasts will be used to get positions
        /// To approx how many casts will called for every scan:
        /// square(_range * 2 / walkablePositionsResolution)
        /// </summary>
        [SerializeField]
        private float walkablePositionsResolution = 2f;

        [SerializeField]
        private List<Vector3> positions = new List<Vector3>();

        private RaycastHit[] groundHitsResultBuffer = {new RaycastHit()};

        #endregion

        #region Public methods

        public List<Vector3> FindWalkablePositions(Vector3 _position, float _range)
        {
            var pos = Vector3.zero;
            positions.Clear();
            var scansCount = (int) (_range / walkablePositionsResolution);
            var scanStart = -scansCount;

            for (var y = scanStart; y < scansCount + 1; y++)
            for (var x = scanStart; x < scansCount + 1; x++)
            {
                if (x == 0 && y == 0) continue;
                pos.x = _position.x + x * walkablePositionsResolution;
                pos.y = _position.y + rayHeight;
                pos.z = _position.z + y * walkablePositionsResolution;

                // assure we're above walkable ground, and get height for scan
                if (!Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 1000, groundLayerMask)) continue;
                //pos.y = hit.point.y + walkablePositionsResolution * .5f;
                pos.y = hit.point.y;
                
                //if (Physics.RaycastNonAlloc(pos, Vector3.down, groundHitsResultBuffer, 1000, groundLayerMask) == 0) continue;
                //pos.y = groundHitsResultBuffer[0].point.y;

                if (Physics.CheckSphere(pos, walkablePositionsResolution * .5f, obstaclesLayerMask)) continue;
                //Debug.DrawLine(pos, pos + Vector3.down * 2, Color.magenta, 1);
                //pos.y += walkablePositionsResolution * .5f;
                
                positions.Add(pos);
            }

            return positions;
        }

        #endregion
    }
}