Shader "Custom/Grass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,4)) = 2
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _WaveFactor("Wave Factor", Range(0.1,1.0)) = 0.4
        _Speed("Speed", Range(0.1, 1.5)) = 1
        _WindTex ("Noise Texture", 2D) = "white" {}
        _WorldSize("World Size", vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        float _WaveFactor;
        float _Speed;
        sampler2D _WindTex;
        float4 _WorldSize;

        struct Input
        {
            float2 uv_MainTex;
            float3 normal : NORMAL;
        };
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void vert(inout appdata_full v){
            // normalize texture based on world size
            float4 worldPos = mul(v.vertex, unity_ObjectToWorld);
            float2 samplePos = worldPos.xz / _WorldSize.xz;
            // get wind sample position
            samplePos += _Time.y * _Speed;
            float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));
            // distort position
            float StableControl = pow(v.vertex.y, 2) - v.vertex.y;
            // apply wave animation
            float finalPos = sin(windSample) * _WaveFactor * StableControl;
            v.vertex += float4(finalPos, 0, finalPos, 0);

        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
