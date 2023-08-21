Shader "Graph/Point Surface"
{
    Properties{
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader{
        CGPROGRAM
        // 知识编译器生成 具有标准光照并完全支持阴影的表面着色器
        // ConfigureSurface指的是我们必须创建的用于配置着色器的方法。
        #pragma surface ConfigureSurface Standard fullforwardshadows
        // 设置着色器目标级别和质量的最小值
        #pragma target 3.0

        struct Input{
            float3 worldPos;
        };
        float _Smoothness;
        // 第二个参数是表面配置数据，类型为SurfaceOutputStandard。
        // inout 表明第二个参数既传递参数也用于函数的结果
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            surface.Albedo.rg = input.worldPos.xy * 0.5 + 0.5;
            surface.Smoothness = _Smoothness;
        }
		ENDCG
    }

    FallBack "Diffuse"
}
