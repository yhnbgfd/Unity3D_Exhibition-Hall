Shader "YoulongShaders/UI_Transparent" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("UI Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	//AlphaTest Greater  0.01
	//ColorMask RGB
	//Cull Back  
	//Lighting Off 
	//ZWrite Off 
	//Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader 
	{
		Pass 
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
				
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				/*float orthoSize = 2 / UNITY_MATRIX_P[1][1];
				float worldUnitPerPixel = orthoSize / _ScreenParams.y;
			
				float pixels = pos.x / worldUnitPerPixel;
				pos.x = floor(pixels) * worldUnitPerPixel + 0.5 * worldUnitPerPixel;
				pixels = pos.y / worldUnitPerPixel;
				pos.y = floor(pixels) * worldUnitPerPixel + 0.5 * worldUnitPerPixel;*/
				
				o.vertex = mul(UNITY_MATRIX_P, pos);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{	
				return tex2D(_MainTex, i.texcoord) * _Color.a;
			}
			ENDCG 
		}
	} 	
	
	// ---- Dual texture cards
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				constantColor [_TintColor]
				combine constant * primary
			}
			SetTexture [_MainTex] {
				combine texture * previous DOUBLE
			}
		}
	}
	
	// ---- Single texture cards (does not do color tint)
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
	
	FallBack "Sprite/Vertex Colored"
}
}
