using Microsoft.EntityFrameworkCore;
using ODataBookStore.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataBookStore.DataAccess.Data
{
    public class ODataBookStoreContext: DbContext
    {
        public ODataBookStoreContext(DbContextOptions<ODataBookStoreContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Press> Presses { get; set; }
        public DbSet<Book> Books { get; set; }

    }
}
