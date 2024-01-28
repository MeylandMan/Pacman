using OpenTK.Mathematics;
using PacMan.Graphics;
using PacMan.Main;
using OpenTK.Graphics.OpenGL4;

namespace PacMan.Engine;

internal class Obj
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 rotation;
    public Vector2 scale;
    public Mesh mesh;

    public Obj(Vector2 Position, Vector2 Rotation, Vector2 Scale)
    {
        this.position = Position;
        this.rotation = Rotation;
        this.scale = Scale;
    }

    public static void LoadObjects(Mesh mesh, float[] vertices, uint[] indices, float[] texCoords, string textureFilePath) {
        mesh = new(vertices, indices, texCoords, textureFilePath);
    }
    public static void DrawObjects(Mesh mesh, ShaderProgram program) { mesh.DrawMesh(program); }

    public static void DeleteObjects(Mesh mesh){ mesh.DeleteMesh(); }
}

internal class Mesh {

    // Graphics pipelines variables
    private VAO vao;
    private VBO textureVBO;
    private IBO ibo;
    private Texture texture;

    public Mesh(float[] vertices, uint[] indices, float[] texCoords, string textureFilePath) {

        vao = new();
        VBO vbo = new(vertices);
        vao.LinkToVAO(0, 3, vbo);

        textureVBO = new(texCoords);
        vao.LinkToVAO(1, 2, textureVBO);
        textureVBO.UnBindVBO();

        ibo = new(indices);
        ibo.UnbindIBO();

        texture = new(textureFilePath);
    }

    public void DrawMesh(ShaderProgram program)
    {
        vao.BindVAO();
        ibo.BindIBO();
        texture.BindTexture();

        //Transformation matrices
        Matrix4 model = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.CreateOrthographic(Consts.WIDTH_ASPECT, 1f, 1f, -1f);

        int modelLocation = GL.GetUniformLocation(program.ID, "model");
        int viewLocation = GL.GetUniformLocation(program.ID, "view");
        int projectionlLocation = GL.GetUniformLocation(program.ID, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionlLocation, true, ref projection);

    }
    public void DeleteMesh()
    {

        vao.DeleteVAO();
        textureVBO.DeleteVBO();
        ibo.DeleteIBO();
    }
}
