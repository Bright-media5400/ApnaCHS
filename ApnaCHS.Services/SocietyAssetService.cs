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
    public interface ISocietyAssetService
    {
        Task<SocietyAsset> Create(SocietyAsset societyAsset);

        Task<SocietyAsset> Read(int key);

        Task Update(SocietyAsset societyAsset);

        Task Delete(int id);

        Task<List<SocietyAsset>> List(SocietyAssetSearchParams searchParams);

    }
  public  class SocietyAssetService : ISocietyAssetService
    {
        ISocietyAssetRepository _societyAssetRepository = null;

        public SocietyAssetService()
        {
            _societyAssetRepository = new SocietyAssetRepository();
        }
        public Task<SocietyAsset> Create(SocietyAsset societyAsset)
        {
            return _societyAssetRepository.Create(societyAsset);
        }

        public Task Delete(int id)
        {
            return _societyAssetRepository.Delete(id);
        }

        public Task<List<SocietyAsset>> List(SocietyAssetSearchParams searchParams)
        {
            return _societyAssetRepository.List(searchParams);
        }

        public Task<SocietyAsset> Read(int key)
        {
            return _societyAssetRepository.Read(key);
        }

        public Task Update(SocietyAsset societyAsset)
        {
            return _societyAssetRepository.Update(societyAsset);
        }
    }
}
