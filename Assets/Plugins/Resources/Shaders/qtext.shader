Shader "YoulongShaders/Text Shader" {
Properties {
   _MainTex ("Font Texture", 2D) = "white" {} 
   _Color ("Text Color", Color) = (1,1,1,1) 
   _OutlineCol("OutLine Color", Color) = (0,0,0,1)
}

Category {
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
	AlphaTest Greater  0.4
	Cull Back  
	Lighting Off 
	ZWrite On 
	Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
		Bind "Color", color
	}
	
	// ---- Fragment program cards
	SubShader 
	{
		/*Pass 
		{
			CGPROGRAM
			#pragma vertex   vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
			    float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				float4 pos = v.vertex;
				pos = mul(UNITY_MATRIX_MV, pos);
				float orthoSize = 2 / UNITY_MATRIX_P[1][1];
				float worldUnitPerPixel = orthoSize / _ScreenParams.y;
				
				float pixels = pos.x / worldUnitPerPixel;
				pos.x = floor(pixels) * worldUnitPerPixel - 0.5 * worldUnitPerPixel;
				pixels = pos.y / worldUnitPerPixel;
				pos.y = floor(pixels) * worldUnitPerPixel - 0.5 * worldUnitPerPixel;
				
				o.vertex = mul(UNITY_MATRIX_P, pos);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{	
				half4 col = tex2D(_MainTex, i.texcoord);
				return float4(0, 0, 0, col.a);
			}
			ENDCG 
		}*/
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex   vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _OutlineCol;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f {
			    float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};
			
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float orthoSize = 2 / UNITY_MATRIX_P[1][1];
				float worldUnitPerPixel = orthoSize / _ScreenParams.y;
				
				float pixels = pos.x / worldUnitPerPixel;
				pos.x = floor(pixels) * worldUnitPerPixel + 0.5 * worldUnitPerPixel;
				pixels = pos.y / worldUnitPerPixel;
				pos.y = floor(pixels) * worldUnitPerPixel + 0.5 * worldUnitPerPixel;
				
				o.vertex = mul(UNITY_MATRIX_P, pos);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{	
				half4 col = tex2D(_MainTex, i.texcoord);
				half4 outlineCol = ((1 - col.a) * 2) * _OutlineCol;
				outlineCol.a = 0;
				return float4(i.color.x * col.x, i.color.y * col.y, i.color.z * col.z, col.a) +  outlineCol;
			}
			ENDCG 
		}
	} 	
}
}
