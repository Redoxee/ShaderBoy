#define MAX_STEP 64.
#define EPSILONE .00001

uniform vec2 iResolution;
uniform float iGlobalTime;


float sphere1(vec3 pos, float radius)
{
	return length(pos) - radius;
}

float sdPlane(vec3 pos, vec4 n )
{
  // n must be normalized
  return dot(pos,n.xyz) + n.w;
}

vec3 box = vec3(1.);
float udBox(vec3 pos, vec3 box)
{
  return length(max(abs(pos)-box,0.0));
}

float distanceField(vec3 pos)
{
	float mind = sphere1(pos, 0.5); 
	mind = min(mind,sphere1(pos - vec3(-1. * sin(iGlobalTime),.5 * sin(iGlobalTime),-1. * cos(iGlobalTime)) , 0.3));
	vec4 plane = normalize(vec4(0,1.,0.,0.));
	mind = min(mind,udBox(pos + vec3 (-2.,-0.15,0.), box * .5));
	mind = min(mind,sdPlane(pos + vec3(0.,1.,0.),plane));
	return mind;
}

float raymarching(vec3 origin,vec3 direction)
{
	float t = .0;
	for(float i = 0.; i<MAX_STEP; ++i )
	{
		float d = distanceField(origin + direction * t);
			if(d < EPSILONE)
			{
				return (MAX_STEP - i) / MAX_STEP;
			}

		t += d;
	}
	return -1.;
}

void main(void)
{
    float u = gl_FragCoord.x * 2.0 / iResolution.y - 1.0;
    float v = gl_FragCoord.y * 2.0 / iResolution.y - 1.0;

	vec3 right = vec3(1.,0.,0.);
	vec3 up = vec3(0.,1.,0.);
	vec3 forward = normalize(cross(right,up)); 
	float focal = 1.;

	vec3 eye = vec3(0.,0.,-2.5);

	vec3 rd = forward * focal + right * u + up * v ;
	rd = normalize(rd);

	float d = raymarching(eye,rd);

	vec3 col = vec3(d);
	gl_FragColor = vec4(col,1.0);
}