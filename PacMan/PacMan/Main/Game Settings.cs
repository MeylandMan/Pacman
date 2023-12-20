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

    float[] vertices = {
        0f, 1f, 0f, //Top Vertex
        -1f, -1f, 0f, //Bottom left Vertex
        1f, -1f, 0f // Bottom right Vertex
    };

    //Render Pipeline variables
    int vao, shaderProgram;

    int w, h;//width and height
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Console.WriteLine("Initializing the Game Window...");

        //Centering this on monitor
        w = Consts.SCREEN_WIDTH;
        h = Consts.SCREEN_HEIGHT;
        CenterWindow(new Vector2i(w, h));
        GameConsole.WriteLine("Resizing Window : " + Consts.SCREEN_WIDTH + ", " + Consts.SCREEN_HEIGHT);

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

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // Bind the vao
        GL.BindVertexArray(vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vao, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Unbinding the vbo
        GL.BindVertexArray(0); // Unbinnding the vao

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

            using(StreamReader reader = new StreamReader("../../../../Shaders/" + filePath)) {
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