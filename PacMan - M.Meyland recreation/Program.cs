using System;
using OpenTK.Platform;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

public class Program
{ 
    static void Main(string[] args)
    {
        Vector2i gameResolution = new Vector2i(640, 480);
        int fps = 60;

        /*  Adding the window stuff with OpenGL   */
        GameWindowSettings gws = GameWindowSettings.Default;
        NativeWindowSettings nws = NativeWindowSettings.Default;

        //Set the FPS
        gws.UpdateFrequency = fps;

        //get the version of OpenGL
        nws.APIVersion = Version.Parse("4.1.0");

        //Get the size of the window (Game resolution)
        nws.ClientSize = gameResolution;

        //Get the title of the game window
        nws.Title = "PacMan - recreation made by M.Meyland";

        GameWindow window = new GameWindow(gws, nws);
        window.Run();

    }

}