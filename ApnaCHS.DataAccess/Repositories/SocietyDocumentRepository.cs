using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface ISocietyDocumentRepository
    {
        Task<SocietyDocument> Create(SocietyDocument societyDocument);

        Task<SocietyDocument> Read(int key);

        Task Update(SocietyDocument flat);

        Task Delete(int id);

        Task<List<SocietyDocument>> List(SocietyDocumentSearchParams searchParams);
    }
    public class SocietyDocumentRepository : ISocietyDocumentRepository
    {
        public Task<SocietyDocument> Create(SocietyDocument societyDocument)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (societyDocument.Society != null)
                    {
                        societyDocument.SocietyId = societyDocument.Society.Id;
                        societyDocument.Society = null;
                    }
                    context.SocietyDocuments.Add(societyDocument);
                    context.SaveChanges();

                    return societyDocument;
                }
            });
            return taskResult;
        }

        public Task Delete(int id)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.SocietyDocuments.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Society Document not found");
                    }

                    context.SocietyDocuments.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }
        public Task<SocietyDocument> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context.SocietyDocuments.FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Society Document not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }
        public Task Update(SocietyDocument societyDocument)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.SocietyDocuments.FirstOrDefault(p => p.Id == societyDocument.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society Document detail not found");
                    }
                    if (societyDocument.Society != null)
                    {
                        societyDocument.SocietyId = societyDocument.Society.Id;
                        societyDocument.Society = null;
                    }
                    existingRecord.Name = societyDocument.Name;
                    existingRecord.FilePath = societyDocument.FilePath;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }
        public Task<List<SocietyDocument>> List(SocietyDocumentSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context.SocietyDocuments.ToList();

                    return ctx;
                }
            });
            return taskResult;
        }
    }
}
