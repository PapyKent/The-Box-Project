// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NicoFxs/Platforms"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Noise("Noise", 2D) = "white" {}
		[HDR]_Emissive("Emissive", Color) = (0,0.3512595,11.18176,1)
		_BaseColor("Base Color", Color) = (0,0.2048912,1,0)
		_UVScale("UV Scale", Float) = 25
		_HorizontalHeightStartGradient("Horizontal Height Start Gradient", Float) = 0.3
		_VerticalHeightStartGradient("Vertical Height Start Gradient", Float) = 0.3
		_Dissolveamount("Dissolve amount", Range( 1.35 , 5)) = 1.35
		_Speed("Speed", Float) = 0.15
		_EmissiveContrast("Emissive Contrast", Float) = 1.5
		_Transparency("Transparency", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BaseColor;
		uniform float4 _Emissive;
		uniform float _VerticalHeightStartGradient;
		uniform sampler2D _Noise;
		uniform float _Speed;
		uniform float _UVScale;
		uniform float _Dissolveamount;
		uniform float _EmissiveContrast;
		uniform float _Transparency;
		uniform float _HorizontalHeightStartGradient;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 objToWorld313 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			v.vertex.xyz += ( float3(0.001,0,0) * objToWorld313.y );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_106_0 = ( ase_vertex3Pos.y - -_VerticalHeightStartGradient );
			float temp_output_188_0 = ( ( _Speed * _Time.x ) * 5.0 );
			float3 ase_worldPos = i.worldPos;
			float2 appendResult179 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 panner187 = ( temp_output_188_0 * float2( -0.1,-0.1 ) + ( appendResult179 / _UVScale ));
			float2 temp_output_181_0 = ( appendResult179 / _UVScale );
			float2 panner186 = ( temp_output_188_0 * float2( 0.2,0.1 ) + temp_output_181_0);
			float temp_output_197_0 = ( 1.0 - _Dissolveamount );
			float4 temp_output_194_0 = ( tex2D( _Noise, panner187 ) + tex2D( _Noise, panner186 ) + temp_output_197_0 );
			float4 lerpResult206 = lerp( _BaseColor , _Emissive , saturate( ( temp_output_106_0 / ( temp_output_194_0 * _EmissiveContrast ) ) ));
			o.Emission = lerpResult206.rgb;
			o.Alpha = _Transparency;
			float4 color162 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 color161 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float clampResult155 = clamp( ( ( ase_vertex3Pos.y - _VerticalHeightStartGradient ) / 0.15 ) , 0.0 , 1.0 );
			float clampResult157 = clamp( ( temp_output_106_0 / -0.15 ) , 0.0 , 1.0 );
			float4 lerpResult163 = lerp( color162 , color161 , ( clampResult155 + clampResult157 ));
			float4 color297 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 color298 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float clampResult296 = clamp( ( ( ase_vertex3Pos.z - _HorizontalHeightStartGradient ) / 0.15 ) , 0.0 , 1.0 );
			float clampResult295 = clamp( ( ( ase_vertex3Pos.z - -_HorizontalHeightStartGradient ) / -0.15 ) , 0.0 , 1.0 );
			float4 lerpResult300 = lerp( color297 , color298 , ( clampResult296 + clampResult295 ));
			float4 temp_output_303_0 = ( lerpResult163 * lerpResult300 );
			clip( ( ( temp_output_194_0 * temp_output_303_0 ) + temp_output_303_0 ).r - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
7;1081;1906;1010;1784.559;509.0988;1.200766;True;True
Node;AmplifyShaderEditor.CommentaryNode;111;-2345.741,-919.4631;Float;False;1536.379;505.7205;Dissolve - Edges;8;114;187;188;189;191;196;197;199;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;177;-2576.709,-1442.818;Float;False;988.8707;455.5483;;8;182;181;180;179;178;222;248;250;World UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;178;-2472.473,-1311.998;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TimeNode;190;-2122.878,-489.1553;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;191;-2073.378,-593.8556;Float;False;Property;_Speed;Speed;8;0;Create;True;0;0;False;0;0.15;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;281;-2429.772,940.9749;Float;False;Property;_HorizontalHeightStartGradient;Horizontal Height Start Gradient;5;0;Create;True;0;0;False;0;0.3;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-2413.813,-125.5849;Float;False;Property;_VerticalHeightStartGradient;Vertical Height Start Gradient;6;0;Create;True;0;0;False;0;0.3;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;284;-2221.771,1020.974;Float;False;Constant;_HorizontalHeightGradient;Horizontal Height Gradient;5;0;Create;True;0;0;False;0;0.15;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;282;-2424.772,773.975;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;285;-2232.196,1221.557;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;147;-2413.813,-285.585;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;150;-2221.813,162.4151;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;283;-2155.225,1382.913;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-1871.88,-644.1553;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;85;-2177.223,304.7098;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;179;-2179.955,-1270.557;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-2205.813,-45.58483;Float;False;Constant;_VerticalHeightGradient;Vertical Height Gradient;6;0;Create;True;0;0;False;0;0.15;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-2191.535,-1145.043;Float;False;Property;_UVScale;UV Scale;4;0;Create;True;0;0;False;0;25;32.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;156;-2141.813,-237.5849;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;288;-1814.798,1479.133;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;286;-2118.616,1192.246;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;152;-2109.813,130.4151;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;287;-2165.771,832.975;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-1786.596,-529.797;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;181;-1994.56,-1270.557;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;-1841.224,400.7098;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;248;-2002.158,-1370.816;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-1713.224,704.7104;Float;False;Constant;_Float7;Float 7;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-1865.907,1234.719;Float;False;Constant;_Float4;Float 4;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-1853.814,98.41513;Float;False;Constant;_Float8;Float 8;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-1713.224,624.7104;Float;False;Constant;_Float0;Float 0;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;291;-1965.772,940.9749;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;294;-1637.951,1582.293;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1853.814,162.4151;Float;False;Constant;_Float9;Float 9;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;290;-1867.907,1157.506;Float;False;Constant;_Float10;Float 10;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;187;-1582.269,-628.1077;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;186;-1571.902,-500.2388;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;107;-1665.224,496.7102;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;151;-1949.814,-125.5849;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;292;-1691.315,1708.606;Float;False;Constant;_Float11;Float 11;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;293;-1689.315,1785.82;Float;False;Constant;_Float12;Float 12;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-1524.157,-846.9986;Float;False;Property;_Dissolveamount;Dissolve amount;7;0;Create;True;0;0;False;0;1.35;1.75;1.35;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;295;-1487.794,1612.003;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-1307.781,-630.5979;Float;True;Property;_Noise;Noise;1;0;Create;True;0;0;False;0;17088603c3ddc734d9c17645934db875;af775ca9fbfb23d4c88f45712d7c9c3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;157;-1505.224,528.7104;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;296;-1664.385,1060.903;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;197;-1242.181,-834.0012;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;176;-1292.565,-397.1361;Float;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;False;0;None;17088603c3ddc734d9c17645934db875;True;0;False;white;Auto;False;Instance;114;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;155;-1645.814,2.415146;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;298;-1402.213,1000.52;Float;False;Constant;_Color1;Color 1;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-1201.224,240.7098;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;162;-1441.224,96.7099;Float;False;Constant;_Color4;Color 4;10;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;297;-1413.789,1186.892;Float;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;161;-1425.224,-79.29007;Float;False;Constant;_Color3;Color 3;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;241;-850.9368,-362.2188;Float;False;Property;_EmissiveContrast;Emissive Contrast;11;0;Create;True;0;0;False;0;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;194;-870.4301,-485.3898;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;299;-1180.665,1319.903;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;163;-1153.224,48.70988;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;300;-1125.848,1138.596;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;-692.9368,-459.2188;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;237;-662.2793,-237.5155;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;-862.2355,405.9154;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;207;-537.9725,-417.983;Float;False;Property;_Emissive;Emissive;2;1;[HDR];Create;True;0;0;False;0;0,0.3512595,11.18176,1;0,0.3512595,11.18176,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;235;-502.2807,-1321.016;Float;False;965.6;572;;8;230;233;232;229;227;231;228;234;Emission gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;238;-479.2793,-216.5155;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;83;-529.9425,-578.6218;Float;False;Property;_BaseColor;Base Color;3;0;Create;True;0;0;False;0;0,0.2048912,1,0;0,0.526397,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;313;-628.9778,437.6216;Float;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-829.749,-94.48238;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;307;-597.9778,288.6216;Float;False;Constant;_Vector0;Vector 0;14;0;Create;True;0;0;False;0;0.001,0,0;0.1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;250;-2290.264,-1052.893;Float;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-288.469,-1000.244;Float;False;Property;_E2;E2;10;0;Create;True;0;0;False;0;0;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-1813.965,-1253.276;Float;False;_worldUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;206;-238.8418,16.8666;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;233;42.99599,-956.2209;Float;False;Constant;_Float5;Float 5;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;199;-1113.364,-833.2165;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;232;268.918,-961.6241;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;222;-2003.901,-1109.944;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-1882.194,-399.1621;Float;False;182;_worldUVs;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;312;-335.9778,367.6216;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;234;35.39597,-885.4071;Float;False;Constant;_Float6;Float 6;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;221;-2168.9,-1031.944;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;228;-192.469,-1177.553;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;227;-384.469,-1241.553;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;230;-496.4692,-1081.553;Float;False;Property;_E1;E1;9;0;Create;True;0;0;False;0;0;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;251;-257.1513,128.4615;Float;False;Property;_Transparency;Transparency;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;229;-32.46901,-1081.553;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;202;-217.3425,215.5871;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1.151968,9.215743;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;NicoFxs/Platforms;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;285;0;281;0
WireConnection;150;0;109;0
WireConnection;189;0;191;0
WireConnection;189;1;190;1
WireConnection;179;0;178;1
WireConnection;179;1;178;2
WireConnection;156;0;147;2
WireConnection;156;1;109;0
WireConnection;288;0;283;3
WireConnection;288;1;285;0
WireConnection;286;0;284;0
WireConnection;152;0;110;0
WireConnection;287;0;282;3
WireConnection;287;1;281;0
WireConnection;188;0;189;0
WireConnection;181;0;179;0
WireConnection;181;1;180;0
WireConnection;106;0;85;2
WireConnection;106;1;150;0
WireConnection;248;0;179;0
WireConnection;248;1;180;0
WireConnection;291;0;287;0
WireConnection;291;1;284;0
WireConnection;294;0;288;0
WireConnection;294;1;286;0
WireConnection;187;0;248;0
WireConnection;187;1;188;0
WireConnection;186;0;181;0
WireConnection;186;1;188;0
WireConnection;107;0;106;0
WireConnection;107;1;152;0
WireConnection;151;0;156;0
WireConnection;151;1;110;0
WireConnection;295;0;294;0
WireConnection;295;1;292;0
WireConnection;295;2;293;0
WireConnection;114;1;187;0
WireConnection;157;0;107;0
WireConnection;157;1;158;0
WireConnection;157;2;159;0
WireConnection;296;0;291;0
WireConnection;296;1;290;0
WireConnection;296;2;289;0
WireConnection;197;0;196;0
WireConnection;176;1;186;0
WireConnection;155;0;151;0
WireConnection;155;1;153;0
WireConnection;155;2;154;0
WireConnection;160;0;155;0
WireConnection;160;1;157;0
WireConnection;194;0;114;0
WireConnection;194;1;176;0
WireConnection;194;2;197;0
WireConnection;299;0;296;0
WireConnection;299;1;295;0
WireConnection;163;0;162;0
WireConnection;163;1;161;0
WireConnection;163;2;160;0
WireConnection;300;0;297;0
WireConnection;300;1;298;0
WireConnection;300;2;299;0
WireConnection;242;0;194;0
WireConnection;242;1;241;0
WireConnection;237;0;106;0
WireConnection;237;1;242;0
WireConnection;303;0;163;0
WireConnection;303;1;300;0
WireConnection;238;0;237;0
WireConnection;223;0;194;0
WireConnection;223;1;303;0
WireConnection;182;0;181;0
WireConnection;206;0;83;0
WireConnection;206;1;207;0
WireConnection;206;2;238;0
WireConnection;199;0;197;0
WireConnection;232;0;229;0
WireConnection;232;1;233;0
WireConnection;232;2;234;0
WireConnection;222;0;180;0
WireConnection;222;1;221;4
WireConnection;222;2;250;0
WireConnection;312;0;307;0
WireConnection;312;1;313;2
WireConnection;228;0;227;2
WireConnection;228;1;230;0
WireConnection;229;0;228;0
WireConnection;229;1;231;0
WireConnection;202;0;223;0
WireConnection;202;1;303;0
WireConnection;0;2;206;0
WireConnection;0;9;251;0
WireConnection;0;10;202;0
WireConnection;0;11;312;0
ASEEND*/
//CHKSM=4231C7FCD97633FEE4BD3DE33382D66C7EC36802