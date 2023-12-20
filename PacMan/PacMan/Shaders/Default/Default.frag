#version 330 core

out vec4 FragColor;

void main() {

	FragColor = vec4(1.);
	gl_Position = vec4(aPosition, 1.);
}