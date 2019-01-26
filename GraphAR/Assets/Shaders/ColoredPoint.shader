// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/ColoredPoint" {
	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		#pragma target 3.0

//		sampler2D _MainTex;

		struct Input {
			float3 worldPos;
			float2 uv_MainTex;
   			float3 localPos;
		};
 
 		void vert (inout appdata_full v, out Input o) {
   			UNITY_INITIALIZE_OUTPUT(Input,o);
   			o.localPos = v.vertex.xyz;
 		}			

		half _Glossiness;
		half _Metallic;
//		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
			o.Alpha = 1;
			o.Albedo.rgb = IN.localPos * 3 + 0.4;
		}
		ENDCG
	}
	FallBack "Diffuse"
}