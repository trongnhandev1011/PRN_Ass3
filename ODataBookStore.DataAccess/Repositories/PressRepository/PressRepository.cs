using ODataBookStore.BusinessObject.Models;
using ODataBookStore.DataAccess.Data;
using ODataBookStore.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataBookStore.DataAccess.Repositories.PressRepository
{
    public class PressRepository : BaseRepository<ODataBookStoreContext, Press>, IPressRepository
    {
        public PressRepository(ODataBookStoreContext context) : base(context)
        {
        }
    }
}
