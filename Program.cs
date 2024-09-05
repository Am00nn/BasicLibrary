using System.Text;
using System.Xml.Linq;

namespace BasicLibrary
{
    internal class Program
    {
        static List<(string BName, string BAuthor, int ID, int quantity)> Books = new List<(string BName, string BAuthor, int ID, int quantity)>();
        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\test\\lib.txt";
        //this ids for test
        static void Main(string[] args)
        {// downloaded form ahmed device 
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
                Console.WriteLine("\n A- Admin Menu");
                Console.WriteLine("\n B- User Menu");

                Console.WriteLine("\n C- Save and Exit");


                string choice = Console.ReadLine().ToUpper();

                try
                {
                    switch (choice)
                    {
                        case "A":
                            AdminMenu();
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
            Console.WriteLine("Enter Book Name");
            string name = Console.ReadLine();

            Console.WriteLine("Enter Book Author");
            string author = Console.ReadLine();

            Console.WriteLine("Enter Book ID");
            if (!int.TryParse(Console.ReadLine(), out int ID))

            {
                Console.WriteLine("Invalid input for Book ID. Please enter a valid integer.");
                return;
            }
            Console.WriteLine("Enter Book quantity");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid input for Book quantity. Please enter a valid integer.");

            }


            Books.Add((name, author, ID, quantity));
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




    }

}
   


