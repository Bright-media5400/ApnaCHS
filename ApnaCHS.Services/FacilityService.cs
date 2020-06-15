using ApnaCHS.Common;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface IFacilityService
    {
        Task<Facility[]> Create(Facility[] facility, long? societyId);

        Task<Facility> Read(int key);

        Task Update(Facility facility);

        Task Delete(int id);

        Task<List<Facility>> List(FacilitySearchParams searchParams);

        Task<List<Facility>> SocietyWiseList(FacilitySearchParams searchParams);

        Task<List<Facility>> LoadFlats(FacilitySearchParams searchParams);

        Task<int> FlatCount(long facilityId);

        Task<List<Facility>> LoadParkings(FacilitySearchParams searchParams);

        Task LinkSocieties(long[] linkSocieties, long[] unlinkSocieties, long facilityId);
    }

    public class FacilityService : IFacilityService
    {
        IFacilityRepository _FacilityRepository = null;

        public FacilityService()
        {
            _FacilityRepository = new FacilityRepository();
        }

        public Task<Facility[]> Create(Facility[] facility, long? societyId)
        {
            return _FacilityRepository.Create(facility, societyId);
        }

        public Task Delete(int id)
        {
            return _FacilityRepository.Delete(id);
        }

        public Task<List<Facility>> List(FacilitySearchParams searchParams)
        {
            return _FacilityRepository.List(searchParams);
        }

        public Task<List<Facility>> SocietyWiseList(FacilitySearchParams searchParams)
        {
            return _FacilityRepository.SocietyWiseList(searchParams);
        }

        public Task<List<Facility>> LoadFlats(FacilitySearchParams searchParams)
        {
            return _FacilityRepository.LoadFlats(searchParams);
        }

        public Task<int> FlatCount(long facilityId)
        {
            return _FacilityRepository.FlatCount(facilityId);
        }

        public Task<List<Facility>> LoadParkings(FacilitySearchParams searchParams)
        {
            return _FacilityRepository.LoadParkings(searchParams);
        }

        public Task<Facility> Read(int key)
        {
            return _FacilityRepository.Read(key);
        }

        public Task Update(Facility facility)
        {
            return _FacilityRepository.Update(facility);
        }

        public Task LinkSocieties(long[] linkSocieties, long[] unlinkSocieties, long facilityId)
        {
            return _FacilityRepository.LinkSocieties(linkSocieties, unlinkSocieties, facilityId);
        }
    }
}
