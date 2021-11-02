??VERTEX
#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aNorm; // the position variable has attribute position 1
layout (location = 2) in vec3 aTex; // the position variable has attribute position 2
layout (location = 3) in vec3 aTang; // the position variable has attribute position 3

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 TexCoord;
out vec3 Normal;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	Normal = (transpose(inverse(model)) * vec4(aNorm, 1.0)).xyz;
	TexCoord = (model * vec4(aPos, 1.0)).xyz;
}

??FRAGMENT
#version 330 core
out vec4 FragColor;
in vec3 TexCoord;
in vec3 Normal;

uniform vec3 light = vec3(0.0f, 5.0f, 0.0f);

void main()
{
	float intensity = dot(light, Normal);
	vec4 color = vec4(0.9f, 0.0f, 0.0f, 1.0f);
	if (intensity > 0.75)
		color = vec4(1.0,0.5,0.5,1.0);
	else if (intensity > 0.5)
		color = vec4(0.6,0.3,0.3,1.0);
	else if (intensity > 0.25)
		color = vec4(0.4,0.2,0.2,1.0);
	else
		color = vec4(0.2,0.1,0.1,1.0);


	intensity = (intensity + 1.0f) / 2.0f;
	if(intensity > 1.0f) {
		intensity = 1.0f;
	}

	if(intensity < 0.0f) {
		intensity = 0.0f;
	}

	color = vec4(0.0f, intensity, 0.0f, 1.0f);
	FragColor = color;
}
