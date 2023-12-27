using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace PacMan.Main;

internal class Game : GameWindow
{
    //Frame Per second
    public double TargetUpdateFrequency = 30.0, TargetRenderFrequency = 30.0;

    float[] vertices = {
        0f, 0.5f, 0f, //Top Vertex
        -0.5f, -0.5f, 0f, //Bottom left Vertex
        0.5f, -0.5f, 0f // Bottom right Vertex
    };

    //Render Pipeline variables
    int vao, shaderProgram, window_scale = 1;

    int w, h;//width and height
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Console.WriteLine("Initializing the Game Window...");

        //Centering this on monitor
        w = Consts.SCREEN_WIDTH * window_scale;
        h = Consts.SCREEN_HEIGHT * window_scale;
        CenterWindow(new Vector2i(w, h));
        GameConsole.WriteLine("Resizing Window : " + Consts.SCREEN_WIDTH + ", " + Consts.SCREEN_HEIGHT);
        //not allowing the player to cgange the size of the Window by their own
        WindowBorder =  WindowBorder.Fixed;

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

        vao = GL.GenVertexArray();

        int vbo = GL.GenBuffer(), slot = 0;
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // Bind the vao
        GL.BindVertexArray(vao);
        GL.VertexAttribPointer(slot, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(slot);
        // GL.EnableVertexArrayAttrib(vao, slot);

        // GL.BindBuffer(BufferTarget.ArrayBuffer, slot); //Unbinding the vbo
        // GL.BindVertexArray(slot); // Unbinnding the vao

        //Create the shader program
        shaderProgram = GL.CreateProgram();

        //Vertex Shader setup
        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, LoadShaderSource("Default.vert"));
        GL.CompileShader(VertexShader);

        //Fragment shader setup
        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, LoadShaderSource("Default.frag"));
        GL.CompileShader(FragmentShader);

        //Attaching the vertex and fragment to the shaderProgram
        GL.AttachShader(shaderProgram, VertexShader);
        GL.AttachShader(shaderProgram, FragmentShader);

        //Link them together
        GL.LinkProgram(shaderProgram);

        //Delete the shaders
        GL.DeleteShader(VertexShader);
        GL.DeleteShader(FragmentShader);
    }
    protected override void OnUnload()
    {
        // Always deleting the stuffs we don't need anymore
        GL.DeleteVertexArray(vao);
        GL.DeleteProgram(shaderProgram);

        Debug.Close();
        GameConsole.WriteLine("Debug ended.");
        Environment.Exit(0);
        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {

        GL.ClearColor(1f, 1f, 1f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);


        //Draw our triangle
        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {

        base.OnUpdateFrame(args);
    }

    public static string LoadShaderSource(string filePath) {
        string shaderSource = "";

        try {

            using(StreamReader reader = new StreamReader("../../../Shaders/" + filePath)) {
                shaderSource = reader.ReadToEnd();
            }
        } catch (Exception e){
            Console.WriteLine("Failed to load shader source file : " + e.Message);
        }

        return shaderSource;
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