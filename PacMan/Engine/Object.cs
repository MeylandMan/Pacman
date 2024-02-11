using OpenTK.Mathematics;
using PacMan.Graphics;
using PacMan.Main;
using OpenTK.Graphics.OpenGL4;

namespace PacMan.Engine;

internal class Obj
{
    VAO vao;
    public AABB mesh;
    Vector2 position;
    Vector2 scale;
    Vector2 rotation;

    public Obj(float mesh_width, float mesh_height, VAO vao, Vector2 position) {
        this.vao = vao;
        mesh = new(mesh_width, mesh_height, this.vao, "DirtTexture.jpg");
        this.position = position;
        mesh.position = new(position.X, position.Y);
    }
}

internal class Rooms {

    public int ID;
    private VAO vao;
    List<Obj> ObjList = new List<Obj>();

    public Rooms(int ID) {
        this.ID = ID;


    }
    
    public static int ChangeCurrentRoom(int ID) {
        return ID;
    }
    public void AddObject(Obj obj) {
        ObjList.Add(obj);

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

    public Mesh(float[] vertices, uint[] indices, float[] texCoords, float[] Normals, float[] Colors, VAO vao, string TexturePath = "") {

        this.vertices = vertices;
        this.indices = indices;
        this.texCoords = texCoords;
        this.TexturePath = TexturePath;
        this.Normals = Normals;
        this.Colors = Colors;

        setupMesh(vao);
    }
    public void setupMesh(VAO vao) {
        this.vao = vao;
        VBO vbo = new(vertices);
        this.vao.LinkToVAO(0, 3, vbo);
        
        textureVBO = new(texCoords);
        this.vao.LinkToVAO(1, 2, textureVBO);
        textureVBO.UnBindVBO();

        NormalVBO = new(Normals);
        this.vao.LinkToVAO(2, 3, NormalVBO);
        NormalVBO.UnBindVBO();

        ColorsVBO = new(Colors);
        this.vao.LinkToVAO(3, 4, ColorsVBO);
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
