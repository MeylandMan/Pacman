using OpenTK.Mathematics;
using PacMan.Graphics;
using PacMan.Main;
using OpenTK.Graphics.OpenGL4;
using static OpenTK.Graphics.OpenGL.GL;
using System.Drawing;
using OpenTK.Compute.OpenCL;
using static PacMan.Main.enums;

namespace PacMan.Engine;

internal class Obj
{
    public VAO vao;
    public string name;
    public AABB shape;
    public Vector2 position;
    public Vector2 scale;
    public Vector2 rotation;

    public Obj(string name, float mesh_width, float mesh_height, VAO vao, Vector2 position, Vector2 scale, Vector2 rotation) {
        this.vao = vao;
        this.name = name;
        this.scale = scale;
        this.rotation = rotation;
        shape = new(mesh_width, mesh_height, this.vao, "DirtTexture.jpg");
        this.position = position;
        shape.position = new(position.X, position.Y);
    }
}

internal class Rooms {

    public int ID;
    private VAO vao;
    List<Obj> ObjList = new List<Obj>();

    public Rooms(int ID, VAO vao) {
        this.ID = ID;
        this.vao = vao;
        Console.WriteLine($"Room ID : {ID} \nRoom Created");
    }
    

    public static int ChangeCurrentRoom(int ID) {
        return ID;
    }
    public void AddObject(Obj obj) {
        ObjList.Add(obj);

    }

    public void setupObjMeshes() {
        GameConsole.WriteLine($"Setup Meshes for Room {ID}.");
        foreach (Obj obj in ObjList) {

            GameConsole.WriteLine($"Setup Meshes for Object {obj.name}.");
            obj.shape.mesh.setupMesh(vao);
            GameConsole.WriteLine($"Setup Meshes for Object {obj.name} completed !");
            GameConsole.WriteLine($"Object mesh : {obj.shape.mesh}");
            GameConsole.WriteLine($"Object position : {obj.position}");
            GameConsole.WriteLine($"Object scale : {obj.scale}");
            GameConsole.WriteLine($"Object rotation : {obj.rotation}");
            Console.WriteLine("---------------------------------");

        }
    }

    public void drawObjMeshes(ShaderProgram program) {
        foreach (Obj obj in ObjList) {
            obj.shape.mesh.DrawMesh(program, obj.position, obj.scale, obj.rotation);
        }
    }

    public void DeleteObjMeshes() {
        foreach (Obj obj in ObjList) {
            GameConsole.WriteLine($"Deleting Meshes for Room {ID}.");
            Console.WriteLine($"Initialzing delete process for {obj.name}");
            obj.shape.mesh.DeleteMesh();
            GameConsole.WriteLine($"Deleting Meshes for Object {obj.name} completed !");
            Console.WriteLine("---------------------------------");
        }
        ObjList.Clear();
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

    public Mesh(float[] vertices, uint[] indices, float[] texCoords, float[] Normals, float[] Colors, VAO vao, string TexturePath = "WhiteTexture.jpg") {

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
