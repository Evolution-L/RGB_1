Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _GirdW ("GridWight", Integer) = 10
        _GirdH ("GridHight", Integer) = 10
        _GirdLineW ("GridLineWight", Integer) = 10
        _GirdLineH ("GridLineHight", Integer) = 10
        _GridColor ("Grid Color", Color) = (0, 0, 0, 1)
        _GridSize ("Grid Size", Range(0.1, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        // #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        int _GirdW;
        int _GirdH;
        int _GirdLineW;
        int _GirdLineH;
        fixed4 _GridColor;
        float _GridSize;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
            // fixed4(worldOrigin.xyz, 1);

        void surf (Input IN, inout SurfaceOutput o) {
            float _girdSize = _GridSize * _CosTime;
            float2 gridUV = floor(IN.uv_MainTex * _GridSize) / _GridSize;
            fixed4 gridColor = _GridColor;
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 finalColor = lerp(texColor, gridColor, step(frac(IN.uv_MainTex.x * _GridSize), 0.01) + step(frac(IN.uv_MainTex.y * _GridSize), 0.01));
            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
