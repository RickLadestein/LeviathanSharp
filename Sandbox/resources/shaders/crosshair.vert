#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aNorm;
layout (location = 2) in vec3 aTex;

out vec3 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 cam;


void main()
{
	gl_Position = model * vec4(aPos, 1.0);
	TexCoord = aTex;
}