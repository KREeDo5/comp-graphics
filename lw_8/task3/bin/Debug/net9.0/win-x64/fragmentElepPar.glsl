#version 330 core

out vec4 FragColor;

uniform vec3 viewPos;
uniform mat4 view;
uniform mat4 projection;
uniform vec2 screenSize;

const int NUM_PARABOLOIDS = 3;

struct Paraboloid {
    float scale;
    vec3 position;
    vec3 color;
};

Paraboloid paraboloids[NUM_PARABOLOIDS];

void setupParaboloids()
{
    paraboloids[0].scale = 1.0;
    paraboloids[0].position = vec3(0.0, 0.0, 0.0);
    paraboloids[0].color = vec3(1.0, 0.4, 0.3);

    paraboloids[1].scale = 0.7;
    paraboloids[1].position = vec3(0.0, 1.0, 0.0);
    paraboloids[1].color = vec3(0.3, 1.0, 0.5);

    paraboloids[2].scale = 0.5;
    paraboloids[2].position = vec3(0.0, 1.7, 0.0);
    paraboloids[2].color = vec3(0.3, 0.5, 1.0);
}

vec3 paraboloidNormal(vec3 p, Paraboloid parab)
{
    vec3 localPos = (p - parab.position) / parab.scale;
    return normalize(vec3(2.0 * localPos.x, -1.0, 2.0 * localPos.z));
}

vec3 capNormal()
{
    return vec3(0.0, 1.0, 0.0);
}

bool intersectParaboloid(vec3 ro, vec3 rd, Paraboloid parab, out float t)
{
    vec3 localRo = (ro - parab.position) / parab.scale;
    vec3 localRd = rd / parab.scale;

    float a = localRd.x * localRd.x + localRd.z * localRd.z;
    float b = 2.0 * (localRo.x * localRd.x + localRo.z * localRd.z) - localRd.y;
    float c = localRo.x * localRo.x + localRo.z * localRo.z - localRo.y;

    float discriminant = b * b - 4.0 * a * c;
    if (discriminant < 0.0)
        return false;

    float sqrtD = sqrt(discriminant);
    float t0 = (-b - sqrtD) / (2.0 * a);
    float t1 = (-b + sqrtD) / (2.0 * a);

    for (int i = 0; i < 2; i++)
    {
        float currT = (i == 0) ? t0 : t1;
        if (currT > 0.0)
        {
            vec3 p = ro + rd * currT;
            float localY = (p.y - parab.position.y) / parab.scale;
            if (localY >= 0.0 && localY <= 1.0)
            {
                t = currT;
                return true;
            }
        }
    }

    return false;
}

bool intersectCap(vec3 ro, vec3 rd, Paraboloid parab, out float t)
{
    float capY = parab.position.y + parab.scale;
    if (abs(rd.y) < 0.0001)
        return false;

    t = (capY - ro.y) / rd.y;
    if (t <= 0.0)
        return false;

    vec3 p = ro + rd * t;
    float dx = (p.x - parab.position.x) / parab.scale;
    float dz = (p.z - parab.position.z) / parab.scale;
    if (dx * dx + dz * dz <= 1.0)
        return true;

    return false;
}

void main()
{
    setupParaboloids();

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
    bool hitSomething = false;

    for (int i = 0; i < NUM_PARABOLOIDS; i++)
    {
        float tParaboloid, tCap;
        bool hitParab = intersectParaboloid(rayOrigin, rayDir, paraboloids[i], tParaboloid);
        bool hitC = intersectCap(rayOrigin, rayDir, paraboloids[i], tCap);

        if (hitParab && tParaboloid < closestT)
        {
            closestT = tParaboloid;
            hitPoint = rayOrigin + rayDir * closestT;
            hitNormal = paraboloidNormal(hitPoint, paraboloids[i]);
            hitColor = paraboloids[i].color;
            hitSomething = true;
        }

        if (hitC && tCap < closestT)
        {
            closestT = tCap;
            hitPoint = rayOrigin + rayDir * closestT;
            hitNormal = capNormal();
            hitColor = paraboloids[i].color;
            hitSomething = true;
        }
    }

    if (!hitSomething)
    {
        FragColor = vec4(0.2, 0.3, 0.4, 1.0);
        return;
    }

    vec3 lightPos = vec3(3.0, 3.0, 3.0);
    vec3 lightColor = vec3(1.0);
    vec3 lightDir = normalize(lightPos - hitPoint);
    vec3 viewDir = normalize(viewPos - hitPoint);

    vec3 ambient = 0.2 * lightColor;
    float diff = max(dot(hitNormal, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    vec3 reflectDir = reflect(-lightDir, hitNormal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64.0);
    vec3 specular = 0.5 * spec * lightColor;

    vec3 color = (ambient + diffuse + specular) * hitColor;

    FragColor = vec4(color, 1.0);
}
