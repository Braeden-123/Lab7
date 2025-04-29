using Lab5.Models;
namespace Lab5.Services
{
    public interface ILibraryService
    {
        List<Book> ReadBooks();
        List<User> ReadUsers();
        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int bookId);
        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int userId);
        bool BorrowBook(int bookId, int userId);
        bool ReturnBook(int bookId, int userId);
        Dictionary<int, List<Book>> GetBorrowedBooks();
    }
}