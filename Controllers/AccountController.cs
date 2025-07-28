using Lab_App.DTO;
using Lab_App.Migrations;
using Lab_App.Models;
using Lab_App.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly myDBcontext db;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config, myDBcontext db)
        {
            this.userManager = userManager;
            this.config = config;
            this.db = db;
        }

        [HttpGet("labs")]
        public IActionResult GetAllLabs()
        {

            var labs = db.Labs.ToList();
            return Ok(labs);
        }

        [HttpGet("allusers/{labId}")]
        public async Task< IActionResult> Getallusers(int labId)
        {
            var users = await db.Users.Where(u=>u.labId == labId).Select(u => new usersDTO
            {
                userID = u.Id,
                username = u.UserName,
                labname = u.Labs.lab_name,
                expdate = u.License.ExpirationDate
            } ).ToListAsync();

            return Ok(users);
        }


        [HttpPut("UpdateLicenseDate/{userId}")]
        public IActionResult UpdateLicenseDate(string userId, [FromBody] DateTime newExpiryDate)
        {
            var user = db.Licenses.FirstOrDefault(l => l.UserId == userId);
            if (user == null)
                return NotFound("User not found");

            user.ExpirationDate = newExpiryDate;
            db.SaveChanges();

            return Ok(new { message = "تم تحديث تاريخ الرخصة بنجاح." });
        }

        [HttpPost]
        public async Task<IActionResult> Registeration(RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //  توليد مفتاح ترخيص عشوائي
            string newLicenseKey = Guid.NewGuid().ToString().ToUpper();

            //  إنشاء مستخدم جديد مع LicenseKey
            ApplicationUser user = new ApplicationUser
            {
                UserName = userDTO.username,
                labId = userDTO.labId,
                LicenseKey = newLicenseKey //  إضافة الترخيص للمستخدم
            };

            IdentityResult result = await userManager.CreateAsync(user, userDTO.password);

            if (result.Succeeded)
            {
                //  جلب المستخدم بعد إنشائه لضمان وجود Id
                var savedUser = await userManager.FindByNameAsync(userDTO.username);
                if (savedUser == null)
                {
                    Console.WriteLine("❌ لم يتم العثور على المستخدم بعد التسجيل!");
                    return BadRequest("حدث خطأ أثناء حفظ بيانات المستخدم.");
                }

                Console.WriteLine($"✅ User Created: {savedUser.UserName}, ID: {savedUser.Id}, LicenseKey: {savedUser.LicenseKey}");

                var roleResult = await userManager.AddToRoleAsync(user, "User");


                // ✅ إنشاء الترخيص باستخدام UserId الصحيح
                var license = new License
                {
                    LicenseKey = newLicenseKey,
                    UserId = savedUser.Id, // ✅ استخدام الـ UserId الذي تم جلبه بعد التسجيل
                    ExpirationDate = DateTime.UtcNow.AddYears(1), // صلاحية سنة
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                db.Licenses.Add(license);
                await db.SaveChangesAsync();

                Console.WriteLine($"✅ License Created for User ID: {license.UserId}, LicenseKey: {license.LicenseKey}");
                if (roleResult.Succeeded)
                {
                    return Ok(new
                    {
                        message = "تم إنشاء الحساب بنجاح!",
                        licenseKey = newLicenseKey,
                        userId = savedUser.Id,
                        rolesforthisUser= roleResult
                    });
                }
            }

                return BadRequest(result.Errors.FirstOrDefault());
        }






        [HttpPost("login")]
        public async Task<IActionResult> loginuser(LoginDTO model)
        {            

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ Model state is invalid.");
                return Unauthorized();
            }

            var user = await userManager.Users
                .Where(u => u.UserName == model.username)
                .Select(u => new { u.Id, u.UserName, u.LicenseKey, u.PasswordHash, u.labId })
                .FirstOrDefaultAsync();


            if (user == null)
            {
                Console.WriteLine($"❌ User '{model.username}' not found in the database.");
                return Unauthorized();
            }

            Console.WriteLine($"✅ User found: {user.UserName} | LicenseKey: {user.LicenseKey}");

            // 🔑 Check password
            var userObject = await userManager.FindByNameAsync(model.username);
            bool isPasswordValid = await userManager.CheckPasswordAsync(userObject, model.Password);

            if (!isPasswordValid)
            {
                Console.WriteLine("❌ Password incorrect.");
                return Unauthorized();
            }

            // Create claims for token
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // 🔍 Get roles
            var roles = await userManager.GetRolesAsync(userObject);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 🔐 Generate token
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            SigningCredentials signincred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken mytoken = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signincred
            );

            Console.WriteLine($"✅ Token generated for {user.UserName}");

            var LicenseKey =  await userManager.Users
                .Where(u => u.UserName == model.username)
                .Select(u =>  u.LicenseKey)
                .FirstOrDefaultAsync();

            var responseDto = new loginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                Expiration = mytoken.ValidTo,
                CurrentUser = user.UserName,
                LicenseKey = LicenseKey,
                LabId = user.labId,
                RolesforthisUser = roles.ToList() // تحويل الأدوار إلى List لتجنب الميتاداتا
            };

           
            return Ok(responseDto);

            //return Ok(new
            //{
            //    Token = new JwtSecurityTokenHandler().WriteToken(mytoken),
            //    expiration = mytoken.ValidTo,
            //    currentuser = user.UserName,
            //    licenseKey = LicenseKey,
            //    labId=user.labId,
            //    rolesforthisUser = roles.ToList()

            //});

        }


    }
}

