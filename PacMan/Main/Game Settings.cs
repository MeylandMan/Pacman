using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Diagnostics;
using PacMan.Graphics;
using PacMan.Engine;

namespace PacMan.Main;

internal class Game : GameWindow
{

    //Frame Per second
    public double TargetUpdateFrequency = 30.0, TargetRenderFrequency = 30.0;

    float[] vertices = {
        -0.1f, 0.1f, 0f, //Top Left Vertex - 0
        0.1f, 0.1f, 0f, //Top Right Vertex - 1
        -0.1f, -0.1f, 0f, // Bottom Left Vertex - 2
        0.1f, -0.1f, 0f // Bottom Right Vertex - 3
    };
    float[] normals = {
        -1f, 0f, 0f,
        1f, 0f, 0f,
        -1f, 0f, 0f,
        1f, 0f, 0f,
    };
    uint[] indices = {
        //First triangle
        0, 1, 2,

        // Second triangle
        1, 2, 3
    };

    float[] texCoords = {
        0f, 1f, // Red
        1f, 1f, // Green
        0f, 0f, // Black
        1f, 0f, // Yellow
    };

    Mesh mesh1;
    Mesh mesh2;

    ShaderProgram program;


    int w, h, window_scale = 1; //width and height
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Console.WriteLine("Initializing the Game Window...");

        //Centering this on monitor
        w = Consts.SCREEN_WIDTH * window_scale;
        h = Consts.SCREEN_HEIGHT * window_scale;
        CenterWindow(new Vector2i(w, h));
        GameConsole.WriteLine("Resizing Window : " + Consts.SCREEN_WIDTH + ", " + Consts.SCREEN_HEIGHT);
        //not allowing the player to cgange the size of the Window by their own
        WindowBorder = WindowBorder.Fixed;

        //The Aspect Ratio gonna always be 4:3
        AspectRatio = (4, 3);

        //Title of the Game
        Title = "PacMan - Recreation by M.Meyland";

    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        w = e.Width;
        h = e.Height;
        GameConsole.WriteLine("Window resized : " + w + ", " + h);
    }
    protected override void OnLoad()
    {

        base.OnLoad();
        mesh1 = new(vertices, indices, texCoords, normals, "DirtTexture.jpg");
        mesh2 = new(vertices, indices, texCoords, normals, "DirtTexture.jpg");

        mesh1.setupMesh();
        mesh2.setupMesh();

        program = new ShaderProgram("Default.vert", "Default.frag");

    }
    protected override void OnUnload()
    {
        // Always deleting the stuffs we don't need anymore

        mesh1.DeleteMesh();
        mesh2.DeleteMesh();

        Debug.Close();
        GameConsole.WriteLine("Debug ended.");
        Environment.Exit(0);

        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {

        GL.ClearColor(0.6f, 0.3f, 0.75f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        
        //Draw
        Vector2 scale = new Vector2(1f, 1f);
        Vector2 rotation = new Vector2(1f, 0f);

        mesh1.DrawMesh(program, new Vector2(-0.2f, 0f), scale, rotation);
        mesh2.DrawMesh(program, new Vector2(0.2f, 0f), scale, rotation);

        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
 
    protected override void OnUpdateFrame(FrameEventArgs args)
    {

        base.OnUpdateFrame(args);
    }
}

internal class GameConsole
{
    public static void WriteLine(string message)
    {
        Console.WriteLine("-> " + message);
    }

    public static void Beep()
    {
        Console.Beep();
    }
}