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
        static List<(int UId, int BId, DateTime Bdate, DateTime Rdate, DateTime? ActualRD, int? Rating, bool isReturn)> borrowBook = new List<(int UId, int BId, DateTime Bdate, DateTime Rdate, DateTime? ActualRD, int? Rating, bool isReturn)>();
        static List<(int UsBorrowCountserID, int TotalBookInLibrary, int mostBorrowedBookID)> report = new List<(int UsBorrowCountserID, int TotalBookInLibrary, int mostBorrowedBookID)>();
        static List<(int CID, string CName, int NOFBooks)> Categories = new List<(int CID, string CName, int NOFBooks)>();



        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\test\\BooksFile.txt";
       
        static string filePathAdmin = "C:\\Users\\Lenovo\\source\\repos\\test\\AdminsFile.txt";
        static string filePathCategories = "C:\\Users\\Lenovo\\source\\repos\\test\\CategoriesFile.txt";
        static string filePathUser = "C:\\Users\\Lenovo\\source\\repos\\test\\UsersFile.txt";
        static string filePathBorrowCounts = "C:\\Users\\Lenovo\\source\\repos\\test\\BorrowingFile.txt";
        static string filePathReport = "C:\\Users\\Lenovo\\source\\repos\\test\\report.txt";



        static int maxBorrowCount = 0;


        static void Main(string[] args)
        {
            bool ExitFlag = false;

            
            try
            {
                LoadBooksFromFile();
                LoadCategoriesFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data from file: " + ex.Message);
                return;
            }

            do
            {

                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("      Welcome to the Library System     ");
                Console.WriteLine("========================================\n");

                Console.WriteLine("Please choose an option from the menu below:");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(" A - Admin");
                Console.WriteLine(" B - User");
                Console.WriteLine(" C - Save and Exit");
                Console.WriteLine("----------------------------------------");
                Console.Write("\nYour choice: ");

                string choice = Console.ReadLine().ToUpper();

                try
                {
                    switch (choice)
                    {
                        case "A":
                            Console.Clear();
                            Console.WriteLine("Admin Menu");
                            AdminFunction();
                            break;

                        case "B":
                            Console.Clear();
                            Console.WriteLine("User Menu");
                            UserFunction();
                            break;

                        case "C":
                            Console.WriteLine("\nSaving data and exiting the system...");
                            SaveBooksToFile();
                            ExitFlag = true;
                            break;

                        default:
                            Console.WriteLine("\nInvalid choice. Please select a valid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while processing your choice: " + ex.Message);
                }

                if (!ExitFlag)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }

            } while (!ExitFlag);


            Console.Clear();
            Console.WriteLine("Thank you for using the Library System. Goodbye!");
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
                                Categories.Add((int.Parse(parts[0].Trim()), parts[1].Trim(), int.Parse(parts[2].Trim())));
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

        static void EditBook()
        {
            Console.Clear();
            Console.WriteLine("============ Editing Book ============\n");
            Books.Clear();

            LoadBooksFromFile();
            ViewAllBooks();

            int index = -1;
            Console.Write("Enter Book ID: ");
            int ID = int.Parse(Console.ReadLine());

            Console.Clear();
            Console.WriteLine("=========== Book Details ===========\n");
            Console.WriteLine("{0,-10} | {1,-30} | {2,-25} | {3,-10}", "Book ID", "Book Name", "Author", "copies");
            Console.WriteLine(new string('=', 80)); 

            // Search for the book and display its details
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].ID == ID)
                {
                    index = i;
                    Console.WriteLine("{0,-10} | {1,-30} | {2,-25} | {3,-10}",
                        Books[i].ID, Books[i].BName, Books[i].BAuthor, Books[i].copies);
                }
            }

            if (index == -1)
            {
                Console.WriteLine("\nInvalid ID. No book found.");
                return;
            }

            Console.WriteLine(new string('=', 80)); 
            Console.WriteLine("\nChoose an option to edit for \"{0}\":", Books[index].BName);
            Console.WriteLine("1. Book Name\n2. Book Author\n3. Book Quantity\n");

            int choice = int.Parse(Console.ReadLine());


            switch (choice)
            {
                case 1:
                    EditBookName(index);
                    break;
                case 2:
                    EditBookAuthor(index);
                    break;
                case 3:
                    EditBooknewcopies(index);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }


        static void EditBookName(int index)
        {
            Console.Clear();
            Console.WriteLine($"==============Edit '{Books[index].BName}' Name =========================\n");

            Console.Write("Enter new Book name: ");
            string newTitle = Console.ReadLine();

            // Check if the new book name already exists
            foreach (var book in Books)
            {
                if (book.BName.Equals(newTitle, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("The title of this book already exists in the library.");
                    return;
                }
            }

            Console.WriteLine($"\nThe new edited book details:\nBook ID: {Books[index].ID}\n Book Name: {newTitle}\nAuthor: {Books[index].BAuthor}\nCopies: {Books[index].copies}");
            Console.Write("\n Are you sure you want to modify (yes / No) ?: ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "yes")
            {
                // Update book details
                Books[index] = (Books[index].ID, newTitle, Books[index].BAuthor, Books[index].copies, Books[index].BorrowedCopies, Books[index].Price, Books[index].Category, Books[index].BorrowPeriod);
                SaveBooksToFile();
                Console.WriteLine("\nThe new book name has been saved.");
            }
            else
            {
                Console.WriteLine("\nThe change was not saved.");
            }
        }


        static void EditBookAuthor(int index)
        {
            Console.Clear();
            Console.WriteLine($"========================Edit '{Books[index].BName}' Author=============================\n");

            Console.Write("Enter New Author name: ");
            string newAuthor = Console.ReadLine();

            Console.WriteLine($"\nThe new edited book details:\nBook ID: {Books[index].ID}\nBook Name: {Books[index].BName}\nAuthor: {newAuthor}\nCopies: {Books[index].copies}");
            Console.Write("\n Are you sure you want to modify (yes / No) ?: ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "yes")
            {
                // Update book details
                Books[index] = (Books[index].ID, Books[index].BName, newAuthor, Books[index].copies, Books[index].BorrowedCopies, Books[index].Price, Books[index].Category, Books[index].BorrowPeriod);
                SaveBooksToFile();
                Console.WriteLine("\nThe New Author has been saved.");
            }
            else
            {
                Console.WriteLine("\nThe change was not saved.");
            }
        }


        static void EditBooknewcopies(int index)
        {
            Console.Clear();
            Console.WriteLine($"===========================Edit '{Books[index].BName}' Quantity=============================\n");

            Console.Write("\nEnter new quantity: ");
            int newcopies = int.Parse(Console.ReadLine());

            if (newcopies < 0)
            {
                Console.WriteLine("Not allowed to add negative copies.");
                return;
            }

            Console.WriteLine($"\nThe new edited book details:\nID: {Books[index].ID}\nTitle: {Books[index].BName}\nAuthor: {Books[index].BAuthor}\nCopies: {newcopies}");
            Console.Write("\n Are you sure you want to modify (yes / No) ?: ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "yes")
            {
                // Update book details
                Books[index] = (Books[index].ID, Books[index].BName, Books[index].BAuthor, newcopies, Books[index].BorrowedCopies, Books[index].Price, Books[index].Category, Books[index].BorrowPeriod);
                SaveBooksToFile();
                Console.WriteLine("The new Quantity has been saved.");
            }
            else
            {
                Console.WriteLine("The change was not saved.");
            }
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
                            if (parts.Length == 7)
                            {
                                // Parse UserID and BookID
                                int userId = int.Parse(parts[0].Trim());
                                int bId = int.Parse(parts[1].Trim());

                                // Parse Borrow and Return Dates
                                DateTime borrowDate = DateTime.Parse(parts[2].Trim());
                                DateTime returnDate = DateTime.Parse(parts[3].Trim());

                                // Parse nullable Due Date
                                DateTime? dueDate = null;
                                if (DateTime.TryParse(parts[4].Trim(), out DateTime parsedDueDate))
                                {
                                    dueDate = parsedDueDate;
                                }

                                // Parse nullable Fine
                                int? fine = null;
                                if (int.TryParse(parts[5].Trim(), out int parsedFine))
                                {
                                    fine = parsedFine;
                                }

                                // Parse the Returned Status
                                bool isReturned = bool.Parse(parts[6].Trim());

                                // Add the record to the borrowBook list
                                borrowBook.Add((userId, bId, borrowDate, returnDate, dueDate, fine, isReturned));
                            }
                        }
                    }
                    Console.WriteLine("Borrow counts loaded from file successfully.");
                }
                else
                {
                    Console.WriteLine("Borrow counts file does not exist.");
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
                    foreach (var book in borrowBook)
                    {

                        string actualRD = book.ActualRD.HasValue ? book.ActualRD.Value.ToString("yyyy-MM-dd") : "N/A";
                        string rating = book.Rating.HasValue ? book.Rating.Value.ToString() : "N/A";

                        writer.WriteLine($"{book.UId}|{book.BId}|{book.Bdate.ToString("yyyy-MM-dd")}" +
                            $"|{book.Rdate.ToString("yyyy-MM-dd")}|{actualRD}|{rating}|{book.isReturn}");

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }

        }

        static void GenerateReports()
        {
            LoadBooksFromFile();
            LoadCategoriesFile();
            LoadBorrowCountsFromFile();

            // Counting total books
            int bookCount = 0;
            for (int i = 0; i < Books.Count; i++)
            {
                bookCount++;
            }

            // Counting total categories
            int categoryCount = 0;
            for (int i = 0; i < Categories.Count; i++)
            {
                categoryCount++;
            }

            // Counting books in each category
            List<(string Category, int Count)> categoryBookCounts = new List<(string Category, int Count)>();
            for (int i = 0; i < Categories.Count; i++)
            {
                int categoryBookCount = 0;
                for (int j = 0; j < Books.Count; j++)
                {
                    if (Books[j].Category == Categories[i].CName)
                    {
                        categoryBookCount++;
                    }
                }
                categoryBookCounts.Add((Categories[i].CName, categoryBookCount));
            }

            // Calculating total number of copies
            int totalCopies = 0;
            for (int i = 0; i < Books.Count; i++)
            {
                totalCopies += Books[i].copies;
            }

            // Calculating total borrowed books
            int totalBorrowed = 0;
            for (int i = 0; i < borrowBook.Count; i++)
            {
                if (!borrowBook[i].isReturn)
                {
                    totalBorrowed++;
                }
            }

            // Calculating total returned books
            int totalReturned = 0;
            for (int i = 0; i < borrowBook.Count; i++)
            {
                if (borrowBook[i].isReturn)
                {
                    totalReturned++;
                }
            }

            // Outputting the report
            Console.WriteLine("Library Reports:");
            Console.WriteLine($"Number of Books: {bookCount}");
            Console.WriteLine($"Number of Categories: {categoryCount}");

            foreach (var cat in categoryBookCounts)
            {
                Console.WriteLine($"Category: {cat.Category}, Number of Books: {cat.Count}");
            }

            Console.WriteLine($"Total Number of Copies: {totalCopies}");
            Console.WriteLine($"Total Number of Borrowed Books: {totalBorrowed}");

            Console.WriteLine($"Total Number of Returned Books: {totalReturned}");
        }



        // *************** (admin function ) *************************

        static void LoginAdmin()
        {

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            // Check if the email exists 
            bool adminFound = false;
            int foundAdmin = -1;

            for (int i = 0; i < Admin.Count; i++)
            {
                if (Admin[i].email.Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    adminFound = true;
                    foundAdmin = i;
                    Console.WriteLine("\nEnter Admin's Password:");
                    string password = Console.ReadLine();
                    if (Admin[i].password == password)
                    {
                        AdminMenu(Admin[i].AID);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Admin's Password.");
                        Console.WriteLine("\nPress Enter key to continue...");
                        Console.ReadLine();
                        return;
                    }
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
        }
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

            return inputPassword == password;
        }
        static void RegisterAdmin()
        {
            int newAID = 1;

            // If there are admins in the list, find the maximum AID 
            if (Admin.Count > 0)
            {
              
                newAID = Admin[0].AID;

            
                for (int i = 1; i < Admin.Count; i++)
                {
                    if (Admin[i].AID > newAID)
                    {
                        newAID = Admin[i].AID; 
                    }
                }

                newAID++; 
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
            SaveAdminToFile();
            AdminFunction();

        }
        static string GetValidPassword()
        {
            string password;
            while (true)
            {
                Console.WriteLine("Enter your password (at least 8 characters, include uppercase, lowercase, and a digit):");
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

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|edu)$"))
                {
                    Console.WriteLine("Invalid email format. Try again.");
                    continue;
                }

                bool emailExists = Users.Any(u => u.email == email) || Admin.Any(a => a.email == email);

                if (!emailExists)
                {
                    break;
                }

                Console.WriteLine("Duplicate email. Try again.");
            }
            return email;
        }
        static void AdminFunction()
        {
            bool ExitFlag = false;


            while (!ExitFlag)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("          Welcome, Admin!            ");
                Console.WriteLine("=====================================\n");

                Console.WriteLine("Please select an operation from the menu below:");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(" A - Admin Registration");
                Console.WriteLine(" B - Admin Login");
                Console.WriteLine(" C - Exit to Main Menu");
                Console.WriteLine("-------------------------------------");
                Console.Write("\nYour choice: ");

                string choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "A":
                        Console.Clear();
                        Console.WriteLine("Admin Registration\n");
                        RegisterAdmin();
                        break;

                    case "B":
                        Console.Clear();
                        Console.WriteLine("Admin Login\n");
                        LoadAdminFromFile();
                        LoginAdmin();
                        break;

                    case "C":
                        ExitFlag = true;
                        Console.WriteLine("\nExiting Admin Menu...");
                        break;

                    default:
                        Console.WriteLine("\nInvalid option. Please choose a valid action.");
                        break;
                }


                if (!ExitFlag)
                {
                    Console.WriteLine("\nPress any key to return to the Admin Menu...");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.WriteLine("Returning to the main menu...");
        }
        static void AdminMenu(int adminID)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===================================");
                Console.WriteLine("          Library Admin Menu       ");
                Console.WriteLine("===================================");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. View Books");
                Console.WriteLine("3. Edit Books");
                Console.WriteLine("4. Remove Books");
                Console.WriteLine("5. Search Books");
                Console.WriteLine("6. Generate Reports");
                Console.WriteLine("7. Log Out");
                Console.WriteLine("===================================");
                Console.Write("Choose an option (1-7): ");
                var option = Console.ReadLine();


                Console.WriteLine("\n-----------------------------------\n");

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Add New Book");
                        Console.WriteLine("-----------------------------------");
                        AddNewBook();
                        break;
                    case "2":
                        Console.WriteLine("View Books");
                        Console.WriteLine("-----------------------------------");
                        ViewAllBooks();
                        break;
                    case "3":
                        Console.WriteLine("Edit Books");
                        Console.WriteLine("-----------------------------------");
                        EditBook();
                        break;
                    case "4":
                        Console.WriteLine("Remove Books");
                        Console.WriteLine("-----------------------------------");
                        DeleteBook();
                        break;
                    case "5":
                        Console.WriteLine("Search Books");
                        Console.WriteLine("-----------------------------------");
                        SearchForBook();
                        break;
                    case "6":
                        Console.WriteLine("Generate Reports");
                        Console.WriteLine("-----------------------------------");
                        GenerateReports();
                        break;
                    case "7":
                        Console.WriteLine("Saving changes and logging out...");
                        SaveBooksToFile();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose a valid option (1-7).");
                        break;
                }

                if (running)
                {

                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }
        static void AddNewBook()
        {

            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("        Add New Book to Library     ");
            Console.WriteLine("===================================");

            int newId = Books.Count + 1;




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
                    SaveCategoriesToFile();
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


            Books.Add((newId, name, author, copies, 0, price, categoryName, borrowPeriod));


            Console.WriteLine("\n===================================");
            Console.WriteLine($"Success! Book '{name}' has been added to the library.");
            Console.WriteLine("===================================");

            SaveBooksToFile();

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
        static void DeleteBook()
        {
            Console.Clear();
            Books.Clear();
            LoadBooksFromFile();
            ViewAllBooks();

            Console.WriteLine("Enter the book name you want to delete:");
            string name = Console.ReadLine();
            bool bookFound = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    // Check if the book has any borrowed copies
                    if (Books[i].BorrowedCopies > 0)
                    {
                        Console.WriteLine($"The book '{name}' cannot be deleted because there are {Books[i].BorrowedCopies} copies currently borrowed.");
                    }
                    else
                    {
                        Books.RemoveAt(i);
                        Console.WriteLine($"The book '{name}' has been deleted successfully.");
                    }
                    bookFound = true;
                    break;
                }
            }

            if (!bookFound)
            {
                Console.WriteLine("Book not available.");
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
                            if (parts.Length == 4)
                            {
                                Admin.Add((int.Parse(parts[0].Trim()), parts[1].Trim(), parts[2].Trim(), parts[3].Trim()));
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

        //************** (user function)******************************  

        static void RegisterUser()
        {
            int newUID = 1;

            
            if (Users.Count > 0)
            {
                newUID = Users[0].UID; 

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
            SaveUserToFile();
            UserFunction();
        }
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

        static void UserMenu(int userID)
        {

            LoadBorrowCountsFromFile();
            CheckOverdueBooks(userID);

            bool exitFlag = false;

            do
            {
                Console.Clear();

                Console.WriteLine($"Welcome to the Library");
                Console.WriteLine("\nEnter the number of the operation you need:");
                Console.WriteLine("\n1 - Search for Book by Name");
                Console.WriteLine("2 - Borrow Book");
                Console.WriteLine("3 - Return Book");
                Console.WriteLine("4 - Show Your Profile");
                Console.WriteLine("5 - Sign Out");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        SearchForBookForUser(userID);
                        break;

                    case "2":
                        Console.Clear();
                        BorrowBook(userID);
                        break;

                    case "3":
                        Console.Clear();
                        ReturnBook(userID);
                        break;


                    case "4":
                        Console.Clear();
                        ShowProfile(userID);
                        break;

                    case "5":
                        Console.Clear();
                        SaveBooksToFile();

                        Console.WriteLine("\nPress Enter key to exit the system...");
                        Console.ReadLine();
                        exitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (!exitFlag)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }

            } while (!exitFlag);
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
                        LoadUserFromFile();
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
        static void CheckOverdueBooks(int userId)
        {
            bool hasOverdue = false;
            DateTime today = DateTime.Now;

            Console.WriteLine("Checking for overdue books...");

            foreach (var borrow in borrowBook)
            {
                if (borrow.UId == userId && !borrow.isReturn && borrow.Rdate < today)
                {
                    hasOverdue = true;
                    Console.WriteLine($"Overdue Book: {borrow.BId}");
                }
            }

            if (hasOverdue)
            {
                Console.WriteLine("\nYou have overdue books. Please return them before proceeding.");
                ReturnBook(userId);
            }
            else
            {
                Console.WriteLine("No overdue books found.");
            }
        }

        static void SearchForBookForUser(int userID)
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
                            BorrowBook(userID);
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
        static void BorrowBook(int userId)
        {
          
            Books.Clear();
            LoadBooksFromFile();

            borrowBook.Clear();
            LoadBorrowCountsFromFile();

           
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("          Borrow a Book            ");
            Console.WriteLine("===================================");

            
            Console.WriteLine("\nAvailable Books for Borrowing:");
            Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10} | {4,-10} | {5,-10} | {6,-15} | {7,-5}",
                              "ID", "Title", "Author", "Copies", "Borrowed", "Price", "Category", "Period");
            Console.WriteLine(new string('-', 110)); 

            foreach (var book in Books)
            {
                if (book.copies > 0)
                {
                    Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-10} | {4,-10} | {5,-10:F2} | {6,-15} | {7,-5} days",
                                      book.ID, book.BName, book.BAuthor, book.copies,
                                      book.BorrowedCopies, book.Price, book.Category,
                                      book.BorrowPeriod);
                }
            }

            
            Console.WriteLine("\nPlease enter the Book ID to borrow:");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Invalid input. Please enter a valid Book ID.");
                return;
            }

            
            bool bookFound = false;
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].ID == ID)
                {
                    bookFound = true;

                    
                    if (Books[i].copies > Books[i].BorrowedCopies)
                    {
                       
                        bool isAlreadyBorrowed = false;
                        for (int j = 0; j < borrowBook.Count; j++)
                        {
                            if (borrowBook[j].UId == userId && borrowBook[j].BId == ID && !borrowBook[j].isReturn)
                            {
                                isAlreadyBorrowed = true;
                                Console.WriteLine("\nYou have already borrowed this book.");
                                Console.WriteLine("\nDo you want to borrow another book?");
                                Console.WriteLine("1. Yes");
                                Console.WriteLine("2. No");

                                string choice = Console.ReadLine();
                                if (choice == "1")
                                {
                                    BorrowBook(userId);
                                }
                                else
                                {
                                    UserMenu(userId);
                                }
                                return;
                            }
                        }

                       
                        if (!isAlreadyBorrowed)
                        {
                            
                            Console.WriteLine("\nBook '{0}' is available for borrowing.", Books[i].BName);
                            Console.WriteLine("Author: {0}, Price: {1:C}, Borrow Period: {2} days",
                                              Books[i].BAuthor, Books[i].Price, Books[i].BorrowPeriod);

                            // Confirm borrowing
                            Console.WriteLine("\nTo confirm borrowing, press 1. To cancel, press any other key.");
                            string confirmChoice = Console.ReadLine();

                            if (confirmChoice == "1")
                            {
                                DateTime today = DateTime.Today;
                                DateTime returnDate = today.AddDays(Books[i].BorrowPeriod);

                                // Update book info
                                Books[i] = (Books[i].ID, Books[i].BName, Books[i].BAuthor,
                                            Books[i].copies, Books[i].BorrowedCopies + 1,
                                            Books[i].Price, Books[i].Category, Books[i].BorrowPeriod);

                                // Update the borrow record and save changes
                                SaveBooksToFile();
                                borrowBook.Add((userId, ID, today, returnDate, null, null, false));
                                BorrowedBookFile();

                                Console.WriteLine("\nThank you! Please return the book within {0} days.", Books[i].BorrowPeriod);
                            }
                            else
                            {
                                Console.WriteLine("Borrowing cancelled.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo copies available for borrowing.");
                    }
                    break;
                }
            }

            // If the book ID was not found
            if (!bookFound)
            {
                Console.WriteLine("\nBook with ID {0} is not available in the library.", ID);
            }
        }
        static void ReturnBook(int userId)
        {
         
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("          Return a Book            ");
            Console.WriteLine("===================================");
            Console.WriteLine("\nBooks you are currently borrowing:");

            bool booksToReturn = false;

            
            for (int i = 0; i < borrowBook.Count; i++)
            {
                if (borrowBook[i].UId == userId && !borrowBook[i].isReturn)
                {
                    booksToReturn = true;

                    // Find and display book details
                    for (int j = 0; j < Books.Count; j++)
                    {
                        if (Books[j].ID == borrowBook[i].BId)
                        {
                            Console.WriteLine("-----------------------------------------------------");
                            Console.WriteLine("Book ID: {0}", Books[j].ID);
                            Console.WriteLine("Title:   {0}", Books[j].BName);
                            Console.WriteLine("Author:  {0}", Books[j].BAuthor);
                            Console.WriteLine("Borrowed on: {0}", borrowBook[i].Bdate.ToShortDateString());
                            Console.WriteLine("-----------------------------------------------------");
                            break;
                        }
                    }
                }
            }

            if (!booksToReturn)
            {
                Console.WriteLine("\nYou have no books to return.");
                return;
            }

            
            Console.Write("\nEnter the Book ID you want to return: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Book ID.");
                return;
            }

            bool bookFound = false;

            // Find the borrowing record and update it
            for (int i = 0; i < borrowBook.Count; i++)
            {
                if (borrowBook[i].UId == userId && borrowBook[i].BId == bookId && !borrowBook[i].isReturn)
                {
                    // Update the borrowing record to reflect the return
                    borrowBook[i] = (borrowBook[i].UId, borrowBook[i].BId, borrowBook[i].Bdate, borrowBook[i].Rdate, DateTime.Now, borrowBook[i].Rating, true);
                    bookFound = true;

                    // Update the book inventory (increment available copies and decrement borrowed copies)
                    for (int j = 0; j < Books.Count; j++)
                    {
                        if (Books[j].ID == bookId)
                        {
                            Books[j] = (Books[j].ID, Books[j].BName, Books[j].BAuthor, Books[j].copies + 1, Books[j].BorrowedCopies - 1, Books[j].Price, Books[j].Category, Books[j].BorrowPeriod);
                            break;
                        }
                    }

                    
                    Console.Write("\nPlease rate the book (1-5): ");
                    int rating;
                    while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5)
                    {
                        Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                    }

                    // Update the rating in the borrowing record
                    borrowBook[i] = (borrowBook[i].UId, borrowBook[i].BId, borrowBook[i].Bdate, borrowBook[i].Rdate, borrowBook[i].ActualRD, rating, true);

                    Console.WriteLine("\nThank you! You have successfully returned the book.");
                    break;
                }
            }

            // If the book ID entered was not found
            if (!bookFound)
            {
                Console.WriteLine("\nInvalid book selection. Please try again.");
            }

            // Save the updated information to the respective files
            SaveBooksToFile();
            BorrowedBookFile();
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
                            if (parts.Length == 4)
                            {
                                Users.Add((int.Parse(parts[0].Trim()), parts[1].Trim(), parts[2].Trim(), parts[3].Trim()));
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

        static void ShowProfile(int userId)
        {

            (int UID, string UserName, string email, string password)? foundUser = null;

            foreach (var user in Users)
            {
                if (user.UID == userId)
                {
                    foundUser = user;
                    break;
                }
            }

            // Display user information
            if (foundUser.HasValue)
            {
                var user = foundUser.Value;  // Extract the tuple
                Console.WriteLine("\nUser Profile");
                Console.WriteLine(new string('-', 30));
                Console.WriteLine($"ID      : {user.UID}");
                Console.WriteLine($"Name    : {user.UserName}");
                Console.WriteLine($"Email   : {user.email}");
                Console.WriteLine(new string('-', 30));
            }
            else
            {
                Console.WriteLine("User not found.");
                return;
            }

            // Display books currently borrowed by the user
            Console.WriteLine("\nBooks Currently Borrowed");
            Console.WriteLine(new string('-', 30));

            bool hasCurrentBorrowedBooks = false;
            foreach (var borrowEntry in borrowBook)
            {
                if (borrowEntry.UId == userId && !borrowEntry.isReturn)
                {
                    // Find the book details 
                    foreach (var book in Books)
                    {
                        if (book.ID == borrowEntry.BId)
                        {
                            Console.WriteLine($"Book Name  : {book.BName}");
                            Console.WriteLine($"Borrowed On: {borrowEntry.Bdate:yyyy-MM-dd}");
                            Console.WriteLine($"Return By  : {borrowEntry.Rdate:yyyy-MM-dd}");
                            Console.WriteLine(new string('-', 30));
                            hasCurrentBorrowedBooks = true;
                            break;
                        }
                    }
                }
            }

            if (!hasCurrentBorrowedBooks)
            {
                Console.WriteLine("No currently borrowed books.");
                Console.WriteLine(new string('-', 30));
            }

            // Display books previously borrowed and returned
            Console.WriteLine("\nBooks Previously Borrowed");
            Console.WriteLine(new string('-', 30));

            bool hasReturnedBooks = false;
            foreach (var borrowEntry in borrowBook)
            {
                if (borrowEntry.UId == userId && borrowEntry.isReturn)
                {
                    // Find the book details manually using foreach
                    foreach (var book in Books)
                    {
                        if (book.ID == borrowEntry.BId)
                        {
                            string rating = borrowEntry.Rating.HasValue ? borrowEntry.Rating.ToString() : "No rating";
                            Console.WriteLine($"Book Name   : {book.BName}");
                            Console.WriteLine($"Returned On : {borrowEntry.ActualRD:yyyy-MM-dd}");
                            Console.WriteLine($"Rating      : {rating}");
                            Console.WriteLine(new string('-', 30));
                            hasReturnedBooks = true;
                            break;
                        }
                    }
                }
            }

            if (!hasReturnedBooks)
            {
                Console.WriteLine("No previously borrowed books.");
                Console.WriteLine(new string('-', 30));
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
                                Books.Add((int.Parse(parts[0].Trim()), parts[1].Trim(), parts[2].Trim(), int.Parse(parts[3].Trim()),
                                    int.Parse(parts[4].Trim()), double.Parse(parts[5].Trim()), parts[6].Trim(), int.Parse(parts[7].Trim())));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }






    }




}



