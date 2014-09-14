Shader "Texture Greyscale Color Fade"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OffsetPower("Power Offset", Range(0, 1)) = 0.1
		_OffsetTex("Offset (RGB)", 2D) = "black" {}
		_Pivot("Pivot", VECTOR) = (0,0,0,1)
		_Radius("Radius", float) = 1
		_Fade("Fade", FLOAT) = 0.01
	}
	
	SubShader
	{
		Tags {"RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		half _OffsetPower;
		sampler2D _OffsetTex;
		half4 _Pivot;
		half _Radius;
		half _Fade;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_OffsetTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 m = tex2D(_MainTex, IN.uv_MainTex);
			half d = distance(IN.worldPos, _Pivot.xyz);
			half od = (1 - tex2D(_OffsetTex, IN.uv_OffsetTex).rgb * 2) * 0.5 * _OffsetPower;
			d += od;
			
			// greyscale of _MainTex
			// ninja on : http://answers.unity3d.com/questions/31823/how-do-i-make-a-texture-turn-greyscael.html
			half4 g = dot(m.rgb, float3(0.3, 0.59, 0.11));
			
			half r = abs(_Radius);
			half f = abs(_Fade);
			half4 c = lerp(m, g, (clamp((d - r) / r / f, -1, 1) + 1) * 0.5);
			
			o.Emission = c.rgb;
			o.Alpha = 1;
		}
		ENDCG
	}
	
	FallBack Off
}