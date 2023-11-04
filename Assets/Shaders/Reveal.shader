﻿Shader "Cara/Reveal" {
	Properties{
		_RimColor("Rim Color", Color) = (0,0.5,0.5,0.0)
		_RimPower("Rim Power", Range(0.5,8)) = 3.0
	}

		SubShader{
			Tags{"Queue" = "Transparent"}

			Pass  {
				ColorMask 0
				ZTest Always
			}

			CGPROGRAM
				#pragma surface surf Lambert alpha:fade
				struct Input {
					float3 viewDir;
				};

				fixed4 _RimColor;
				float _RimPower;

				void surf(Input IN, inout SurfaceOutput o) {
					half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
					o.Emission = _RimColor.rgb * pow(rim, _RimPower) * 10;
					o.Alpha = pow(rim, _RimPower);
				}
			ENDCG
		}

		Fallback "Diffuse"

}