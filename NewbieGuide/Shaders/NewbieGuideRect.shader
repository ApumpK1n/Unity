Shader "UI/NewbieGuide/Rect"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15


		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0


			//-------------------add----------------------
		_Center("Center", vector) = (0, 0, 0, 0)
		_SliderX("SliderX",Range(0,1500)) = 1500
		_SliderY("SliderY",Range(0,1500)) = 1500
			//-------------------add----------------------
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile __ UNITY_UI_ALPHACLIP

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					float4 screenPos : TEXCOORD2;
					UNITY_VERTEX_OUTPUT_STEREO

				};

				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;
				//-------------------add----------------------
				float2 _Center;
				float _SliderX;
				float _SliderY;
				//-------------------add----------------------
				v2f vert(appdata_t IN)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(IN);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
					OUT.worldPosition = IN.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = IN.texcoord;

					OUT.color = IN.color * _Color;
					OUT.screenPos = ComputeScreenPos(OUT.vertex);
					return OUT;
				}

				sampler2D _MainTex;

				fixed4 frag(v2f IN) : SV_Target
				{
					half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif
					//-------------------add----------------------
					IN.screenPos = IN.screenPos / IN.screenPos.w; // deal perspective
					IN.screenPos.xy = IN.screenPos.xy * _ScreenParams.xy;
					float2 dis = IN.screenPos.xy - _Center.xy;
					color.a *= (abs(dis.x) > _SliderX) || (abs(dis.y) > _SliderY);
					color.rgb *= color.a;

					return color;
					//-------------------add----------------------

				}
			ENDCG
			}
		}
}