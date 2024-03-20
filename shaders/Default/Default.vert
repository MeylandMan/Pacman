#version 410

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec2 in_TexCoord;
// layout(location = 2) in vec3 in_normal; // (Unused for now)
layout(location = 3) in vec4 in_Color;

out vec4 Colors;
out vec2 texCoord;

// Uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
    mat4 MATRIX_VIEW_PROJECTION = model * view * projection;

    gl_Position = vec4(in_Position, 1.0) * MATRIX_VIEW_PROJECTION;
    texCoord = in_TexCoord;
    Colors = in_Color;
}