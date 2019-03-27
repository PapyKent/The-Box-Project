// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NicoFxs/DissolveParticles"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tiling("Tiling", Float) = 1
		[HDR]_Color("Color", Color) = (0.1698113,0.1698113,0.1698113,0)
		_ColorBlend("Color Blend", Float) = 0
		_ColorPower("ColorPower", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform sampler2D _TextureSample0;
		uniform float _Tiling;
		uniform float _ColorPower;
		uniform float _ColorBlend;
		uniform float _Cutoff = 0.5;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord14 = i.uv_texcoord * temp_cast_0;
			float4 tex2DNode16 = tex2D( _TextureSample0, uv_TexCoord14 );
			float4 temp_cast_1 = (_ColorBlend).xxxx;
			float4 clampResult67 = clamp( pow( ( tex2DNode16 * i.vertexColor.a * _ColorPower ) , temp_cast_1 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult60 = lerp( _Color , i.vertexColor , clampResult67);
			o.Emission = lerpResult60.rgb;
			o.Alpha = 1;
			clip( ( (-0.6 + (i.vertexColor.a - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2DNode16 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
7;7;1906;1004;2599.387;285.2657;1.418225;True;True
Node;AmplifyShaderEditor.RangedFloatNode;28;-2103.575,471.6564;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;1;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1898.359,446.0452;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;7;-1450.276,691.2479;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1623.337,416.9593;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;c3a863161c533f74faad79a170a71cee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-1287.237,71.78146;Float;False;Property;_ColorPower;ColorPower;5;0;Create;True;0;0;False;0;0;8.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-932.7842,396.4756;Float;False;Property;_ColorBlend;Color Blend;4;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1007.611,191.8992;Float;False;3;3;0;COLOR;0,0,0,1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;63;-726.4481,321.5098;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;21.62;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-1085.438,773.3824;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;67;-440.331,312.7201;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;40;-653.8745,-243.5516;Float;False;Property;_Color;Color;3;1;[HDR];Create;True;0;0;False;0;0.1698113,0.1698113,0.1698113,0;14.33965,0.2029192,0.7670328,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;60;-291.7391,87.19817;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-757.3173,697.3767;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1.151968,9.215743;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;NicoFxs/DissolveParticles;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;28;0
WireConnection;16;1;14;0
WireConnection;65;0;16;0
WireConnection;65;1;7;4
WireConnection;65;2;66;0
WireConnection;63;0;65;0
WireConnection;63;1;64;0
WireConnection;50;0;7;4
WireConnection;67;0;63;0
WireConnection;60;0;40;0
WireConnection;60;1;7;0
WireConnection;60;2;67;0
WireConnection;49;0;50;0
WireConnection;49;1;16;0
WireConnection;0;2;60;0
WireConnection;0;10;49;0
ASEEND*/
//CHKSM=7B8C5A694681A5D307635F30B8F6ACACF966BF9B