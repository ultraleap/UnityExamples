Shader "Ultrahaptics/BasicTransparency" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _OpacityValue("OpacityValue", Range(0, 1)) = 0.6923077
        [HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }
        SubShader{
            Tags {
                "IgnoreProjector" = "True"
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
            }
            Pass { ColorMask 0 }
            Pass {
                Name "FORWARD"
                Tags {
                    "LightMode" = "ForwardBase"
                }
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #pragma multi_compile_fwdbase
                #pragma multi_compile_fog
                #pragma only_renderers d3d9 d3d11 glcore gles metal 
                #pragma target 3.0
                uniform float4 _LightColor0;
                uniform float4 _Color;
                uniform float _OpacityValue;
                struct VertexInput {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };
                struct VertexOutput {
                    float4 pos : SV_POSITION;
                    float4 posWorld : TEXCOORD0;
                    float3 normalDir : TEXCOORD1;
                    UNITY_FOG_COORDS(2)
                };
                VertexOutput vert(VertexInput v) {
                    VertexOutput o = (VertexOutput)0;
                    o.normalDir = UnityObjectToWorldNormal(v.normal);
                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                    float3 lightColor = _LightColor0.rgb;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    UNITY_TRANSFER_FOG(o,o.pos);
                    return o;
                }
                float4 frag(VertexOutput i) : COLOR {
                    i.normalDir = normalize(i.normalDir);
                    float3 normalDirection = i.normalDir;
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    float3 lightColor = _LightColor0.rgb;
                    ////// Lighting:
                                    float attenuation = 1;
                                    float3 attenColor = attenuation * _LightColor0.xyz;
                                    /////// Diffuse:
                                                    float NdotL = max(0.0,dot(normalDirection, lightDirection));
                                                    float3 directDiffuse = max(0.0, NdotL) * attenColor;
                                                    float3 indirectDiffuse = float3(0,0,0);
                                                    indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                                                    float3 diffuseColor = _Color.rgb;
                                                    float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                                                    /// Final Color:
                                                                    float3 finalColor = diffuse;
                                                                    fixed4 finalRGBA = fixed4(finalColor,(_Color.a*_OpacityValue));
                                                                    UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                                                                    return finalRGBA;
                                                                }
                                                                ENDCG
                                                            }
                                                            Pass {
                                                                Name "FORWARD_DELTA"
                                                                Tags {
                                                                    "LightMode" = "ForwardAdd"
                                                                }
                                                                Blend One One
                                                                ZWrite Off

                                                                CGPROGRAM
                                                                #pragma vertex vert
                                                                #pragma fragment frag
                                                                #include "UnityCG.cginc"
                                                                #include "AutoLight.cginc"
                                                                #pragma multi_compile_fwdadd
                                                                #pragma multi_compile_fog
                                                                #pragma only_renderers d3d9 d3d11 glcore gles metal 
                                                                #pragma target 3.0
                                                                uniform float4 _LightColor0;
                                                                uniform float4 _Color;
                                                                uniform float _OpacityValue;
                                                                struct VertexInput {
                                                                    float4 vertex : POSITION;
                                                                    float3 normal : NORMAL;
                                                                };
                                                                struct VertexOutput {
                                                                    float4 pos : SV_POSITION;
                                                                    float4 posWorld : TEXCOORD0;
                                                                    float3 normalDir : TEXCOORD1;
                                                                    LIGHTING_COORDS(2,3)
                                                                    UNITY_FOG_COORDS(4)
                                                                };
                                                                VertexOutput vert(VertexInput v) {
                                                                    VertexOutput o = (VertexOutput)0;
                                                                    o.normalDir = UnityObjectToWorldNormal(v.normal);
                                                                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                                                                    float3 lightColor = _LightColor0.rgb;
                                                                    o.pos = UnityObjectToClipPos(v.vertex);
                                                                    UNITY_TRANSFER_FOG(o,o.pos);
                                                                    TRANSFER_VERTEX_TO_FRAGMENT(o)
                                                                    return o;
                                                                }
                                                                float4 frag(VertexOutput i) : COLOR {
                                                                    i.normalDir = normalize(i.normalDir);
                                                                    float3 normalDirection = i.normalDir;
                                                                    float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                                                                    float3 lightColor = _LightColor0.rgb;
                                                                    ////// Lighting:
                                                                                    UNITY_LIGHT_ATTENUATION(attenuation, i, _LightColor0.xyz);
                                                                                    float3 attenColor = attenuation * _LightColor0.xyz;
                                                                                    /////// Diffuse:
                                                                                                    float NdotL = max(0.0,dot(normalDirection, lightDirection));
                                                                                                    float3 directDiffuse = max(0.0, NdotL) * attenColor;
                                                                                                    float3 diffuseColor = _Color.rgb;
                                                                                                    float3 diffuse = directDiffuse * diffuseColor;
                                                                                                    /// Final Color:
                                                                                                                    float3 finalColor = diffuse;
                                                                                                                    fixed4 finalRGBA = fixed4(finalColor * (_Color.a*_OpacityValue),0);
                                                                                                                    UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                                                                                                                    return finalRGBA;
                                                                                                                }
                                                                                                                ENDCG
                                                                                                            }
    }
        FallBack "Standard"
}
