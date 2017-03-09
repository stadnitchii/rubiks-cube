#version 330

in vec3 vert;
in vec2 uv;
in vec3 normal;

uniform sampler2D texture;
uniform bool picked;

out vec4 outColor;

uniform bool l;
uniform bool f;
uniform bool r;
uniform bool ba;
uniform bool t;
uniform bool bo;

uniform vec4 blockColor = vec4(25.0 / 255, 25.0 / 255, 25.0 / 255, 1);

bool isBlue(vec4 c);
bool isRed(vec4 c);
bool isYellow(vec4 c);
bool isGreen(vec4 c);
bool isWhite(vec4 c);
bool isOrange(vec4 c);

void main()
{
	vec4 texColor = texture2D(texture, uv);  

		 if(isGreen(texColor) && !l) outColor = blockColor;
	else if(isOrange(texColor) && !f) outColor = blockColor;
	else if(isBlue(texColor) && !r) outColor = blockColor;
	else if(isRed(texColor) && !ba) outColor = blockColor;
	else if(isYellow(texColor) && !t) outColor = blockColor;
	else if(isWhite(texColor) && !bo) outColor = blockColor;
	else

		outColor = texColor;
}

bool isOrange(vec4 c)
{
	if(c.r > c.b && c.g > c.b && c.g < c.r) return true;

	return false;
}

bool isWhite(vec4 c)
{
	if(c.r == c.g && c.g == c.b) return true;

	return false;
}

bool isYellow(vec4 c)
{
	if(c.r > c.b && c.g > c.b && c.r == c.g) return true;

	return false;
}

bool isBlue(vec4 c)
{
	if(c.b > c.r && c.b > c.g) return true;

	return false;
}

bool isGreen(vec4 c)
{
	if(c.g > c.r && c.g > c.b) return true;

	return false;
}

bool isRed(vec4 c)
{
	if(c.r / 2.3 > c.g && c.r > c.b) return true;

	return false;
}