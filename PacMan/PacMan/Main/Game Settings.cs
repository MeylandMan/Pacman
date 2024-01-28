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

    //Render Pipeline variables
    int vao, ebo, TextureVBO, shaderProgram, window_scale = 1, textureID;

    int w, h; //width and height
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

        vao = GL.GenVertexArray();

        // Bind the vao
        GL.BindVertexArray(vao);

        // ---- Vertices VBO ----

        int vbo = GL.GenBuffer(), slot = 0;
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        

        // put the vertex vbo in slot 0 of our VAO
        GL.VertexAttribPointer(slot, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(slot);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // ---- Texture VBO ----

        TextureVBO = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);

        GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);
        
        // put the Texture in slot 1 of our VAO
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

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

        // --- TEXTURES ---
        textureID = GL.GenTexture();

        // Activate the texture in the unit
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        // --- TEXTURES PARAMETERS
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (uint)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (uint)TextureMagFilter.Nearest);

        // Load image
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult testTexture = ImageResult.FromStream(File.OpenRead("../../../Media/Textures/DirtTexture.jpg"), ColorComponents.RedGreenBlueAlpha);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, testTexture.Width, testTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, testTexture.Data);

        //Unbind the texture
        GL.BindTexture(TextureTarget.Texture2D, 0);

    }
    protected override void OnUnload()
    {
        // Always deleting the stuffs we don't need anymore
        GL.DeleteVertexArray(vao);
        GL.DeleteProgram(shaderProgram);
        GL.DeleteTexture(textureID);
        GL.DeleteBuffer(ebo);

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
        GL.UseProgram(shaderProgram);

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        //Transformation matrices
        Matrix4 model = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.CreateOrthographic(Consts.WIDTH_ASPECT, 1f, 1f, -1f);

        int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
        int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
        int projectionlLocation = GL.GetUniformLocation(shaderProgram, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionlLocation, true, ref projection);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
 
    protected override void OnUpdateFrame(FrameEventArgs args)
    {

        base.OnUpdateFrame(args);
    }

    public static string LoadShaderSource(string filePath)
    {
        string shaderSource = "";

        try
        {

            using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
            {
                shaderSource = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
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