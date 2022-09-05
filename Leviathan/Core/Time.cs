using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core
{
    public static class Time
    {
        /// <summary>
        /// The frame delta 
        /// </summary>
        public static float FrameDelta { get; set; }

        /// <summary>
        /// The current time in UTC time stamp
        /// </summary>
        public static long UTCNow { get { return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; } }

        public static DateTime Now { get { return DateTime.Now; } }
    }
}
