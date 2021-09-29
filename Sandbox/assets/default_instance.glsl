??VERTEX
#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aNorm; // the position variable has attribute position 1
layout (location = 2) in vec3 aTex; // the position variable has attribute position 2
layout (location = 3) in vec3 aTang; // the position variable has attribute position 3
layout (location = 4) in vec3 iVec;
layout (location = 5) in vec3 iCol;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 albedo;
void main()
{
	gl_Position = projection * view * model * vec4(aPos + iVec, 1.0);
	albedo = iCol;
}
??FRAGMENT
#version 330 core
in vec3 albedo;
out vec4 FragColor;

void main()
{
	FragColor = vec4(albedo, 1.0f);
}
