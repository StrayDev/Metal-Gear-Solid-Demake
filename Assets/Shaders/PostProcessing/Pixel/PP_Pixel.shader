Shader "Hidden/PP_Pixel"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_ResX ("Resolution X", float) = 256
		_ResY ("Resolution Y", float) = 128
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

			// Vertex to frag Struct
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 screenPosition : TEXCOORD1;
            };

			// Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

			// Fragment Shader Parameters
            sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ResX, _ResY;

			// Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
				// Assign rendered texture to col
                fixed4 col = tex2D(_MainTex, i.uv);
				
				float4 screenPos = i.screenPosition / i.screenPosition.w;
				
				float2 pixelSize = float2(_ResX, _ResY);
				float2 pixelUV = floor(screenPos.xy * pixelSize) / pixelSize;

				// Return render texture with modified UV
                return tex2D(_MainTex, pixelUV);
            }
            ENDCG
        }
    }
}
