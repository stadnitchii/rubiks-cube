namespace OpenGL
{
    public class Shaders
    {
        public static ShaderProgram SkyboxShader()
        {
            return new ShaderProgram(SkyboxShaderVertex, SkyboxShaderFragment, "SkyboxShader", false);
        }

        public static ShaderProgram FrameShader()
        {
            return new ShaderProgram(FrameShaderVertex, FrameShaderFragment, "FrameShader", false);
        }

        #region ShaderCode
        #region Skybox
        private static string SkyboxShaderVertex = @"
#version 330

layout (location = 0) in vec3 vertIn;

out vec3 uv;

uniform mat4 view;
uniform mat4 proj;

void main()
{
	mat4 view2 = view;
	view2[3][0] = 0;
	view2[3][1] = 0;
	view2[3][2] = 0;
	vec4 pos = proj * view2 * vec4(vertIn, 1);
    gl_Position = pos.xyww;
	uv = vertIn;
}";

        private static string SkyboxShaderFragment = @"
#version 330

in vec3 uv;

out vec4 outColor;
out vec4 outColor2;

uniform samplerCube skybox;

void main()
{
    outColor = texture(skybox, uv);
    outColor2 = vec4(0,0,0,1);
}";
        #endregion
        #region Frame
        private static string FrameShaderVertex = @"
#version 330

in vec3 vertIn;
in vec2 uvIn;

out vec2 uv;

void main()
{
    gl_Position = vec4(vertIn.xy, 0, 1);
    uv = uvIn;
}
";

        private static string FrameShaderFragment = @"
#version 330

in vec2 uv;

uniform sampler2D text;

out vec4 outColor;

void main()
{
    outColor = texture2D(text, uv);
}
";
        #endregion
        #endregion
    }
}
