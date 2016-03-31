﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace Totem {
	[Activity (Label = "Profielen", WindowSoftInputMode=SoftInput.StateAlwaysHidden)]			
	public class ProfielenActivity : BaseActivity {
		ProfielAdapter profielAdapter;
		ListView profielenListView;
		List<Profiel> profielen;

		TextView title;
		ImageButton back;
		ImageButton close;
		ImageButton add;
		ImageButton delete;
		TextView noProfiles;

		Database db;
		Toast mToast;

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Profielen);

			//Action bar
			base.InitializeActionBar (ActionBar);
			title = base.ActionBarTitle;
			close = base.ActionBarClose;
			back = base.ActionBarBack;
			add = base.ActionBarAdd;
			delete = base.ActionBarDelete;

			db = DatabaseHelper.GetInstance (this);

			//single toast for entire activity
			mToast = Toast.MakeText (this, "", ToastLength.Short);

			profielen = db.GetProfielen ();
			
			profielAdapter = new ProfielAdapter (this, profielen);
			profielenListView = FindViewById<ListView> (Resource.Id.profielen_list);
			profielenListView.Adapter = profielAdapter;

			profielenListView.ItemClick += ShowTotems;
			profielenListView.ItemLongClick += DeleteProfile;

			noProfiles = FindViewById<TextView> (Resource.Id.empty_profiel);

			title.Text = "Profielen";

			add.Visibility = ViewStates.Visible;
			add.Click += (object sender, EventArgs e) => AddProfile();

			delete.Click += ShowDeleteProfiles;
			close.Click += HideDeleteProfiles;

			if (profielen.Count == 0) {
				noProfiles.Visibility = ViewStates.Visible;
				delete.Visibility = ViewStates.Gone;
			} else {
				delete.Visibility = ViewStates.Visible;
			}
		}

		//updates data of the adapter and shows/hides the "empty"-message when needed
		private void UpdateList() {
			this.profielen = db.GetProfielen();
			if (profielen.Count == 0) {
				noProfiles.Visibility = ViewStates.Visible;
				delete.Visibility = ViewStates.Gone;
			} else {
				noProfiles.Visibility = ViewStates.Gone;
				delete.Visibility = ViewStates.Visible;
			}
			profielAdapter.UpdateData(profielen);
			profielAdapter.NotifyDataSetChanged();
		}

		private void ShowTotems(object sender, AdapterView.ItemClickEventArgs e) {
			int pos = e.Position;
			var item = profielAdapter.GetItemAtPosition(pos);

			if (db.GetTotemsFromProfiel (item.name).Count == 0) {
				mToast.SetText("Profiel " + item.name + " bevat geen totems");
				mToast.Show();
			} else {
				var totemsActivity = new Intent (this, typeof(ProfielTotemsActivity));
				totemsActivity.PutExtra ("profileName", item.name);
				StartActivity (totemsActivity);
			}
		}

		private void DeleteProfile(object sender, AdapterView.ItemLongClickEventArgs e) {
			int pos = e.Position;
			var item = profielAdapter.GetItemAtPosition(pos);

			AlertDialog.Builder alert = new AlertDialog.Builder (this);
			alert.SetMessage ("Profiel " + item.name + " verwijderen?");
			alert.SetPositiveButton ("Ja", (senderAlert, args) => {
				db.DeleteProfile(item.name);
				mToast.SetText("Profiel " + item.name + " verwijderd");
				mToast.Show();
				UpdateList();
			});

			alert.SetNegativeButton ("Nee", (senderAlert, args) => {});

			Dialog dialog = alert.Create();
			RunOnUiThread (() => {
				dialog.Show();
			} );
		}

		private void AddProfile() {
			AlertDialog.Builder alert = new AlertDialog.Builder (this);
			alert.SetTitle ("Nieuw profiel");
			EditText input = new EditText (this); 
			input.InputType = Android.Text.InputTypes.TextFlagCapWords;
			input.Hint = "Naam";
			KeyboardHelper.ShowKeyboard (this, input);
			alert.SetView (input);
			alert.SetPositiveButton ("Ok", (sender, args) => {
				string value = input.Text;
				if(value.Replace("'", "").Replace(" ", "").Equals("")) {
					mToast.SetText("Ongeldige naam");
					mToast.Show();				
				} else if(db.GetProfielNamen().Contains(value)) {
					input.Text = "";
					mToast.SetText("Profiel " + value + " bestaat al");
					mToast.Show();
				} else {
					db.AddProfile(value);
					UpdateList();
				}
			});

			AlertDialog d1 = alert.Create();

			//add profile when enter is clicked
			input.EditorAction += (sender, e) => {
				if (e.ActionId == ImeAction.Done)
					d1.GetButton(-1).PerformClick();
				else
					e.Handled = false;
			};

			RunOnUiThread (() => {
				d1.Show();
			});
		}
			
		private void ShowDeleteProfiles(object sender, EventArgs e) {
			profielAdapter.ShowDelete ();
			profielAdapter.NotifyDataSetChanged ();

			back.Visibility = ViewStates.Gone;
			close.Visibility = ViewStates.Visible;
			title.Visibility = ViewStates.Gone;
			add.Visibility = ViewStates.Gone;

			delete.Click -= ShowDeleteProfiles;
			delete.Click += RemoveSelectedProfiles;
		}

		private void HideDeleteProfiles(object sender, EventArgs e) {
			profielAdapter.HideDelete ();
			profielAdapter.NotifyDataSetChanged ();

			back.Visibility = ViewStates.Visible;
			close.Visibility = ViewStates.Gone;
			title.Visibility = ViewStates.Visible;
			add.Visibility = ViewStates.Visible;

			delete.Click -= RemoveSelectedProfiles;
			delete.Click += ShowDeleteProfiles;
		}

		private void RemoveSelectedProfiles(object sender, EventArgs e) {
			bool selected = false;
			foreach(Profiel p in profielen) {
				if (p.selected) {
					selected = true;
					break;
				}
			}

			if (selected) {		
				AlertDialog.Builder alert1 = new AlertDialog.Builder (this);
				alert1.SetMessage ("Geselecteerde profielen verwijderen?");
				alert1.SetPositiveButton ("Ja", (senderAlert, args) => {
					foreach (Profiel p in profielen)
						if (p.selected)
							db.DeleteProfile (p.name);
				
					UpdateList ();
					HideDeleteProfiles (sender, e);
				});

				alert1.SetNegativeButton ("Nee", (senderAlert, args) => {});

				Dialog d2 = alert1.Create ();

				RunOnUiThread (() => {
					d2.Show ();
				});
			} else {
				mToast.SetText("Geen profielen geselecteerd om te verwijderen");
				mToast.Show();
			}
		}
	}
}