Shader "TFN/DepthMask"
{
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
        }
 
        Pass
        {
            ZWrite Off
        }
    }
}