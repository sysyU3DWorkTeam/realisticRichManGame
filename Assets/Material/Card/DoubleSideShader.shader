Shader "Custom/DoubleShader" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)//Tint Color  
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
		_BackColor("Main Color", Color) = (1,1,1,1)//Tint Color  
		_BackTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass{
			Cull Back
			Lighting Off
			SetTexture[_MainTex]{ combine texture }
			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
				Combine Previous * Constant
			}
		}

		Pass
		{
			Cull Front
			Lighting Off
			SetTexture[_BackTex]{ combine texture }
			SetTexture[_BackTex]
			{
				ConstantColor[_BackColor]
				Combine Previous * Constant
			}
		}
	}
}