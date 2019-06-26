Shader "Nick/Backface_old002" {
    //http://answers.unity3d.com/questions/514637/how-do-i-invert-normals-of-a-sphere.html

    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Front
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        fixed4 _Color;
        
        struct Input {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Emission = c.rgb;
        }

        ENDCG  
    }
    
    FallBack "Diffuse"
 }