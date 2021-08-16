// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//
//  Created by Olivier VENERI on 05/04/2016.
//  Copyright (c) 2016 FenschValleyGames. All rights reserved.
//

Shader "Unlit/WaterShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaterTint ("Water tint", Color) = (1,1,1,1)
		_DispMap1 ("Displacement1", 2D) = "bump" {}
		_DispMap2 ("Displacement2", 2D) = "bump" {}
		_WaterHeight ("Water Height", Range (0,1.0)) = 0.5
		_ReflDistort ("Reflection distort", Range (0,1.5)) = 0.025
		_WaterSpeed1 ("Water Speed 1", Float) = 1.0
		_WaterSpeed2 ("Water Speed 2", Float) = 1.0
		_xPositionOffset ("x Position Offset", Float) = 0.0
		_transparency ("Water Transparency", Float) = 1.0
		_distPerspectiveLink ("Dist/Perspective Link", Range (0,1.0)) = 1.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100


		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D	_MainTex;
			float4 		_WaterTint;
			sampler2D 	_DispMap1;
			sampler2D 	_DispMap2;
			float4 		_MainTex_ST;
			float  		_ReflDistort;
			float  		_WaterSpeed1;
			float  		_WaterSpeed2;
			float  		_WaterHeight;
			float  		_xPositionOffset;
			float  		_transparency;
			float       _distPerspectiveLink;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				if (i.uv.y < _WaterHeight) 
	            {
	               discard; // drop the fragment if y coordinate > _WaterHeight
	            }

				// sample the texture
				half2 perspectiveCorrection = half2(1.0f * (0.5f - i.uv.x) * i.uv.y, 0.0f);
				half2 timeDisplacement = i.uv.xy + perspectiveCorrection;
				half2 timeDisplacement2 = timeDisplacement;
				timeDisplacement.x += _Time * _WaterSpeed1 + _xPositionOffset;
				half3 displacement1 = tex2D( _DispMap1, timeDisplacement );

				timeDisplacement2.x += _Time * _WaterSpeed2 + _xPositionOffset;
				half3 displacement2 = tex2D( _DispMap2, timeDisplacement2 );

				half3 finalDisplacement = ((displacement1-displacement2)) *_ReflDistort * max((i.uv.y-0.5f)*2.0f, _distPerspectiveLink);
				half2 adjusted = i.uv.xy + finalDisplacement.rg;
				adjusted.y = max(adjusted.y,_WaterHeight);

				half4 output = tex2D(_MainTex, adjusted);
				output *= _WaterTint + finalDisplacement.x;
				output.a = _transparency;
		
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, output);

				return output;
			}
			ENDCG
		}
	}
}
