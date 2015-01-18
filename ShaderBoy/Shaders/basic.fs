#define BLURE_STEP  10.

uniform vec2 iResolution;
uniform float iGlobalTime;

float rectangle(vec2 p, float w, float h,float dx,float dy,float a)
{
	p.x -= dx;
	p.y -= dy;
	float x =  p.x *cos(a) - p.y* sin( a) ;
	float y = p.y *cos(a) + p.x *sin(a) ;
	float s = 0.01;
	return smoothstep(-w/2.,-w/2.+s,x)*smoothstep(x,x+s,w/2.)*smoothstep(-h/2.,-h/2.+s,y)*smoothstep(y,y+s,h/2.);
}
vec3 processImage(float time){
	vec2 uv = gl_FragCoord.xy / iResolution.xy;
	vec2 p = vec2(-1.) + 2. * uv;
	
	vec3 col = vec3(.32,.45,.25);
	float f =1. - length(p);
	col = mix(col,vec3(.58,.86,.46),f);
	
	p.x*=iResolution.x/iResolution.y; 
	
	f = 1.;
	f = - sin( time +f) * (1. - sin( time+f))*cos(time+f) ;
	
	p.x +=f*3.;
	
	f = sin( time) * (1. - sin( time))*cos(time) ;
	
	float a = f*0.8;
	vec2 p2 = vec2(0.,-1.);
	p -= p2;
	float x =  p.x *cos(a) - (p.y)* sin( a) ;
	float y = (p.y) *cos(a) + p.x *sin(a) ;
	p = vec2(x,y);
	p+=p2;
	
	
	col = mix(col,vec3(0.),rectangle(p,1.3,.5,.0,.0,.0));
	col = mix(col,vec3(0.),rectangle(p,.35,1.5,.0,-.5,.0));
	col = mix(col,vec3(1.),rectangle(p,.2,.2,.45,.0,.0));
	col = mix(col,vec3(1.),rectangle(p,.2,.2,-.45,.0,.0));
	col = mix(col,vec3(1.),rectangle(p,.2,.05,.0,-.15,.0));
	return col;
}

void main(void)
{
	float time = iGlobalTime;
	vec3 col = processImage(time);
	for(float f = 1.;f < BLURE_STEP ;f += 1.){
		col = mix(col,processImage(time-0.007*f),0.007*(BLURE_STEP - f));
	}
	gl_FragColor = vec4(col,1.0);
}