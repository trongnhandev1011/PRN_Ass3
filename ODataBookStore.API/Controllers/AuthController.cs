using ODataBookStore.API.Extensions;
using ODataBookStore.API.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ODataBookStore.DataAccess.Repositories.AccountRepository;
using ODataBookStore.BusinessObject.Models;

namespace ODataBookStore.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly IAccountRepository _accountRepository;

        public AuthController(JwtService jwtService, IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request)
        {

            var account = _accountRepository.FirstOrDefault(expression: x => x.Username == request.Username && x.Password == request.Password);

            if (account == null)
            {
                return Unauthorized();
            }

            return Ok(new { accessToken = _jwtService.GenerateJSONWebToken(account), role = account.Role });
        }


        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] LoginRequest request)
        {

            var account = _accountRepository.FirstOrDefault(expression: x => x.Username == request.Username);

            if (account != null)
            {
                return BadRequest();
            }

            Account createdAccount = new Account()
            {
                Password = request.Password,
                Username = request.Username,
                Role = Role.User
            };

            _accountRepository.Add(createdAccount);

            return Ok();
        }
    }
}
