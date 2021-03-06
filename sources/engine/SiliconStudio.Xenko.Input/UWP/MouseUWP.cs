// Copyright (c) 2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

#if SILICONSTUDIO_PLATFORM_UWP
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Games;
using PointUWP = Windows.Foundation.Point;

namespace SiliconStudio.Xenko.Input
{
    internal class MouseUWP : PointerUWP, IMouseDevice
    {
        private MouseDeviceStateUWP mouseState;
        private bool isPositionLocked;
        private PointUWP capturedPosition;
        
        public MouseUWP(InputSourceUWP source, CoreWindow uiControl)
            : base(source, uiControl)
        {
            mouseState = new MouseDeviceStateUWP(PointerState, this);
            uiControl.PointerWheelChanged += UIControlOnPointerWheelChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            UIControl.PointerWheelChanged -= UIControlOnPointerWheelChanged;
        }
        
        public override string Name { get; } = "UWP Mouse";

        public override Guid Id { get; } = new Guid("5156D1C2-7B9B-46E7-A54E-CFCC67DA6958");

        public bool IsPositionLocked => isPositionLocked;

        public IReadOnlySet<MouseButton> PressedButtons => mouseState.PressedButtons;
        public IReadOnlySet<MouseButton> ReleasedButtons => mouseState.ReleasedButtons;
        public IReadOnlySet<MouseButton> DownButtons => mouseState.DownButtons;

        public Vector2 Position => mouseState.Position;
        public Vector2 Delta => mouseState.Delta;

        public override void Update(List<InputEvent> inputEvents)
        {
            base.Update(inputEvents);
            mouseState.Update(inputEvents);
        }

        protected override void UIControlOnPointerReleased(CoreWindow o, PointerEventArgs args)
        {
            mouseState.HandlePointerReleased(args.CurrentPoint);
        }

        protected override void UIControlOnPointerPressed(CoreWindow o, PointerEventArgs args)
        {
            mouseState.HandlePointerPressed(args.CurrentPoint);
        }

        protected override void UIControlOnPointerMoved(CoreWindow o, PointerEventArgs args)
        {
            if (isPositionLocked)
            {
                var position = args.CurrentPoint.Position;
                position.X += UIControl.Bounds.Left;
                position.Y += UIControl.Bounds.Top;
                mouseState.HandleMouseDelta(new Vector2(
                    (float)position.X - (float)capturedPosition.X, 
                    (float)position.Y - (float)capturedPosition.Y));
                UIControl.PointerPosition = capturedPosition;
            }
            else
            {
                mouseState.HandlePointerMoved(args.CurrentPoint);
            }
        }

        private void UIControlOnPointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            mouseState.HandlePointerWheelChanged(args.CurrentPoint);
        }

        public void SetPosition(Vector2 normalizedPosition)
        {
            var position = normalizedPosition * SurfaceSize;
            UIControl.PointerPosition = new PointUWP(position.X, position.Y);
        }
        
        public void LockPosition(bool forceCenter = false)
        {
            if (!isPositionLocked)
            {
                capturedPosition = UIControl.PointerPosition;
                if (forceCenter)
                {
                    capturedPosition = new PointUWP(UIControl.Bounds.Left, UIControl.Bounds.Top);
                    capturedPosition.X += UIControl.Bounds.Width / 2;
                    capturedPosition.Y += UIControl.Bounds.Height / 2;
                }
                UIControl.PointerPosition = capturedPosition;
                isPositionLocked = true;
            }
        }

        public void UnlockPosition()
        {
            if (isPositionLocked)
            {
                isPositionLocked = false;
            }
        }
    }
}
#endif
