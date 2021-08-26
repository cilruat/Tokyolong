Shader "Custom/WavySpriteLit"{
	Properties{
		[PerRendererData] _MainTex("Texture",2D)="white"{}
		_Color ("Tint",Color)=(1,1,1,1)
		[HideInInspector] _RendererColor("RendererColor",Color)=(1,1,1,1)
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
			"CanUseSpriteAtlas"="True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#include "UnitySprites.cginc"
		struct Input{
			float2 uv_MainTex;
			fixed4 color;
		};
		float _WaveDirection;
		float _StaticSide;
		float _WaveFrequency;
		float _WaveForce;
		float _WaveSpeed;
		float _TextureSpeed;

		void vert(inout appdata_full v,out Input o){
			v.normal=float3(0,0,-1);
			//Decide a static side
			float multiplier=0;
			if(_StaticSide==1) multiplier=1-v.texcoord.y; //Top
			if(_StaticSide==2) multiplier=1-v.texcoord.x; //Right
			if(_StaticSide==3) multiplier=v.texcoord.y; //Bottom
			if(_StaticSide==4) multiplier=v.texcoord.x; //Left
			if(_StaticSide==5) multiplier=(0.5-abs(v.texcoord.y-0.5))*2; //Top and bottom
			if(_StaticSide==6) multiplier=(0.5-abs(v.texcoord.x-0.5))*2; //Left and right
			if(_StaticSide==0) multiplier=1; //None
			//Based on wave direction decide a vector for oscilations and axis of movement
			float3 osc;
			float side;
			if(_WaveDirection==0){
				osc=float3(1,0,0);
				side=v.vertex.y;
			}else{
				osc=float3(0,1,0);
				side=v.vertex.x;
			}
			//Multiply by wave force
			osc*=_WaveForce;
			//Increment it with our sine waves
			v.vertex.xyz+=osc*multiplier*sin(side*_WaveFrequency-_Time.a*_WaveSpeed);
			//Do whatever this is
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.color=v.color*_Color*_RendererColor;
		}
		/*
		void vert_old(inout appdata_full v,out Input o){
			v.normal=float3(0,0,-1);
			float3 wpos=mul(unity_ObjectToWorld,v.vertex).xyz;
			//Decide a static side
			float multiplier=0;
			if(_StaticSide==1) multiplier=1-v.texcoord.y; //Top
			if(_StaticSide==2) multiplier=1-v.texcoord.x; //Right
			if(_StaticSide==3) multiplier=v.texcoord.y; //Bottom
			if(_StaticSide==4) multiplier=v.texcoord.x; //Left
			if(_StaticSide==5) multiplier=(0.5-abs(v.texcoord.y-0.5))*2; //Top and bottom
			if(_StaticSide==6) multiplier=(0.5-abs(v.texcoord.x-0.5))*2; //Left and right
			if(_StaticSide==0) multiplier=1; //None
			//Horizontal waves
			if(_WaveDirection==0) v.vertex.x+=sin((wpos.y*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*multiplier);
			//Vertical waves
			else v.vertex.y+=sin((wpos.x*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*multiplier);
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.color=v.color*_Color*_RendererColor;
		}
		*/
		void surf(Input IN, inout SurfaceOutput o){
			float2 incrementUV=float2(0,0);
			//Horizontal waves
			if(_WaveDirection==0) incrementUV.y-=(_Time.a*_TextureSpeed)*0.1;
			//Vertical waves
			else incrementUV.x-=(_Time.a*_TextureSpeed)*0.1;
			//Get texture
			fixed4 c=SampleSpriteTexture(IN.uv_MainTex+incrementUV)*IN.color;
			o.Albedo=c.rgb*c.a;
			o.Alpha=c.a;
		}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
}