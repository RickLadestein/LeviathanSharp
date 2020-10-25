#version 330 core
out vec4 FragColor;
in vec3 TexCoord;

uniform sampler2D ourTexture;
void main()
{
	vec4 color = texture(ourTexture, vec2(TexCoord[0], TexCoord[1]));
	FragColor = color;
} 