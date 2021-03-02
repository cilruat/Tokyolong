Shader "ScratchCard/MaskProgressCutOff" {
    Properties {
        _MainTex ("Main", 2D) = "white" {}
        _SourceTex ("SourceTex", 2D) = "white" {}
        _CutOff ("Cut Off", Range (0, 1)) = 0.33
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Lighting Off
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform sampler2D _SourceTex;
            uniform float4 _SourceTex_ST;
            uniform float _CutOff;

            struct app2vert
            {
                float4 position: POSITION;
                float4 color: COLOR;
                float2 texcoord: TEXCOORD0;
            };

            struct vert2frag
            {
                float4 position: POSITION;
                float4 color: COLOR;
                float2 texcoord: TEXCOORD0;
            };

            vert2frag vert(app2vert input)
            {
                vert2frag output;
                output.position = UnityObjectToClipPos(input.position);
				output.color = input.color;
                output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
                return output;
            }

            float4 frag(vert2frag input) : COLOR
            {
                float count = 0.0f;
                float countSource = 0.0f;
                float texWidth = 15.0f;
                float texHeight = 15.0f;
                float div = 225.0f;
                //sampling RenderTexture 225 times to get avegave alpha value
                for	(int i = 0; i < 15; i++)
                {
                	for	(int j = 0; j < 15; j++)
                	{
                		float2 newTexcoord = float2(i / (texWidth - 1.0f), j / (texHeight - 1.0f));
                        float newCountSource = tex2D(_SourceTex, newTexcoord).a;
                        //calculating source texture average alpha value
                        countSource += newCountSource;
                        if (newCountSource > _CutOff)
                        {
                            count += tex2D(_MainTex, newTexcoord).a;
                        }
                	}
                }
                countSource /= div;
                count /= div;
                count /= countSource;
                return count;
            }
            ENDCG
        }
    }
}
