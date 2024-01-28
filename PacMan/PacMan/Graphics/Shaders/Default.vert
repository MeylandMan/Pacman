#version 410

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec2 in_TexCoord;

out vec2 texCoord;

// Uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {

    gl_Position = vec4(in_Position, 1.0) * model * view * projection;
    texCoord = in_TexCoord;
}