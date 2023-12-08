using System;
using OpenTK.Platform;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main(string[] args)
    {

        /*  Adding the window stuff with OpenGL   */
        GameWindowSettings gws = GameWindowSettings.Default;
        NativeWindowSettings nws = NativeWindowSettings.Default;
        GameWindow window = new GameWindow(gws, nws);
        window.Run();

    }

}