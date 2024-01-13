using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MyDriver.Model.DriversData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Services.AuthServices;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace MyDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthService service;

        public DriverController( IUnitOfWork unitOfWork , IAuthService service )
        {
            this.unitOfWork = unitOfWork;
            this.service = service;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok( await unitOfWork.drivers.GetAllAsync()) ;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await unitOfWork.drivers.GetByIdAsync(id));
        }

        [HttpGet("GetWithInclude")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await unitOfWork.drivers.GetOneWithInclude(d=>d.UserName==name , new[] {"null"} ) );
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Driver newDriver)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            await unitOfWork.drivers.AddAsync(newDriver);
            return Ok(unitOfWork.complete());
        }

        [Authorize(Roles ="Driver")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(DriverUpdateModel newDriver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await service.UpdateDriver(newDriver,UserId);

            if (!string.IsNullOrEmpty(res.Message) )
                return BadRequest(res.Message);

            await unitOfWork.drivers.UpdateDriver(newDriver,UserId);
            return Ok(unitOfWork.complete());
        }

        [Authorize(Roles ="Driver")]
        [HttpPut("AddFavArea")]
        public async Task<IActionResult> AddNewArea([FromBody] string area)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await unitOfWork.drivers.AddNewFavArea(area, driverId);
            return Ok(unitOfWork.complete());

        }

        [Authorize(Roles = "Driver")]
        [HttpDelete("DeleteFavArea")]
        public async Task<IActionResult> DeleteArea(string area)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await unitOfWork.drivers.DeleteArea(area, driverId);
            if (!res)
                return BadRequest("Invalid Area [ please check that you hava this area in your fav areas ]");
            
            unitOfWork.complete();
            return Ok($"{area} deleted successfully from your fav areas ");

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await unitOfWork.drivers.DeleteAsync(id);
            await service.DeleteUser(res.UserName);
            unitOfWork.complete();
            return Ok();
        }

        [Authorize(Roles ="Driver")]
        [HttpGet("ShowNotification")]
        
        public async Task<IActionResult> ShowNotification()
        {
            var Id = User.FindFirstValue( ClaimTypes.NameIdentifier );
            var res = await unitOfWork.drivers.ShowNotificationAsync(Id);

            if (res == null)
                return Ok("No Notification");

            return Ok(res);

        }
        
        [Authorize(Roles ="Driver")]
        [HttpPost("OfferPrice")]
        public async Task<IActionResult> OfferPrice(OfferPriceModel model)
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await unitOfWork.drivers.OfferPrice(model, Id);

            return Ok(res);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("CheckStatus")]
        public async Task<IActionResult> CheckStatus()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var res = await unitOfWork.drivers.CheckStatus(userName);

            if (res == null)
                return Ok("No Trips Accepted");


            return Ok($"You have a trip from {res.Location} to {res.Destination} and this is " +
                $" your passenger info To contact phone[ {res.PassengerPhoneNumber} ] , Email[ {res.PassengerEmail} ] ");

        }

        [Authorize(Roles = "Driver")]
        [HttpGet("ShowMyTrips")]
        public async Task<IActionResult> ShowMyTrips()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await unitOfWork.drivers.ShowTrips(Id);

            if (res.Count() == 0)
                return Ok("You have no trip with us , but we hope to have soon");

            return Ok(res);

        }




    }
}
