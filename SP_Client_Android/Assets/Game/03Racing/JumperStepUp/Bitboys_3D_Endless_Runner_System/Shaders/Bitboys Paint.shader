Shader "Custom/Bitboys Paint" {
	Properties {
		_Paint ("Paint", Color) = (1,1,1,1)
	}
	SubShader {
	
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
			#pragma surface surf Lambert

			float4 _Paint;

			struct Input {
				float4 color: COLOR;
			};


			void surf (Input IN, inout SurfaceOutput o) {
				o.Albedo = IN.color * _Paint;
				o.Alpha = _Paint.a;
			}
		ENDCG
	
	}

	Fallback "VertexLit"
}
