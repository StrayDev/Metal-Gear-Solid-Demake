Shader "Hidden/PP_CRT"
{
    Properties
    {
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Intensity ("Intensity", range(0, 1)) = 0.5
		_Tiling ("CRT Effect Tiling", int) = 3
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
            };

			// Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.screenPos = ComputeScreenPos(v.vertex);
                return o;
            }

			// Fragment Parameters
            sampler2D _MainTex;
			float _Intensity;
			int _Tiling;

			// Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				float2 pixel = (i.screenPos.xy * _ScreenParams.xy / i.screenPos.w) / _Tiling;

				uint pp = (uint)pixel.x % 3;

				// Create emtpy color to assign colors
				float4 CRTColor = float4(0, 0, 0, 1);

				if (pp == 1)
				{
					CRTColor.r = col.r;
				}
				if (pp == 2)
				{
					CRTColor.g = col.g;
				}
				else
				{
					CRTColor.b = col.b;
				}

                return lerp(col, CRTColor, _Intensity);
            }
            ENDCG
        }
    }
}
