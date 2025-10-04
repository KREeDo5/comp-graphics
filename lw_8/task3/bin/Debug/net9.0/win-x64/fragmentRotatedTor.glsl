#version 330 core

out vec4 FragColor;

uniform vec3 viewPos;
uniform mat4 view;
uniform mat4 projection;
uniform vec2 screenSize;

const int NUM_TORS = 5;

struct Torus {
    mat4 modelMatrix;
    float majorRadius;
    float minorRadius;
    vec3 color;
};

struct Sphere {
    vec3 position;
    float radius;
    vec3 color;
};

Torus tors[NUM_TORS];
Sphere sphere;

mat4 rotateY(float angle) {
    float s = sin(angle);
    float c = cos(angle);
    return mat4(
        vec4(c, 0.0, -s, 0.0),
        vec4(0.0, 1.0, 0.0, 0.0),
        vec4(s, 0.0, c, 0.0),
        vec4(0.0, 0.0, 0.0, 1.0)
    );
}

mat4 rotateX(float angle) {
    float s = sin(angle);
    float c = cos(angle);
    return mat4(
        vec4(1.0, 0.0, 0.0, 0.0),
        vec4(0.0, c, s, 0.0),
        vec4(0.0, -s, c, 0.0),
        vec4(0.0, 0.0, 0.0, 1.0)
    );
}

mat4 rotateZ(float angle) {
    float s = sin(angle);
    float c = cos(angle);
    return mat4(
        vec4(c, s, 0.0, 0.0),
        vec4(-s, c, 0.0, 0.0),
        vec4(0.0, 0.0, 1.0, 0.0),
        vec4(0.0, 0.0, 0.0, 1.0)
    );
}

mat4 translate(vec3 t) {
    return mat4(
        vec4(1.0, 0.0, 0.0, 0.0),
        vec4(0.0, 1.0, 0.0, 0.0),
        vec4(0.0, 0.0, 1.0, 0.0),
        vec4(t, 1.0)
    );
}

void setupTors() {
    float baseMajor = 0.8;
    float baseMinor = 0.25;
    float y = 0.0;

    mat4 rotationX = rotateX(radians(45.0));
    mat4 rotationZ = rotateZ(radians(45.0));
    mat4 pyramidRotation = rotationX * rotationZ;

    for (int i = 0; i < NUM_TORS; ++i) {
        float scaleFactor = 1.0 - float(i) * 0.15;
        float major = baseMajor * scaleFactor;
        float minor = baseMinor * scaleFactor;

        if (i > 0) {
            float prevMinor = tors[i - 1].minorRadius;
            y += prevMinor * 2.0;
        }

        mat4 translation = translate(vec3(0.0, y, 0.0));
        tors[i].modelMatrix = pyramidRotation * translation;
        tors[i].majorRadius = major;
        tors[i].minorRadius = minor;
        tors[i].color = vec3(0.2 + 0.15 * i, 1.0 - 0.15 * i, 0.5 + 0.1 * i);
    }

    float topY = y + tors[NUM_TORS - 1].minorRadius * 2.0;
    vec4 rotatedSpherePos = pyramidRotation * vec4(0.0, topY, 0.0, 1.0);
    sphere.position = vec3(rotatedSpherePos);
    sphere.radius = 0.25;
    sphere.color = vec3(1.0, 1.0, 0.2);
}

int solveQuadratic(float a, float b, float c, out float t0, out float t1) {
    t0 = 0.0;
    t1 = 0.0;
    float discr = b*b - 4.0*a*c;
    if (discr < 0.0) return 0;
    float sqrtDiscr = sqrt(discr);
    t0 = (-b - sqrtDiscr) / (2.0*a);
    t1 = (-b + sqrtDiscr) / (2.0*a);
    if (t0 > t1) {
        float tmp = t0; t0 = t1; t1 = tmp;
    }
    return 2;
}

float torusEquationPoint(vec3 p, float R, float r) {
    float sumSq = dot(p, p);
    float k = sumSq + R*R - r*r;
    return k*k - 4.0*R*R*(p.x*p.x + p.z*p.z);
}

float torusEquation(float t, vec3 ro, vec3 rd, float R, float r) {
    vec3 p = ro + t * rd;
    float sumSquared = dot(p, p);
    float k = sumSquared + R*R - r*r;
    return k*k - 4.0*R*R*(p.x*p.x + p.z*p.z);
}

float torusEquationDerivative(float t, vec3 ro, vec3 rd, float R, float r) {
    vec3 p = ro + t * rd;
    float sumSquared = dot(p, p);
    float k = sumSquared + R*R - r*r;

    float dk_dt = 2.0 * dot(p, rd);
    float dEq_dt = 2.0 * k * dk_dt - 8.0 * R*R * (p.x*rd.x + p.z*rd.z);
    return dEq_dt;
}

bool intersectTorus(vec3 ro, vec3 rd, float R, float r, out float tHit) {
    tHit = 0.0; 
    float t = 0.5;
    const int maxIter = 20;
    const float epsilon = 0.0001;

    for (int i = 0; i < maxIter; i++) {
        float f = torusEquation(t, ro, rd, R, r);
        float df = torusEquationDerivative(t, ro, rd, R, r);

        if (abs(f) < epsilon) {
            if (t > 0.0) {
                tHit = t;
                return true;
            } else {
                return false;
            }
        }

        if (df == 0.0) break;

        t = t - f / df;

        if (t < 0.0) t = 0.0;
    }
    return false;
}

bool intersectSphere(vec3 ro, vec3 rd, Sphere s, out float tHit, out vec3 normal, out vec3 color) {
    tHit = 0.0; 
    normal = vec3(0.0);
    color = vec3(0.0); 
    
    vec3 oc = ro - s.position;
    float a = dot(rd, rd);
    float b = 2.0 * dot(oc, rd);
    float c = dot(oc, oc) - s.radius * s.radius;
    float t0 = 0.0, t1 = 0.0; 

    int roots = solveQuadratic(a, b, c, t0, t1);
    if (roots == 0) return false;

    float t = (t0 > 0.0) ? t0 : ((t1 > 0.0) ? t1 : 1e20);
    if (t == 1e20) return false;

    tHit = t;
    vec3 p = ro + t * rd;
    normal = normalize(p - s.position);
    color = s.color;
    return true;
}

void main() {
    setupTors();

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

    float closestT = 1e20;
    vec3 hitPoint;
    vec3 hitNormal;
    vec3 hitColor;
    bool hit = false;

    for (int i = 0; i < NUM_TORS; ++i) {
        mat4 invModel = inverse(tors[i].modelMatrix);
        vec3 localRo = vec3(invModel * vec4(rayOrigin, 1.0));
        vec3 localRd = normalize(mat3(invModel) * rayDir);

        float tLocal = 0.0; 
        if (intersectTorus(localRo, localRd, tors[i].majorRadius, tors[i].minorRadius, tLocal)) {
            if (tLocal < closestT && tLocal > 0.0) {
                closestT = tLocal;

                hitPoint = rayOrigin + rayDir * closestT;

                const float eps = 0.001; 
                vec3 pLocal = localRo + localRd * tLocal;

                float dx = torusEquationPoint(pLocal + vec3(eps, 0, 0), tors[i].majorRadius, tors[i].minorRadius) - torusEquationPoint(pLocal - vec3(eps, 0, 0), tors[i].majorRadius, tors[i].minorRadius);
                float dy = torusEquationPoint(pLocal + vec3(0, eps, 0), tors[i].majorRadius, tors[i].minorRadius) - torusEquationPoint(pLocal - vec3(0, eps, 0), tors[i].majorRadius, tors[i].minorRadius);
                float dz = torusEquationPoint(pLocal + vec3(0, 0, eps), tors[i].majorRadius, tors[i].minorRadius) - torusEquationPoint(pLocal - vec3(0, 0, eps), tors[i].majorRadius, tors[i].minorRadius);

                vec3 normalLocal = normalize(vec3(dx, dy, dz));
                hitNormal = normalize(mat3(transpose(inverse(tors[i].modelMatrix))) * normalLocal);

                hitColor = tors[i].color;
                hit = true;
            }
        }
    }

    float tSphere = 0.0;
    vec3 normalSph = vec3(0.0);
    vec3 colorSph = vec3(0.0); 
    
    if (intersectSphere(rayOrigin, rayDir, sphere, tSphere, normalSph, colorSph)) {
        if (tSphere < closestT) {
            closestT = tSphere;
            hitPoint = rayOrigin + rayDir * closestT;
            hitNormal = normalSph;
            hitColor = colorSph;
            hit = true;
        }
    }

    if (!hit) {
        FragColor = vec4(0.05, 0.05, 0.1, 1.0);
        return;
    }

    vec3 lightPos = vec3(10.0, 5.0, 0.0);
    vec3 lightColor = vec3(1.0);

    vec3 lightDir = normalize(lightPos - hitPoint);
    vec3 viewDir = normalize(viewPos - hitPoint);

    vec3 ambient = 0.2 * lightColor;
    float diff = max(dot(hitNormal, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 reflectDir = reflect(-lightDir, hitNormal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
    vec3 specular = 0.3 * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * hitColor;
    FragColor = vec4(result, 1.0);
}