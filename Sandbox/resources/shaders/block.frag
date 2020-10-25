#version 330 core
out vec4 FragColor;

in vec2 TexCoord; // the input variable from the vertex shader (same name and same type)  

uniform sampler2D ourTexture;
void main()
{
	vec4 color = texture(ourTexture, vec2(TexCoord[0], TexCoord[1]));
	FragColor = color;
} 