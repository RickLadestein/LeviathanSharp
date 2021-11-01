??VERTEX
#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aNorm; // the position variable has attribute position 1
layout (location = 2) in vec3 aTex; // the position variable has attribute position 2
layout (location = 3) in vec3 aTang; // the position variable has attribute position 3

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
}

??FRAGMENT
#version 330 core
out vec4 FragColor;

void main()
{
	FragColor = vec4(0.9f, 0.9f, 0.9f, 1.0f);
}
