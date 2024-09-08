using System.Text;
using System.Xml.Linq;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int ID, int quantity)> Books = new List<(string BName, string BAuthor, int ID, int quantity)>();
        static List<(string Username, string Email, string UserID)> users = new List<(string Username, string Email, string UserID)>();
        static List<(string Username1, string Email, string password)> Admin = new List<(string Username1, string Email, string password)>();

        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\test\\lib.txt";
        static string filePathBorrow = "C:\\Users\\Lenovo\\source\\repos\\test\\borrow.txt";
        static string filePathAdmin = "C:\\Users\\Lenovo\\source\\repos\\test\\Admin.txt";
        static string filePathUser= "C:\\Users\\Lenovo\\source\\repos\\test\\user.txt";



        static int id = 0;
      

        static void Main(string[] args)
        {
            bool ExitFlag = false;

            LoadAdminFromFile();
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
                            UserMenu();
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
            Console.WriteLine("How many books do you want to add?");
            int input = int.Parse(Console.ReadLine());

            for (int i = 0; i < input; i++)
            {
                Console.WriteLine($"Enter details for Book {i + 1}:");

                Console.WriteLine("Enter Book Name");
                string name = Console.ReadLine();

                Console.WriteLine("Enter Book Author");
                string author = Console.ReadLine();

                Console.WriteLine("Enter Book Quantity");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.WriteLine("Invalid input for Book quantity. Please enter a valid integer.");

                }


                int bookID = id++;


                Books.Add((name, author, bookID, quantity));



            }
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
                        sb.Append("Book ").Append(BookNumber).Append(" Quantity : ").Append(Books[i].quantity);
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
                            Console.WriteLine("Book Author is : " + Books[i].BAuthor);
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
                                    Books.Add((parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3])));
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
                                Admin.Add((parts[0], parts[1],(parts[2])));
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
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        foreach (var book in Books)
                        {
                            writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.ID}|{book.quantity}");
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
                using (StreamWriter writer = new StreamWriter(filePathAdmin, true))
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

        static void BorrowBook()

            {


                try
                {
                    int BookNumber = 0;

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < Books.Count; i++)
                    {
                        BookNumber = i + 1;
                        sb.Append("Book ").Append(BookNumber).Append(" name : ").Append(Books[i].BName);
                        sb.AppendLine();
                        sb.Append("Book ").Append(BookNumber).Append(" Author : ").Append(Books[i].BAuthor);
                        sb.AppendLine();
                        sb.Append("Book ").Append(BookNumber).Append(" ID : ").Append(Books[i].ID);
                        sb.AppendLine();
                        sb.Append("Book ").Append(BookNumber).Append(" Quantity : ").Append(Books[i].quantity);
                        sb.AppendLine().AppendLine();
                        Console.WriteLine(sb.ToString());
                        sb.Clear();

                    }

                    Console.WriteLine("Enter the book name you want");
                    string name = Console.ReadLine();
                    bool flag = false;

                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].BName == name && Books[i].quantity > 0)
                        {

                            Console.WriteLine("Book is available ");
                            int newquantity = Books[i].quantity - 1;
                            Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, newquantity);
                            Console.WriteLine("The book has been borrowed ");
                            flag = true;
                            break;
                        }
                    }

                    if (flag != true)
                    { Console.WriteLine("book not found"); }






                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);

                }

            }
            static void DeleteBook()
            {

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

            }

            static void EditBook()

            {


                bool ExitFlag = false;
                do
                {

                    Console.Write("Enter what you want to edit :");
                    Console.Write(" A- Book Name  ");
                    Console.Write(" B- Book Author  ");
                    Console.Write(" C- Book quantity ");
                    Console.Write(" D- Exit ");

                    string choice = Console.ReadLine().ToUpper(); ;

                    Console.WriteLine("Enter the book name you want");
                    string name = Console.ReadLine();
                    bool flag = false;
                    switch (choice)
                    {
                        case "A":


                            for (int i = 0; i < Books.Count; i++)
                            {
                                if (Books[i].BName == name)
                                {
                                    Console.WriteLine("Enter new book name :");
                                    String Newname = Console.ReadLine();
                                    Books[i] = (Newname, Books[i].BAuthor, Books[i].ID, Books[i].quantity);
                                    Console.WriteLine("Name Changed successfully ");
                                    flag = true;
                                    break;
                                }
                            }
                            break;

                        case "B":

                            for (int i = 0; i < Books.Count; i++)
                            {
                                if (Books[i].BName == name)
                                {
                                    Console.WriteLine("Enter new book Author :");
                                    string NewAuthor = Console.ReadLine();
                                    Books[i] = (Books[i].BName, NewAuthor, Books[i].ID, Books[i].quantity);
                                    Console.WriteLine("Author Changed successfully ");
                                    flag = true;
                                    break;

                                }

                            }

                            break;

                        case "C":

                            for (int i = 0; i < Books.Count; i++)
                            {
                                if (Books[i].BName == name)
                                {
                                    Console.WriteLine("Enter new book Quantity :");
                                    int Newquantity = int.Parse(Console.ReadLine());
                                    Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, Newquantity);
                                    Console.WriteLine("Author Changed successfully ");
                                    flag = true;
                                    break;

                                }

                            }
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

                    Console.Clear();

                } while (ExitFlag != true);


            }

            static void ReturnBooks()
            {

                Console.WriteLine("Enter the book name you want to return  :");
                string name = Console.ReadLine();

                bool flage = false;
                for (int i = 0; i < Books.Count; i++)
                {

                    if (Books[i].BName == name)
                    {

                        int newquantity = Books[i].quantity + 1;

                        Books[i] = (Books[i].BName, Books[i].BAuthor, Books[i].ID, newquantity);
                        Console.WriteLine("The book has been returned ");
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

                    Console.WriteLine("Welcome Admin");
                    Console.WriteLine("\n Enter the char of operation you need :");
                    Console.WriteLine("\n A- Add New Book");
                    Console.WriteLine("\n B- Display All Books");
                    Console.WriteLine("\n C- Search for Book by Name");
                    Console.WriteLine("\n D- Edit book");
                    Console.WriteLine("\n E- delet book");
                    Console.WriteLine("\n F- Save and Exit");

                    string choice = Console.ReadLine().ToUpper(); ;

                    switch (choice)
                    {

                        case "A":
                            AddnNewBook();
                            break;

                        case "B":
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
                        case "F":
                            SaveBooksToFile();
                            SaveAdminToFile();
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
            static void UserMenu()
            {
                bool ExitFlag = false;
                do
                {

                    Console.WriteLine("Welcome user");
                    Console.WriteLine("\n Enter the char of operation you need :");
                    Console.WriteLine("\n A- Search for Book by Name");
                    Console.WriteLine("\n B- Borrow Books");
                    Console.WriteLine("\n C- Return Books");
                    Console.WriteLine("\n D- Save and Exit");

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
                            SaveBooksToFile();
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

        static void AdminFunction()
        {
            bool ExitFlag = false;
            while (ExitFlag != true)
            {

                Console.WriteLine("Welcome Admin");
                Console.WriteLine("\n Enter the char of operation you need :");
                Console.WriteLine("\n A- Admain registeration");
                Console.WriteLine("\n B- Admain login");
                Console.WriteLine("\n C- Save and Exit");

                string choice = Console.ReadLine().ToUpper(); ;

                switch (choice)
                {
                    case "A":
                        RegisterAdmin();
                        break;
                    case "B":
                        LoginAdmin();
                        break;

                    case "C":
                        SaveAdminToFile();
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

            Console.Write("Enter Email: ");
            string Email = Console.ReadLine();

            Console.Write("Enter User ID: ");
            string UserID = Console.ReadLine();

            Console.WriteLine("User registered successfully!");

            users.Add((Username, Email, UserID));
        }

        static void RegisterAdmin()
        {
            Console.Write("Enter Username: ");
            string Username = Console.ReadLine();

            Console.Write("Enter Email: ");
            string Email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Admin registered successfully!");
            Admin.Add((Username, Email, password));
        }

        static void LoginAdmin()
        {
            
           


                Console.Write("Enter Username: ");
                string adminUsername = Console.ReadLine();

                Console.Write("Enter Email: ");
                string Email = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                bool flag = false;

                for (int i = 0; i < Admin.Count; i++)
                {
                    if (adminUsername == Admin[i].Username1 && Email == Admin[i].Email && password == Admin[i].password)
                    {
                        Console.Write("\n Admin authenticated successfully!");
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

                    Console.WriteLine("Failed to authenticate. Invalid username, email, or password.");


                }










        }



    }

}
   


