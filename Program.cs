using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace BasicLibrary
{
    internal class Program
    {
        

        static List<(int ID, string BName, string BAuthor, int copies, int BorrowedCopies, double Price, string Category, int BorrowPeriod)> Books = new List<(int ID, string BName, string BAuthor, int copies, int BorrowedCopies, double Price, string Category, int BorrowPeriod)>();
        static List<(int UID, string UserName, string email, string password)> Users = new List<(int UID, string UserName, string email, string password)>();
        static List<(int AID, string Aname, string email, string password)> Admin = new List<(int AID, string Aname, string email, string password)>();

        static List<(int UserID, int BookID, DateTime BorrowDate, DateTime ReturnDate, DateTime ActualReturnDate, int Rating, bool ISReturned)> BorrowCounts = new List<(int UserID, int BookID, DateTime BorrowDate, DateTime ReturnDate, DateTime ActualReturnDate, int Rating, bool ISReturned)>();
        static List<(int UsBorrowCountserID, int TotalBookInLibrary, int mostBorrowedBookID)> report = new List<(int UsBorrowCountserID, int TotalBookInLibrary, int mostBorrowedBookID)>();
        static List<(int CID, string CName, int NOFBooks)> Categories = new List<(int CID, string CName, int NOFBooks)>();



        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\test\\lib.txt";
        static string filePathBorrow = "C:\\Users\\Lenovo\\source\\repos\\test\\borrow.txt";
        static string filePathAdmin = "C:\\Users\\Lenovo\\source\\repos\\test\\AdminsFile.txt";
        static string filePathCategories = "C:\\Users\\Lenovo\\source\\repos\\test\\CategoriesFile.txt";
        static string filePathUser = "C:\\Users\\Lenovo\\source\\repos\\test\\user.txt";
        static string filePathBorrowCounts = "C:\\Users\\Lenovo\\source\\repos\\test\\borrowCounts.txt";
        static string filePathReport = "C:\\Users\\Lenovo\\source\\repos\\test\\report.txt";


        static int maxBorrowCount = 0;


        static int UserId = 0;


        static void Main(string[] args)
        {


            bool ExitFlag = false;

            try
            {
                LoadBooksFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading books from file: " + ex.Message);

                return;
            }

            do
            {

                Console.WriteLine("Welcome to Lirary");
                Console.WriteLine("\n choose A for admin or B for user or C for save & Exit:");
                Console.WriteLine("\n A- Admin ");
                Console.WriteLine("\n B- User ");

                Console.WriteLine("\n C- Save and Exit");


                string choice = Console.ReadLine().ToUpper();

                try
                {
                    switch (choice)
                    {
                        case "A":
                            AdminFunction();
                            break;

                        case "B":
                            UserFunction();
                            break;

                        case "C":

                            SaveBooksToFile();

                            ExitFlag = true;
                            break;

                        default:
                            Console.WriteLine("Sorry your choice was wrong");
                            break;



                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while processing your choice: " + ex.Message);
                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);



        }
        static void AddNewBook()
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("        Add New Book to Library     ");
            Console.WriteLine("===================================");

            int newId = 1;


            if (Books.Count > 0)
            {
                newId = Books[0].ID;


                for (int i = 1; i < Users.Count; i++)
                {
                    if (Books[i].ID > newId)
                    {
                        newId = Books[i].ID;
                    }
                }
                newId++;
            }

            // Check for duplicate Book ID
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].ID == newId)
                {
                    Console.WriteLine("Error: A book with this ID already exists.");
                    Console.WriteLine("===================================");
                    return;
                }
            }


            Console.Write("Enter Book Name: ");
            string name = Console.ReadLine();

            // Check for duplicate Book Name
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: A book with this name already exists.");
                    Console.WriteLine("===================================");
                    return;
                }
            }


            Console.Write("Enter Book Author: ");
            string author = Console.ReadLine();

            // Input Number of Copies
            Console.Write("Enter Number of Copies: ");
            int copies = int.Parse(Console.ReadLine());

            // Input Book Price
            Console.Write("Enter Book Price: ");
            double price = double.Parse(Console.ReadLine());

            LoadCategoriesFile();
            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < Categories.Count; i++)
            {
                Console.WriteLine($"{Categories[i].CID}: {Categories[i].CName}");
            }

            // Select Category
            Console.Write("\nSelect Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            string categoryName = null;
            bool categoryFound = false;

            // Find the category and update the NOFBooks count
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i].CID == categoryId)
                {
                    categoryName = Categories[i].CName;
                    categoryFound = true;


                    Categories[i] = (Categories[i].CID, Categories[i].CName, Categories[i].NOFBooks + 1);
                   
                    break;
                }
            }

            if (!categoryFound)
            {
                Console.WriteLine("Error: Category not found.");
                Console.WriteLine("===================================");
                return;
            }


            Console.Write("Enter Borrow Period (days): ");
            int borrowPeriod = int.Parse(Console.ReadLine());

            //static List<(string BName, string BAuthor, int ID, int Copies, int BorrowedCopies, double Price, string Category, int BorrowPeriod)>
            Books.Add((id, name, author, copies, 0, price, categoryName, borrowPeriod));


            Console.WriteLine("\n===================================");
            Console.WriteLine($"Success! Book '{name}' has been added to the library.");
            Console.WriteLine("===================================");

            SaveBooksToFile();
            SaveCategoriesToFile();
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
        static void ViewAllBooks()
        {
            try
            {

                if (Books == null || Books.Count == 0)
                {
                    Console.WriteLine("No books available to display.");
                    return;
                }


                StringBuilder sb = new StringBuilder();


                for (int i = 0; i < Books.Count; i++)
                {
                    int bookNumber = i + 1;


                    sb.AppendLine($"====================================");
                    sb.AppendLine($"       Book {bookNumber} Information");
                    sb.AppendLine($"====================================");
                    sb.AppendLine($"Book Name: {Books[i].BName}");
                    sb.AppendLine($"Author: {Books[i].BAuthor}");
                    sb.AppendLine($"Book ID: {Books[i].ID}");
                    sb.AppendLine($"Copies: {Books[i].copies}");
                    sb.AppendLine($"Borrowed Copies: {Books[i].BorrowedCopies}");
                    sb.AppendLine($"Price: ${Books[i].Price}");
                    sb.AppendLine($"Category: {Books[i].Category}");
                    sb.AppendLine($"Borrow Period: {Books[i].BorrowPeriod} days");
                    sb.AppendLine();
                }


                Console.WriteLine(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying the books: {ex.Message}");
            }
        }

        static void SearchForBook()
        {
            Console.Clear();
            ViewAllBooks();
            if (Books == null || Books.Count == 0)
            {
                Console.WriteLine("No books available to display.");
                return;
            }
            try
            {

                Console.WriteLine("Enter part of the book name you want to search for");
                string searchTerm = Console.ReadLine();
                bool flag = false;

                foreach (var book in Books)
                {
                    if (book.BName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine("\n====================================");
                        Console.WriteLine($"Book Name: {book.BName}");
                        Console.WriteLine($"Author: {book.BAuthor}");
                        Console.WriteLine($"Copies: {book.copies}");
                        Console.WriteLine($"Borrowed Copies: {book.BorrowedCopies}");
                        Console.WriteLine($"Price: ${book.Price:F2}");
                        Console.WriteLine($"Category: {book.Category}");
                        Console.WriteLine($"Borrow Period: {book.BorrowPeriod} days");
                        Console.WriteLine("====================================");
                        flag = true;
                        break;
                    }
                }

                if (flag != true)
                { Console.WriteLine("book not found"); }

            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred while searching for the book: " + ex.Message);
            }

        }

        static void SearchForBookForUser()
        {
            Console.Clear();
            ViewAllBooks();
            if (Books == null || Books.Count == 0)
            {
                Console.WriteLine("No books available to display.");
                return;
            }
            try
            {

                Console.WriteLine("Enter part of the book name you want to search for");
                string searchTerm = Console.ReadLine();
                bool flag = false;

                foreach (var book in Books)
                {
                    if (book.BName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine("\n====================================");
                        Console.WriteLine($"Book Name: {book.BName}");
                        Console.WriteLine($"Author: {book.BAuthor}");
                        Console.WriteLine($"Copies: {book.copies}");
                        Console.WriteLine($"Borrowed Copies: {book.BorrowedCopies}");
                        Console.WriteLine($"Price: ${book.Price:F2}");
                        Console.WriteLine($"Category: {book.Category}");
                        Console.WriteLine($"Borrow Period: {book.BorrowPeriod} days");
                        Console.WriteLine("====================================");
                        flag = true;

                        Console.WriteLine("Would you like to borrow this book? (yes/no)");
                        string response = Console.ReadLine().ToLower();

                        if (response == "yes")
                        {
                            BorrowBook();
                        }
                        break;
                    }
                }

                if (flag != true)
                { Console.WriteLine("book not found"); }

            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred while searching for the book: " + ex.Message);
            }

        }
        static void LoadBooksFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 8)
                            {
                                Books.Add((int.Parse(parts[0]), parts[1], parts[2], int.Parse(parts[3]),int.Parse(parts[4]), double.Parse(parts[5]), parts[6], int.Parse(parts[7])));

                                
                            }
                        }
                    }
                    Console.WriteLine("Books loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }
        //static void LoadAdminFromFile()
        //{
        //    try
        //    {
        //        if (File.Exists(filePathAdmin))
        //        {
        //            using (StreamReader reader = new StreamReader(filePathAdmin))
        //            {
        //                string line;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    var parts = line.Split('|');
        //                    if (parts.Length == 4)
        //                    {
        //                        Admin.Add((int.Parse(parts[0]),parts[1], parts[2], parts[3]));
        //                    }
        //                }
        //            }
        //            Console.WriteLine("Admins loaded from file successfully.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error loading from file: {ex.Message}");
        //    }
        //}


        static void LoadAdminFromFile()
        {
            try
            {
                if (File.Exists(filePathAdmin))
                {
                    using (StreamReader reader = new StreamReader(filePathAdmin))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');

                            // Ensure there are exactly 4 parts and the first part is an integer (AID)
                            if (parts.Length == 4 && int.TryParse(parts[0], out int adminId))
                            {
                                string adminName = parts[1];
                                string email = parts[2];
                                string password = parts[3];

                                // Add the tuple to the Admin list
                                Admin.Add((adminId, adminName, email, password));
                            }
                            else
                            {
                                Console.WriteLine($"Invalid format in line: {line}. Skipping.");
                            }
                        }
                    }
                    Console.WriteLine("Admins loaded from file successfully.");
                }
                else
                {
                    Console.WriteLine($"File not found: {filePathAdmin}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        static void SaveBooksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.ID}|{book.BName}|{book.BAuthor}|{book.copies}|{book.BorrowedCopies}|{book.Price}|{book.Category}|{book.BorrowPeriod}");
                    }
                }
                Console.WriteLine("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        static void SaveReportsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePathReport))
                {
                    foreach (var r in report)
                    {
                        writer.WriteLine($"{r.UsBorrowCountserID}|{r.TotalBookInLibrary}|{r.mostBorrowedBookID}");
                    }
                }
                Console.WriteLine("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void SaveAdminToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePathAdmin))
                {
                    foreach (var admins in Admin)
                    {
                        writer.WriteLine($"{admins.AID}|{admins.Aname}|{admins.email}|{admins.password}");
                    }
                }
                Console.WriteLine("Admin saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void SaveUserToFile()
        {
            try
            {

                using (StreamWriter writer = new StreamWriter(filePathUser))
                {
                    foreach (var user in Users)
                    {
                        writer.WriteLine($"{user.UID}|{user.UserName}|{user.email}|{user.password}");
                    }
                }
                Console.WriteLine("user saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        static void SaveCategoriesToFile()
      
        {
            try
            {

                using (StreamWriter writer = new StreamWriter(filePathCategories))
                {
                    foreach (var Categories in Categories)
                    {
                        writer.WriteLine($"{Categories.CID}|{Categories.CName}|{Categories.NOFBooks}");
                    }
                }
                Console.WriteLine("user saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }


        }
        static void LoadUserFromFile()
        {
            try
            {
                if (File.Exists(filePathUser))
                {
                    using (StreamReader reader = new StreamReader(filePathUser))
                    {
                        //   users.Add((Username, UserId, Email, Password));
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                Users.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                    Console.WriteLine("user loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }
        static void LoadCategoriesFile()
        {
            try
            {
                if (File.Exists(filePathCategories))
                {
                    using (StreamReader reader = new StreamReader(filePathCategories))
                    {
                      
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                Categories.Add((int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
                            }
                        }
                    }
                    Console.WriteLine("user loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }
        static void BorrowBook()

        {
            Books.Clear();
            LoadBooksFromFile();
            BorrowCounts.Clear();
            LoadBorrowCountsFromFile();
            Console.Clear();
            ViewAllBooks();


            try
            {


                

                Console.WriteLine("Enter Book ID :");

                int ID = int.Parse(Console.ReadLine());
                bool flag = false;

                /* foreach (var borrow in BorrowCounts)
                 {
                     if (borrow.UserID == UserId && borrow.BookID== ID)
                     {
                         Console.WriteLine("You have already borrowed this book.");
                         return;
                     }
                 }*/

                
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].ID == ID && Books[i].copies > Books[i].BorrowedCopies)
                    {

                        Console.WriteLine("Book is available ");


                        int newCopies = Books[i].copies - 1;
                        int newBorrowedCopies = Books[i].BorrowedCopies + 1;
                        DateTime BorrowDate = DateTime.Now;
                        DateTime ReturnDate = DateTime.Now.AddDays(Books[i].BorrowPeriod);

                       
                        Books[i] = (Books[i].ID, Books[i].BName, Books[i].BAuthor, newCopies, newBorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                        Console.WriteLine("The book has been borrowed ");
                        SaveBooksToFile();
                        BorrowCounts.Add((UserId, ID, BorrowDate, ReturnDate, DateTime.MinValue, 0, false));
                        BorrowedBookFile();
                        flag = true;

                               /* for (int j = 0; j < CategoriesFile.Count; j++)
                                {
                                    if (CategoriesFile[j].CName == Books[i].Category)
                                    {
                                        CategoriesFile[j] = (CategoriesFile[j].CID, CategoriesFile[j].CName, CategoriesFile[j].NOFBooks - 1);
                                        SaveCategoriesToFile();
                                        break;
                                    }
                                } */

                        break;
                    }
                }

                if (flag != true)
                { Console.WriteLine("Book not found or no copies available "); }






            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

            }

        }



        static void DeleteBook()
        {
            Console.Clear();
            Books.Clear();
            LoadBooksFromFile();
            ViewAllBooks();

            Console.WriteLine("Enter the book name you want delete :");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Books.RemoveAt(i);
                    flag = true;
                    Console.WriteLine("The book :  " + name + "   has been deleted successfully.");
                    break;
                }
            }
            if (flag != true)
            {
                Console.WriteLine("Book not availabe");
            }

        }

        static void EditBook()

        {


            Console.Clear();
            Books.Clear();

            LoadBooksFromFile();
            ViewAllBooks();

            bool ExitFlag = false;
            do
            {

                Console.Write("Enter what you want to edit :");
                Console.Write(" A- Book Name  ");
                Console.Write(" B- Book Author  ");
                Console.Write(" C- Book Copies ");
                Console.Write(" D- Book Price ");
                Console.Write(" F- Book Category ");
                Console.Write(" M- Book Borrow Period ");
                Console.Write(" S- Exit ");
                Console.WriteLine();
                char choice = Char.ToUpper(Console.ReadKey().KeyChar);


                switch (choice)
                {
                    case 'A':

                        EditingBookAttribute(choice);

                        break;

                    case 'B':
                        EditingBookAttribute(choice);


                        break;

                    case 'C':
                        EditingBookAttribute(choice);
                        break;
                    case 'D':
                        EditingBookAttribute(choice);
                        break;
                    case 'F':
                        EditingBookAttribute(choice);
                        break;
                    case 'M':
                        EditingBookAttribute(choice);
                        break;

                    case 'S':


                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);


        }

        static void ReturnBooks()
        {
            Books.Clear();
            LoadBooksFromFile();
            BorrowCounts.Clear();
            LoadBorrowCountsFromFile();
            Console.Clear();


            Console.WriteLine("Enter the book ID you want to return  :");
            int ID = int.Parse(Console.ReadLine());

            bool flage = false;
            for (int i = 0; i < Books.Count; i++)
            {

                if (Books[i].ID == ID)
                {

                    int newCopies = Books[i].copies + 1;
                    int newBorrowedCopies = Books[i].BorrowedCopies - 1;

                    Books[i] = (Books[i].ID, Books[i].BName, Books[i].BAuthor, newCopies, newBorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);

                    Console.WriteLine("The book has been returned ");
                    for (int j = 0; j < BorrowCounts.Count; j++)
                    {
                        if (BorrowCounts[j].UserID == UserId && BorrowCounts[j].BookID == ID && !BorrowCounts[j].ISReturned)
                        {
                            Console.WriteLine(" Please rate the book 1-5");
                            int rating = int.Parse(Console.ReadLine());
                            DateTime ActualReturnDate = DateTime.Now;
                            BorrowCounts[j] = (BorrowCounts[j].UserID, BorrowCounts[j].BookID, BorrowCounts[j].BorrowDate, BorrowCounts[j].ReturnDate, ActualReturnDate, rating, true);



                            BorrowedBookFile();
                            flage = true;
                            break;
                        }








                    }

                    SaveBooksToFile();
                    break;

                }


            }
            if (!flage)
            {
                Console.WriteLine("Book not found or it has not been borrowed.");
            }

        }
        static void AdminMenu(int adminID)
        {
            bool ExitFlag = false;
            do
            {
                Console.Clear();

                Console.WriteLine("Welcome Admin");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Add New Book");
                Console.WriteLine("\n B- Display All Books");
                Console.WriteLine("\n C- Search for Book by Name");
                Console.WriteLine("\n D- Edit book");
                Console.WriteLine("\n E- delet book");
                Console.WriteLine("\n M- Admin Report");
                Console.WriteLine("\n F- Save and Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {

                    case "A":
                        AddNewBook();
                        break;

                    case "B":
                        Console.Clear();
                        ViewAllBooks();
                        break;

                    case "C":
                        SearchForBook();
                        break;
                    case "D":
                        EditBook();
                        break;

                    case "E":
                        DeleteBook();
                        break;
                    case "M":
                        GenerateReport();

                        break;
                    case "F":

                        SaveBooksToFile();
                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();



            } while (ExitFlag != true);




        }
        static void UserMenu(int userID)
        {
            bool ExitFlag = false;
            do
            {
                Console.Clear();

                Console.WriteLine("Welcome ");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Search for Book by Name");
                Console.WriteLine("\n B- Borrow Books");
                Console.WriteLine("\n C- Return Books");
                Console.WriteLine("\n D- Display User Profile");
                Console.WriteLine("\n E- Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {
                    case "A":
                        SearchForBookForUser();
                        break;

                    case "B":
                        BorrowBook();
                        break;

                    case "C":
                        ReturnBooks();
                        break;
                    case "D":
                       // ShowProfile();
                        break;
                    case "E":

                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();



            } while (ExitFlag != true);


        }

        static void AdminFunction()
        {
            bool ExitFlag = false;
            while (ExitFlag != true)
            {

                Console.WriteLine("Welcome Admin");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Admain registeration");
                Console.WriteLine("\n B- Admain login");
                Console.WriteLine("\n C- Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {
                    case "A":
                        RegisterAdmin();
                        break;
                    case "B":
                        LoadAdminFromFile();
                        LoginAdmin();
                        break;

                    case "C":

                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }



            }
        }

        static void UserFunction()
        {
            bool ExitFlag = false;
            while (ExitFlag != true)
            {

                Console.WriteLine("Welcome User");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- User registeration");
                Console.WriteLine("\n B- User login");
                Console.WriteLine("\n C-  Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {
                    case "A":
                        RegisterUser();
                        break;
                    case "B":
                        LoginUser();
                        break;

                    case "C":

                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong");
                        break;



                }



            }
        }

        //static void RegisterUser()
        //{
        //    int UserId = users.Count + 1;




        //    Console.Write("Enter Username: ");
        //    string Username = Console.ReadLine();


        //    Console.Write("Enter Email: ");
        //    string Email = Console.ReadLine();


        //    Console.Write("Enter Password: ");
        //    string Password = Console.ReadLine();



        //    users.Add((Username, UserId, Email, Password));
        //    Console.WriteLine("User registered successfully!");
        //    SaveUserToFile();
        //}


        //static void RegisterAdmin()
        //{
        //    string Password = "a123";

        //    Console.Write("Enter Admin Password to Register: ");
        //    string Password1 = Console.ReadLine();


        //    if (Password1 == Password)
        //    {
        //        Console.Write("Enter Username: ");
        //        string username = Console.ReadLine();

        //        Console.Write("Enter Email: ");
        //        string email = Console.ReadLine();

        //        Console.WriteLine("Admin registered successfully!");
        //        Admin.Add((username, email, Password1));
        //        UserId = users.Count;
        //        SaveAdminToFile();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Incorrect password. Registration failed.");
        //    }
        //}

        //static void LoginAdmin()
        //{
        //    LoadAdminFromFile();
        //    string Password = "a123";

        //    Console.Write("Enter Admin Password: ");
        //    string Password1 = Console.ReadLine();


        //    if (Password1 == Password)
        //    {
        //        Console.Write("Enter Username: ");
        //        string adminUsername = Console.ReadLine();

        //        Console.Write("Enter Email: ");
        //        string email = Console.ReadLine();

        //        bool flag = false;

        //        for (int i = 0; i < Admin.Count; i++)
        //        {
        //            if (adminUsername == Admin[i].Username1 && email == Admin[i].Email)
        //            {
        //                Console.WriteLine("\nAdmin authenticated successfully!");
        //                flag = true;
        //                break;
        //            }
        //        }

        //        if (flag)
        //        {
        //            AdminMenu();
        //        }
        //        else
        //        {
        //            Console.WriteLine("Failed to authenticate. Invalid username or email.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Incorrect password.");
        //    }
        //}

        //static void LoginUser()
        //{


        //    LoadUserFromFile();

        //    Console.Write("Enter Username: ");
        //    string Username = Console.ReadLine();

        //    Console.Write("Enter Email: ");
        //    string Email = Console.ReadLine();

        //    Console.Write("Enter Password: ");
        //    string Password = Console.ReadLine();


        //    bool flag = false;

        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        if (Username == users[i].Username && Email == users[i].Email && Password == users[i].Password)
        //        {
        //            Console.Write("\n user authenticated successfully!");
        //            flag = true;

        //            break;
        //        }

        //    }
        //    if (flag)
        //    {
        //        UserMenu();


        //    }
        //    else
        //    {

        //        Console.WriteLine("Failed to authenticate. Invalid username, email, or password.");


        //    }










        //}



        static void ReportFromFile()
        {
            try
            {
                if (File.Exists(filePathReport))
                {
                    using (StreamReader reader = new StreamReader(filePathReport))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                report.Add((int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));


                            }
                        }
                    }
                    Console.WriteLine("Borrow counts  loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }

        }


        static void LoadBorrowCountsFromFile()
        {
            try
            {
                if (File.Exists(filePathBorrowCounts))
                {
                    using (StreamReader reader = new StreamReader(filePathBorrowCounts))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 7)
                            {
                                BorrowCounts.Add((int.Parse(parts[0]), int.Parse(parts[1]), DateTime.Parse(parts[2]), DateTime.Parse(parts[3]), DateTime.Parse(parts[4]), int.Parse(parts[5]), bool.Parse(parts[6])));

                            }
                        }
                    }
                    Console.WriteLine("Borrow counts  loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }

        }


        static void BorrowedBookFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePathBorrowCounts))
                {
                    foreach (var borrow in BorrowCounts)
                    {

                        writer.WriteLine($"{borrow.UserID}|{borrow.BookID}|{borrow.BorrowDate}|{borrow.ReturnDate}|{borrow.ActualReturnDate}|{borrow.Rating}|{borrow.ISReturned}");

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }

        }

        static void EditingBookAttribute(char choice)

        {
            Console.WriteLine("Enter the book name you want");
            string name = Console.ReadLine();
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {


                    switch (choice)
                    {


                        case 'A':
                            Console.WriteLine("Enter new book name :");
                            String Newname = Console.ReadLine();
                            Books[i] = (Books[i].ID, Newname, Books[i].BAuthor, Books[i].copies, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                            
                            Console.WriteLine("Name Changed successfully ");
                            break;

                        case 'B':
                            Console.WriteLine("Enter new book Author :");
                            string NewAuthor = Console.ReadLine();
                           
                            Books[i] = (Books[i].ID, Books[i].BName, NewAuthor, Books[i].copies, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                            Console.WriteLine("Author Changed successfully ");
                            break;

                        case 'C':
                            Console.WriteLine("Enter new book Quantity :");
                            int Newquantity = int.Parse(Console.ReadLine());
                            Books[i] = (Books[i].ID, Books[i].BName, Books[i].BAuthor, Newquantity, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                           
                            Console.WriteLine("Quantity Changed successfully ");
                            break;

                    }

                    break;


                }


            }
        }


        //static void DisplayUserProfile()
        //{
        //    Console.WriteLine("Plase Enter User  ID :");

        //    int userID = int.Parse(Console.ReadLine());
        //    bool userFound = false;
        //    bool hasBorrowedBooks = false;


        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        if (users[i].UserID == userID)
        //        {
        //            userFound = true;

        //            Console.WriteLine("User found");
        //            Console.WriteLine($"UserID: {users[i].UserID}");
        //            Console.WriteLine($"Username: {users[i].Username}");
        //            Console.WriteLine($"Email: {users[i].Email}");


        //            for (int j = 0; j < BorrowCounts.Count; j++)
        //            {
        //                if (BorrowCounts[j].UserID == userID && !BorrowCounts[j].ISReturned)
        //                {
        //                    Console.WriteLine($"BookID: {BorrowCounts[j].BookID}, Borrow Date: {BorrowCounts[j].BorrowDate}, Return Date: {BorrowCounts[j].ReturnDate}");
        //                    hasBorrowedBooks = true;
        //                }
        //            }

        //            if (!hasBorrowedBooks)
        //            {
        //                Console.WriteLine("No books currently borrowed.");
        //            }
        //            break;
        //        }
        //    }

        //    if (!userFound)
        //    {
        //        Console.WriteLine("User not found.");
        //    }
        
        //}

        static void GenerateReport()
        {
            LoadBooksFromFile();
            LoadCategoriesFile();
            LoadBorrowCountsFromFile();

            // Number of books in the library
            int totalBooks = Books.Count;

            // Number of categories
            int totalCategories = Categories.Count;

            // Total number of copies of all books
            int totalCopies = 0;
            foreach (var book in Books)
            {
                totalCopies += book.copies;
            }

            // Total number of borrowed books
            int totalBorrowedBooks = 0;

            for (int i = 0; i < Books.Count; i++)
            {
                totalBorrowedBooks += Books[i].BorrowedCopies;
            }

            // Displaying the report
            Console.WriteLine("----- Library Report -----");
            Console.WriteLine($"Total Number of Books: {totalBooks}");
            Console.WriteLine($"Total Number of Categories: {totalCategories}");

            // Display each category name with the number of books in each category
            Console.WriteLine("Categories and their book counts:");
            foreach (var category in Categories)
            {
                Console.WriteLine($"Category: {category.CName}, Number of Books: {category.NOFBooks}");
            }

            Console.WriteLine($"Total Number of Copies of All Books: {totalCopies}");
            Console.WriteLine($"Total Number of Borrowed Books: {totalBorrowedBooks}");
            Console.WriteLine("-----------------------------");
        }

        static void RegisterUser()
        {
            int newUID = 1;

            // If there are users in the list, find the maximum UID manually
            if (Users.Count > 0)
            {
                newUID = Users[0].UID; // Start by assuming the first user's UID is the highest

                // Loop through the Users list to find the maximum UID
                for (int i = 1; i < Users.Count; i++)
                {
                    if (Users[i].UID > newUID)
                    {
                        newUID = Users[i].UID; // Update if we find a higher UID
                    }
                }
                newUID++; // Increment to get the next available UID
            }

            Console.WriteLine("Enter your name:");
            string userName = Console.ReadLine();

            // Use a loop to check for duplicate usernames
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].UserName == userName)
                {
                    Console.WriteLine("Username already exists.");
                    return;
                }
            }

            string email = GetValidEmail();
            string password = GetValidPassword();

            Users.Add((newUID, userName, email, password));
            Console.WriteLine("User registered successfully!");
        
               UserFunction();
        }

        // Validate Password
        static string GetValidPassword()
        {
            string password;
            while (true)
            {
                Console.WriteLine("Enter your password at least 8 characters, include uppercase, lowercase, digit, and symbol:");
                password = Console.ReadLine();
                if (password.Length >= 8 &&
                    Regex.IsMatch(password, @"[A-Z]") &&
                    Regex.IsMatch(password, @"[a-z]") &&
                    Regex.IsMatch(password, @"[0-9]"))
                {
                    break;
                }
                Console.WriteLine("Password does not meet criteria. Try again.");
            }
            return password;
        }

        static string GetValidEmail()
        {
            string email;
            while (true)
            {
                Console.WriteLine("Enter your email (must contain '@' and end with .com or .edu):");
                email = Console.ReadLine();

                // Check if email is in the correct format
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|edu)$"))
                {
                    Console.WriteLine("Invalid email format. Try again.");
                    continue; // Ask for input again
                }

                // Check for duplicate email in Users list
                bool emailExists = false;
                for (int i = 0; i < Users.Count; i++)
                {
                    if (Users[i].email == email)
                    {
                        emailExists = true;
                        break; // Stop checking further if duplicate is found
                    }
                }

                // Check for duplicate email in Admin list
                for (int i = 0; i < Admin.Count; i++)
                {
                    if (Admin[i].email == email)
                    {
                        emailExists = true;
                        break; // Stop checking further if duplicate is found
                    }
                }

                // If email is not a duplicate, exit the loop
                if (!emailExists)
                {
                    break;
                }

                Console.WriteLine("Duplicate email. Try again.");
            }
            return email;
        }

        // User login
        static void LoginUser()
        {

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            // Check if the email exists in the Users list
            bool userFound = false;
            int userIndex = -1;

            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].email == email)
                {
                    userFound = true;
                    userIndex = i;
                    break;
                }
            }

            if (!userFound)
            {
                Console.WriteLine("User is not registered. Do you want to register? (yes / no )");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    RegisterUser();
                }
                else
                {
                    UserFunction();
                }
            }
            else
            {
                var user = Users[userIndex];
                if (Authenticate(user.password))
                {
                    UserMenu(user.UID);
                }
            }
        }
        static void LoginAdmin()
        {
            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            // Check if the email exists in the Admin list manually
            bool adminFound = false;
            int foundAdmin = -1;

            for (int i = 0; i < Admin.Count; i++)
            {
                if (Admin[i].email == email)
                {
                    adminFound = true;
                    foundAdmin = i;
                    break;
                }
            }

            if (!adminFound)
            {

                Console.WriteLine("Admin is not registered. Do you want to register? (yes / no)");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    RegisterAdmin();
                }
                else
                {
                    AdminFunction();
                }

            }
            else
            {
                var admins = Admin[foundAdmin];
                if (Authenticate(admins.password))
                {
                 

                    AdminMenu(admins.AID);
                }
            }


        }


        // Authenticate user || admin with password
        static bool Authenticate(string password)
        {
            Console.WriteLine("Enter your password:");
            string inputPassword = Console.ReadLine();
            Console.WriteLine("Re-enter your password:");
            string reEnterPassword = Console.ReadLine();

            if (inputPassword != reEnterPassword)
            {
                Console.WriteLine("Passwords do not match.");
                return false;
            }

            if (inputPassword.ToString() == password)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password.");
                return false;
            }
        }


        static void RegisterAdmin()
        {
            int newAID = 1;

            // If there are admins in the list, find the maximum AID manually
            if (Admin.Count > 0)
            {
                // Assume the first admin has the highest AID
                newAID = Admin[0].AID;

                // Loop through the Admin list to find the maximum AID
                for (int i = 1; i < Admin.Count; i++)
                {
                    if (Admin[i].AID > newAID)
                    {
                        newAID = Admin[i].AID; // Update newAID if a higher AID is found
                    }
                }

                newAID++; // Increment to get the next available AID
            }

            Console.WriteLine("Enter admin name:");
            string adminName = Console.ReadLine();

            // Check if the admin name already exists
            bool nameExists = false;
            for (int i = 0; i < Admin.Count; i++)
            {
                if (Admin[i].Aname == adminName)
                {
                    nameExists = true;
                    break;
                }
            }

            if (nameExists)
            {
                Console.WriteLine("Admin name already exists.");
                return;
            }

            // Get a valid email and password
            string email = GetValidEmail();
            string password = GetValidPassword();

            // Add the new admin
            Admin.Add((newAID, adminName, email, password));
            Console.WriteLine("Admin registered successfully!");
            AdminFunction();
        }



    }




}



