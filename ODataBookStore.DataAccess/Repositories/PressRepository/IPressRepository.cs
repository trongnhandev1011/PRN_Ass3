using ODataBookStore.BusinessObject.Models;
using ODataBookStore.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataBookStore.DataAccess.Repositories.PressRepository
{
    public interface IPressRepository: IBaseRepository<Press>
    {
    }
}
