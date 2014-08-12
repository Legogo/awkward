// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'

Shader "Custom/ProgressBar" {

	Properties {
	    _MainTex ("Main Tex (RGBA)", 2D) = "white" {}
	    _Progress ("Progress", Range(0.0,1.0)) = 0.0
	    _Flip ("Flip", float) = 0.0
	}
	
	SubShader {
    	Tags { "Queue"="Overlay+1" }
    	ZTest Always
    	Blend SrcAlpha OneMinusSrcAlpha
        Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float _Progress;
			uniform float _Flip;
			
			struct v2f {
			    float4 pos : POSITION;
			    float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = TRANSFORM_UV(0);
			
			    return o;
			}
			
			half4 frag( v2f i ) : COLOR
			{
			    half4 color = tex2D( _MainTex, i.uv);
			    
			    if(_Flip == 0)	color.a *= i.uv.x < _Progress;
			    else color.a *= !((1 - i.uv.x) > _Progress);
			    
			    return color;
			}
			
			ENDCG
			
	    }
	}

}