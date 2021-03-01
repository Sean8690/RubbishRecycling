using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Dtos.Order;
using InfoTrack.Common.Application;
using InfoTrack.Common.Application.Automapper;
using InfoTrack.Platform.Core.Orders.Contracts;
using InfoTrack.Platform.Core.Orders.Contracts.Attributes;

namespace InfoTrack.Cdd.Application.Commands.PersonOrder
{
    /// <summary>
    /// Detailed Person info (Platform.Core Order Request)
    /// </summary>
    [ServiceType(nameof(ServiceIdentifier.CddPersonRiskReport))]
    public class PersonOrderRequestCommand : Request,
        ICommand<PersonListResponseDto>, IOrderRequest,
        IMapFrom<PersonListResponseDto>, IMatches<PersonOrderRequestCommand>
    {
        /// <summary>
        /// Detailed person info (Platform.Core Order Request)
        /// </summary>
        public PersonOrderRequestCommand() { }

        /// <summary>
        /// Detailed person info (Platform.Core Order Request)
        /// </summary>
        public PersonOrderRequestCommand(AmlPersonLookupRequest request)
        {
            if (request != null)
            {
                GivenName = request.GivenName;
                MiddleName = request.MiddleName;
                FamilyName = request.FamilyName;
                DateOfBirth = request.DateOfBirth;
                YearOfBirth = request.YearOfBirth;
                ClientReference = request.ClientReference;
                RetailerReference = request.RetailerReference;
                ServiceIdentifier = request.ServiceIdentifier;
                QuoteId = request.QuoteId;
            }
        }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// QuoteId (optional)
        /// </summary>
        public Guid? QuoteId { get; set; }

        /// <summary>
        /// First name of person to lookup for.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Middle name of person to lookup for.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of person to lookup for.
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Date of Birth of the person to look for.
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Year of Birth of the person to look for.
        /// </summary>
        public string YearOfBirth { get; set; }

        public void Mapping(Profile profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<PersonListResponseDto, PersonOrderRequestCommand>();
        }

        /// <summary>
        /// Shallow "equality" comparer. Ignores QuoteId and all base class properties.
        /// </summary>
        public bool Matches(PersonOrderRequestCommand other) => other != null &&
                   other.ServiceIdentifier == ServiceIdentifier;
    }
}
