// Copyright (c) 2011-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;

namespace SiliconStudio.Xenko.Graphics
{
    public struct InputElementDescription : IEquatable<InputElementDescription>
    {
        public string SemanticName;
        public int SemanticIndex;
        public PixelFormat Format;
        public int InputSlot;
        public int AlignedByteOffset;
        public InputClassification InputSlotClass;
        public int InstanceDataStepRate;

        public bool Equals(InputElementDescription other)
        {
            return string.Equals(SemanticName, other.SemanticName)
                   && SemanticIndex == other.SemanticIndex
                   && Format == other.Format
                   && InputSlot == other.InputSlot
                   && AlignedByteOffset == other.AlignedByteOffset
                   && InputSlotClass == other.InputSlotClass
                   && InstanceDataStepRate == other.InstanceDataStepRate;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is InputElementDescription && Equals((InputElementDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SemanticName.GetHashCode();
                hashCode = (hashCode*397) ^ SemanticIndex;
                hashCode = (hashCode*397) ^ (int)Format;
                hashCode = (hashCode*397) ^ InputSlot;
                hashCode = (hashCode*397) ^ AlignedByteOffset;
                hashCode = (hashCode*397) ^ (int)InputSlotClass;
                hashCode = (hashCode*397) ^ InstanceDataStepRate;
                return hashCode;
            }
        }

        public static bool operator ==(InputElementDescription left, InputElementDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InputElementDescription left, InputElementDescription right)
        {
            return !left.Equals(right);
        }
    }
}
