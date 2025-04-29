using Lab5.Models;
using Lab5.Services;
namespace MSTestProject
{
    [TestClass]
    public class LibraryServiceTests
    {
        private const string BooksPath = "./Data/Books.csv";
        private const string UsersPath = "./Data/Users.csv";
        private LibraryService _service;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            Directory.CreateDirectory("./Data");
            File.WriteAllLines(BooksPath, new[]
            {
            "1,Book One,Author A,ISBN1",
            "2,Book Two,Author B,ISBN2"
        });
            File.WriteAllLines(UsersPath, new[]
            {
            "1,User One,user1@example.com"
        });

            // Act
            _service = new LibraryService();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists("./Data"))
                Directory.Delete("./Data", true);
        }

        [TestMethod]
        public void ReadBooks_ShouldReturnAllBooks()
        {
            // Act
            List<Book> books = _service.ReadBooks();

            // Assert
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book One", books[0].Title);
            Assert.AreEqual("Book Two", books[1].Title);
        }

        [TestMethod]
        public void AddBook_ShouldAddBookToLibrary()
        {
            // Arrange
            var newBook = new Book { Title = "New Book", Author = "Author C", ISBN = "ISBN3" };

            // Act
            _service.AddBook(newBook);
            List<Book> books = _service.ReadBooks();

            Book addedBook = null;
            foreach (Book b in books)
            {
                if (b.Title == "New Book")
                {
                    addedBook = b;
                    break;
                }
            }

            // Assert
            Assert.AreEqual(3, books.Count);
            Assert.IsNotNull(addedBook);
            Assert.AreEqual("Author C", addedBook.Author);
            Assert.AreEqual("ISBN3", addedBook.ISBN);
            Assert.AreEqual(3, addedBook.Id);
        }

        [TestMethod]
        public void EditBook_ShouldUpdateExistingBook()
        {
            // Arrange
            var updatedBook = new Book { Id = 1, Title = "Book One Edited", Author = "Author A Edited", ISBN = "ISBN1E" };

            // Act
            _service.EditBook(updatedBook);
            List<Book> books = _service.ReadBooks();

            Book foundBook = null;
            foreach (Book b in books)
            {
                if (b.Id == 1)
                {
                    foundBook = b;
                    break;
                }
            }

            // Assert
            Assert.IsNotNull(foundBook);
            Assert.AreEqual("Book One Edited", foundBook.Title);
            Assert.AreEqual("Author A Edited", foundBook.Author);
            Assert.AreEqual("ISBN1E", foundBook.ISBN);
        }

        [TestMethod]
        public void DeleteBook_ShouldRemoveBookFromLibrary()
        {
            // Arrange
            int initialCount = _service.ReadBooks().Count;

            // Act
            _service.DeleteBook(2);
            List<Book> books = _service.ReadBooks();

            Book deletedBook = null;
            foreach (Book b in books)
            {
                if (b.Id == 2)
                {
                    deletedBook = b;
                    break;
                }
            }

            // Assert
            Assert.AreEqual(initialCount - 1, books.Count);
            Assert.IsNull(deletedBook);
        }

        [TestMethod]
        public void ReadUsers_ShouldReturnAllUsers()
        {
            // Act
            List<User> users = _service.ReadUsers();

            // Assert
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("User One", users[0].Name);
            Assert.AreEqual("user1@example.com", users[0].Email);
        }

        [TestMethod]
        public void AddUser_ShouldAddUserToLibrary()
        {
            // Arrange
            var newUser = new User { Name = "User Two", Email = "user2@example.com" };

            // Act
            _service.AddUser(newUser);
            List<User> users = _service.ReadUsers();

            User addedUser = null;
            foreach (User u in users)
            {
                if (u.Name == "User Two")
                {
                    addedUser = u;
                    break;
                }
            }

            // Assert
            Assert.AreEqual(2, users.Count);
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("user2@example.com", addedUser.Email);
            Assert.AreEqual(2, addedUser.Id);
        }

        [TestMethod]
        public void EditUser_ShouldUpdateExistingUser()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Name = "User One Edited", Email = "user1edited@example.com" };

            // Act
            _service.EditUser(updatedUser);
            List<User> users = _service.ReadUsers();

            User foundUser = null;
            foreach (User u in users)
            {
                if (u.Id == 1)
                {
                    foundUser = u;
                    break;
                }
            }

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("User One Edited", foundUser.Name);
            Assert.AreEqual("user1edited@example.com", foundUser.Email);
        }

        [TestMethod]
        public void DeleteUser_ShouldRemoveUserFromLibrary()
        {
            // Arrange
            int initialCount = _service.ReadUsers().Count;

            // Act
            _service.DeleteUser(1);
            List<User> users = _service.ReadUsers();

            User deletedUser = null;
            foreach (User u in users)
            {
                if (u.Id == 1)
                {
                    deletedUser = u;
                    break;
                }
            }

            // Assert
            Assert.AreEqual(initialCount - 1, users.Count);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void ReadBooks_MissingFile_ShouldReturnEmptyList()
        {
            // Arrange
            File.Delete(BooksPath);

            // Act
            List<Book> books = _service.ReadBooks();

            // Assert
            Assert.IsNotNull(books);
            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void ReadUsers_MissingFile_ShouldReturnEmptyList()
        {
            // Arrange
            File.Delete(UsersPath);

            // Act
            List<User> users = _service.ReadUsers();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count);
        }
    }
}