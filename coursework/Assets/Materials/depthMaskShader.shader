﻿Shader "Masked/Mask" {
	SubShader {
		Tags {"Queue" = "Geometry+10" }
		// Don't draw in the RGBA channels; just the depth buffer
		ColorMask 0
		ZWrite On
		Pass {}
	}
}