Shader "Custom/moon"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Scale ("Texture DScale", Range(0,2)) = 1.0
        
        _NormalMap ("Bumpmap", 2D) = "bump" {}
        _NormalScale ("Normal Scale", Range(0.0, 1.0)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        float _NormalScale;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 localPos;
            float3 worldNormal; INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        float _Scale;
        fixed4 _Color;

         void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.localPos = v.vertex.xyz;
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float2 uv = float2(IN.localPos.x/(1000.0*_Scale), IN.localPos.z/(1000.0*_Scale));
            fixed4 c = tex2D (_MainTex, uv);
            o.Albedo = c.rgb;
            float3 worldInterpolatedNormalVector = WorldNormalVector ( IN, float3( 0, 0, 1 ) );

            fixed3 normal = UnpackScaleNormal (tex2D (_NormalMap, uv), _NormalScale);

            o.Normal =  normal;
            
            // o.Albedo = float3(IN.uv_MainTex.x/10.0, IN.uv_MainTex.y/1.0, 0.0);
            // o.Emission = float3(IN.localPos.x/1000.0, IN.localPos.z/1000.0,0);
            // Metallic and smoothness come from slider variables
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
