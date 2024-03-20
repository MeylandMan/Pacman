#version 410

out vec4 fragColor;
out vec4 in_Color;

in vec2 texCoord;
in vec4 Colors;

uniform sampler2D texture0;

void main()
{
    fragColor = Colors * texture(texture0, texCoord);
    // fragColor = vec4(texCoord, 0.0, 1.0); /*(Showing the Texture mapping)*/
}

