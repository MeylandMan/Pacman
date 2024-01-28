using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace PacMan.Engine.Graphics;
internal class VBO {
    public int ID;

    public VBO(float[] data) {

        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
    }

    public void BindVBO() { GL.BindBuffer(BufferTarget.ArrayBuffer, ID); }
    public void UnBindVBO() { GL.BindBuffer(BufferTarget.ArrayBuffer, 0); }
    public void DeleteVBO() { GL.DeleteBuffer(ID); }
}

internal class VAO {

    public int ID;

    public VAO() {
        ID = GL.GenVertexArray();
        GL.BindVertexArray(ID);
    }

    public void LinkToVAO(int location , int size, VBO vbo) {
        BindVAO();
        vbo.BindVBO();
        GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(location);
        UnbindVAO();

    }

    public void BindVAO() { GL.BindVertexArray(ID); }
    public void UnbindVAO() { GL.BindVertexArray(0); }
    public void DeleteVAO() { GL.DeleteVertexArray(ID); }
}

internal class IBO
{

    public int ID;

    public IBO(uint[] data)
    {
        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length*sizeof(float), data, BufferUsageHint.StaticDraw);
    }

    public void BindIBO() { GL.BindBuffer(BufferTarget.ArrayBuffer, ID); }
    public void UnbindIBO() { GL.BindBuffer(BufferTarget.ArrayBuffer, 0); }
    public void DeleteIBO() { GL.DeleteBuffer(ID); }
}

internal class Texture
{

    public int ID;

    public Texture(string filePath)
    {
        ID = GL.GenTexture();

        // Activate the texture in the unit
        GL.ActiveTexture(TextureUnit.Texture0);
        BindTexture();

        // --- TEXTURES PARAMETERS
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (uint)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (uint)TextureMagFilter.Nearest);

        //Load The Image
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult texture2D = ImageResult.FromStream(File.OpenRead("../../../Media/Textures/" + filePath), ColorComponents.RedGreenBlueAlpha);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture2D.Width, texture2D.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture2D.Data);

        UnbindTexture();

    }

    public void BindTexture() { GL.BindTexture(TextureTarget.Texture2D, ID); }
    public void UnbindTexture() { GL.BindTexture(TextureTarget.Texture2D, 0); }
    public void DeleteTexture() { GL.DeleteTexture(ID); }
}

internal class ShaderProgram
{

    public int ID;

    public ShaderProgram(string vertexShaderFilePath, string fragmentShaderFilePath) {

        //Create the shader program
        ID = GL.CreateProgram();

        //Vertex Shader setup
        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, LoadShaderSource(vertexShaderFilePath));
        GL.CompileShader(VertexShader);

        //Fragment shader setup
        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, LoadShaderSource(fragmentShaderFilePath));
        GL.CompileShader(FragmentShader);

        //Attaching the vertex and fragment to the shaderProgram
        GL.AttachShader(ID, VertexShader);
        GL.AttachShader(ID, FragmentShader);

        //Link them together
        GL.LinkProgram(ID);

        //Delete the shaders
        GL.DeleteShader(VertexShader);
        GL.DeleteShader(FragmentShader);

    }

    public void BindProgram() { GL.UseProgram(ID); }
    public void UnbindProgram() { GL.UseProgram(0); }
    public void DeleteProgram() { GL.DeleteProgram(ID); }
    public static string LoadShaderSource(string filePath)
    {
        string shaderSource = "";

        try
        {

            using (StreamReader reader = new StreamReader("../../../Graphics/Shaders/" + filePath))
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