using GraveDefensor.Engine.Core;
using GraveDefensor.Shared;
using System;

namespace GraveDefensor.Windows
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ScreenInfo.Default = ScreenInfo.Window(768, 1280, hasMouse: true);
            using (var game = new GraveDefensorGame())
            {
                game.Run();
            }
        }
    }
#endif
}
