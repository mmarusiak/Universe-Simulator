// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Unity CG Sprite Circle Shader v1.0 - Mitch Zais

// https://forum.unity.com/threads/i-need-a-circular-sprite.245021/
Shader "MZ/Sprites/Sprite_CircleCutout" 
{
	Properties 
	{
	   [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	   _Color("Tint / Transparency", Color) = (1,1,1,1)
	   _CutTint("Cut Tint / Transparency", Color) = (1,1,1,0)
	   _CutOffX("Cut Offset X", Float) = 0.50
	   _CutOffY("Cut Offset Y", Float) = 0.50
	   _Radius("Cut Radius", Float) = 0.50
	   _BlurWidth("Edge Blur Width", Float) = 1
	   [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader 
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		 	Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
	    Pass 
	    {
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#pragma glsl
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#include "UnityCG.cginc"
			float4 _Color;
			float4 _CutTint;
			float _CutOffX;
			float _CutOffY;
			float _Radius;
			float _BlurWidth;
			sampler2D _MainTex;
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			v2f vert (appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}
			fixed4 frag(v2f IN) : COLOR
			{
				float X = _CutOffX - IN.texcoord.x, Y = _CutOffY - IN.texcoord.y;
				float uvDist = X * X + Y * Y;
				float cenDist = _Radius * 0.01 - uvDist;
				float blurDist = _BlurWidth * 4 * length( float2( ddx(cenDist),ddy(cenDist) ) );
				float alphaVal = saturate((cenDist / blurDist));
				return lerp(tex2D(_MainTex,IN.texcoord) * _CutTint, tex2D(_MainTex,IN.texcoord) * _Color, alphaVal);
			}
		ENDCG
	    }
	} 
Fallback "Transparent/VertexLit"
}
