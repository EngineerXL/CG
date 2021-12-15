#version 150 core

in vec3 cord3f;
in vec3 col3f;
in vec3 norm3f;
out vec3 position;
out vec3 normal;

uniform mat4 proj4f;
uniform mat4 view4f;
uniform mat4 model4f;
uniform bool useSingleColor;
uniform vec3 singleColor3f;
uniform bool moveToCorner;

const vec4 cornerVec = vec4(0.85f, -0.85f, 0, 0);

void main(void) {
    vec4 vertexPos = vec4(cord3f, 1.0f);
    vertexPos = (proj4f * view4f * model4f) * vertexPos;
    vec4 vertexNormal = vec4(norm3f, 0.0f);
    vertexNormal = (view4f * model4f) * vertexNormal;
    position = vertexPos.xyz;
    normal = normalize(vertexNormal.xyz);
    gl_Position = vertexPos;
    if (moveToCorner) {
        gl_Position += cornerVec;
    }
}
