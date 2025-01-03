using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Helpers;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestDto registerDto)
        {
            // E-posta adresinin zaten var olup olmadığını kontrol et
            var existingUser = await _userService.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Email is already in use!" });
            }
            if (string.IsNullOrEmpty(registerDto.Password))
            {
                return BadRequest("Password is required");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                //PasswordHash= registerDto.Password,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),  // PasswordHash kullan
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                BirthDate = registerDto.DateOfBirth,  // DateOfBirth uyumlu
                Gender = registerDto.Gender
            };

            // Yeni kullanıcıyı ekle
            await _userService.AddUserAsync(user);

            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto loginDto)
        {
            // E-posta ile kullanıcıyı getir
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) // PasswordHash kontrolü
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            // JWT token oluştur
            var token = JwtHelper.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
