using System;

namespace RVModules.RVUtilities
{
    /// <summary>
    /// Works like bool, but calls some method on value change, property-simulator-like-thingy, so fields can be used as props. Can be used just like normal bool.
    /// (Unity can't display props in inspector and is often not worth time to create custom editor for the)
    /// </summary>
    public class ActionBool
    {
        private bool boolValue;
        private Action<bool> onValueChange;

        public ActionBool(Action<bool> onValueChange)
        {
            this.onValueChange = onValueChange;
        }

        public bool BoolValue
        {
            get
            {
                return boolValue;
            }

            set
            {
                if (value != boolValue && onValueChange != null)
                    onValueChange.Invoke(value);
                boolValue = value;
            }
        }

        public static implicit operator bool(ActionBool _actionBool)
        {
            return _actionBool.boolValue;
        }

        //public static implicit operator ActionBool(bool _value)
        //{
        //    ActionBool ab = new ActionBool(null);
        //    ab.BoolValue = _value;
        //    return ab;
        //}
    }
}
