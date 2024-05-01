using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Libraries
{
    /// <summary>
    /// A library that handles inputs on the controller
    /// </summary>
    internal class InputLIB
    {
        public static bool GetLeftGrip()
        {
            var lg = ControllerInputPoller.instance.leftGrab;
            return lg;
        }
        public static bool GetRightGrip()
        {
            var rg = ControllerInputPoller.instance.rightGrab;
            return rg;
        }
        public static bool GetLeftTrigger()
        {
            var lt = ControllerInputPoller.instance.leftControllerIndexFloat >= 0.5f;
            return lt;
        }
        public static bool GetRightTrigger()
        {
            var rt = ControllerInputPoller.instance.rightControllerIndexFloat >= 0.5f;
            return rt;
        }
        public static bool GetAButton()
        {
            var a = ControllerInputPoller.instance.rightControllerPrimaryButton;
            return a;
        }
        public static bool GetBButton()
        {
            var b = ControllerInputPoller.instance.rightControllerSecondaryButton;
            return b;
        }
        public static bool GetYButton()
        {
            var y = ControllerInputPoller.instance.leftControllerSecondaryButton;
            return y;
        }
        public static bool GetXButton()
        {
            var x = ControllerInputPoller.instance.leftControllerPrimaryButton;
            return x;
        }
    }
}
