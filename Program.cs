using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int ID, int Copies, int BorrowedCopies, decimal Price , string Category ,  int BorrowPeriod )> Books = new List<(string BName, string BAuthor, int ID, int Copies, int BorrowedCopies, decimal Price, string Category, int BorrowPeriod)>();
        static List<(string Username, string Email, int UserID)> users = new List<(string Username, string Email, int UserID)>();
        static List<(string Username1, string Email, string password)> Admin = new List<(string Username1, string Email, string password)>();
        static List<(int UserID, int ID)> BorrowCounts = new List<(int UserID, int ID)>();
        static List<(int UsBorrowCountserID, int TotalBookInLibrary , int mostBorrowedBookID)> report = new List<(int UsBorrowCountserID, int TotalBookInLibrary, int mostBorrowedBookID)>();

   


        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\test\\lib.txt";
        static string filePathBorrow = "C:\\Users\\Lenovo\\source\\repos\\test\\borrow.txt";
        static string filePathAdmin = "C:\\Users\\Lenovo\\source\\repos\\test\\Admin.txt";
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
        static void AddnNewBook()

        {
            Console.Clear();
          
            LoadBooksFromFile();
            ViewAllBooks();
               
            int bookID = 0 ;
            for (int i = 0; i < Books.Count; i++)
            {
                if (i == Books.Count - 1) {

                    bookID = Books[i].ID +1;
                }
            
            
            
            
            }

                 Console.WriteLine($"Enter details for Book :");

                Console.WriteLine("Enter Book Name");
                string name = Console.ReadLine();

                Console.WriteLine("Enter Book Author");
                string author = Console.ReadLine();

                Console.WriteLine("Enter Book Copies ");
                int Copies = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Price:");
                decimal price = decimal.Parse(Console.ReadLine());

               Console.WriteLine("Enter Category:");
               string category = Console.ReadLine();

               Console.WriteLine("Enter Borrow Period (days):");
               int borrowPeriod = int.Parse(Console.ReadLine());


               Books.Add((name, author, bookID, Copies, 0, price, category, borrowPeriod));

               SaveBooksToFile();
               Console.WriteLine("Book Added Succefully");



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

                int BookNumber = 0;

                for (int i = 0; i < Books.Count; i++)
                {
                    BookNumber = i + 1;
                    sb.Append("Book ").Append(BookNumber).Append(" name : ").Append(Books[i].BName);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" Author : ").Append(Books[i].BAuthor);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" ID : ").Append(Books[i].ID);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" Copies : ").Append(Books[i].Copies);
                    sb.AppendLine();
    
                    sb.Append("Book ").Append(BookNumber).Append(" Borrowed Copies : ").Append(Books[i].BorrowedCopies);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" Price : ").Append(Books[i].Price);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" Category : ").Append(Books[i].Category);
                    sb.AppendLine();
                    sb.Append("Book ").Append(BookNumber).Append(" Borrow Period : ").Append(Books[i].BorrowPeriod);

                    sb.AppendLine().AppendLine();
                    Console.WriteLine(sb.ToString());
                    sb.Clear();

                }

            }
            catch (Exception ex)
            {




                Console.WriteLine("An error occurred while displaying the books: " + ex.Message);
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

                Console.WriteLine("Enter the book name you want");
                string name = Console.ReadLine();
                bool flag = false;

                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BName == name)
                    {
                        Console.WriteLine("Book Namae is : " + Books[i].BName);
                        Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                        Console.WriteLine("Book Copies is : " + Books[i].Copies);
                        Console.WriteLine("Book Borrowed Copies is : " + Books[i].BorrowedCopies);
                        Console.WriteLine("Book Price is : " + Books[i].Price);
                        Console.WriteLine("Book Borrow Period is : " + Books[i].BorrowPeriod);
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
                            if (parts.Length == 4)
                            {
                                Books.Add((parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), decimal.Parse(parts[5]), parts[6] ,int.Parse(parts[7])));
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
                            if (parts.Length == 3)
                            {
                                Admin.Add((parts[0], parts[1], (parts[2])));
                            }
                        }
                    }
                    Console.WriteLine("Admins loaded from file successfully.");
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
                        writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.ID}|{book.Copies}|{book.BorrowedCopies}|{book.Price}|{book.Category}|{book.BorrowPeriod}");
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
                        writer.WriteLine($"{admins.Username1}|{admins.Email}|{admins.password}");
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
                    foreach (var user in users)
                    {
                        writer.WriteLine($"{user.Username}|{user.Email}|{user.UserID}");
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
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                users.Add((parts[0], parts[1], int.Parse(parts[2])));
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

                foreach (var borrow in BorrowCounts)
                {
                    if (borrow.UserID == UserId && borrow.ID == ID)
                    {
                        Console.WriteLine("You have already borrowed this book.");
                        return;
                    }
                }


                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].ID == ID && Books[i].Copies > Books[i].BorrowedCopies)
                    {

                        Console.WriteLine("Book is available ");


                        int newCopies = Books[i].Copies - 1;
                        int newBorrowedCopies = Books[i].BorrowedCopies + 1;


                        Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, newCopies, newBorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod
);
                        Console.WriteLine("The book has been borrowed ");
                        SaveBooksToFile();
                        BorrowCounts.Add((UserId, ID));
                        BorrowedBookFile();
                        flag = true;
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

                    int newCopies = Books[i].Copies + 1;
                    int newBorrowedCopies = Books[i].BorrowedCopies - 1;

                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, newCopies , newBorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                    Console.WriteLine("The book has been returned ");
                   
                   

                    BorrowCounts.Remove((UserId, ID));
                    BorrowedBookFile();
                    SaveBooksToFile();

                    flage = true;
                    break;

                }



            }


        }

        static void AdminMenu()
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
                        AddnNewBook();
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
                       AdminReport();

                        break;
                    case "F":
                      

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
        static void UserMenu()
        {
            bool ExitFlag = false;
            do
            {
                Console.Clear();

                Console.WriteLine("Welcome user");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Search for Book by Name");
                Console.WriteLine("\n B- Borrow Books");
                Console.WriteLine("\n C- Return Books");
                Console.WriteLine("\n D- Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {
                    case "A":
                        SearchForBook();
                        break;

                    case "B":
                        BorrowBook();
                        break;

                    case "C":
                        ReturnBooks();
                        break;

                    case "D":
                     
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
                        //LoadAdminFromFile();
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

        static void RegisterUser()
        {

            Console.Write("Enter Username: ");
            string Username = Console.ReadLine();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == Username)
                {
                    UserId = i;
                }
            }
            Console.Write("Enter Email: ");
            string Email = Console.ReadLine();

           
            Console.WriteLine(" User ID:" + UserId);

            Console.WriteLine("User registered successfully!");

            users.Add((Username, Email, UserId));
        }


        static void RegisterAdmin()
        {
            string Password = "a123";

            Console.Write("Enter Admin Password to Register: ");
            string Password1 = Console.ReadLine();


            if (Password1 == Password)
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.WriteLine("Admin registered successfully!");
                Admin.Add((username, email, Password1));
                UserId = users.Count;
                SaveAdminToFile();
            }
            else
            {
                Console.WriteLine("Incorrect password. Registration failed.");
            }
        }

        static void LoginAdmin()
        {
            LoadAdminFromFile();
            string Password = "a123"; 

            Console.Write("Enter Admin Password: ");
            string Password1 = Console.ReadLine();


            if (Password1 == Password)
            {
                Console.Write("Enter Username: ");
                string adminUsername = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                bool flag = false;

                for (int i = 0; i < Admin.Count; i++)
                {
                    if (adminUsername == Admin[i].Username1 && email == Admin[i].Email)
                    {
                        Console.WriteLine("\nAdmin authenticated successfully!");
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    AdminMenu();
                }
                else
                {
                    Console.WriteLine("Failed to authenticate. Invalid username or email.");
                }
            }
            else
            {
                Console.WriteLine("Incorrect password.");
            }
        }

        static void LoginUser()
        {




            Console.Write("Enter Username: ");
            string Username = Console.ReadLine();

            Console.Write("Enter Email: ");
            string Email = Console.ReadLine();



            bool flag = false;

            for (int i = 0; i < users.Count; i++)
            {
                if (Username == users[i].Username && Email == users[i].Email && users[i].UserID == users[i].UserID)
                {
                    Console.Write("\n user authenticated successfully!");
                    flag = true;

                    break;
                }

            }
            if (flag)
            {
                UserMenu();


            }
            else
            {

                Console.WriteLine("Failed to authenticate. Invalid username, email, or password.");


            }










        }

         static void AdminReport()
         {
             Books.Clear();
             //ReportFromFile();
             LoadBorrowCountsFromFile();
             LoadBooksFromFile();

             int TotalBookInLibrary = 0;


             if (Books.Count == 0)
             {
                 Console.WriteLine("No books in the system.");
                 return;
             }

             for (int i = 0; i < Books.Count; i++)
             {

                 TotalBookInLibrary += Books[i].Copies;

             }

             int[] mostBorrowedBook = new int[Books.Count];
             int mostBorrowedBookID = -1;
             for (int i = 0; i < BorrowCounts.Count; i++)
             {
                 for (int j = 0; j < mostBorrowedBook.Length; j++)
                 {
                     if (BorrowCounts[i].ID == j)
                     {
                         mostBorrowedBook[j]++;
                     }
                 }

             }

             for (int i = 0; i < mostBorrowedBook.Length; i++)
             {
                 if (mostBorrowedBook[i] == mostBorrowedBook.Max())
                 {
                     mostBorrowedBookID = i;
                 }
             }

             Console.WriteLine("Number of Borroed Books is : " + BorrowCounts.Count);
             Console.WriteLine("Number of Books in Library is : " + TotalBookInLibrary);
             Console.WriteLine("Most borrowed book : " + Books[mostBorrowedBookID].BName);

            SaveReportsToFile();
         }

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
                            if (parts.Length == 2)
                            {
                                BorrowCounts.Add((int.Parse(parts[0]), int.Parse(parts[1])));

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
                    foreach (var book in BorrowCounts)
                    {

                        writer.WriteLine($"{book.UserID}|{book.ID}");

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
                            Books[i] = (Newname, Books[i].BAuthor, Books[i].ID, Books[i].Copies, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                            Console.WriteLine("Name Changed successfully ");
                            break;

                        case 'B':
                            Console.WriteLine("Enter new book Author :");
                            string NewAuthor = Console.ReadLine();
                            Books[i] = (Books[i].BName, NewAuthor, Books[i].ID,Books[i].Copies, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                            Console.WriteLine("Author Changed successfully ");
                            break;

                        case 'C':
                            Console.WriteLine("Enter new book Quantity :");
                            int Newquantity = int.Parse(Console.ReadLine());
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, Newquantity, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);
                            Console.WriteLine("Quantity Changed successfully ");
                            break;
                        case 'D':
                            Console.WriteLine("Enter new book Price :");
                            int NewPrice = int.Parse(Console.ReadLine());
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, Books[i].Copies, Books[i].BorrowedCopies, NewPrice, Books[i].Category, Books[i].BorrowPeriod);
                            Console.WriteLine("Price Changed successfully ");
                            break;
                        case 'F':
                            Console.WriteLine("Enter new book Category :");
                            string NewCategory = (Console.ReadLine());
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, Books[i].Copies, Books[i].BorrowedCopies, Books[i].Price, NewCategory, Books[i].BorrowPeriod);
                            Console.WriteLine("Category Changed successfully ");
                            break;
                        case 'M':
                            Console.WriteLine("Enter new book Borrow Period :");
                            int NewBorrowPeriod = int.Parse(Console.ReadLine());
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, Books[i].Copies, Books[i].BorrowedCopies, Books[i].Price, Books[i].Category, NewBorrowPeriod);
                            Console.WriteLine("Borrow Period Changed successfully ");
                            break;



                    }

                    break;


                }


            }
        }

    }
}
   


