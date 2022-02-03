﻿using System;
using Foundation;
using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Comet.iOS;
using Microsoft.Maui.Graphics;
using CoreGraphics;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, CUIScrollView>
	{

		private UIView _content;

		protected override CUIScrollView CreateNativeView() =>
			new CUIScrollView {
				ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always,
				CrossPlatformArrange = Arange,
			};

		void Arange(Rectangle rect)
		{
			var sizeAllowed = this.VirtualView.Orientation == Orientation.Vertical ? new Size(rect.Width, Double.PositiveInfinity) : new Size(Double.PositiveInfinity, rect.Height);
			var measuredSize = VirtualView?.Content?.Measure(sizeAllowed.Width, sizeAllowed.Height) ?? Size.Zero;
			//Make sure we at least fit the scroll view
			if (double.IsInfinity(measuredSize.Width))
				measuredSize.Width = rect.Width;
			if (double.IsInfinity(measuredSize.Height))
				measuredSize.Height = rect.Height;
			measuredSize.Width = Math.Max(measuredSize.Width, rect.Width);
			measuredSize.Height = Math.Max(measuredSize.Height, rect.Height);

			NativeView.ContentSize = measuredSize.ToCGSize();
			_content.Frame = new CGRect(CGPoint.Empty, measuredSize);
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			var oldContent = _content;
			_content = VirtualView?.Content?.ToPlatform(MauiContext);
			if(oldContent != _content)
				oldContent?.RemoveFromSuperview();
			if (_content != null)
			{
				//_content.SizeToFit();
				NativeView.Add(_content);
			}

			if (VirtualView.Orientation == Orientation.Horizontal)
			{
				NativeView.ShowsVerticalScrollIndicator = false;
				NativeView.ShowsHorizontalScrollIndicator = true;
			}
			else
			{
				NativeView.ShowsVerticalScrollIndicator = true;
				NativeView.ShowsHorizontalScrollIndicator = false;
			}
		}
		
	}
}
