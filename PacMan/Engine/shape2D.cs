using PacMan.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Engine;

internal class AABB
{
    VAO vao;
    public Vector2 position;

    public AABB( float width, float height, VAO vao, string texturepath = "WhiteTexture.jpg") {
        this.vao = vao;
        
        float vert_default_coords = 0.1f;
        float[] vertices = {
            -vert_default_coords, vert_default_coords, 0f, // Top Left - 0
            width*vert_default_coords*0.1f, vert_default_coords, 0f, // Top Right - 1
            -vert_default_coords, height*vert_default_coords*0.1f, 0f, // Bottom left - 2
            width*vert_default_coords*0.1f, height*vert_default_coords*0.1f,0f // Bottom right - 3
        };

        uint[] indices =
        {
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

        float[] normals = {
            -1f, 0f, 0f,
            1f, 0f, 0f,
            -1f, 0f, 0f,
            1f, 0f, 0f,
        };

        float[] Colors =
        {
            0.5f, 0.5f, 0.5f, 1f,
            0.5f, 0.5f, 0.5f, 1f,
            0.5f, 0.5f, 0.5f, 1f,
            0.5f, 0.5f, 0.5f, 1f
        };
        Mesh mesh = new(vertices, indices, texCoords, normals, Colors, vao, texturepath);
    }
}
