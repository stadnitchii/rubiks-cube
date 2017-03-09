#version 330

in vec3 vert;

out vec4 outColor;

uniform vec3 pickColor;

void main()
{
	outColor = vec4(pickColor / 255.0, 1);
}