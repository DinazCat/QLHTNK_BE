﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using QLHTNK_BE.Services;
using System.Net.Mail;
using System.Text;

namespace QLHTNK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService jwtService;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly DentalCentreManagementContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(JwtService jwtService, DentalCentreManagementContext context, UserManager<TaiKhoan> userManager)
        {
            this.jwtService = jwtService;
            _context = context;
            _userManager = userManager;
            _emailSender = new EmailSender();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
        {
            var result = await jwtService.Authenticate(request);
            if (result is null)
                return Unauthorized();

            return result;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] TaiKhoan user)
        {
            if (user == null)
                return BadRequest(new { Message = "Invalid user data." });
            var existingUser = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.MaNguoiDung == user.MaNguoiDung || x.Email == user.Email);

            if (existingUser != null)
            {
                var message = existingUser.MaNguoiDung == user.MaNguoiDung
                    ? "CCCD này đã được sử dụng để đăng ký!"
                    : "Email này đã được sử dụng để đăng ký!";
                return BadRequest(new { Success = false, message });
            }

            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                user.Token = encodedToken;
                var param = new Dictionary<string, string?>
                                {
                                    {"token",encodedToken },
                                    {"email",user.Email }
                                };
                Console.WriteLine("Hello, World!3");
                var callback = QueryHelpers.AddQueryString("http://192.168.1.88:3000/confirm_email", param);
                var emailBody = $@"
                                    <html>
                                    <body>
                                        <h2>Xác nhận email đăng ký tài khoản BestSmile Dentistry</h2>
                                        <p>Chào {user.Ten},</p>
                                        <p>Bạn đã đăng ký tài khoản bằng email này. Vui lòng nhấn vào liên kết bên dưới để xác nhận email:</p>
                                        <a href='{callback}'>{callback}</a>
                                        <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.</p>
                                        <p>Trân trọng,</p>
                                    </body>
                                    </html>";
                try
                {
                    await _emailSender.SendEmailAsync(user.Email, "Email confirmation", emailBody);
                }
                catch (SmtpException smtpEx)
                {
                    return StatusCode(500, new { Success = false, Message = "Error sending email.", Error = smtpEx.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Success = false, Message = "An error occurred while sending email.", Error = ex.Message });
                }
                _context.TaiKhoans.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { Success = true, Message = "Register successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while register.", Error = ex.Message });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return BadRequest(new { Success = false, message = "User not found" });

            if (user.Token == token)
            {
                user.XacNhan = 1;

                try
                {
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(new { Success = true, Message = "Email confirmed successfully!" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Success = false, Message = "An error occurred while confirming.", Error = ex.Message });
                }

            }
            return BadRequest(new { Success = false, message = "Invalid confirmation token." });
        }

        [HttpGet("getCode")]
        public async Task<IActionResult> GetCode(string email)
        {
            var user = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return BadRequest(new { Success = false, message = "User not found" });

            // Tạo mã code ngẫu nhiên gồm 6 chữ số
            var code = new Random().Next(100000, 999999).ToString();

            // Nội dung email
            var emailBody = $@"
        <html>
        <body>
            <p>Chào {user.Ten},</p>
            <p>Đây là mã code để bạn có thể reset mật khẩu: <b>{code}</b></p>
            <p>Nếu bạn không thực hiện yêu cầu reset mật khẩu, vui lòng bỏ qua email.</p>
            <p>Trân trọng,</p>
        </body>
        </html>";
            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Reset Password", emailBody);
                return Ok(new { Success = true, Email = email, Code = code });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while send code via your email.", Error = ex.Message });
            }

        }

        // Update a user's password by email
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] TaiKhoan user)
        {
            if (string.IsNullOrEmpty(user?.Email) || string.IsNullOrEmpty(user?.MatKhau))
                return BadRequest(new { Message = "Invalid data provided." });

            try
            {
                // Tìm tài khoản theo email
                var existingUser = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser == null)
                    return NotFound(new { Message = "User not found." });

                // Cập nhật mật khẩu
                existingUser.MatKhau = user.MatKhau;
                _context.Entry(existingUser).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { Message = "User not found for update." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the password.", Error = ex.Message });
            }
        }

    }
}
