using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

         
        // DTO for user registration
        public class RegisterUserDto
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string FullName { get; set; }
            [Required]
            public string Password { get; set; }
            public string PhoneNumber { get; set; }

            public string ProfilePictureUrl { get; set; }
        }

        // DTO for user login
        public class LoginDto
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
        }

        // DTO for user update
        public class UpdateUserDto
        {
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string ProfilePictureUrl { get; set; }
        }

        // DTO for changing password
        public class ChangePasswordDto
        {
            [Required]
            public string CurrentPassword { get; set; }
            [Required]
            public string NewPassword { get; set; }
        }

        // DTO for assigning/removing roles
        public class RoleDto
        {
            [Required]
            public string RoleName { get; set; }
        }

        // POST: api/ApplicationUser/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Get the user's roles
            var roles = await _userManager.GetRolesAsync(user);

            // Create claims including the user's roles
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add each role as a claim
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        // GET: api/ApplicationUser
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only admins can view all users
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/ApplicationUser/
        [HttpGet("me")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // GET: api/ApplicationUser/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can view user details
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // POST: api/ApplicationUser/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                ProfilePictureUrl = string.IsNullOrEmpty(registerDto.ProfilePictureUrl)
                    ? "https://rslqld.org/-/media/rslqld/stock-images/find-help/advocacy/dva-claims-icons/rsl-contact-methods_in-person-01.png?modified=20201013230428"
                    : registerDto.ProfilePictureUrl,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/ApplicationUser/{id}
        [HttpPut("{id}")]
        [Authorize] // Users can update their own profile; admins can update any user
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ensure the user can only update their own profile unless they're an admin
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            user.FullName = updateDto.FullName ?? user.FullName;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
            user.ProfilePictureUrl = updateDto.ProfilePictureUrl ?? user.ProfilePictureUrl; // Will not be null due to model default
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // DELETE: api/ApplicationUser/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete users
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the user has any associated data that needs to be handled
            var orders = await _context.Orders.AnyAsync(o => o.UserId == id);
            var paymentMethods = await _context.PaymentMethods.AnyAsync(pm => pm.UserId == id);
            if (orders || paymentMethods)
            {
                return BadRequest("Cannot delete user because they have associated orders or payment methods.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // POST: api/ApplicationUser/change-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }

        // POST: api/ApplicationUser/logout
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT tokens are stateless, so logout typically involves client-side token deletion
            // In a production environment, you might implement token blacklisting
            return Ok("Logged out successfully. Please remove the token on the client side.");
        }

        // POST: api/ApplicationUser/{id}/assign-role
        [HttpPost("{id}/assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string id, RoleDto roleDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleDto.RoleName))
            {
                return BadRequest($"Role '{roleDto.RoleName}' does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleDto.RoleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Role '{roleDto.RoleName}' assigned to user '{user.UserName}'.");
        }

        // POST: api/ApplicationUser/{id}/remove-role
        [HttpPost("{id}/remove-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole(string id, RoleDto roleDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleDto.RoleName))
            {
                return BadRequest($"Role '{roleDto.RoleName}' does not exist.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleDto.RoleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Role '{roleDto.RoleName}' removed from user '{user.UserName}'.");
        }
    }
}