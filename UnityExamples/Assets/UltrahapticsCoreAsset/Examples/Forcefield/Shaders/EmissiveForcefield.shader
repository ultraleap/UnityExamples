

Shader "Ultrahaptics/ForcefieldShader" {
    Properties{
        _RefractionIntensity("Refraction Intensity", Range(0, 1)) = 0.5791302
        _Refraction("Refraction", 2D) = "bump" {}
        _BlendDepth("Blend Depth", Range(0, 1)) = 0
        _node_6138("node_6138", Color) = (0.2941176,0,0,1)
        _node_6996("node_6996", 2D) = "bump" {}
        _Opacity("Opacity", Range(0, 1)) = 0.2106182
        _node_4508("node_4508", 2D) = "black" {}
        _node_664("node_664", Color) = (0,0.462069,1,1)
        _node_5070("node_5070", 2D) = "white" {}
        [HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        GrabPass{ }
        Pass { ColorMask 0 }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode" = "ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _RefractionIntensity;
            uniform sampler2D _Refraction; uniform float4 _Refraction_ST;
            uniform float _BlendDepth;
            uniform float4 _node_6138;
            uniform sampler2D _node_6996; uniform float4 _node_6996_ST;
            uniform float _Opacity;
            uniform sampler2D _node_4508; uniform float4 _node_4508_ST;
            uniform float4 _node_664;
            uniform sampler2D _node_5070; uniform float4 _node_5070_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                LIGHTING_COORDS(7,8)
            };
            VertexOutput vert(VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = (facing >= 0 ? 1 : 0);
                float faceSign = (facing >= 0 ? 1 : -1);
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4(i.screenPos.xy / i.screenPos.w, 0, 0);
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_7948 = _Time + _TimeEditor;
                float2 node_27 = ((i.uv0 + node_7948.g*float2(0,0.2))*1.0);
                float3 _Refraction_var = UnpackNormal(tex2D(_Refraction,TRANSFORM_TEX(node_27, _Refraction)));
                float3 normalLocal = lerp(float3(0,0,1),_Refraction_var.rgb,_RefractionIntensity);
                float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
                float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
                float sceneZ = max(0,LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5 + 0.5 + (_Refraction_var.rgb.rg*(_RefractionIntensity*0.2));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection + lightDirection);
                ////// Lighting:
                    UNITY_LIGHT_ATTENUATION(attenuation, i, _LightColor0.xyz);
                    float3 attenColor = attenuation * _LightColor0.xyz;
                    float Pi = 3.141592654;
                    float InvPi = 0.31830988618;
                ///////// Gloss:
                    float gloss = 0.6;
                    float perceptualRoughness = 1.0 - 0.6;
                    float roughness = perceptualRoughness * perceptualRoughness;
                    float specPow = exp2(gloss * 10.0 + 1.0);
                /////// GI Data:
                    UnityLight light;
                    #ifdef LIGHTMAP_OFF
                        light.color = lightColor;
                        light.dir = lightDirection;
                        light.ndotl = LambertTerm(normalDirection, light.dir);
                    #else
                        light.color = half3(0.f, 0.f, 0.f);
                        light.ndotl = 0.0f;
                        light.dir = half3(0.f, 0.f, 0.f);
                    #endif
                    UnityGIInput d;
                    d.light = light;
                    d.worldPos = i.posWorld.xyz;
                    d.worldViewDir = viewDirection;
                    d.atten = attenuation;
                    #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                        d.boxMin[0] = unity_SpecCube0_BoxMin;
                        d.boxMin[1] = unity_SpecCube1_BoxMin;
                    #endif
                    #if UNITY_SPECCUBE_BOX_PROJECTION
                        d.boxMax[0] = unity_SpecCube0_BoxMax;
                        d.boxMax[1] = unity_SpecCube1_BoxMax;
                        d.probePosition[0] = unity_SpecCube0_ProbePosition;
                        d.probePosition[1] = unity_SpecCube1_ProbePosition;
                    #endif
                    d.probeHDR[0] = unity_SpecCube0_HDR;
                    d.probeHDR[1] = unity_SpecCube1_HDR;
                    Unity_GlossyEnvironmentData ugls_en_data;
                    ugls_en_data.roughness = 1.0 - gloss;
                    ugls_en_data.reflUVW = viewReflectDirection;
                    UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data);
                    lightDirection = gi.light.dir;
                    lightColor = gi.light.color;
                ////// Specular:
                    float NdotL = saturate(dot(normalDirection, lightDirection));
                    float LdotH = saturate(dot(lightDirection, halfDirection));
                    float3 specularColor = 0.2;
                    float specularMonochrome;
                    float3 diffuseColor = _node_664.rgb; // Need this for specular when using metallic
                    diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
                    specularMonochrome = 1.0 - specularMonochrome;
                    float NdotV = abs(dot(normalDirection, viewDirection));
                    float NdotH = saturate(dot(normalDirection, halfDirection));
                    float VdotH = saturate(dot(viewDirection, halfDirection));
                    float visTerm = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
                    float normTerm = GGXTerm(NdotH, roughness);
                    float specularPBL = (visTerm*normTerm) * UNITY_PI;
                    #ifdef UNITY_COLORSPACE_GAMMA
                        specularPBL = sqrt(max(1e-4h, specularPBL));
                    #endif
                    specularPBL = max(0, specularPBL * NdotL);
                    #if defined(_SPECULARHIGHLIGHTS_OFF)
                        specularPBL = 0.0;
                    #endif
                    half surfaceReduction;
                    #ifdef UNITY_COLORSPACE_GAMMA
                        surfaceReduction = 1.0 - 0.28*roughness*perceptualRoughness;
                    #else
                        surfaceReduction = 1.0 / (roughness*roughness + 1.0);
                    #endif
                    specularPBL *= any(specularColor) ? 1.0 : 0.0;
                    float3 directSpecular = (floor(attenuation) * _LightColor0.xyz)*specularPBL*FresnelTerm(specularColor, LdotH);
                    half grazingTerm = saturate(gloss + specularMonochrome);
                    float3 indirectSpecular = (gi.indirect.specular);
                    indirectSpecular *= FresnelLerp(specularColor, grazingTerm, NdotV);
                    indirectSpecular *= surfaceReduction;
                    float3 specular = (directSpecular + indirectSpecular);
                /////// Diffuse:
                    NdotL = dot(normalDirection, lightDirection);
                    float node_29 = 0.2;
                    float3 w = float3(node_29,node_29,node_29)*0.5; // Light wrapping
                    float3 NdotLWrap = NdotL * (1.0 - w);
                    float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w);
                    float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w) * float3(node_29,node_29,node_29);
                    NdotL = max(0.0,dot(normalDirection, lightDirection));
                    half fd90 = 0.5 + 2 * LdotH * LdotH * (1 - gloss);
                    float nlPow5 = Pow5(1 - NdotLWrap);
                    float nvPow5 = Pow5(1 - NdotV);
                    float3 directDiffuse = ((forwardLight + backLight) + ((1 + (fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                    float3 indirectDiffuse = float3(0,0,0);
                    float4 _node_5070_var = tex2D(_node_5070,TRANSFORM_TEX(i.uv0, _node_5070));
                    indirectDiffuse += _node_5070_var.rgb; // Diffuse Ambient Light
                    float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                ////// Emissive:
                    float4 _node_6996_var = tex2D(_node_6996,TRANSFORM_TEX(i.uv0, _node_6996));
                    float3 emissive = (((1.0 - _node_6996_var.rgb)*_node_6138.rgb*_node_6138.a)*(1.0 - saturate((sceneZ - partZ) / _BlendDepth)));
                /// Final Color:
                    float3 finalColor = diffuse + specular + emissive;
                    float4 _node_4508_var = tex2D(_node_4508,TRANSFORM_TEX(i.uv0, _node_4508));
                    return fixed4(lerp(sceneColor.rgb, finalColor,(_Opacity + _node_4508_var.r)),1);
                }
                ENDCG
            }
            Pass {
                Name "FORWARD_DELTA"
                Tags {
                    "LightMode" = "ForwardAdd"
                }
                Blend One One
                Cull Off
                ZWrite Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #define _GLOSSYENV 1
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
                #include "UnityPBSLighting.cginc"
                #include "UnityStandardBRDF.cginc"
                #pragma multi_compile_fwdadd_fullshadows
                #pragma only_renderers d3d9 d3d11 glcore gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu
                #pragma target 3.0
                uniform sampler2D _GrabTexture;
                uniform sampler2D _CameraDepthTexture;
                uniform float4 _TimeEditor;
                uniform float _RefractionIntensity;
                uniform sampler2D _Refraction; uniform float4 _Refraction_ST;
                uniform float _BlendDepth;
                uniform float4 _node_6138;
                uniform sampler2D _node_6996; uniform float4 _node_6996_ST;
                uniform float _Opacity;
                uniform sampler2D _node_4508; uniform float4 _node_4508_ST;
                uniform float4 _node_664;
                struct VertexInput {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct VertexOutput {
                    float4 pos : SV_POSITION;
                    float2 uv0 : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                    float3 normalDir : TEXCOORD2;
                    float3 tangentDir : TEXCOORD3;
                    float3 bitangentDir : TEXCOORD4;
                    float4 screenPos : TEXCOORD5;
                    float4 projPos : TEXCOORD6;
                    LIGHTING_COORDS(7,8)
                };
                VertexOutput vert(VertexInput v) {
                    VertexOutput o = (VertexOutput)0;
                    o.uv0 = v.texcoord0;
                    o.normalDir = UnityObjectToWorldNormal(v.normal);
                    o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                    o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                    float3 lightColor = _LightColor0.rgb;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.projPos = ComputeScreenPos(o.pos);
                    COMPUTE_EYEDEPTH(o.projPos.z);
                    o.screenPos = o.pos;
                    TRANSFER_VERTEX_TO_FRAGMENT(o)
                    return o;
                }
                float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                    float isFrontFace = (facing >= 0 ? 1 : 0);
                    float faceSign = (facing >= 0 ? 1 : -1);
                    #if UNITY_UV_STARTS_AT_TOP
                        float grabSign = -_ProjectionParams.x;
                    #else
                        float grabSign = _ProjectionParams.x;
                    #endif
                    i.screenPos = float4(i.screenPos.xy / i.screenPos.w, 0, 0);
                    i.screenPos.y *= _ProjectionParams.x;
                    float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
                    float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                    float4 node_9336 = _Time + _TimeEditor;
                    float2 node_27 = ((i.uv0 + node_9336.g*float2(0,0.2))*1.0);
                    float3 _Refraction_var = UnpackNormal(tex2D(_Refraction,TRANSFORM_TEX(node_27, _Refraction)));
                    float3 normalLocal = lerp(float3(0,0,1),_Refraction_var.rgb,_RefractionIntensity);
                    float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
                    float sceneZ = max(0,LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                    float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                    float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5 + 0.5 + (_Refraction_var.rgb.rg*(_RefractionIntensity*0.2));
                    float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                    float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                    float3 lightColor = _LightColor0.rgb;
                    float3 halfDirection = normalize(viewDirection + lightDirection);
                    ////// Lighting:
                        UNITY_LIGHT_ATTENUATION(attenuation, i, _LightColor0.xyz);
                        float3 attenColor = attenuation * _LightColor0.xyz;
                        float Pi = 3.141592654;
                        float InvPi = 0.31830988618;
                    ///////// Gloss:
                        float gloss = 0.6;
                        float perceptualRoughness = 1.0 - 0.6;
                        float roughness = perceptualRoughness * perceptualRoughness;
                        float specPow = exp2(gloss * 10.0 + 1.0);
                    ////// Specular:
                        float NdotL = saturate(dot(normalDirection, lightDirection));
                        float LdotH = saturate(dot(lightDirection, halfDirection));
                        float3 specularColor = 0.2;
                        float specularMonochrome;
                        float3 diffuseColor = _node_664.rgb; // Need this for specular when using metallic
                        diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
                        specularMonochrome = 1.0 - specularMonochrome;
                        float NdotV = abs(dot(normalDirection, viewDirection));
                        float NdotH = saturate(dot(normalDirection, halfDirection));
                        float VdotH = saturate(dot(viewDirection, halfDirection));
                        float visTerm = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
                        float normTerm = GGXTerm(NdotH, roughness);
                        float specularPBL = (visTerm*normTerm) * UNITY_PI;
                        #ifdef UNITY_COLORSPACE_GAMMA
                            specularPBL = sqrt(max(1e-4h, specularPBL));
                        #endif
                        specularPBL = max(0, specularPBL * NdotL);
                        #if defined(_SPECULARHIGHLIGHTS_OFF)
                            specularPBL = 0.0;
                        #endif
                        specularPBL *= any(specularColor) ? 1.0 : 0.0;
                        float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                        float3 specular = directSpecular;
                    /////// Diffuse:
                        NdotL = dot(normalDirection, lightDirection);
                        float node_29 = 0.2;
                        float3 w = float3(node_29,node_29,node_29)*0.5; // Light wrapping
                        float3 NdotLWrap = NdotL * (1.0 - w);
                        float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w);
                        float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w) * float3(node_29,node_29,node_29);
                        NdotL = max(0.0,dot(normalDirection, lightDirection));
                        half fd90 = 0.5 + 2 * LdotH * LdotH * (1 - gloss);
                        float nlPow5 = Pow5(1 - NdotLWrap);
                        float nvPow5 = Pow5(1 - NdotV);
                        float3 directDiffuse = ((forwardLight + backLight) + ((1 + (fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                        float3 diffuse = directDiffuse * diffuseColor;
                    /// Final Color:
                        float3 finalColor = diffuse + specular;
                        float4 _node_4508_var = tex2D(_node_4508,TRANSFORM_TEX(i.uv0, _node_4508));
                        return fixed4(finalColor * (_Opacity + _node_4508_var.r),0);
                    }
                    ENDCG
                }
    }
        FallBack "Diffuse"

}
