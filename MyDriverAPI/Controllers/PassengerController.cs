using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.Model.PassengersData;
using MyDriverAPI.Services.AuthServices;
using MyDriverAPI.Services.TripServices;
using System.Security.Claims;

namespace MyDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthService service;
        private readonly ITripService tripService;

        public PassengerController(IUnitOfWork unitOfWork , IAuthService service ,ITripService tripService)
        {
            this.unitOfWork = unitOfWork;
            this.service = service;
           this.tripService = tripService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await unitOfWork.passengers.GetAllAsync());
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await unitOfWork.passengers.GetByIdAsync(id));
        }

        [HttpGet("GetWithInclude")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await unitOfWork.passengers.GetOneWithInclude(d => d.UserName == name, new[] { "null" }));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Passenger newPassenger)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await unitOfWork.passengers.AddAsync(newPassenger);
            return Ok(unitOfWork.complete());
        }

        [Authorize(Roles ="Passenger")]
        [HttpGet("RequestTrip")]
        public async Task<IActionResult> RequestTripAsync(string Location , string Destination)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await tripService.requestTrip(Location, Destination,userId);
            return Ok("Trip Is Requested please wait for drivers offers");
            
        }
        
        [Authorize(Roles = "Passenger")]
        [HttpGet("ShowOffers")]
        public async Task<IActionResult> ShowOffers()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var res = await unitOfWork.passengers.ShowOffers(userName);

            if(res != null)
                return Ok(res);
            return Ok("No Offers Form Drivers Till Now");

        }

        [Authorize(Roles ="Passenger")]
        [HttpGet("ChooseOffer")]
        public async Task<IActionResult> ChooseOffer(int offerNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var res = await unitOfWork.passengers.ChooseOffer(offerNumber, userId);

            if (res == null)
                return Ok("Please enter valid offer number");

            return Ok($"Your Driver is on way for you . you can contact with him by this info " +
                $" phone Number[ {res.PhoneNumber} ]  , Email:[ {res.Email} ]");
        }

        [Authorize(Roles = "Passenger")]
        [HttpGet("ShowMyTrips")]
        public async Task<IActionResult> ShowMyTrips()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await unitOfWork.passengers.ShowTrips(Id);

            if (res.Count()==0)
                return Ok("You have no trip with us , but we hope to have soon");

            return Ok(res);

        }

        [Authorize(Roles = "Passenger")]
        [HttpGet("RateDriver")]
        public async Task<IActionResult> RateDriver(int rating)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var res =await unitOfWork.passengers.RateDriver(name , rating);

            if (!res)
                return Ok("Please enter number betweed 0 and 5 ");

            return Ok("Thank You for helping us to improve our services");

        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(PassengerUpdateModel newPassenger)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await service.UpdatePassenger(newPassenger, UserId);

            if (!string.IsNullOrEmpty(res.Message))
                return BadRequest(res.Message);

            await unitOfWork.passengers.UpdatePassenger(newPassenger, UserId);
            return Ok(unitOfWork.complete());
        }
    
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await unitOfWork.passengers.DeleteAsync(id);
            await service.DeleteUser(res.UserName);
            unitOfWork.complete();
            return Ok();
        }

    }
}
