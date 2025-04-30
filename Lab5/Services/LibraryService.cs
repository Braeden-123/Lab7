using Lab5.Models;
namespace Lab5.Services
{
    #public class LibraryService : ILibraryService
    {
        private readonly string _booksPath = "./Data/Books.csv";
        private readonly string _usersPath = "./Data/Users.csv";
        private List<Book> _books;
        private List<User> _users;
        private Dictionary<int, List<Book>> borrowedBooks = new Dictionary<int, List<Book>>();
        public LibraryService()
        {
            _books = ReadBooks();
            _users = ReadUsers();
            borrowedBooks = new Dictionary<int, List<Book>>();
        }
        public List<Book> ReadBooks()
        {
            var books = new List<Book>();
            try
            {
                foreach (var line in File.ReadLines(_booksPath))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 4)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        };
                        books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return books;
        }
        public List<User> ReadUsers()
        {
            var users = new List<User>();
            try
            {
                foreach (var line in File.ReadLines(_usersPath))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        var user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return users;
        }
        private void SaveBooks()
        {
            try
            {
                var lines = _books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
                File.WriteAllLines(_booksPath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving books: {ex.Message}");
            }
        }
        private void SaveUsers()
        {
            try
            {
                var lines = _users.Select(u => $"{u.Id},{u.Name},{u.Email}");
                File.WriteAllLines(_usersPath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving users: {ex.Message}");
            }
        }
        public void AddBook(Book book)
        {
            book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(book);
            SaveBooks();
        }
        public void EditBook(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                SaveBooks();
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
        public void DeleteBook(int bookId)
        {
            var bookToDelete = _books.FirstOrDefault(b => b.Id == bookId);
            if (bookToDelete != null)
            {
                _books.Remove(bookToDelete);
                SaveBooks();
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
        public void AddUser(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            SaveUsers();
        }
        public void EditUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                SaveUsers();
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        public void DeleteUser(int userId)
        {
            var userToDelete = _users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete != null)
            {
                _users.Remove(userToDelete);
                SaveUsers();
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        public bool BorrowBook(int bookId, int userId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine("Book not available for borrowing.");
                return false;
            }
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }
            if (!borrowedBooks.ContainsKey(userId))
                borrowedBooks[userId] = new List<Book>();
            borrowedBooks[userId].Add(book);
            _books.Remove(book);
            SaveBooks();
            return true;
        }
        public bool ReturnBook(int bookId, int userId)
        {
            if (borrowedBooks.TryGetValue(userId, out var userBorrowed))
            {
                var book = userBorrowed.FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    userBorrowed.Remove(book);
                    _books.Add(book);
                    SaveBooks();
                    return true;
                }
            }
            Console.WriteLine("The book or user record does not exist in borrowed records.");
            return false;
        }
        public Dictionary<int, List<Book>> GetBorrowedBooks()
        {
            return borrowedBooks;
        }
    }
}
