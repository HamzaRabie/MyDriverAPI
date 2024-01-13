using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyDriver.Controllers.DTOS;
using MyDriver.JWT_Options;
using MyDriver.Migrations;
using MyDriver.Model;
using MyDriver.Model.AuthManagment;
using MyDriver.Model.DB;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.PassengersData;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyDriverAPI.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly JWT jwtOptions;
        private readonly AppDbContext context;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwtOptions
            , IUnitOfWork unitOfWork, AppDbContext context
            )
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.jwtOptions = jwtOptions.Value;
            this.context = context; 
        }

        public async Task<AuthModel> Login(LoginDTO model)
        {
            var authModel = new AuthModel();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || await userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                return new AuthModel()
                {
                    Message = "Email Or Password Is Wrong"
                };
            }

            var JwtToken = await GenerateJWT(user);
            var refreshtoken = user.refreshtokens.FirstOrDefault(t => t.IsActive);

            if (refreshtoken == null)
            {
                var refreshToken = GenerateRefreshToken();
                user.refreshtokens.Add(refreshToken);
                await userManager.UpdateAsync(user);

                authModel.refreshToken = refreshToken.Token;
                authModel.refreshTokenExpiration = refreshToken.ExpireOn;
            }
            else
            {
                authModel.refreshToken = refreshtoken.Token;
                authModel.refreshTokenExpiration = refreshtoken.ExpireOn;
            }
            
            var roles = await userManager.GetRolesAsync(user);
            
            foreach (var role in roles)
            {
                if(role=="Driver")
                {
                    var driver = await context.drivers.SingleOrDefaultAsync( d=>d.UserName == user.UserName );
                    driver.IsLogged = true; 
                    context.SaveChanges();
                }
            }

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            authModel.TokenExpiration = JwtToken.ValidTo;
            authModel.UserName = user.UserName;
            authModel.Roles = roles.ToList();

            return authModel;

        }

        public async Task<AuthModel> RegiserDriver(DriverRegister newDriver)
        {

            if (await userManager.FindByEmailAsync(newDriver.Email) != null)
                return new AuthModel()
                {
                    Message = "Email Already Registered"
                };

            if (await userManager.FindByNameAsync(newDriver.UserName) != null)
                return new AuthModel()
                {
                    Message = "Username Already Registered"
                };

            ApplicationUser driver = new ApplicationUser();
            driver.Email = newDriver.Email;
            driver.UserName = newDriver.UserName;
            driver.FirstName = newDriver.FirstName;
            driver.LastName = newDriver.LastName;
            driver.PhoneNumber = newDriver.PhoneNumber;

            var res = await userManager.CreateAsync(driver, newDriver.Password);
            var errors = "";
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    errors += item.Description;
                    errors += " , ";
                }
                return new AuthModel()
                {
                    Message = errors
                };
            }

            await userManager.AddToRoleAsync(driver, "Driver");

            var JWtToken = await GenerateJWT(driver);
            var refreshToken = GenerateRefreshToken();

            driver.refreshtokens.Add(refreshToken);
            await userManager.UpdateAsync(driver);

            //Add in Drivers Table 

            await unitOfWork.drivers.AddAsync(new Driver
            {
                AppUserId = driver.Id,
                Email = driver.Email,
                UserName = driver.UserName,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                PhoneNumber = driver.PhoneNumber,
                Password = newDriver.Password,
                LicenseNumber = newDriver.LicenseNumber,
                FavAreas = newDriver.FavAreas,
                IsLogged = true

            }) ;
            unitOfWork.complete();

            return new AuthModel()
            {
                UserName = driver.UserName,
                Roles = new List<string> { "Driver" },
                Token = new JwtSecurityTokenHandler().WriteToken(JWtToken),
                TokenExpiration = JWtToken.ValidTo,
                IsAuthenticated = true,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.ExpireOn

            };


        }

        public async Task<AuthModel> RegiserPassenger(PassengerRegister newPassenger)
        {
            if (await userManager.FindByEmailAsync(newPassenger.Email) != null)
                return new AuthModel()
                {
                    Message = "Email Already Registered"
                };

            if (await userManager.FindByNameAsync(newPassenger.UserName) != null)
                return new AuthModel()
                {
                    Message = "Username Already Registered"
                };

            ApplicationUser passenger = new ApplicationUser();
            passenger.Email = newPassenger.Email;
            passenger.UserName = newPassenger.UserName;
            passenger.FirstName = newPassenger.FirstName;
            passenger.LastName = newPassenger.LastName;
            passenger.PhoneNumber = newPassenger.PhoneNumber;

            var res = await userManager.CreateAsync(passenger, newPassenger.Password);
            var errors = "";
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    errors += item.Description;
                    errors += " , ";
                }
                return new AuthModel()
                {
                    Message = errors
                };
            }


            await userManager.AddToRoleAsync(passenger, "Passenger");

            var JWtToken = await GenerateJWT(passenger);
            var refreshToken = GenerateRefreshToken();

            passenger.refreshtokens.Add(refreshToken);
            await userManager.UpdateAsync(passenger);

            await unitOfWork.passengers.AddAsync(new Passenger
            {
                AppUserId = passenger.Id,
                Email = passenger.Email,
                UserName = passenger.UserName,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                PhoneNumber = passenger.PhoneNumber,
                Password = newPassenger.Password,
            });
            unitOfWork.complete();

            return new AuthModel()
            {
                UserName = passenger.UserName,
                Roles = new List<string> { "Passenger" },
                Token = new JwtSecurityTokenHandler().WriteToken(JWtToken),
                TokenExpiration = JWtToken.ValidTo,
                IsAuthenticated = true,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.ExpireOn

            };


        }

        public async Task<AuthModel> RefreshToken(string userToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.refreshtokens.Any(t => t.Token == userToken));
            if (user == null)
            {
                return new AuthModel()
                {
                    Message = "Invalid Token"
                };
            }

            var token = user.refreshtokens.Single(t => t.Token == userToken);
            if (!token.IsActive)
            {
                return new AuthModel()
                {
                    Message = "Refresh Token Is Not Active"
                };
            }

            token.RevokedON = DateTime.UtcNow;

            var JwtToken = await GenerateJWT(user);
            var refreshToken = GenerateRefreshToken();
            user.refreshtokens.Add(refreshToken);
            await userManager.UpdateAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            return new AuthModel()
            {
                IsAuthenticated = true,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.ExpireOn,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                TokenExpiration = JwtToken.ValidTo,
                UserName = user.UserName,
                Roles = roles.ToList(),
            };

        }

        public async Task<bool> RevokeRefreshToken(string userToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.refreshtokens.Any(t => t.Token == userToken));
            if (user == null)
                return false;

            var token = user.refreshtokens.Single(t => t.Token == userToken);
            if (!token.IsActive)
                return false;

            token.RevokedON = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return true;

        }

        public async Task<DriverUpdateModel> UpdateDriver(DriverUpdateModel newDriver, string driverId)
        {
            var driverDb = await userManager.FindByIdAsync(driverId);

            //username duplicate check 
            var usernameDuplicate = await userManager.FindByNameAsync(newDriver.UserName) ?? null;
            if (usernameDuplicate != null && driverDb.UserName != usernameDuplicate.UserName)
            {
                return new DriverUpdateModel
                {
                    Message = "Username Already Registered"
                };
            }

            //Email duplicate check
            var EmailDuplicate = await userManager.FindByEmailAsync(newDriver.Email) ?? null;
            if (EmailDuplicate != null && driverDb.Email != EmailDuplicate.Email)
            {
                return new DriverUpdateModel
                {
                    Message = "Email Already Registered"

                };
            }

            //password check
            var currDriver = await unitOfWork.drivers.GetByAppIDAsync(d => d.AppUserId == driverId);
            var currPassword = currDriver.Password;
            var res = await userManager.ChangePasswordAsync(driverDb, currPassword, newDriver.Password);
            if (!res.Succeeded)
            {
                var errors = "";
                foreach (var item in res.Errors)
                {
                    errors += item.Description;
                    errors += " , ";
                }
                return new DriverUpdateModel
                {
                    Message = errors
                };
            }

            driverDb.Email = newDriver.Email;
            driverDb.FirstName = newDriver.FirstName;
            driverDb.LastName = newDriver.LastName;
            driverDb.PhoneNumber = newDriver.PhoneNumber;
            driverDb.UserName = newDriver.UserName;
            driverDb.NormalizedEmail = newDriver.Email.ToUpper();
            driverDb.NormalizedUserName = newDriver.UserName.ToUpper();

            return new DriverUpdateModel
            {
                UserName = newDriver.UserName,
                Email = newDriver.Email,
                FirstName = newDriver.FirstName,
                FavAreas = newDriver.FavAreas,
                LicenseNumber = newDriver.LicenseNumber,
                LastName = newDriver.LastName,
                Password = newDriver.Password,
                PhoneNumber = newDriver.PhoneNumber,
            };

        }

        public async Task<PassengerUpdateModel> UpdatePassenger(PassengerUpdateModel newPassenger, string passengerId)
        {
            var passengerDb = await userManager.FindByIdAsync(passengerId);

            //username duplicate check 
            var usernameDuplicate = await userManager.FindByNameAsync(newPassenger.UserName) ?? null;
            if (usernameDuplicate != null && passengerDb.UserName != usernameDuplicate.UserName)
            {
                return new PassengerUpdateModel
                {
                    Message = "Username Already Registered"
                };
            }

            //Email duplicate check
            var EmailDuplicate = await userManager.FindByEmailAsync(newPassenger.Email) ?? null;
            if (EmailDuplicate != null && passengerDb.Email != EmailDuplicate.Email)
            {
                return new PassengerUpdateModel
                {
                    Message = "Email Already Registered"

                };
            }

            //password check
            var currPassenger = await unitOfWork.passengers.GetByAppIDAsync(d => d.AppUserId == passengerId);
            var currPassword = currPassenger.Password;
            var res = await userManager.ChangePasswordAsync(passengerDb, currPassword, newPassenger.Password);
            if (!res.Succeeded)
            {
                var errors = "";
                foreach (var item in res.Errors)
                {
                    errors += item.Description;
                    errors += " , ";
                }
                return new PassengerUpdateModel
                {
                    Message = errors
                };
            }

            //mapping values 
            passengerDb.Email = newPassenger.Email;
            passengerDb.FirstName = newPassenger.FirstName;
            passengerDb.LastName = newPassenger.LastName;
            passengerDb.PhoneNumber = newPassenger.PhoneNumber;
            passengerDb.UserName = newPassenger.UserName;
            passengerDb.NormalizedEmail = newPassenger.Email.ToUpper();
            passengerDb.NormalizedUserName = newPassenger.UserName.ToUpper();

            return new PassengerUpdateModel
            {
                Email = newPassenger.Email,
                FirstName = newPassenger.FirstName,
                LastName = newPassenger.LastName,
                PhoneNumber = newPassenger.PhoneNumber,
                UserName = newPassenger.UserName,
                Password = newPassenger.Password,

            };

        }


        private async Task<JwtSecurityToken> GenerateJWT(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            List<Claim> roleList = new List<Claim>();

            foreach (var role in userRoles)
            {
                roleList.Add(new Claim(ClaimTypes.Role, role));
            }

            var myClaims = new[]{
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            }.Union(roleList)
            .Union(userClaims);

            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var signingCredentials = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.DurationInMinutes),
                claims: myClaims

                );

            return jwtToken;

        }

        private RefreshToken GenerateRefreshToken()
        {
            string token = Guid.NewGuid().ToString();
            return new RefreshToken()
            {
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(10),
                Token = token
            };

        }

        public async Task<bool> DeleteUser(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            await userManager.DeleteAsync(user);
            context.SaveChanges();
            return true;

        }


    }
}
