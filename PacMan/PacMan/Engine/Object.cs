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
    public float[] vertices;
    public uint[] indices;
    public float[] texCoords;
    public float[] Normals;

    private VAO vao;
    private VBO textureVBO;
    private VBO NormalVBO;
    private IBO ibo;
    private Texture texture;
    private string TexturePath;

    public Mesh(float[] vertex, uint[] index, float[] texCoordinate, float[] in_Normal, string textureFilePath) {

        vertices = vertex;
        indices = index;
        texCoords = texCoordinate;
        TexturePath = textureFilePath;
        Normals = in_Normal;

        setupMesh();
    }
    public void setupMesh() {
        vao = new();
        VBO vbo = new(vertices);
        vao.LinkToVAO(0, 3, vbo);
        
        textureVBO = new(texCoords);
        vao.LinkToVAO(1, 2, textureVBO);
        textureVBO.UnBindVBO();

        NormalVBO = new(Normals);
        vao.LinkToVAO(2, 3, NormalVBO);
        NormalVBO.UnBindVBO();

        ibo = new(indices);
        ibo.UnbindIBO();

        texture = new(TexturePath);
    }
    public void DrawMesh(ShaderProgram program)
    {
        program.BindProgram();
        vao.BindVAO();
        ibo.BindIBO();
        texture.BindTexture();
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    public void DeleteMesh()
    {
        vao.DeleteVAO();
        textureVBO.DeleteVBO();
        ibo.DeleteIBO();
    }
}
