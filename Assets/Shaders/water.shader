// The logic is referred to https://catlikecoding.com/unity/tutorials/flow/looking-through-water/
Shader "Custom/water"
{
    Properties
    {
        _Color ("Color", Color) = (54,156,145,255)
        _Glossiness ("Smoothness", Range(0,1)) = 1
        _Metallic ("Metallic", Range(0,1)) = 0.14
        _NoiseTex ("Noise texture", 2D) = "white" {}

        // variables that control wave of water
        _Scale ("Noise scale", Range(0.01, 0.1)) = 0.07
        _Amplitude ("Amplitude", Range(0.01, 0.1)) = 0.01
        _Speed ("Speed", Range(0.01, 0.3)) = 0.205

        // factors for refraction and foamline
        _SoftFactor("Soft Factor", Range(1.0, 7.0)) = 7
        _EdgeColor("Edge Color", Color) = (87, 168, 161, 0)
        _FoamLineStrength("FoamLine Strength", Range(1.0, 5.0)) = 5
        _DistortStrength("Distort Strength", Range(0.001, 0.05)) = 0.01
        _DistortAmplitude("Distort Amplitude", Range(0.2, 0.7)) = 0.3
        _FogVolumn("Fog Volumn", Range(0.01, 0.5)) = 0.5

    }
    SubShader
    {
        Tags { "Queue" = "Transparent"}
        GrabPass { "_BackgroundTexture" }
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha vertex:vert
        #pragma target 3.0

        struct Input
        {
            float4 screenPos;
            float eyeDepth;
        };
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _EdgeColor;
        
        sampler2D _NoiseTex;
        sampler2D _BackgroundTexture;
        sampler2D _CameraDepthTexture;
        float _Scale;
        float _Amplitude;
        float _Speed;
        float _SoftFactor;
        float _FoamLineStrength;
        float _DistortAmplitude;
        float  _DistortStrength;
        float _FogVolumn;

        // varying position of wave vertices
        // take position of mesh vertices, edit it and output data for the surf function
        void vert(inout appdata_full v, out Input o)
        {
            // // normalize texture based on world size
            // float4 worldPos = mul(v.vertex, unity_ObjectToWorld);
            // float2 waveCoor = worldPos.xz / _WorldSize.xz;
            // get the changing coordinator of wave
            float2 waveCoor = float2((v.texcoord.xy + sin(_Time* _Speed) ) * _Scale);
            // calculate noise value
            float noiseValue = tex2Dlod(_NoiseTex, float4(waveCoor, 0, 0)).x * _Amplitude/3;
            // set vertex position by adding noise
            v.vertex.z += noiseValue;
            // get the distance from pixel to camera and apply to the surf function 
            UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);

        }


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // get camera depth data and convert it to linear space
            float rawDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, 
                             UNITY_PROJ_COORD(IN.screenPos));
            float screenDepth = LinearEyeDepth(rawDepth);

            // change the transparency of water based on the depth of water 
            float fade = saturate(_SoftFactor * (screenDepth - IN.eyeDepth));
            o.Alpha = fade * 0.8;
            // add foamline
            float foamline = (1 - fade) * _EdgeColor * _FoamLineStrength;
            // refraction + volumn
            float4 distortCoor = IN.screenPos * sin(_Time * _Speed) * _DistortStrength;
            float distortValue = tex2Dlod(_NoiseTex, distortCoor);
            IN.screenPos.y +=  distortValue * _DistortAmplitude;
            IN.screenPos.x +=  distortValue * 0.1 - 0.05;
            o.Emission = tex2Dproj(_BackgroundTexture, IN.screenPos) * _FogVolumn;

            o.Albedo = _Color.rgb + foamline; 
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
