// Copyright (c) 2016-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

#if SILICONSTUDIO_XENKO_GRAPHICS_API_NULL 

namespace SiliconStudio.Xenko.Graphics
{
    /// <summary>
    /// GraphicsResource class
    /// </summary>
    public abstract partial class GraphicsResourceBase
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            NullHelper.ToImplement();
        }

        /// <summary>
        /// Called when graphics device has been detected to be internally destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed()
        {
            NullHelper.ToImplement();
        }

        /// <summary>
        /// Called when graphics device has been recreated.
        /// </summary>
        /// <returns>True if item transitioned to a <see cref="GraphicsResourceLifetimeState.Active"/> state.</returns>
        protected internal virtual bool OnRecreate()
        {
            NullHelper.ToImplement();
            return false;
        }
    }
} 
#endif 
