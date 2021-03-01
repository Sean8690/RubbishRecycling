using System;
using System.Linq;
using RubbishRecyclingAU.ControllerModels;
using System.Threading.Tasks;
using RubbishRecyclingAU.Models;

namespace RubbishRecyclingAU.Services
{
    public interface IItemService
    {
        Task<ServiceActionResult> AddItem(ItemDetails itemDetails);
    }

    public class ItemService : IItemService
    {
        private readonly RubbishRecyclingContext _ef;

        public ItemService(RubbishRecyclingContext rubbishRecyclingContext)
        {
            _ef = rubbishRecyclingContext;
        }

        public async Task<ServiceActionResult> AddItem(ItemDetails itemDetails)
        {
            try
            {
                if (itemDetails == null) throw new Exception("Invalid item details");

                _ef.Products.Add(new Product
                {
                    Description = itemDetails?.Description,
                    Name = itemDetails?.Name,
                    ImageUrl = itemDetails?.ImageUrl,
                    Address = new Address
                    {
                        StreetOrUnitNumber = itemDetails.Address?.StreetOrUnitNumber,
                        StreetName = itemDetails.Address?.StreetName,
                        Suburb = itemDetails.Address?.Suburb,
                        State = itemDetails.Address?.State,
                        PostCode = itemDetails.Address?.PostCode,
                        Country = itemDetails.Address?.Country
                    },
                    isExchanged = false,
                    User = new User()
                });

                await _ef.SaveChangesAsync();

                var createdItem = _ef.Products.FirstOrDefault(x => x.Name == itemDetails.Name);
                if (createdItem == null)
                {
                    return new ServiceActionResult
                    {
                        CanProceed = false,
                        Message = "Failed to create item"
                    };
                }
                return new ServiceActionResult
                {
                    CanProceed = true,
                    Message = "Sucess"

                };
            }
            catch (Exception ex)
            {
                return new ServiceActionResult
                {
                    CanProceed = false,
                    Message = ex.Message
                };
            }
        }
    }
}
