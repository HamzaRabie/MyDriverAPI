using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDriver.Controllers.DTOS;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriverAPI.Services.AuthServices;
using MyDriverAPI.Services.TripServices;

namespace MyDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly ITripService tripService;

        public AccountController( IAuthService service ,ITripService tripService )
        {
            this.service = service;
            this.tripService = tripService;
        }

        [HttpPost("DriverRegister")]
        public async Task<IActionResult> driverRegister(DriverRegister newDriver)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await service.RegiserDriver(newDriver);
            if(!res.IsAuthenticated)
                return BadRequest(res.Message);

            setRefreshTokenInCookies(res.refreshToken, res.refreshTokenExpiration);

            await tripService.AddAsync(newDriver);
            return Ok(res);
        }

        [HttpPost("RegisterPassenger")]
        public async Task<IActionResult> passengerRegister(PassengerRegister newPassenger)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await service.RegiserPassenger(newPassenger);
            if (!res.IsAuthenticated)
                return BadRequest(res.Message);

            setRefreshTokenInCookies(res.refreshToken, res.refreshTokenExpiration);
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await service.Login(user);
            if(!res.IsAuthenticated)
                return BadRequest(res.Message);

            setRefreshTokenInCookies(res.refreshToken, res.refreshTokenExpiration);

            return Ok(res);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> refreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var res = await service.RefreshToken(refreshToken);
            if( !res.IsAuthenticated)
                return BadRequest(res.Message);

            setRefreshTokenInCookies(res.refreshToken, res.refreshTokenExpiration);
            return Ok(res);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> revokeToken( RefreshTokenDTO refreshToken )
        {
            var token = refreshToken.refreshToken ?? Request.Cookies["refreshToken"];
            var res = await service.RevokeRefreshToken(token);

            if (res == false)
                return BadRequest("Invalid TOken");

            return Ok("Token Is Revoked Successfully");
        }

        private void setRefreshTokenInCookies(string token , DateTime expireon)
        {
            var options = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expireon,
            };
            Response.Cookies.Append("refreshToken",token,options);
        }


    }
}
