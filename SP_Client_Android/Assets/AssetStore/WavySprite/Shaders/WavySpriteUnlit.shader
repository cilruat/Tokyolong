Shader "Custom/WavySpriteUnlit"{
	Properties{
		[PerRendererData] _MainTex("Texture",2D)="white"{}
		_Color("Tint",Color)=(1,1,1,1)
		_WaveDirection("Wave Direction",range(0,1))=0
		_StaticSide("Static Side",range(0,4))=3
		_WaveFrequency("Wave Frequency",float)=10
		_WaveForce("Wave Force",float)=0.1
		_WaveSpeed("Wave Speed",float)=1
		_TextureSpeed("Texture Speed",float)=0
	}
	SubShader{
		Tags{
			"Queue"="Transparent" 
			"IgnoreProjector"="True"
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
		}
		ZWrite Off 
		Lighting Off 
		Cull Off 
		Fog {Mode Off} 
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			//#pragma vertex vert_old
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			struct v2f{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
			};
			sampler2D _MainTex;
			fixed4 _Color;
			float _WaveDirection;
			float _StaticSide;
			float _WaveFrequency;
			float _WaveForce;
			float _WaveSpeed;
			float _TextureSpeed;

			v2f vert(appdata v){
				v2f o;
				//Decide a static side
				float multiplier=0;
				if(_StaticSide==1) multiplier=1-v.uv.y; //Top
				if(_StaticSide==2) multiplier=1-v.uv.x; //Right
				if(_StaticSide==3) multiplier=v.uv.y; //Bottom
				if(_StaticSide==4) multiplier=v.uv.x; //Left
				if(_StaticSide==5) multiplier=(0.5-abs(v.uv.y-0.5))*2; //Top and bottom
				if(_StaticSide==6) multiplier=(0.5-abs(v.uv.x-0.5))*2; //Left and right
				if(_StaticSide==0) multiplier=1; //None
				//Based on wave direction decide a vector for oscilations and axis of movement
				float3 osc;
				float side;
				if(_WaveDirection==0){
					osc=float3(1,0,0);
					side=v.pos.y;
				}else{
					osc=float3(0,1,0);
					side=v.pos.x;
				}
				//Multiply by wave force
				osc*=_WaveForce;
				//Convert to world space
				osc=UnityObjectToClipPos(osc)-UnityObjectToClipPos(float3(0,0,0));
				//Get position of a vertex
				o.pos=UnityObjectToClipPos(v.pos);
				//Increment it with our sine waves
				o.pos.xyz+=osc*multiplier*sin(side*_WaveFrequency-_Time.a*_WaveSpeed);
				//Pass UV as it is
				o.uv=v.uv;
				return o;
			}

			/*
			v2f vert_old(appdata v){
				v2f o;
				float3 unit=UnityObjectToClipPos(float3(1,1,0))-UnityObjectToClipPos(float3(0,0,0));
				float3 wpos=mul(unity_ObjectToWorld,v.pos).xyz;
				o.pos=UnityObjectToClipPos(v.pos);
				//Decide a static side
				float multiplier=0;
				if(_StaticSide==1) multiplier=1-v.uv.y; //Top
				if(_StaticSide==2) multiplier=1-v.uv.x; //Right
				if(_StaticSide==3) multiplier=v.uv.y; //Bottom
				if(_StaticSide==4) multiplier=v.uv.x; //Left
				if(_StaticSide==5) multiplier=(0.5-abs(v.uv.y-0.5))*2; //Top and bottom
				if(_StaticSide==6) multiplier=(0.5-abs(v.uv.x-0.5))*2; //Left and right
				if(_StaticSide==0) multiplier=1; //None
				//Horizontal waves
				if(_WaveDirection==0) o.pos.x+=sin((wpos.y*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*unit.x*multiplier);
				//Vertical waves
				else o.pos.y+=sin((wpos.x*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*unit.y*multiplier);
				o.uv=v.uv;
				return o;
			}
			*/

			fixed4 frag(v2f i):SV_Target{
				float2 incrementUV=float2(0,0);
				//Horizontal waves
				if(_WaveDirection==0) incrementUV.y-=(_Time.a*_TextureSpeed)*0.1;
				//Vertical waves
				else incrementUV.x-=(_Time.a*_TextureSpeed)*0.1;
				//Get texture
				fixed4 col=tex2D(_MainTex,i.uv+incrementUV)*_Color;
				return col;
			}
			ENDCG
		}
	}
}