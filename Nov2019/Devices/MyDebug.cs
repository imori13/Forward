using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    public static class MyDebug
    {
        public static bool DebugMode { get; private set; }
        public static bool DebugTimeStopMode { get; private set; }

        public static void Initialize()
        {
            DebugMode = false;
        }

        public static void Update()
        {
            if (Input.GetKeyDown(Keys.F12))
            {
                DebugMode = !DebugMode;
            }

            if (Input.GetKeyDown(Keys.F11))
            {
                DebugTimeStopMode = !DebugTimeStopMode;
            }
        }
    }
}
