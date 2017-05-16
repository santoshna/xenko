// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;

namespace SiliconStudio.Shaders.Ast.Hlsl
{
    /// <summary>
    /// Type of constant buffer.
    /// </summary>
    public partial class ConstantBufferType : CompositeEnum
    {
        #region Constants and Fields

        /// <summary>
        ///   Constant buffer (cbuffer).
        /// </summary>
        public static readonly ConstantBufferType Constant = new ConstantBufferType("cbuffer");

        /// <summary>
        ///   Texture buffer (tbuffer).
        /// </summary>
        public static readonly ConstantBufferType Texture = new ConstantBufferType("tbuffer");

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBufferType"/> class.
        /// </summary>
        public ConstantBufferType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBufferType"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public ConstantBufferType(string key)
            : base(key, false)
        {
        }

        /// <summary>
        /// Parses the specified enum name.
        /// </summary>
        /// <param name="enumName">
        /// Name of the enum.
        /// </param>
        /// <returns>
        /// A qualifier
        /// </returns>
        public static ConstantBufferType Parse(string enumName)
        {
            if (enumName == (string)Constant.Key)
                return Constant;
            if (enumName == (string)Texture.Key)
                return Texture;

            throw new ArgumentException(string.Format("Unable to convert [{0}] to constant buffer type", enumName), "key");
        }

        #endregion
    }
}
