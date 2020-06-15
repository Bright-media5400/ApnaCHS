using ApnaCHS.AppCommon;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface IMasterValueService
    {
        Task<List<MasterValue>> GetListActive(EnMasterValueType type);

        Task<List<MasterValue>> GetListAll(EnMasterValueType type);

        Task<List<MasterValue>> DropDown(EnMasterValueType type);

        Task<MasterValue> New(MasterValue model);

        Task Delete(MasterValue model);

        Task Update(MasterValue model);

        Task Active(long id);
    }

    public class MasterValueService : IMasterValueService
    {
        private IMasterValueRepository _masterValueRepository;

        public MasterValueService()
        {
            _masterValueRepository = new MasterValueRepository();
        }

        public Task Delete(MasterValue model)
        {
           return _masterValueRepository.Delete(model);
        }

        public Task<List<MasterValue>> GetListActive(EnMasterValueType type)
        {
            return _masterValueRepository.GetListActive(type);
        }
        
        public Task<List<MasterValue>> GetListAll(EnMasterValueType type)
        {
            return _masterValueRepository.GetListAll(type);
        }

        public Task<List<MasterValue>> DropDown(EnMasterValueType type)
        {
            return _masterValueRepository.DropDown(type);
        }

        public Task<MasterValue> New(MasterValue model)
        {
            return _masterValueRepository.New(model);
        }

        public Task Update(MasterValue model)
        {
            return _masterValueRepository.Update(model);
        }

        public Task Active(long id)
        {
            return _masterValueRepository.Active(id);
        }
    }
}
