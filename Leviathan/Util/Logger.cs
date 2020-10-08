using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Leviathan.Util
{
    class Logger
    {

        private static Logger instance;
        public static Logger GetInstance()
        {
            if(instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        public readonly ConsoleColor ERROR_COLOR = ConsoleColor.Red;
        public readonly ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        public readonly ConsoleColor DEBUG_COLOR = ConsoleColor.Cyan;
        public readonly ConsoleColor DEFAULT_COLOR = ConsoleColor.White;

        public void LogError(string message, bool halt = false)
        {
#if DEBUG
            if(halt)
                Debugger.Break();
#endif
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine(message);
            Console.ForegroundColor = DEFAULT_COLOR;
        }

        public void LogWarning(string message, bool halt = false)
        {
#if DEBUG
            if (halt)
                Debugger.Break();
#endif
            Console.ForegroundColor = WARNING_COLOR;
            Console.WriteLine(message);
            Console.ForegroundColor = DEFAULT_COLOR;
        }

        public void LogDebug(string message, bool halt = false)
        {
#if DEBUG
            if (halt)
                Debugger.Break();
#endif
            Console.ForegroundColor = DEBUG_COLOR;
            Console.WriteLine(message);
            Console.ForegroundColor = DEFAULT_COLOR;
        }

        public void LogInfo(string message, bool halt = false)
        {
#if DEBUG
            if (halt)
                Debugger.Break();
#endif
            Console.ForegroundColor = DEFAULT_COLOR;
            Console.WriteLine(message);
        }
    }
}
