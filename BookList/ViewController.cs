using System;

using UIKit;
using SQLite;
using System.IO;
using System.Collections.Generic;

namespace BookList
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}
			
		// define the file path on the device where the DB will be stored
		string filePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), 
										"BookList.db3");

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			// Create our connection, if the database file and/or table doesn't exist create it
			var db = new SQLiteConnection (filePath);
			//db.DropTable<Book> ();
			db.CreateTable<Book>();

			// TODO: Add code to populate the Table View if it contains data
			PopulateTableView();

		}

		private void PopulateTableView()
		{
			var db = new SQLiteConnection (filePath);
			// retrieve all the data in the DB table
			var bookList = db.Table<Book>();

			List<string> bookTitles = new List<string> ();

			// loop through the data and retrieve the BookTitle column data only 
			foreach (var book in bookList) {
				bookTitles.Add (book.BookTitle);
			}

			tblBooks.Source = new BookListTableSource (bookTitles.ToArray ());

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void btnAdd_TouchUpInside (UIButton sender)
		{
			// Input Validation: only insert a book if the title is not empty
			if (!string.IsNullOrEmpty(txtTitle.Text))
			{
				// Insert a new book into the database
				var newBook = new Book { BookTitle = txtTitle.Text, ISBN = txtISBN.Text };

				var db = new SQLiteConnection (filePath);
				db.Insert (newBook);

				// TODO: Add code to populate the Table View with the new values


				// show an alert to confirm that the book has been added
				new UIAlertView(
					"Success", 
					string.Format("Book ID: {0} with Title: {1} has been successfully added!", newBook.BookId, newBook.BookTitle),
					null,
					"OK").Show();

				PopulateTableView();

				// call this method to refresh the Table View data
				tblBooks.ReloadData ();
			}
			else
			{
				new UIAlertView("Failed", "Enter a valid Book Title",null,"OK").Show();
			}
		}
	}
}

