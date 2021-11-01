using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Sandbox
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DHEntry
    {
        public int id;                 //Link-No
        public float Theta { get; set; } // Angle that the Z axis must turn in order to line up the respective X axis with the previous link
        public float Alpha { get; set; } //Angle that the X axis must turn in order to match Z axises of previous joint
        public float X_Displacement { get; set; } // The offset between Joint n-1 to n in X axis
        public float Z_Displacement { get; set; } // The offset between Joint n-1 to n in Z axis
    }
}
