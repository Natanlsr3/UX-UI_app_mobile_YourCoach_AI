Shader "Unlit/ChromaKey"
{
    Properties
    {
        _MainTex("Texture",    2D) = "white" {}
        _KeyColor("Key Color",  Color) = (0.85, 0.85, 0.85, 1)
        _Threshold("Threshold",  Range(0, 1)) = 0.15
        _Softness("Softness",   Range(0, 0.5)) = 0.05
    }
        SubShader
        {
            Tags { "Queue" = "Transparent"
                   "RenderType" = "Transparent"
                   "IgnoreProjector" = "True" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Pass
            {
                CGPROGRAM
                #pragma vertex   vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4    _KeyColor;
                float     _Threshold;
                float     _Softness;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv     : TEXCOORD0;
                };
                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv     : TEXCOORD0;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float  diff = distance(col.rgb, _KeyColor.rgb);
                    col.a = smoothstep(
                        _Threshold - _Softness,
                        _Threshold + _Softness,
                        diff
                    );
                    return col;
                }
                ENDCG
            }
        }
}
