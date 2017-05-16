// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System;

namespace SiliconStudio.Xenko.Rendering
{
    /// <summary>
    /// The core interface of a renderer.
    /// </summary>
    public interface IGraphicsRendererCore : IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IGraphicsRenderer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this renderer is initialized.
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        /// Loads this renderer. See remarks.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>This method allow a renderer to prepare for rendering. This method should be called once to initialize a renderer.</remarks>
        void Initialize(RenderContext context);
    }
}
