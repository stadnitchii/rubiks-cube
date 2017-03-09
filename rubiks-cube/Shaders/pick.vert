#version 330

layout (location=0) in vec3 vertIn;

out vec3 vert;

uniform mat4 proj;
uniform mat4 view;
uniform mat4 model;

void main()
{
	gl_Position = proj * view * model * vec4(vertIn, 1);
	vert = vec3(model * vec4(vertIn , 1));
}