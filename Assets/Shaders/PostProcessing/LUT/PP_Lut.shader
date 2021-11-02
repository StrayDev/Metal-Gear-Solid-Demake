Shader "Hidden/PP_Lut"
{
    Properties
    {
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Lut("LUT", 2D) = "white" {}
		_LutLevel("Lut level", Range(0, 1)) = 1
		_RedIntensity("Red channel Intensity", Range(0, 1)) = 0.5
		_GreenIntensity("Green channel Intensity", Range(0, 1)) = 0.5
		_BlueIntensity("Blue channel Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			#define COLORS 32.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex, _Lut;
			float4 _Lut_TexelSize;
			float _LutLevel, _RedIntensity, _GreenIntensity, _BlueIntensity;

            fixed4 frag (v2f i) : SV_Target
            {
				float maxColor = COLORS - 1.0;

				fixed4 col = saturate(tex2D(_MainTex, i.uv));
				float halfColorX = 0.5 / _Lut_TexelSize.z;
				float halfColorY = 0.5 / _Lut_TexelSize.w;

				float threshold = maxColor / COLORS;

				col.r = col.r * _RedIntensity;
				col.g = col.g * _GreenIntensity;
				col.b = col.b * _BlueIntensity;

				float xOffset = halfColorX + col.r * threshold / COLORS;
				float yOffset = halfColorY + col.g * threshold;

				float cell = floor(col.b * maxColor);
				float cell2 = floor(col.r * maxColor);

				float2 lutPos = float2(cell / COLORS + xOffset, cell + yOffset);

				float4 gradedColor = tex2D(_Lut, lutPos);

				// return col;
				return lerp(col, gradedColor, _LutLevel);
            }
            ENDCG
        }
    }
}
