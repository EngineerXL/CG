#version 150 core

in vec3 position;
in vec3 normal;
out vec4 color;

uniform bool useSingleColor;
uniform vec3 singleColor3f;
uniform vec3 ka3f;
uniform vec3 kd3f;
uniform vec3 ks3f;
uniform vec3 light3f;
uniform vec3 ia3f;
uniform vec3 il3f;
uniform float p;
uniform vec3 camera = vec3(0, 0, -1e9f);
uniform float k = 0.5;

void main(void) {
    if (useSingleColor) {
        color = vec4(singleColor3f, 1.0f);
        return;
    }
    vec3 res = singleColor3f;
    vec3 l = light3f - position;
    float d = length(l);
    l = normalize(l);
    vec3 s = normalize(camera - position);
    vec3 r = normalize(2 * normal * dot(normal, l) - l);
    float diffusal = dot(l, normal);
    float specular = dot(r, s);
    if (diffusal < 1e-3) {
        diffusal = 0;
        specular = 0;
    }
    if (specular < 1e-3) {
        specular = 0;
    }
    for (int i = 0; i < 3; ++i) {
        res[i] = res[i] * (ia3f[i] * ka3f[i] + il3f[i] * (kd3f[i] * diffusal + ks3f[i] * pow(specular, p)) / (d + k));
    }
    color = vec4(res, 1.0f);
}
