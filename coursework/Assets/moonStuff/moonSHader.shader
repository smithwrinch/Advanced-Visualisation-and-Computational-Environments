Shader "Unlit/moonSHader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _TextureScale("Scale texture",  Range(0,0.1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float _TextureScale;

            v2f vert (appdata v)
            {
                v2f o;
                // v.vertex.y = sin(_Time.x * v.vertex.x) * 0.2;
                // v.vertex.y += tex2Dlod(_NoiseTex, v.uv2).x;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.vertex += tex2D(_NoiseTex, v.uv2);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.vertex.xz * _TextureScale;
                // float4 tex = tex2Dlod (_NoiseTex, float4(v.vertex.xy,0,0));
                // v.vertex.y += tex.r * 10.0;
                // v.vertex.y += 1.0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
                return float4(i.uv.x, i.uv.y, 1., 1.);
            }
            ENDCG
        }
    }
}
