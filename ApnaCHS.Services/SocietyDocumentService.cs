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
    public interface ISocietyDocumentService
    {
        Task<SocietyDocument> Create(SocietyDocument societyDocument);

        Task<SocietyDocument> Read(int key);

        Task Update(SocietyDocument societyDocument);

        Task Delete(int id);

        Task<List<SocietyDocument>> List(SocietyDocumentSearchParams searchParams);

    }
    public class SocietyDocumentService : ISocietyDocumentService
    {
        ISocietyDocumentRepository _societyDocumentRepository = null;

       public SocietyDocumentService()
        {
            _societyDocumentRepository = new SocietyDocumentRepository();
        }

       public Task<SocietyDocument> Create(SocietyDocument societyDocument)
       {
           return _societyDocumentRepository.Create(societyDocument);
       }

       public Task Delete(int id)
       {
           return _societyDocumentRepository.Delete(id);
       }

       public Task<List<SocietyDocument>> List(SocietyDocumentSearchParams searchParams)
       {
           return _societyDocumentRepository.List(searchParams);
       }

       public Task<SocietyDocument> Read(int key)
       {
           return _societyDocumentRepository.Read(key);
       }

       public Task Update(SocietyDocument societyDocument)
       {
           return _societyDocumentRepository.Update(societyDocument);
       }
    }
}
