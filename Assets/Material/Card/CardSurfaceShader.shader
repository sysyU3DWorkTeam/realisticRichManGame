Shader " DoubleSideShader" {
	Properties{
		//正面5个参数  
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
		//反面拷贝 改名 也是5个  
		_BackColor("Back Main Color", Color) = (1,1,1,1)
		_BackMainTex("Back Base (RGB) Gloss (A)", 2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 400
		Cull back
		//开始渲染正面      
		CGPROGRAM
		//表明是surface渲染方式 主渲染程序是surf 光照模型是BLinnPhong  
		#pragma surface surf OffLightModel    
  
        //命名规则：Lighting接#pragma suface之后起的名字   
        //lightDir :点到光源的单位向量   viewDir:点到摄像机的单位向量   atten:衰减系数   
		float4 LightingOffLightModel(SurfaceOutput s, float3 lightDir,half3 viewDir, half atten)
		{
			float4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}



		sampler2D _MainTex;
		fixed4 _Color;

		float4 _RimColor;
		float _RimPower;
		float _RimIntensity;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a * _Color.a;
		}
		ENDCG


		Cull front
		//开始渲染反面 其实和就是拷贝了一份正面渲染的代码 除了变量名要改      
		CGPROGRAM
		#pragma surface surf OffLightModel

		//命名规则：Lighting接#pragma suface之后起的名字   
		//lightDir :点到光源的单位向量   viewDir:点到摄像机的单位向量   atten:衰减系数   
		float4 LightingOffLightModel(SurfaceOutput s, float3 lightDir, half3 viewDir, half atten)
		{
			float4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}


		sampler2D _BackMainTex;
		fixed4 _BackColor;

		struct Input {
			float2 uv_BackMainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_BackMainTex, IN.uv_BackMainTex);
			o.Albedo = tex.rgb * _BackColor.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a * _BackColor.a;
			o.Normal = o.Normal;
		}
		ENDCG
	}
	FallBack "Specular"
}