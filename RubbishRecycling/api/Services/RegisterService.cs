using RubbishRecyclingAU.ControllerModels;
using RubbishRecyclingAU.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Services
{
    public interface IRegisterService
    {
        Task<ServiceActionResult> RegisterUser(UserDetails userDetails);
    }

    public class RegisterService : IRegisterService
    {
        private readonly RubbishRecyclingContext _ef;

        public RegisterService(RubbishRecyclingContext rubbishRecyclingContext)
        {
            _ef = rubbishRecyclingContext;
        }

        public async Task<ServiceActionResult> RegisterUser(UserDetails userDetails)
        {
            var user = _ef.Users.FirstOrDefault(x => x.Email == userDetails.Email);
            if (user == null)
            {
                _ef.Users.Add(new User
                {
                    Email = userDetails.Email,
                    Password = userDetails.Password,
                    AvatarUrl = userDetails.AvatarUrl,
                    Address = new Address
                    {
                        StreetOrUnitNumber = userDetails.Address?.StreetOrUnitNumber,
                        StreetName = userDetails.Address?.StreetName,
                        Suburb = userDetails.Address?.Suburb,
                        State = userDetails.Address?.State,
                        PostCode = userDetails.Address?.PostCode,
                        Country = userDetails.Address?.Country
                    }
                });
                await _ef.SaveChangesAsync();

                var createdUser = _ef.Users.FirstOrDefault(x => x.Email == userDetails.Email);
                if (createdUser == null)
                {
                    return new ServiceActionResult
                    {
                        CanProceed = false,
                        Message = "Failed to create user"
                    };
                }

                return new ServiceActionResult
                {
                    CanProceed = true,
                    Message = "Sucess"
                };
            }

            return new ServiceActionResult
            {
                CanProceed = false,
                Message = "User Already Exists"
            };
        }
    }
}
