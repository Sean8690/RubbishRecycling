using System;
using System.Collections.Generic;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Common.Application.Automapper;

namespace InfoTrack.Cdd.Application.Dtos.Order
{
    /// <summary>
    /// Detailed person info
    /// </summary>
    public class PersonListResponseDto : OrderDto,
        IMapFrom<IOrder>,
        IMapFrom<PersonOrderRequestCommand>
    {
        public List<PersonLite> Matches = new List<PersonLite>();

#pragma warning disable 1591
        public void Mapping(Profile profile)
#pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<PersonOrderRequestCommand, PersonListResponseDto>();

            profile.CreateMap<IOrder, PersonListResponseDto>();
        }
    }
}
