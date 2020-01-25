Shader "Hidden/Custom/Drunk"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Waving;
        float _Duplicating;

        float4 Frag (float4 vertex:SV_POSITION):COLOR
        {            
			vector <float,2> uv = vertex.xy/_ScreenParams.xy;
			uv.x+=cos(uv.y*2.0+_Time.g)*_Waving;
			uv.y+=sin(uv.x*2.0+_Time.g)*_Waving;
			float offset = sin(_Time.g *0.5) * _Duplicating;    
			float4 a = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);   
			float4 b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv-float2(sin(offset),0.0));    
			float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv+float2(sin(offset),0.0));    
			float4 d = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv-float2(0.0,sin(offset)));    
			float4 e = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv+float2(0.0,sin(offset)));        
			return (a+b+c+d+e)/5.0;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}