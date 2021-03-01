using System;
using AutoMapper;
using InfoTrack.Common.Application.Automapper;
using InfoTrack.Orders.Api.Client;

namespace InfoTrack.Cdd.Application.Models
{
    public class OrderResponse : OrderDto, IMapFrom<OrderDto>
    {
        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        public void Mapping(Profile profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<OrderDto, OrderResponse>();
        }
    }
}
