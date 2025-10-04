#version 330 core

out vec4 FragColor;

uniform vec3 viewPos;
uniform mat4 view;
uniform mat4 projection;
uniform vec2 screenSize;

int solveQuadratic(float a, float b, float c, out float t0, out float t1) {
    float discr = b*b - 4.0*a*c;
    if (discr < 0.0) {
        t0 = t1 = 0.0;
        return 0;
    }
    float sqrtDiscr = sqrt(discr);
    t0 = (-b - sqrtDiscr) / (2.0*a);
    t1 = (-b + sqrtDiscr) / (2.0*a);
    if (t0 > t1) {
        float tmp = t0;
        t0 = t1;
        t1 = tmp;
    }
    return discr > 0.0 ? 2 : 1;
}

bool intersectHyperbolicParaboloid(vec3 ro, vec3 rd, out float t, out vec3 normal) {
    t = -1.0;
    normal = vec3(0.0);
    float a = rd.x*rd.x - rd.y*rd.y;
    float b = 2.0*(ro.x*rd.x - ro.y*rd.y) - rd.z;
    float c = ro.x*ro.x - ro.y*ro.y - ro.z;

    float t0, t1;
    int roots = solveQuadratic(a, b, c, t0, t1);
    if (roots == 0) return false;

    bool valid0 = (t0 > 0.001);
    bool valid1 = (t1 > 0.001);

    if (valid0) {
        vec3 p0 = ro + t0*rd;
        if (abs(p0.x) <= 1.5 && abs(p0.y) <= 1.5) {
            t = t0;
            valid0 = true;
        } else {
            valid0 = false;
        }
    }

    if (valid1) {
        vec3 p1 = ro + t1*rd;
        if (abs(p1.x) <= 1.5 && abs(p1.y) <= 1.5) {
            if (valid0 && t1 < t) t = t1;
            else if (!valid0) t = t1;
            valid1 = true;
        } else {
            valid1 = false;
        }
    }

    if (t < 0.0) return false;

    vec3 p = ro + t*rd;
    normal = normalize(vec3(2.0*p.x, -2.0*p.y, -1.0));
    if (dot(normal, rd) > 0.0) normal = -normal;

    return true;
}

void main() {
    vec2 uv = (gl_FragCoord.xy / screenSize) * 2.0 - 1.0;
    uv.x *= screenSize.x / screenSize.y;
    mat4 invProj = inverse(projection);
    mat4 invView = inverse(view);

    vec4 rayClip = vec4(uv, -1.0, 1.0);
    vec4 rayEye = invProj * rayClip;
    rayEye.z = -1.0;
    rayEye.w = 0.0;

    vec3 rayDir = normalize((invView * rayEye).xyz);
    vec3 rayOrigin = viewPos;

    float t;
    vec3 normal;
    bool hit = intersectHyperbolicParaboloid(rayOrigin, rayDir, t, normal);

    if (!hit) {
        FragColor = vec4(0.1, 0.1, 0.2, 1.0);
        return;
    }

    vec3 hitPoint = rayOrigin + t*rayDir;
    vec3 lightPos = viewPos + vec3(3.0, 2.0, 0.0);
    vec3 lightDir = normalize(lightPos - hitPoint);
    vec3 viewDir = normalize(viewPos - hitPoint);
    vec3 surfaceColor = vec3(0.5, 0.7, 1.0);

    vec3 ambient = 0.2 * vec3(1.0);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * vec3(1.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
    vec3 specular = 0.3 * spec * vec3(1.0);

    vec3 result = (ambient + diffuse + specular) * surfaceColor;
    FragColor = vec4(result, 1.0);
}