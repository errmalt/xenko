﻿// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Paradox Shader Mixin Code Generator.
// To generate it yourself, please install SiliconStudio.Paradox.VisualStudio.Package .vsix
// and re-save the associated .pdxfx.
// </auto-generated>

using System;
using SiliconStudio.Core;
using SiliconStudio.Paradox.Effects;
using SiliconStudio.Paradox.Graphics;
using SiliconStudio.Paradox.Shaders;
using SiliconStudio.Core.Mathematics;
using Buffer = SiliconStudio.Paradox.Graphics.Buffer;

namespace SiliconStudio.Paradox.Effects.Images
{
    internal static partial class LightStreakShaderKeys
    {
        public static readonly ParameterKey<Vector2[]> TapOffsetsWeights = ParameterKeys.New<Vector2[]>();
        public static readonly ParameterKey<Vector2> Direction = ParameterKeys.New<Vector2>();
        public static readonly ParameterKey<Vector3> ColorAberrationCoefficients = ParameterKeys.New<Vector3>();
        public static readonly ParameterKey<Vector3[]> AnamorphicOffsetsWeight = ParameterKeys.New<Vector3[]>();
    }
}
