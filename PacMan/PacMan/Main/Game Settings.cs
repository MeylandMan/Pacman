using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Diagnostics;
using StbImageSharp;
using PacMan.Engine.Graphics;

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

    VAO vao;
    VBO vbo;
    IBO ibo;
    ShaderProgram shaderProgram;
    Texture texture;

    int window_scale = 1, w, h; //width and height
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
        //AspectRatio = (4, 3);

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

        vao = new VAO();
        vbo = new VBO(vertices);

        vao.LinkToVAO(0, 3, vbo);

        ibo = new IBO(indices);

        shaderProgram = new ShaderProgram("Default.vert", "Default.frag");

        texture = new Texture("DirtTexture.jpg");





    }
    protected override void OnUnload()
    {
        // Always deleting the stuffs we don't need anymore
        vao.DeleteVAO();
        shaderProgram.DeleteProgram();
        texture.DeleteTexture();
        ibo.DeleteIBO();

        Debug.Close();
        GameConsole.WriteLine("Debug ended.");
        Environment.Exit(0);
        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {

        GL.ClearColor(0.6f, 0.3f, 0.75f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);


        //Draw our triangle
        shaderProgram.BindProgram();
        vao.BindVAO();
        ibo.BindIBO();
        texture.BindTexture();

        //Transformation matrices
        Matrix4 model = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.CreateOrthographic(Consts.WIDTH_ASPECT, 1f, 1f, -1f);

        int modelLocation = GL.GetUniformLocation(shaderProgram.ID, "model");
        int viewLocation = GL.GetUniformLocation(shaderProgram.ID, "view");
        int projectionlLocation = GL.GetUniformLocation(shaderProgram.ID, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionlLocation, true, ref projection);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

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