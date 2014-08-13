
// Written by Romain Péchot ( http://opotable.tumblr.com/ )

Shader "Texture Swap"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SecTex("Swap (RGB)", 2D) = "black" {}
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
		sampler2D _SecTex;
		half _OffsetPower;
		sampler2D _OffsetTex;
		half4 _Pivot;
		half _Radius;
		half _Fade;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_SecTex;
			float2 uv_OffsetTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 m = tex2D(_MainTex, IN.uv_MainTex);
			half4 s = tex2D(_SecTex, IN.uv_SecTex);
			half d = distance(IN.worldPos, _Pivot.xyz);
			half od = (1 - tex2D(_OffsetTex, IN.uv_OffsetTex).rgb * 2) * 0.5 * _OffsetPower;
			d += od;
			
			half r = abs(_Radius);
			half f = abs(_Fade);
			half4 c = lerp(m, s, (clamp((d - r) / r / f, -1, 1) + 1) * 0.5);
			
			o.Albedo = c.rgb;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
