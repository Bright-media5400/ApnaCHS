using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ApnaCHS.Entities;
using ApnaCHS.Web.Models;
using ApnaCHS.Common;

namespace ApnaCHS.Web
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.maxAttempts, opts => opts.MapFrom(src => src.MaxAttempts))
                    .ForMember(dest => dest.bBlocked, opts => opts.MapFrom(src => src.bBlocked))
                    .ForMember(dest => dest.bChangePass, opts => opts.MapFrom(src => src.bChangePass))
                    .ForMember(dest => dest.bDeleted, opts => opts.MapFrom(src => src.Deleted))
                    .ForMember(dest => dest.userName, opts => opts.MapFrom(src => src.UserName))
                    .ReverseMap();

                cfg.CreateMap<ApplicationUser, ApplicationUserTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.maxAttempts, opts => opts.MapFrom(src => src.MaxAttempts))
                    .ForMember(dest => dest.bBlocked, opts => opts.MapFrom(src => src.bBlocked))
                    .ForMember(dest => dest.bChangePass, opts => opts.MapFrom(src => src.bChangePass))
                    .ForMember(dest => dest.userName, opts => opts.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.bDeleted, opts => opts.MapFrom(src => src.Deleted))
                    .ReverseMap();

                cfg.CreateMap<ApplicationUser, ApplicationUserTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.maxAttempts, opts => opts.MapFrom(src => src.MaxAttempts))
                    .ForMember(dest => dest.bBlocked, opts => opts.MapFrom(src => src.bBlocked))
                    .ForMember(dest => dest.bChangePass, opts => opts.MapFrom(src => src.bChangePass))
                    .ForMember(dest => dest.userName, opts => opts.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.bDeleted, opts => opts.MapFrom(src => src.Deleted))
                    .ReverseMap();

                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>()
                 .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                 .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                 .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                 .ForMember(dest => dest.canChange, opts => opts.MapFrom(src => src.CanChange))
                 .ReverseMap();

                cfg.CreateMap<ApplicationRole, ApplicationRoleTrimViewModel>()
                 .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                 .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                 .ReverseMap();

                cfg.CreateMap<ApplicationRoleSearchParams, ApplicationRoleSearchParamViewModel>()
                 .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                 .ForMember(dest => dest.complexId, opts => opts.MapFrom(src => src.ComplexId))
                 .ReverseMap();

                cfg.CreateMap<Instance, InstanceViewModel>()
                 .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                 .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                 .ReverseMap();

                cfg.CreateMap<MasterValue, MasterValueViewModel>()
                 .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                 .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                 .ForMember(dest => dest.description, opts => opts.MapFrom(src => src.Description))
                 .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                  .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                  .ForMember(dest => dest.customFields, opts => opts.MapFrom(src => src.CustomFields))
                 .ReverseMap();

                cfg.CreateMap<Society, SocietyViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                    .ForMember(dest => dest.dateOfIncorporation, opts => opts.MapFrom(src => src.DateOfIncorporation))
                    .ForMember(dest => dest.dateOfRegistration, opts => opts.MapFrom(src => src.DateOfRegistration))
                    .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                    .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                    .ForMember(dest => dest.billingCycle, opts => opts.MapFrom(src => src.BillingCycle))
                    .ForMember(dest => dest.dueDays, opts => opts.MapFrom(src => src.DueDays))
                    .ForMember(dest => dest.second2Wheeler, opts => opts.MapFrom(src => src.Second2Wheeler))
                    .ForMember(dest => dest.second4Wheeler, opts => opts.MapFrom(src => src.Second4Wheeler))
                    .ForMember(dest => dest.interestPercent, opts => opts.MapFrom(src => src.InterestPercent))
                    .ForMember(dest => dest.complex, opts => opts.MapFrom(src => src.Complex))
                    .ForMember(dest => dest.approvalsCount, opts => opts.MapFrom(src => src.ApprovalsCount))
                    .ForMember(dest => dest.societyAssets, opts => opts.MapFrom(src => src.SocietyAssets))
                    .ForMember(dest => dest.openingInterest, opts => opts.MapFrom(src => src.OpeningInterest))
                    .ReverseMap();


                cfg.CreateMap<Society, SocietyTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                    .ForMember(dest => dest.dateOfIncorporation, opts => opts.MapFrom(src => src.DateOfIncorporation))
                    .ForMember(dest => dest.dateOfRegistration, opts => opts.MapFrom(src => src.DateOfRegistration))
                    .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                    .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                    .ForMember(dest => dest.billingCycle, opts => opts.MapFrom(src => src.BillingCycle))
                    .ForMember(dest => dest.dueDays, opts => opts.MapFrom(src => src.DueDays))
                    .ForMember(dest => dest.second2Wheeler, opts => opts.MapFrom(src => src.Second2Wheeler))
                    .ForMember(dest => dest.second4Wheeler, opts => opts.MapFrom(src => src.Second4Wheeler))
                    .ForMember(dest => dest.interestPercent, opts => opts.MapFrom(src => src.InterestPercent))
                    .ForMember(dest => dest.approvalsCount, opts => opts.MapFrom(src => src.ApprovalsCount))
                    .ForMember(dest => dest.openingInterest, opts => opts.MapFrom(src => src.OpeningInterest))
                    .ReverseMap();

                cfg.CreateMap<SocietyImportResult, SocietyImportResultViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.result, opts => opts.MapFrom(src => src.Result))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ReverseMap();

                cfg.CreateMap<MapUserToSociety, MapUserToSocietyViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Society.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Society.Name))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.Society.RegistrationNo))
                    .ForMember(dest => dest.city, opts => opts.MapFrom(src => src.Society.Complex.City.Text))
                    .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Society.Complex.Address))
                    .ForMember(dest => dest.group, opts => opts.MapFrom(src => src.Role.Name))
                    .ReverseMap();

                cfg.CreateMap<MapUserToComplex, MapUserToComplexViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Complex.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Complex.Name))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.Complex.RegistrationNo))
                    .ForMember(dest => dest.city, opts => opts.MapFrom(src => src.Complex.City.Text))
                    .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Complex.Address))
                    .ForMember(dest => dest.group, opts => opts.MapFrom(src => src.Role.Name))
                    .ReverseMap();

                cfg.CreateMap<SocietySearchParams, SocietySearchParamViewModel>()
                    .ForMember(dest => dest.complexId, opts => opts.MapFrom(src => src.ComplexId))
                    .ReverseMap();

                cfg.CreateMap<Facility, FacilityViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.noOfFloors, opts => opts.MapFrom(src => src.NoOfFloors))
                    .ForMember(dest => dest.noOfFlats, opts => opts.MapFrom(src => src.NoOfFlats))
                    .ForMember(dest => dest.noOfLifts, opts => opts.MapFrom(src => src.NoOfLifts))
                    .ForMember(dest => dest.noOfParkinglots, opts => opts.MapFrom(src => src.NoOfParkinglots))
                    .ForMember(dest => dest.isParkingLot, opts => opts.MapFrom(src => src.IsParkingLot))
                    .ForMember(dest => dest.complex, opts => opts.MapFrom(src => src.Complex))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ForMember(dest => dest.societies, opts => opts.MapFrom(src => src.Societies))
                    .ForMember(dest => dest.floors, opts => opts.MapFrom(src => src.Floors))
                    .ForMember(dest => dest.flatParkings, opts => opts.MapFrom(src => src.FlatParkings))
                    .ForMember(dest => dest.societyAssets, opts => opts.MapFrom(src => src.SocietyAssets))
                    .ReverseMap();

                cfg.CreateMap<Facility, FacilityTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.noOfFloors, opts => opts.MapFrom(src => src.NoOfFloors))
                    .ForMember(dest => dest.noOfFlats, opts => opts.MapFrom(src => src.NoOfFlats))
                    .ForMember(dest => dest.noOfLifts, opts => opts.MapFrom(src => src.NoOfLifts))
                    .ForMember(dest => dest.noOfParkinglots, opts => opts.MapFrom(src => src.NoOfParkinglots))
                    .ForMember(dest => dest.isParkingLot, opts => opts.MapFrom(src => src.IsParkingLot))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ReverseMap();

                cfg.CreateMap<Facility, FacilityForFlatViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.noOfFloors, opts => opts.MapFrom(src => src.NoOfFloors))
                    .ForMember(dest => dest.noOfFlats, opts => opts.MapFrom(src => src.NoOfFlats))
                    .ForMember(dest => dest.noOfLifts, opts => opts.MapFrom(src => src.NoOfLifts))
                    .ForMember(dest => dest.noOfParkinglots, opts => opts.MapFrom(src => src.NoOfParkinglots))
                    .ForMember(dest => dest.isParkingLot, opts => opts.MapFrom(src => src.IsParkingLot))
                    .ForMember(dest => dest.floors, opts => opts.MapFrom(src => src.Floors))
                    .ReverseMap();

                cfg.CreateMap<Facility, FacilityForFlatParkingsViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.noOfFloors, opts => opts.MapFrom(src => src.NoOfFloors))
                    .ForMember(dest => dest.noOfFlats, opts => opts.MapFrom(src => src.NoOfFlats))
                    .ForMember(dest => dest.noOfLifts, opts => opts.MapFrom(src => src.NoOfLifts))
                    .ForMember(dest => dest.noOfParkinglots, opts => opts.MapFrom(src => src.NoOfParkinglots))
                    .ForMember(dest => dest.isParkingLot, opts => opts.MapFrom(src => src.IsParkingLot))
                    .ForMember(dest => dest.floors, opts => opts.MapFrom(src => src.Floors))
                    .ForMember(dest => dest.flatParkings, opts => opts.MapFrom(src => src.FlatParkings))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ReverseMap();

                cfg.CreateMap<FacilitySearchParams, FacilitySearchParamViewModel>()
                    .ForMember(dest => dest.complexId, opts => opts.MapFrom(src => src.ComplexId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ReverseMap();

                cfg.CreateMap<MapSocietiesToFacilities, MapSocietiesToFacilitiesViewModel>()
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.facility, opts => opts.MapFrom(src => src.Facility))
                    .ReverseMap();

                cfg.CreateMap<Floor, FloorViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.facility, opts => opts.MapFrom(src => src.Facility))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ForMember(dest => dest.flats, opts => opts.MapFrom(src => src.Flats))
                    .ForMember(dest => dest.flatParkings, opts => opts.MapFrom(src => src.FlatParkings))
                    .ReverseMap();

                cfg.CreateMap<Floor, FloorForFlatsViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ForMember(dest => dest.flats, opts => opts.MapFrom(src => src.Flats))
                    .ReverseMap();

                cfg.CreateMap<Floor, FloorForFlatParkingsViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ForMember(dest => dest.flatParkings, opts => opts.MapFrom(src => src.FlatParkings))
                    .ReverseMap();

                cfg.CreateMap<Floor, FloorTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.facilityId, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ReverseMap();

                cfg.CreateMap<FloorSearchParams, FloorSearchParamViewModel>()
                    .ForMember(dest => dest.facilityId, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ReverseMap();

                cfg.CreateMap<Flat, FlatViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.totalArea, opts => opts.MapFrom(src => src.TotalArea))
                    .ForMember(dest => dest.carpetArea, opts => opts.MapFrom(src => src.CarpetArea))
                    .ForMember(dest => dest.buildUpArea, opts => opts.MapFrom(src => src.BuildUpArea))
                    .ForMember(dest => dest.haveParking, opts => opts.MapFrom(src => src.HaveParking))
                    .ForMember(dest => dest.isRented, opts => opts.MapFrom(src => src.IsRented))
                    .ForMember(dest => dest.isCommercialSpace, opts => opts.MapFrom(src => src.IsCommercialSpace))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.flatOwners, opts => opts.MapFrom(src => src.FlatOwners))
                    .ForMember(dest => dest.flatParkings, opts => opts.MapFrom(src => src.FlatParkings))
                    .ForMember(dest => dest.flatType, opts => opts.MapFrom(src => src.FlatType))
                    .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ReverseMap();

                cfg.CreateMap<Flat, FlatTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.totalArea, opts => opts.MapFrom(src => src.TotalArea))
                    .ForMember(dest => dest.carpetArea, opts => opts.MapFrom(src => src.CarpetArea))
                    .ForMember(dest => dest.buildUpArea, opts => opts.MapFrom(src => src.BuildUpArea))
                    .ForMember(dest => dest.haveParking, opts => opts.MapFrom(src => src.HaveParking))
                    .ForMember(dest => dest.isRented, opts => opts.MapFrom(src => src.IsRented))
                    .ForMember(dest => dest.isCommercialSpace, opts => opts.MapFrom(src => src.IsCommercialSpace))
                    .ForMember(dest => dest.floorId, opts => opts.MapFrom(src => src.FloorId))
                    .ForMember(dest => dest.flatTypeId, opts => opts.MapFrom(src => src.FlatTypeId))
                    .ReverseMap();

                cfg.CreateMap<FlatSearchParams, FlatSearchParamViewModel>()
                    .ForMember(dest => dest.floorId, opts => opts.MapFrom(src => src.FloorId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.facilityId, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.flatName, opts => opts.MapFrom(src => src.FlatName))
                    .ForMember(dest => dest.owner, opts => opts.MapFrom(src => src.Owner))
                    .ForMember(dest => dest.tenant, opts => opts.MapFrom(src => src.Tenant))
                    .ForMember(dest => dest.username, opts => opts.MapFrom(src => src.Username))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ReverseMap();

                cfg.CreateMap<FlatReportResult, FlatReportResultViewModel>()
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.building, opts => opts.MapFrom(src => src.Building))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.buildingId, opts => opts.MapFrom(src => src.BuildingId))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.floorId, opts => opts.MapFrom(src => src.FloorId))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.currentOwner, opts => opts.MapFrom(src => src.CurrentOwner))
                    .ForMember(dest => dest.currentOwnerId, opts => opts.MapFrom(src => src.CurrentOwnerId))
                    .ForMember(dest => dest.tenantList, opts => opts.MapFrom(src => src.TenantList))
                    .ForMember(dest => dest.isCommercialSpace, opts => opts.MapFrom(src => src.IsCommercialSpace))
                    .ForMember(dest => dest.typeName, opts => opts.MapFrom(src => src.TypeName))
                    .ForMember(dest => dest.totalArea, opts => opts.MapFrom(src => src.TotalArea))
                    .ForMember(dest => dest.currentOwnerType, opts => opts.MapFrom(src => src.CurrentOwnerType))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                    .ReverseMap();


                cfg.CreateMap<TenantResult, TenantResultViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name));

                cfg.CreateMap<FlatOwner, FlatOwnerViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.emailId, opts => opts.MapFrom(src => src.EmailId))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.flats, opts => opts.MapFrom(src => src.Flats))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.vehicles, opts => opts.MapFrom(src => src.Vehicles))
                    .ForMember(dest => dest.flatOwnerFamilies, opts => opts.MapFrom(src => src.FlatOwnerFamilies))
                    .ForMember(dest => dest.approvals, opts => opts.MapFrom(src => src.Approvals))
                    .ForMember(dest => dest.comments, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                    .ReverseMap();

                cfg.CreateMap<FlatOwner, FlatOwnerTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.emailId, opts => opts.MapFrom(src => src.EmailId))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ReverseMap();

                cfg.CreateMap<FlatOwnerSearchParams, FlatOwnerSearchParamViewModel>()
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.flatOwnerType, opts => opts.MapFrom(src => src.FlatOwnerType))
                    .ReverseMap();

                cfg.CreateMap<FlatOwnerFamily, FlatOwnerFamilyViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.adminMember, opts => opts.MapFrom(src => src.AdminMember))
                    .ForMember(dest => dest.approverMember, opts => opts.MapFrom(src => src.ApproverMember))
                    .ForMember(dest => dest.flatOwner, opts => opts.MapFrom(src => src.FlatOwner))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.relationship, opts => opts.MapFrom(src => src.Relationship))
                    .ForMember(dest => dest.flatOwnerId, opts => opts.MapFrom(src => src.FlatOwnerId))
                    .ForMember(dest => dest.approvals, opts => opts.MapFrom(src => src.Approvals))
                    .ForMember(dest => dest.comments, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                    .ReverseMap();

                cfg.CreateMap<FlatOwnerFamily, FlatOwnerFamilyTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.adminMember, opts => opts.MapFrom(src => src.AdminMember))
                    .ForMember(dest => dest.approverMember, opts => opts.MapFrom(src => src.ApproverMember))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.relationship, opts => opts.MapFrom(src => src.Relationship))
                    .ForMember(dest => dest.flatOwnerId, opts => opts.MapFrom(src => src.FlatOwnerId))
                    .ReverseMap();

                cfg.CreateMap<FlatOwnerFamilySearchParams, FlatOwnerFamilySearchParamViewModel>()
                    .ForMember(dest => dest.flatOwnerId, opts => opts.MapFrom(src => src.FlatOwnerId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ReverseMap();

                cfg.CreateMap<MapFlatToFlatOwner, MapFlatToFlatOwnerViewModel>()
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.flatOwner, opts => opts.MapFrom(src => src.FlatOwner))
                    .ForMember(dest => dest.memberSinceDate, opts => opts.MapFrom(src => src.MemberSinceDate))
                    .ForMember(dest => dest.memberTillDate, opts => opts.MapFrom(src => src.MemberTillDate))
                    .ForMember(dest => dest.flatOwnerType, opts => opts.MapFrom(src => src.FlatOwnerType))
                    .ReverseMap();

                cfg.CreateMap<SecurityStaff, SecurityStaffViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.aadharCardNo, opts => opts.MapFrom(src => src.AadharCardNo))
                .ForMember(dest => dest.photo, opts => opts.MapFrom(src => src.Photo))
                .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.nativeAddress, opts => opts.MapFrom(src => src.NativeAddress))
                .ForMember(dest => dest.joiningDate, opts => opts.MapFrom(src => src.JoiningDate))
                .ForMember(dest => dest.lastWorkingDay, opts => opts.MapFrom(src => src.LastWorkingDay))
                .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                .ForMember(dest => dest.shiftType, opts => opts.MapFrom(src => src.ShiftType))
                .ReverseMap();

                cfg.CreateMap<SecurityStaffSearchParams, SecurityStaffSearchParamViewModel>()
                .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                .ReverseMap();

                cfg.CreateMap<SocietyStaff, SocietyStaffViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.aadharCardNo, opts => opts.MapFrom(src => src.AadharCardNo))
                .ForMember(dest => dest.photo, opts => opts.MapFrom(src => src.Photo))
                .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.nativeAddress, opts => opts.MapFrom(src => src.NativeAddress))
                .ForMember(dest => dest.joiningDate, opts => opts.MapFrom(src => src.JoiningDate))
                .ForMember(dest => dest.lastWorkingDay, opts => opts.MapFrom(src => src.LastWorkingDay))
                .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                .ForMember(dest => dest.staffType, opts => opts.MapFrom(src => src.StaffType))
                .ReverseMap();

                cfg.CreateMap<SocietyStaffSearchParams, SocietyStaffSearchParamViewModel>()
                .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                .ReverseMap();

                cfg.CreateMap<SocietyDocument, SocietyDocumentViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.filePath, opts => opts.MapFrom(src => src.FilePath))
                .ReverseMap();

                cfg.CreateMap<SocietyDocumentSearchParams, SocietyDocumentSearchParamViewModel>()
                .ReverseMap();

                cfg.CreateMap<FlatParking, FlatParkingViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.sqftArea, opts => opts.MapFrom(src => src.SqftArea))
                .ForMember(dest => dest.facility, opts => opts.MapFrom(src => src.Facility))
                .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                .ForMember(dest => dest.parkingNo, opts => opts.MapFrom(src => src.ParkingNo))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ReverseMap();

                cfg.CreateMap<FlatParking, FlatParkingTrimViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.sqftArea, opts => opts.MapFrom(src => src.SqftArea))
                .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ForMember(dest => dest.parkingNo, opts => opts.MapFrom(src => src.ParkingNo))
                .ReverseMap();

                cfg.CreateMap<FlatParkingSearchParams, FlatParkingSearchParamViewModel>()
                .ReverseMap();

                cfg.CreateMap<Vehicle, VehicleViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.make, opts => opts.MapFrom(src => src.Make))
                .ForMember(dest => dest.number, opts => opts.MapFrom(src => src.Number))
                .ForMember(dest => dest.flatOwner, opts => opts.MapFrom(src => src.FlatOwner))
                .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ForMember(dest => dest.approvals, opts => opts.MapFrom(src => src.Approvals))
                .ForMember(dest => dest.comments, opts => opts.MapFrom(src => src.Comments))
                .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                .ReverseMap();

                cfg.CreateMap<Vehicle, VehicleTrimViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.make, opts => opts.MapFrom(src => src.Make))
                .ForMember(dest => dest.number, opts => opts.MapFrom(src => src.Number))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ReverseMap();

                cfg.CreateMap<VehicleSearchParams, VehicleSearchParamViewModel>()
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.flatOwnerId, opts => opts.MapFrom(src => src.FlatOwnerId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ReverseMap();

                cfg.CreateMap<Attendance, AttendanceViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.day, opts => opts.MapFrom(src => src.Day))
                    .ForMember(dest => dest.inTime, opts => opts.MapFrom(src => src.InTime))
                    .ForMember(dest => dest.outTime, opts => opts.MapFrom(src => src.OutTime))
                    .ForMember(dest => dest.securityStaff, opts => opts.MapFrom(src => src.SecurityStaff))
                    .ForMember(dest => dest.societyStaff, opts => opts.MapFrom(src => src.SocietyStaff))
                    .ForMember(dest => dest.shiftType, opts => opts.MapFrom(src => src.ShiftType))
                    .ReverseMap();

                cfg.CreateMap<AttendanceSearchParams, AttendanceSearchParamViewModel>()
                .ReverseMap();


                cfg.CreateMap<MaintenanceCost, MaintenanceCostViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.definition, opts => opts.MapFrom(src => src.Definition))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.date, opts => opts.MapFrom(src => src.Date))
                    .ForMember(dest => dest.fromDate, opts => opts.MapFrom(src => src.FromDate))
                    .ForMember(dest => dest.toDate, opts => opts.MapFrom(src => src.ToDate))
                    .ForMember(dest => dest.perSqrArea, opts => opts.MapFrom(src => src.PerSqrArea))
                    .ForMember(dest => dest.allFlats, opts => opts.MapFrom(src => src.AllFlats))
                    .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.flats, opts => opts.MapFrom(src => src.Flats))
                    .ForMember(dest => dest.comments, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.approvals, opts => opts.MapFrom(src => src.Approvals))
                .ReverseMap();

                cfg.CreateMap<MaintenanceCostSearchParams, MaintenanceCostSearchParamViewModel>()
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isDeleted, opts => opts.MapFrom(src => src.IsDeleted))
                    .ForMember(dest => dest.isActive, opts => opts.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ReverseMap();

                cfg.CreateMap<MaintenanceReceiptSearchParams, MaintenanceReceiptSearchParamViewModel>()
                    .ReverseMap();

                cfg.CreateMap<MaintenanceReceiptDetailSearchParams, MaintenanceReceiptDetailSearchParamViewModel>()
                    .ReverseMap();

                cfg.CreateMap<Complex, ComplexViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.area, opts => opts.MapFrom(src => src.Area))
                .ForMember(dest => dest.city, opts => opts.MapFrom(src => src.City))
                .ForMember(dest => dest.state, opts => opts.MapFrom(src => src.State))
                .ForMember(dest => dest.dateOfIncorporation, opts => opts.MapFrom(src => src.DateOfIncorporation))
                .ForMember(dest => dest.dateOfRegistration, opts => opts.MapFrom(src => src.DateOfRegistration))
                .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.noOfSocieties, opts => opts.MapFrom(src => src.NoOfSocieties))
                .ForMember(dest => dest.noOfBuilding, opts => opts.MapFrom(src => src.NoOfBuilding))
                .ForMember(dest => dest.noOfGate, opts => opts.MapFrom(src => src.NoOfGate))
                .ForMember(dest => dest.noOfAmenities, opts => opts.MapFrom(src => src.NoOfAmenities))
                .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.facilities, opts => opts.MapFrom(src => src.Facilities))
                .ForMember(dest => dest.societies, opts => opts.MapFrom(src => src.Societies))
                .ForMember(dest => dest.societyAssets, opts => opts.MapFrom(src => src.SocietyAssets))
                .ForMember(dest => dest.pincode, opts => opts.MapFrom(src => src.Pincode))
                .ReverseMap();

                cfg.CreateMap<Complex, ComplexTrimViewModel>()
                .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.area, opts => opts.MapFrom(src => src.Area))
                .ForMember(dest => dest.city, opts => opts.MapFrom(src => src.City))
                .ForMember(dest => dest.state, opts => opts.MapFrom(src => src.State))
                .ForMember(dest => dest.dateOfIncorporation, opts => opts.MapFrom(src => src.DateOfIncorporation))
                .ForMember(dest => dest.dateOfRegistration, opts => opts.MapFrom(src => src.DateOfRegistration))
                .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.noOfSocieties, opts => opts.MapFrom(src => src.NoOfSocieties))
                .ForMember(dest => dest.noOfBuilding, opts => opts.MapFrom(src => src.NoOfBuilding))
                .ForMember(dest => dest.noOfGate, opts => opts.MapFrom(src => src.NoOfGate))
                .ForMember(dest => dest.noOfAmenities, opts => opts.MapFrom(src => src.NoOfAmenities))
                .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.phoneNo, opts => opts.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                .ReverseMap();

                cfg.CreateMap<ComplexSearchParams, ComplexSearchParamViewModel>()
                    .ReverseMap();

                cfg.CreateMap<Bill, BillingViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.receiptNo, opts => opts.MapFrom(src => src.ReceiptNo))
                    .ForMember(dest => dest.date, opts => opts.MapFrom(src => src.Date))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.month, opts => opts.MapFrom(src => src.Month))
                    .ForMember(dest => dest.year, opts => opts.MapFrom(src => src.Year))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.monthlyAmount, opts => opts.MapFrom(src => src.MonthlyAmount))
                    .ForMember(dest => dest.pending, opts => opts.MapFrom(src => src.Pending))
                    .ForMember(dest => dest.billType, opts => opts.MapFrom(src => src.BillType))
                    .ForMember(dest => dest.note, opts => opts.MapFrom(src => src.Note))
                    .ReverseMap();

                cfg.CreateMap<BillingSearchParams, BillingSearchParamViewModel>()
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.year, opts => opts.MapFrom(src => src.Year))
                    .ForMember(dest => dest.month, opts => opts.MapFrom(src => src.Month))
                    .ForMember(dest => dest.facility, opts => opts.MapFrom(src => src.Facility))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ReverseMap();

                cfg.CreateMap<BillingTransaction, BillingTransactionViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.transactionNo, opts => opts.MapFrom(src => src.TransactionNo))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.date, opts => opts.MapFrom(src => src.Date))
                    .ForMember(dest => dest.reference, opts => opts.MapFrom(src => src.Reference))
                    .ForMember(dest => dest.mode, opts => opts.MapFrom(src => src.Mode))
                    .ForMember(dest => dest.authorizedBy, opts => opts.MapFrom(src => src.AuthorizedBy))
                    .ForMember(dest => dest.description, opts => opts.MapFrom(src => src.Description))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.bank, opts => opts.MapFrom(src => src.Bank))
                    .ForMember(dest => dest.branch, opts => opts.MapFrom(src => src.Branch))
                    .ForMember(dest => dest.chequeNo, opts => opts.MapFrom(src => src.ChequeNo))
                    .ForMember(dest => dest.chequeDate, opts => opts.MapFrom(src => src.ChequeDate))
                    .ReverseMap();

                cfg.CreateMap<BillingTransactionSearchParams, BillingTransactionSearchParamViewModel>()
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.username, opts => opts.MapFrom(src => src.Username))
                    .ForMember(dest => dest.year, opts => opts.MapFrom(src => src.Year))
                    .ForMember(dest => dest.month, opts => opts.MapFrom(src => src.Month))
                    .ReverseMap();

                cfg.CreateMap<MaintenanceCostDefinition, MaintenanceCostDefinitionViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.calculationOnPerSftArea, opts => opts.MapFrom(src => src.CalculationOnPerSftArea))
                    .ForMember(dest => dest.for2Wheeler, opts => opts.MapFrom(src => src.For2Wheeler))
                    .ForMember(dest => dest.for4Wheeler, opts => opts.MapFrom(src => src.For4Wheeler))
                    .ForMember(dest => dest.facilityType, opts => opts.MapFrom(src => src.FacilityType))
                    .ReverseMap();

                cfg.CreateMap<MaintenanceCostDefinitionSearchParams, MaintenanceCostDefinitionSearchParamViewModel>()
                   .ReverseMap();

                cfg.CreateMap<SocietyAsset, SocietyAssetViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.isUsable, opts => opts.MapFrom(src => src.IsUsable))
                    .ForMember(dest => dest.isOperational, opts => opts.MapFrom(src => src.IsOperational))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.facility, opts => opts.MapFrom(src => src.Facility))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.complex, opts => opts.MapFrom(src => src.Complex))
                    .ForMember(dest => dest.quantity, opts => opts.MapFrom(src => src.Quantity))
                    .ForMember(dest => dest.companyName, opts => opts.MapFrom(src => src.CompanyName))
                    .ForMember(dest => dest.brand, opts => opts.MapFrom(src => src.Brand))
                    .ForMember(dest => dest.purchaseDate, opts => opts.MapFrom(src => src.PurchaseDate))
                    .ForMember(dest => dest.modelNo, opts => opts.MapFrom(src => src.ModelNo))
                    .ForMember(dest => dest.srNo, opts => opts.MapFrom(src => src.SrNo))
                    .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                    .ForMember(dest => dest.customerCareNo, opts => opts.MapFrom(src => src.CustomerCareNo))
                    .ReverseMap();

                cfg.CreateMap<SocietyAsset, SocietyAssetTrimViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.isUsable, opts => opts.MapFrom(src => src.IsUsable))
                    .ForMember(dest => dest.isOperational, opts => opts.MapFrom(src => src.IsOperational))
                    .ForMember(dest => dest.quantity, opts => opts.MapFrom(src => src.Quantity))
                    .ForMember(dest => dest.companyName, opts => opts.MapFrom(src => src.CompanyName))
                    .ForMember(dest => dest.brand, opts => opts.MapFrom(src => src.Brand))
                    .ForMember(dest => dest.purchaseDate, opts => opts.MapFrom(src => src.PurchaseDate))
                    .ForMember(dest => dest.modelNo, opts => opts.MapFrom(src => src.ModelNo))
                    .ForMember(dest => dest.srNo, opts => opts.MapFrom(src => src.SrNo))
                    .ForMember(dest => dest.contactPerson, opts => opts.MapFrom(src => src.ContactPerson))
                    .ForMember(dest => dest.customerCareNo, opts => opts.MapFrom(src => src.CustomerCareNo))
                    .ReverseMap();

                cfg.CreateMap<SocietyAssetSearchParams, SocietyAssetSearchParamViewModel>()
                    .ForMember(dest => dest.facilityId, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                   .ReverseMap();

                cfg.CreateMap<UploadFlatOwner, UploadFlatOwnerViewModel>()
                    .ForMember(dest => dest.srno, opts => opts.MapFrom(src => src.SrNo))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.building, opts => opts.MapFrom(src => src.Building))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.emailId, opts => opts.MapFrom(src => src.EmailId))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.memberSinceDate, opts => opts.MapFrom(src => src.MemberSinceDate))
                    .ForMember(dest => dest.memberTillDate, opts => opts.MapFrom(src => src.MemberTillDate))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ForMember(dest => dest.message, opts => opts.MapFrom(src => src.Message))
                   .ReverseMap();

                cfg.CreateMap<ReportFlatOwnersTenantsDetail, ReportFlatOwnersTenantsDetailViewModel>()
                    .ForMember(dest => dest.srno, opts => opts.MapFrom(src => src.Srno))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.building, opts => opts.MapFrom(src => src.Building))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.buildingId, opts => opts.MapFrom(src => src.BuildingId))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.floorId, opts => opts.MapFrom(src => src.FloorId))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.owner, opts => opts.MapFrom(src => src.Owner))
                    .ForMember(dest => dest.ownerType, opts => opts.MapFrom(src => src.OwnerType))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.gender, opts => opts.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.relationship, opts => opts.MapFrom(src => src.Relationship))
                    .ForMember(dest => dest.dateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ForMember(dest => dest.message, opts => opts.MapFrom(src => src.Message))
                    .ReverseMap();

                cfg.CreateMap<ReportFlatOwnersVehicleDetail, ReportFlatOwnersVehicleDetailViewModel>()
                    .ForMember(dest => dest.srno, opts => opts.MapFrom(src => src.Srno))
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.building, opts => opts.MapFrom(src => src.Building))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.buildingId, opts => opts.MapFrom(src => src.BuildingId))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.floorId, opts => opts.MapFrom(src => src.FloorId))
                    .ForMember(dest => dest.flat, opts => opts.MapFrom(src => src.Flat))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.flatOwner, opts => opts.MapFrom(src => src.FlatOwner))
                    .ForMember(dest => dest.flatOwnerType, opts => opts.MapFrom(src => src.FlatOwnerType))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.make, opts => opts.MapFrom(src => src.Make))
                    .ForMember(dest => dest.number, opts => opts.MapFrom(src => src.Number))
                    .ForMember(dest => dest.type, opts => opts.MapFrom(src => src.Type))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ForMember(dest => dest.message, opts => opts.MapFrom(src => src.Message))
                    .ReverseMap();

                cfg.CreateMap<UploadFlat, UploadFlatViewModel>()
                    .ForMember(dest => dest.srno, opts => opts.MapFrom(src => src.Srno))
                    .ForMember(dest => dest.registrationNo, opts => opts.MapFrom(src => src.RegistrationNo))
                    .ForMember(dest => dest.society, opts => opts.MapFrom(src => src.Society))
                    .ForMember(dest => dest.building, opts => opts.MapFrom(src => src.Building))
                    .ForMember(dest => dest.wing, opts => opts.MapFrom(src => src.Wing))
                    .ForMember(dest => dest.floor, opts => opts.MapFrom(src => src.Floor))
                    .ForMember(dest => dest.floorType, opts => opts.MapFrom(src => src.FloorType))
                    .ForMember(dest => dest.flatType, opts => opts.MapFrom(src => src.FlatType))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.totalArea, opts => opts.MapFrom(src => src.TotalArea))
                    .ForMember(dest => dest.carpetArea, opts => opts.MapFrom(src => src.CarpetArea))
                    .ForMember(dest => dest.buildUpArea, opts => opts.MapFrom(src => src.BuildUpArea))
                    .ForMember(dest => dest.isCommercialSpace, opts => opts.MapFrom(src => src.IsCommercialSpace))
                    .ForMember(dest => dest.haveParking, opts => opts.MapFrom(src => src.HaveParking))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ForMember(dest => dest.message, opts => opts.MapFrom(src => src.Message))
                    .ReverseMap();


                cfg.CreateMap<CommentMC, CommentMCViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.maintenanceCostId, opts => opts.MapFrom(src => src.MaintenanceCostId))
                    .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                    .ForMember(dest => dest.createdBy, opts => opts.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.createdDate, opts => opts.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.commentBy, opts => opts.MapFrom(src => src.CommentBy))
                    .ReverseMap();

                cfg.CreateMap<CommentFlatOwner, CommentFlatOwnerViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.flatOwnerId, opts => opts.MapFrom(src => src.FlatOwnerId))
                    .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                    .ForMember(dest => dest.createdBy, opts => opts.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.createdDate, opts => opts.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.commentBy, opts => opts.MapFrom(src => src.CommentBy))
                    .ReverseMap();

                cfg.CreateMap<CommentFlat, CommentFlatViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                    .ForMember(dest => dest.createdBy, opts => opts.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.createdDate, opts => opts.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.commentBy, opts => opts.MapFrom(src => src.CommentBy))
                    .ReverseMap();

                cfg.CreateMap<CommentFlatOwnerFamily, CommentFlatOwnerFamilyViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.flatOwnerFamilyId, opts => opts.MapFrom(src => src.FlatOwnerFamilyId))
                    .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                    .ForMember(dest => dest.createdBy, opts => opts.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.createdDate, opts => opts.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.commentBy, opts => opts.MapFrom(src => src.CommentBy))
                    .ReverseMap();

                cfg.CreateMap<CommentVehicle, CommentVehicleViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.vehicleId, opts => opts.MapFrom(src => src.VehicleId))
                    .ForMember(dest => dest.text, opts => opts.MapFrom(src => src.Text))
                    .ForMember(dest => dest.createdBy, opts => opts.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.createdDate, opts => opts.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.commentBy, opts => opts.MapFrom(src => src.CommentBy))
                    .ReverseMap();

                cfg.CreateMap<AllIndiaPincode, AllIndiaPincodeViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.officeName, opts => opts.MapFrom(src => src.OfficeName))
                    .ForMember(dest => dest.pincode, opts => opts.MapFrom(src => src.Pincode))
                    .ForMember(dest => dest.officeType, opts => opts.MapFrom(src => src.OfficeType))
                    .ForMember(dest => dest.deliveryStatus, opts => opts.MapFrom(src => src.DeliveryStatus))
                    .ForMember(dest => dest.divisionName, opts => opts.MapFrom(src => src.DivisionName))
                    .ForMember(dest => dest.regionName, opts => opts.MapFrom(src => src.RegionName))
                    .ForMember(dest => dest.circleName, opts => opts.MapFrom(src => src.CircleName))
                    .ForMember(dest => dest.taluk, opts => opts.MapFrom(src => src.Taluk))
                    .ForMember(dest => dest.districtName, opts => opts.MapFrom(src => src.DistrictName))
                    .ForMember(dest => dest.stateName, opts => opts.MapFrom(src => src.StateName))
                    .ForMember(dest => dest.telephone, opts => opts.MapFrom(src => src.Telephone))
                    .ForMember(dest => dest.relatedSuboffice, opts => opts.MapFrom(src => src.RelatedSuboffice))
                    .ForMember(dest => dest.relatedHeadoffice, opts => opts.MapFrom(src => src.RelatedHeadoffice))
                    .ForMember(dest => dest.longitude, opts => opts.MapFrom(src => src.Longitude))
                    .ForMember(dest => dest.latitude, opts => opts.MapFrom(src => src.Latitude))
                    .ReverseMap();

                cfg.CreateMap<DataApproval, DataApprovalViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.approvedDate, opts => opts.MapFrom(src => src.ApprovedDate))
                    .ForMember(dest => dest.approvedBy, opts => opts.MapFrom(src => src.ApprovedBy))
                    .ForMember(dest => dest.note, opts => opts.MapFrom(src => src.Note))
                    .ForMember(dest => dest.approvedName, opts => opts.MapFrom(src => src.ApprovedName))
                    .ReverseMap();

                cfg.CreateMap<ApprovalReply, ApprovalReplyViewModel>()
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.IsSucces, opts => opts.MapFrom(src => src.IsSucces))
                    .ForMember(dest => dest.Message, opts => opts.MapFrom(src => src.Message))
                    .ReverseMap();

                cfg.CreateMap<UploadReply, UploadReplyViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.isSuccess, opts => opts.MapFrom(src => src.IsSuccess))
                    .ForMember(dest => dest.message, opts => opts.MapFrom(src => src.Message))
                    .ReverseMap();

                cfg.CreateMap<ReportFamilyResult, ReportFamilyResultViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.mobileNo, opts => opts.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.aadhaarCardNo, opts => opts.MapFrom(src => src.AadhaarCardNo))
                    .ForMember(dest => dest.flatId, opts => opts.MapFrom(src => src.FlatId))
                    .ForMember(dest => dest.flatName, opts => opts.MapFrom(src => src.FlatName))
                    .ForMember(dest => dest.genderText, opts => opts.MapFrom(src => src.GenderText))
                    .ForMember(dest => dest.relationshipText, opts => opts.MapFrom(src => src.RelationshipText))
                    .ForMember(dest => dest.flatOwnerName, opts => opts.MapFrom(src => src.FlatOwnerName))
                    .ForMember(dest => dest.deleted, opts => opts.MapFrom(src => src.Deleted))
                    .ForMember(dest => dest.isApproved, opts => opts.MapFrom(src => src.IsApproved))
                    .ForMember(dest => dest.isRejected, opts => opts.MapFrom(src => src.IsRejected))
                    .ForMember(dest => dest.comments, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.approvals, opts => opts.MapFrom(src => src.Approvals))
                    .ReverseMap();

                cfg.CreateMap<ReportFamilySearchParams, ReportFamilySearchParamViewModel>()
                    .ForMember(dest => dest.societyId, opts => opts.MapFrom(src => src.SocietyId))
                    .ReverseMap();

                cfg.CreateMap<PendingBill, PendingBillViewModel>()
                    .ForMember(dest => dest.id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.receiptNo, opts => opts.MapFrom(src => src.ReceiptNo))
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.date, opts => opts.MapFrom(src => src.Date))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.pending, opts => opts.MapFrom(src => src.Pending))
                    .ForMember(dest => dest.month, opts => opts.MapFrom(src => src.Month))
                    .ForMember(dest => dest.year, opts => opts.MapFrom(src => src.Year))
                    .ReverseMap();

                cfg.CreateMap<UploadBillDetail, UploadBillDetailViewModel>()
                    .ForMember(dest => dest.srNo, opts => opts.MapFrom(src => src.SrNo))
                    .ForMember(dest => dest.flatName, opts => opts.MapFrom(src => src.FlatName))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.note, opts => opts.MapFrom(src => src.Note))
                    .ReverseMap();

                cfg.CreateMap<GenerateBillResult, GenerateBillResultViewModel>()
                    .ForMember(dest => dest.name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.email, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.amount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.month, opts => opts.MapFrom(src => src.Month))
                    .ForMember(dest => dest.year, opts => opts.MapFrom(src => src.Year))
                    .ReverseMap();
            });
        }
    }
}