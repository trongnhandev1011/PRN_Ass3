using ODataBookStore.BusinessObject.Models;
using ODataBookStore.DataAccess.Data;
using ODataBookStore.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataBookStore.DataAccess.Repositories.BookRepository
{
    public interface IBookRepository: IBaseRepository<Book>
    {
    }
}
