#version 150 core

out vec4 color;

uniform bool useSingleColor;
uniform vec3 color3f;

void main(void) {
    color = vec4(color3f, 1.0f);
}
