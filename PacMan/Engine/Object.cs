using OpenTK.Mathematics;
using PacMan.Graphics;
using PacMan.Main;
using OpenTK.Graphics.OpenGL4;

namespace PacMan.Engine;

internal class Obj
{
    public AABB square;
    public Obj(float mesh_width, float mesh_height) {

        square = new(mesh_width, mesh_height);
    }
}

internal class Mesh {

    // Graphics pipelines variables
    public float[] vertices;
    public uint[] indices;
    public float[] texCoords;
    public float[] Normals;
    public float[] Colors;

    private VAO vao;
    private VBO textureVBO;
    private VBO NormalVBO;
    private VBO ColorsVBO;
    private IBO ibo;
    private Texture texture;
    private string TexturePath;

    public Mesh(float[] vertices, uint[] indices, float[] texCoords, float[] Normals, float[] Colors,  string TexturePath = "") {

        this.vertices = vertices;
        this.indices = indices;
        this.texCoords = texCoords;
        this.TexturePath = TexturePath;
        this.Normals = Normals;
        this.Colors = Colors;

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

        ColorsVBO = new(Colors);
        vao.LinkToVAO(3, 4, ColorsVBO);
        ColorsVBO.UnBindVBO();

        ibo = new(indices);
        ibo.UnbindIBO();
        texture = new(TexturePath);
    }
    public void DrawMesh(ShaderProgram program, Vector2 position, Vector2 scale, Vector2 rotation)
    {
        program.BindProgram();
        vao.BindVAO();
        ibo.BindIBO();
        texture.BindTexture();

        //Transformation matrices
        Matrix4 model = Matrix4.Identity;
        model *= Matrix4.CreateScale(new Vector3(scale.X, scale.Y, 0f));
        model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X))*
                 Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y))*
                 Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0f));
        model *= Matrix4.CreateTranslation(position.X, position.Y, 0f);

        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.CreateOrthographic(Consts.WIDTH_ASPECT, 1f, 1f, -1f);

        int modelLocation = GL.GetUniformLocation(program.ID, "model");
        int viewLocation = GL.GetUniformLocation(program.ID, "view");
        int projectionlLocation = GL.GetUniformLocation(program.ID, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionlLocation, true, ref projection);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    public void DeleteMesh()
    {
        vao.DeleteVAO();
        textureVBO.DeleteVBO();
        ibo.DeleteIBO();
    }
}
