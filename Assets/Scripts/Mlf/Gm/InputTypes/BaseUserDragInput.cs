

using UnityEngine;

namespace Mlf.Gm.InputTypes
{

    public abstract class BaseUserDragInput : MonoBehaviour
    {

        bool _dragging;
        public Vector3 offsetPos;
        public Vector3 currentPos;

        public void setNewDragPosition(Vector3 pos)
        {
            pos.z = 0;
            currentPos = pos;
            //move the object
            this.gameObject.transform.position = pos - offsetPos;
        }

        public void startDrag(Vector3 pos)
        {
            _dragging = true;
            offsetPos = pos - transform.position;
            currentPos = pos;
        }

        public void stopDrag()
        {
            _dragging = false;
        }

        public bool _isDragging()
        {
            return _dragging;
        }


    }
}