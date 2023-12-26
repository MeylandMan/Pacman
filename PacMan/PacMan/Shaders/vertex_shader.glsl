#Version 410

layout(position=0) in vec3 vertices;
layout(position=1) in vec3 colors;

out vec3 fragColor;

void main() {
	fragColor = colors;

	gl_position = vec4(vertices, 1.0);

}