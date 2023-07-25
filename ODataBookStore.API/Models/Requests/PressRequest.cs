
using ODataBookStore.BusinessObject.Models;

namespace ODataBookStore.API.Models.Requests
{
    public class PressRequest
    {
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
