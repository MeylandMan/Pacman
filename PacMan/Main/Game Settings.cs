using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Diagnostics;
using PacMan.Graphics;
using PacMan.Engine;
using static PacMan.Main.enums;

namespace PacMan.Main;

internal class Game : GameWindow
{

    //Frame Per second
    public double TargetUpdateFrequency = 30.0, TargetRenderFrequency = 30.0;

    // Shader Program
    ShaderProgram program;
    VAO mainSurface;

    int w, h, window_scale = 1; //width and height
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {

        Console.WriteLine("Initializing the Game Window...");

        //Centering this on monitor
        w = Consts.SCREEN_WIDTH * window_scale;
        h = Consts.SCREEN_HEIGHT * window_scale;
        CenterWindow(new Vector2i(w, h));
        GameConsole.WriteLine("Resizing Window : " + Consts.SCREEN_WIDTH + ", " + Consts.SCREEN_HEIGHT);
        //not allowing the player to change the size of the Window by their own
        WindowBorder = WindowBorder.Fixed;

        //The Aspect Ratio gonna always be 4:3
        AspectRatio = (4, 3);

        //Title of the Game
        Title = "PacMan - Recreation by M.Meyland";

    }

    protected override void OnResize(ResizeEventArgs e) {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        w = e.Width;
        h = e.Height;
        GameConsole.WriteLine("Window resized : " + w + ", " + h);
    }
    Obj ObjetctTest1;
    Obj ObjetctTest2;
    Rooms temp_room;

    protected override void OnLoad() {

        base.OnLoad();
        Console.WriteLine("Setup mainsurface vao...");
        mainSurface = new();
        Console.WriteLine($"Setup completed ! \nvao : {mainSurface.ID}");

        Console.WriteLine("Creating Objects.");
        ObjetctTest1 = new("Object test 1",1f, 1f, mainSurface, new Vector2(-0.2f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0f));
        Console.WriteLine($"Creating Objects completed ! \nObject : {ObjetctTest1.name}");
        ObjetctTest2 = new("Object test 2", 1f, 1f, mainSurface, new Vector2(0.2f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0f));
        Console.WriteLine($"Creating Objects completed ! \nObject : {ObjetctTest2.name}");
        temp_room = new((int)ROOM_ORDER.TEMP, mainSurface);

        Console.WriteLine($"Adding objects to the room {temp_room.ID}");
        temp_room.AddObject(ObjetctTest1);
        temp_room.AddObject(ObjetctTest2);
        Console.WriteLine($"Objects added.");
        Console.WriteLine($"{ObjetctTest1.name} vao : {ObjetctTest1.vao.ID}");
        Console.WriteLine($"{ObjetctTest2.name} vao : {ObjetctTest2.vao.ID}");

        temp_room.setupObjMeshes();
        program = new ShaderProgram("Default.vert", "Default.frag");

    }
    protected override void OnUnload() {
        // Always deleting the stuffs we don't need anymore
        temp_room.DeleteObjMeshes();
        Debug.Close();
        GameConsole.WriteLine("Debug ended.");
        Environment.Exit(0);

        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args) {

        GL.ClearColor(0.6f, 0.3f, 0.75f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);


        //Draw
        temp_room.drawObjMeshes(program);
        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
 
    protected override void OnUpdateFrame(FrameEventArgs args) {

        base.OnUpdateFrame(args);
    }
}

internal class GameConsole {
    public static void WriteLine(string message)
    {
        Console.WriteLine("-> " + message);
    }

    public static void Beep()
    {
        Console.Beep();
    }
}