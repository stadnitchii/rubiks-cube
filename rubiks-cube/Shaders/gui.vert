#version 330

layout (location=0) in vec2 vertIn;

uniform mat4 ortho;

void main()
{
	vec4 vert =  ortho * vec4(vertIn, 0 , 1);
	gl_Position = vec4(vert);
}