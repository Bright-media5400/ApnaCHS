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
    public interface IComplexService
    {
        Task<Complex> Create(Complex complex, ApplicationUser currentUser);

        Task<Complex> Read(int key);

        Task Update(Complex complex);

        Task Delete(int id);

        Task<List<Complex>> List(ComplexSearchParams searchParams);

        Task<List<AllIndiaPincode>> ReadArea(int pincode);

        Task UpdateLoginDetails(Complex complex);
    }
    public class ComplexService : IComplexService
    {
         IComplexRepository _complexRepository = null;

         public ComplexService()
        {
            _complexRepository = new ComplexRepository();
        }

         public Task<Complex> Create(Complex complex, ApplicationUser currentUser)
        {
            return _complexRepository.Create(complex, currentUser);
        }

        public Task Delete(int id)
        {
            return _complexRepository.Delete(id);
        }

        public Task<List<Complex>> List(ComplexSearchParams searchParams)
        {
            return _complexRepository.List(searchParams);
        }

        public Task<Complex> Read(int key)
        {
            return _complexRepository.Read(key);
        }

        public Task Update(Complex complex)
        {
            return _complexRepository.Update(complex);
        }

        public Task<List<AllIndiaPincode>> ReadArea(int pincode)
        {
            return _complexRepository.ReadArea(pincode);
        }

        public Task UpdateLoginDetails(Complex complex)
        {
            return _complexRepository.UpdateLoginDetails(complex);
        }
    }
}

    