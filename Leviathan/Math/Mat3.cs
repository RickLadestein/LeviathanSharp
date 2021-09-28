using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Leviathan.Math
{
    //TODO Add further functionality: Mat3
    [StructLayout(LayoutKind.Sequential)]
    public struct Mat3
    {
        public Vector3f Row0;
        public Vector3f Row1;
        public Vector3f Row2;
    }
}
