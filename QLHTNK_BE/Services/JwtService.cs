using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLHTNK_BE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLHTNK_BE.Services
{
    public class JwtService
    {
        private readonly DentalCentreManagementContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(DentalCentreManagementContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            var userAccount = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (userAccount is null || userAccount.MatKhau != request.Password || userAccount.XacNhan == 0 || userAccount.XacNhan == null)
                return null;
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValididtyMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            //var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValididtyMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.Email),
                    new Claim(ClaimTypes.Role, userAccount.LoaiNguoiDung)
                }),
                Expires = null,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                     SecurityAlgorithms.HmacSha512Signature),

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                AccessToken = accessToken,
                Id = userAccount.MaTk,
                Role = userAccount.LoaiNguoiDung
            };

        }

    }
}
