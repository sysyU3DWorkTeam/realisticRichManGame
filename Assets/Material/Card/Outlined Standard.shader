
 Shader "ztc_Blend" 
 {
     Properties 
     {
         _Color ("Main Color",Color) = (1,1,1,0.5)
         _MainTex ("FrontTex (RGB) Gloss (A)", 2D) = "white" {}
         _BackTex ("BackTex (RGB) Gloss (A)", 2D) = "white" {}
     }
     SubShader 
     {
         Pass
         {
             Material
             {
                 Diffuse[_Color]
             }
             Lighting On
             SetTexture [_MainTex] //the default Texture
             {
                 Combine texture
             }
             SetTexture [_BackTex] //use the combine lerp to mix two texture by the Main color's Alpha
             {
                 constantColor [_Color]
                 Combine previous lerp(constant) texture
             }
             
         }
     } 
     FallBack "Diffuse"
 }