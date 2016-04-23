using System;
using RubikCube;

namespace RubikCubeFinal
{
#if WINDOWS || XBOX
    static class Program
    {
        [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Main game = new Main())
            {
                game.Run();
            }
        }
    }
#endif
}

