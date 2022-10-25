??VERTEX
#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aNorm; // the position variable has attribute position 1
layout (location = 2) in vec3 aTex; // the position variable has attribute position 2
layout (location = 3) in vec3 aTang; // the position variable has attribute position 3

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 norm;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	norm = aNorm;
}

??FRAGMENT
#version 330 core

in vec3 norm;
out vec4 FragColor;

void main()
{
	float intensity;
	vec4 color;
	vec3 up = vec3(0.0f, 100000.0f, 0.0f);

	intensity = dot(up,normal);

	if (intensity > 0.95)
		color = vec4(1.0,0.5,0.5,1.0);
	else if (intensity > 0.5)
		color = vec4(0.6,0.3,0.3,1.0);
	else if (intensity > 0.25)
		color = vec4(0.4,0.2,0.2,1.0);
	else
		color = vec4(0.2,0.1,0.1,1.0);
	gl_FragColor = color;
}
