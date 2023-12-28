using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Diagnostics;
using StbImageSharp;

namespace PacMan.Main;

internal class Game : GameWindow
{
    //Frame Per second
    public double TargetUpdateFrequency = 30.0, TargetRenderFrequency = 30.0;

    float[] vertices = {

        -0.5f, 0.5f, 0f, //Top left Vertex - 0 
        0.5f, 0.5f, 0f, //Top right Vertex - 1
        -0.5f, -0.5f, 0f, // Bottom left Vertex - 2
        0.5f, -0.5f, 0f // Bottom right Vertex - 3

    };

    float[] texCoords = {
        0f, 1f,
        1f, 1f,
        1f, 0f,
        0f, 0f
    };
    uint[] indices = {
        //Top triangle
        0, 1, 3,

        //bottom triangle
        2, 3, 0
    };
    //Render Pipeline variables
    int vao, vbo, shaderProgram, window_scale = 1, ebo, TextureID;

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

        // Bind the vao
        GL.BindVertexArray(vao);

        // --- vertices VBO ---
        int TextureVBO, slot = 0;

        vbo = GL.GenBuffer();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, slot);
        // Put the vertex VBO in slot 0 of our VAO

        GL.VertexAttribPointer(slot, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(slot);
        // GL.EnableVertexArrayAttrib(vao, slot); WORKS FOR OPENGL 4.5 OR ABOVE

        GL.BindBuffer(BufferTarget.ArrayBuffer, slot); //Unbinding the vbo

        // --- Texture VBO ---

        TextureVBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

        // Put the texture VBO in slot 1 of our VAO
        GL.VertexAttribPointer(slot+1, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(slot+1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, slot + 1); //Unbinding the texture
        //GL.BindVertexArray(slot); // Unbinnding the vao

        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, slot); // Unbinding the ebo

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

        // --- TEXTURES ----
        TextureID = GL.GenTexture();

        // Activate the texture in the unit
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, TextureID);

        //Textures parameters
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Load Image
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult pacmanTex = ImageResult.FromStream(File.OpenRead("../../../Media/Textures/Pacman/walking/pacman_0.png"), ColorComponents.RedGreenBlueAlpha);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, pacmanTex.Width, pacmanTex.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pacmanTex.Data);

        GL.BindTexture(TextureTarget.Texture2D, slot);

    }
    protected override void OnUnload()
    {
        // Always deleting the stuffs we don't need anymore
        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ebo);
        GL.DeleteProgram(shaderProgram);
        GL.DeleteTexture(TextureID);

        Debug.Close();
        GameConsole.WriteLine("Debug ended.");
        Environment.Exit(0);
        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {

        GL.ClearColor(0f, 0f, 0f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);


        //Draw our square
        GL.UseProgram(shaderProgram);

        GL.BindTexture(TextureTarget.Texture2D, TextureID);


        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

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