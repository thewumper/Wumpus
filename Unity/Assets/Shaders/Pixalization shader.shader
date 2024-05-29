Shader "Hidden/Pixalization shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width ("Width", Integer) = 100
        _Height ("Height", Integer) = 100
        _DistortionSpeed ("Distortion", Float) = 1.0
        _DistortionMagnitude ("Distortion Magnitude", Float) = 5.0
        _DistortionMinTime ("Distortion Min", Float) = 1.0
        _DistortionMaxTime ("Distortion Min", Float) = 3.0
        _OverallDistortionMag ("Overall distortion", Float) = 20.0
        _OverallDistortionFrequency ("Overall distortion frequency", Float) = 0.1
        _OverallDistortionChangeRate ("Overall distortion change", Float) = 0.3
        _DoDistortionAfterPixelization ("Distortion first", Integer) = 0
        _PosterzationBands ("Posterzation Bands", Integer) = 100

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Width;
            float _Height;
            float _DistortionSpeed;
            float _DistortionMagnitude;
            float _DistortionMinTime;
            float _DistortionMaxTime;
            float _OverallDistortionMag;
            float _OverallDistortionChangeRate;
            int _DoDistortionAfterPixelization;
            float _OverallDistortionFrequency;
            int _PosterzationBands;
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 loopedTime = _Time % (_DistortionMaxTime - _DistortionMinTime) + _DistortionMinTime;
                float2 distortAmount = float2 ((sin((uv.y + (_Time * _OverallDistortionChangeRate)) *_OverallDistortionFrequency) * _OverallDistortionMag).x,0);


                float2 newRes = float2 (_Width, _Height) + (sin(loopedTime * _DistortionSpeed) * _DistortionMagnitude).xy;
                uv = uv*newRes;
                if (_DoDistortionAfterPixelization == 1)
                {
                    uv += distortAmount;
                }
                
                uv = floor(uv);
                if (_DoDistortionAfterPixelization == 0)
                {
                    uv += distortAmount;
                }
                uv /= newRes;

                fixed4 col = tex2D(_MainTex, uv);
                col.rgb = floor((col.rgb * _PosterzationBands)) / _PosterzationBands;
                
                return col;
            }
            ENDCG
        }
    }
}
