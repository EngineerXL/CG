#version 150 core

in vec3 cord3f;

void main(void) {
    gl_Position = vec4(cord3f, 1.0f);
}
