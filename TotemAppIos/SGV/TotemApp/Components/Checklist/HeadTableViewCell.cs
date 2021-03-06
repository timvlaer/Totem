﻿using Foundation;

using System;

using UIKit;

namespace TotemAppIos {

	//header cell
	public partial class HeadTableViewCell : BaseChecklistTableViewCell	{
		new public static NSString _key = new NSString ("HeadTableViewCell");
		new public static UINib Nib;

		public override NSString Key { 
			get {
				return _key;
			}
		}

		static HeadTableViewCell () {
			Nib = UINib.FromName ("HeadTableViewCell", NSBundle.MainBundle);
		}

		public static HeadTableViewCell Create () {
			return (HeadTableViewCell)Nib.Instantiate (null, null) [0];
		}

		public HeadTableViewCell (IntPtr handle) : base (handle) {}

		public override void setData(string s, bool firstItem, bool lastItem, bool fill) {
			heightPadding.Constant = firstItem ? 15 : 0;
			bottomPadding.Constant = lastItem ? 15 : 0;
			
			lblHead.Text = s;
		}

		public override void toggle() {}
	}
}