// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TotemAppIos
{
	[Register ("ChecklistViewController")]
	partial class ChecklistViewController
	{
		[Outlet]
		UIKit.UIButton btnReturn { get; set; }

		[Outlet]
		UIKit.UIImageView imgReturn { get; set; }

		[Outlet]
		UIKit.UILabel lblFoot { get; set; }

		[Outlet]
		UIKit.UILabel lblHead { get; set; }

		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UITableView tblChecklist { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnReturn != null) {
				btnReturn.Dispose ();
				btnReturn = null;
			}
			if (imgReturn != null) {
				imgReturn.Dispose ();
				imgReturn = null;
			}
			if (lblFoot != null) {
				lblFoot.Dispose ();
				lblFoot = null;
			}
			if (lblHead != null) {
				lblHead.Dispose ();
				lblHead = null;
			}
			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}
			if (tblChecklist != null) {
				tblChecklist.Dispose ();
				tblChecklist = null;
			}
		}
	}
}
