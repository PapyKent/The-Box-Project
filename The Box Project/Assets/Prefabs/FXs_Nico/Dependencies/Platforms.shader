// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Particles/Platforms"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Noise("Noise", 2D) = "white" {}
		[HDR]_Emissive("Emissive", Color) = (0,0.3512595,11.18176,1)
		_BaseColor("Base Color", Color) = (0,0.2048912,1,0)
		_UVScale("UV Scale", Float) = 25
		_HeightGradient("Height Gradient", Float) = 0.15
		_HeightStartGradient("Height Start Gradient", Float) = 0.3
		_Dissolveamount("Dissolve amount", Range( 1.35 , 5)) = 1.35
		_Speed("Speed", Float) = 0.15
		_EmissiveContrast("Emissive Contrast", Float) = 1.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BaseColor;
		uniform float4 _Emissive;
		uniform float _HeightStartGradient;
		uniform sampler2D _Noise;
		uniform float _Speed;
		uniform float _UVScale;
		uniform float _Dissolveamount;
		uniform float _EmissiveContrast;
		uniform float _HeightGradient;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_106_0 = ( ase_vertex3Pos.y - -_HeightStartGradient );
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
			o.Alpha = 1;
			float4 color162 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 color161 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float clampResult155 = clamp( ( ( ase_vertex3Pos.y - _HeightStartGradient ) / _HeightGradient ) , 0.0 , 1.0 );
			float clampResult157 = clamp( ( temp_output_106_0 / -_HeightGradient ) , 0.0 , 1.0 );
			float4 lerpResult163 = lerp( color162 , color161 , ( clampResult155 + clampResult157 ));
			clip( ( ( temp_output_194_0 * lerpResult163 ) + lerpResult163 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
7;1081;1906;1010;3112.211;1769.114;1.507259;True;True
Node;AmplifyShaderEditor.CommentaryNode;111;-2345.741,-919.4631;Float;False;1536.379;505.7205;Dissolve - Edges;8;114;187;188;189;191;196;197;199;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;177;-3284.73,-1985.737;Float;False;988.8707;455.5483;;8;182;181;180;179;178;222;248;250;World UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-2073.378,-593.8556;Float;False;Property;_Speed;Speed;9;0;Create;True;0;0;False;0;0.15;0.71;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;178;-3180.494,-1854.918;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TimeNode;190;-2122.878,-489.1553;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;179;-2887.976,-1813.477;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-2899.556,-1687.963;Float;False;Property;_UVScale;UV Scale;5;0;Create;True;0;0;False;0;25;8.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-2128,-32;Float;False;Property;_HeightStartGradient;Height Start Gradient;7;0;Create;True;0;0;False;0;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-1871.88,-644.1553;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-1920,48;Float;False;Property;_HeightGradient;Height Gradient;6;0;Create;True;0;0;False;0;0.15;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;248;-2710.178,-1913.736;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;85;-1853.453,409.9392;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-1786.596,-529.797;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;150;-1930.425,248.583;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;147;-2123,-199;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;181;-2702.58,-1813.477;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;152;-1816.844,219.2719;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;187;-1582.269,-628.1077;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-1524.157,-846.9986;Float;False;Property;_Dissolveamount;Dissolve amount;8;0;Create;True;0;0;False;0;1.35;1.48;1.35;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;-1513.026,506.1589;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;156;-1864,-140;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;186;-1571.902,-500.2388;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;107;-1336.179,609.3195;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-1387.543,812.8461;Float;False;Constant;_Float7;Float 7;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;176;-1292.565,-397.1361;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;None;17088603c3ddc734d9c17645934db875;True;0;False;white;Auto;False;Instance;114;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;153;-1566.135,184.5319;Float;False;Constant;_Float8;Float 8;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1564.135,261.7455;Float;False;Constant;_Float9;Float 9;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;197;-1242.181,-834.0012;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-1307.781,-630.5979;Float;True;Property;_Noise;Noise;2;0;Create;True;0;0;False;0;17088603c3ddc734d9c17645934db875;17088603c3ddc734d9c17645934db875;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;151;-1664,-32;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-1389.543,735.6324;Float;False;Constant;_Float0;Float 0;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;194;-870.4301,-485.3898;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-850.9368,-362.2188;Float;False;Property;_EmissiveContrast;Emissive Contrast;12;0;Create;True;0;0;False;0;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;155;-1362.613,87.92873;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;157;-1186.021,639.0292;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;161;-1100.44,27.54501;Float;False;Constant;_Color3;Color 3;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;162;-1112.016,213.9175;Float;False;Constant;_Color4;Color 4;10;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;-692.9368,-459.2188;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-878.8925,346.9288;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;237;-662.2793,-237.5155;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;163;-824.0756,165.6219;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;235;-502.2807,-1321.016;Float;False;965.6;572;;8;230;233;232;229;227;231;228;234;Emission gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;83;-529.9425,-578.6218;Float;False;Property;_BaseColor;Base Color;4;0;Create;True;0;0;False;0;0,0.2048912,1,0;0,0.526397,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-829.749,-94.48238;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;238;-479.2793,-216.5155;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;207;-537.9725,-417.983;Float;False;Property;_Emissive;Emissive;3;1;[HDR];Create;True;0;0;False;0;0,0.3512595,11.18176,1;0,0.3512595,11.18176,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;250;-2998.285,-1595.812;Float;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;221;-2876.921,-1574.863;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-2521.985,-1796.196;Float;False;_worldUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-288.469,-1000.244;Float;False;Property;_E2;E2;11;0;Create;True;0;0;False;0;0;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;232;268.918,-961.6241;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;229;-32.46901,-1081.553;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;35.39597,-885.4071;Float;False;Constant;_Float6;Float 6;-1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;222;-2711.921,-1652.863;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-496.4692,-1081.553;Float;False;Property;_E1;E1;10;0;Create;True;0;0;False;0;0;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;227;-384.469,-1241.553;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;206;-218.8418,-107.1334;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;202;-567.3425,-26.41289;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;228;-192.469,-1177.553;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;199;-1113.364,-833.2165;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-1882.194,-399.1621;Float;False;182;_worldUVs;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;233;42.99599,-956.2209;Float;False;Constant;_Float5;Float 5;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1.151968,9.215743;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Particles/Platforms;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;179;0;178;1
WireConnection;179;1;178;2
WireConnection;189;0;191;0
WireConnection;189;1;190;1
WireConnection;248;0;179;0
WireConnection;248;1;180;0
WireConnection;188;0;189;0
WireConnection;150;0;109;0
WireConnection;181;0;179;0
WireConnection;181;1;180;0
WireConnection;152;0;110;0
WireConnection;187;0;248;0
WireConnection;187;1;188;0
WireConnection;106;0;85;2
WireConnection;106;1;150;0
WireConnection;156;0;147;2
WireConnection;156;1;109;0
WireConnection;186;0;181;0
WireConnection;186;1;188;0
WireConnection;107;0;106;0
WireConnection;107;1;152;0
WireConnection;176;1;186;0
WireConnection;197;0;196;0
WireConnection;114;1;187;0
WireConnection;151;0;156;0
WireConnection;151;1;110;0
WireConnection;194;0;114;0
WireConnection;194;1;176;0
WireConnection;194;2;197;0
WireConnection;155;0;151;0
WireConnection;155;1;153;0
WireConnection;155;2;154;0
WireConnection;157;0;107;0
WireConnection;157;1;158;0
WireConnection;157;2;159;0
WireConnection;242;0;194;0
WireConnection;242;1;241;0
WireConnection;160;0;155;0
WireConnection;160;1;157;0
WireConnection;237;0;106;0
WireConnection;237;1;242;0
WireConnection;163;0;162;0
WireConnection;163;1;161;0
WireConnection;163;2;160;0
WireConnection;223;0;194;0
WireConnection;223;1;163;0
WireConnection;238;0;237;0
WireConnection;182;0;181;0
WireConnection;232;0;229;0
WireConnection;232;1;233;0
WireConnection;232;2;234;0
WireConnection;229;0;228;0
WireConnection;229;1;231;0
WireConnection;222;0;180;0
WireConnection;222;1;221;4
WireConnection;222;2;250;0
WireConnection;206;0;83;0
WireConnection;206;1;207;0
WireConnection;206;2;238;0
WireConnection;202;0;223;0
WireConnection;202;1;163;0
WireConnection;228;0;227;2
WireConnection;228;1;230;0
WireConnection;199;0;197;0
WireConnection;0;2;206;0
WireConnection;0;10;202;0
ASEEND*/
//CHKSM=387CAE33502FA00468E06F97BBF6EE2B8256A12B