Shader "ScratchCard/Brush" {
    Properties {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _BlendOpValue ("__blendOp", Int) = 0
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Lighting Off
        BlendOp [_BlendOpValue]
        Blend One OneMinusSrcAlpha
        Pass {
            SetTexture[_MainTex] {
                ConstantColor [_Color]
                combine texture * constant
            }
        }
    }
}
