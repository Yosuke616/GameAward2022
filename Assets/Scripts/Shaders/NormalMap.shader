Shader "NormalMap"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}

		[Normal] _NormalMap("Normal map", 2D) = "bump" {}

		_Shininess("Shininess", Range(0.0, 1.0)) = 0.078125
	}
		SubShader
		{

		Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}

		Pass {
			CGPROGRAM
		   #include "UnityCG.cginc"

		   #pragma vertex vert
		   #pragma fragment frag

			float4 _LightColor0;
			sampler2D _MainTex;
			sampler2D _NormalMap;
			half _Shininess;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
								 float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				half3 lightDir : TEXCOORD1;
				half3 viewDir : TEXCOORD2;
			};

			v2f vert(appdata v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;

				// UnityCG.cginc に定義されているマクロ
				TANGENT_SPACE_ROTATION;
				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
				o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));

				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				i.lightDir = normalize(i.lightDir);
				i.viewDir = normalize(i.viewDir);
				//half3 halfDir = normalize(i.lightDir + i.viewDir);
				half3 halfDir = normalize(i.lightDir);

				fixed4 tex = tex2D(_MainTex, i.uv);
				// ノーマルマップから法線を取得
				half3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
				// ノーマルマップの法線が確実に正規化されているならなくてもいい
				normal = normalize(normal);

				half3 diffuse = max(0, dot(normal, i.lightDir)) * _LightColor0.rgb;
				half3 specular = pow(max(0, dot(normal, halfDir)), _Shininess * 256.0) * _LightColor0.rgb;

				fixed4 color;
				//color.rgb = tex.rgb * diffuse + specular;
				color.rgb = tex.rgb * diffuse * 1.5f;
				return color;
			}

			ENDCG
		}

		}
			FallBack "Diffuse"
}