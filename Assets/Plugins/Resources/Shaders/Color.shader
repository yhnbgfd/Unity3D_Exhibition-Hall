Shader "YoulongShaders/Color" {
Properties {
	_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off 
	BindChannels {
		Bind "Vertex", vertex
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

			fixed4 _Color;
			
			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
			    float4 vertex : POSITION;
			};
			

			v2f vert (appdata_t v)
			{
				v2f o;
				
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				
				o.vertex = mul(UNITY_MATRIX_P, pos);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{	
				return _Color;
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
