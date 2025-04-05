using BookStore.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BookDbContext _bookContext;

        public BookController(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        [HttpGet]
        public IActionResult GetBooks(int pageMany = 5, int pageNum = 1, string sortBy = "title",
        string sortOrder = "asc")
        {
            var query = _bookContext.Books.AsQueryable();

            // Apply sorting
            if (sortBy.ToLower() == "title")
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title);
            }
            
            var result = _bookContext.Books
                .Skip((pageNum-1)*pageMany)
                .Take(pageMany)
                .ToList();

            var totalBooks = _bookContext.Books.Count();
            var someObj = new
            {
                Books = result,
                TotalBooks = totalBooks
            };
            return Ok(someObj);

        }
    }
}
