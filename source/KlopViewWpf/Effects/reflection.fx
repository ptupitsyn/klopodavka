/// <class>ReflectionEffect</class>

/// <description>An effect that applies a radial blur to the input.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The height of the reflection.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float reflectionHeight : register(c0);

/// <summary>Effect opacity.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float opacity : register(c1);


sampler2D input : register(s0);


float4 Reflect(float2 uv : TEXCOORD) : COLOR
{
  float edge = 0.5;
  float limit = reflectionHeight / 2 + edge;
  
  if (uv.y > limit)
  {
    return 0;
  }
  
  if (uv.y > edge)
  {
    float op = 1 - (uv.y - edge) * 2 / reflectionHeight;
    uv.y = 1 - uv.y;
    return tex2D(input, uv) * op * opacity;
  }
  
  return tex2D(input, uv);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
  return Reflect(uv);
}