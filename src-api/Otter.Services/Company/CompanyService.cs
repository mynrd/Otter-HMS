using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Services.Company
{
    using Data;
    using Otter.Core;
    using Otter.Core.Account;
    using Otter.Core.Company;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICompanyService
    {
        Task Create(CompanyModel model);
        Task Delete(int id);
        Task<IEnumerable<CompanyListModel>> GetList();
        Task Update(int id, CompanyModel model);
    }

    public class CompanyService : BaseService, ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Company> _repositoryCompany;

        public CompanyService(
            ICurrentUser currentUser
            , IUnitOfWork unitOfWork
            , IRepository<Company> repositoryCompany
            ) : base(currentUser)
        {
            _unitOfWork = unitOfWork;
            _repositoryCompany = repositoryCompany;
        }

        public async Task Create(CompanyModel model)
        {
            var data = PrepareEntity(new Company()
            {
                Code = model.Code,
                Name = model.Name,
            });
            _repositoryCompany.Insert(data);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(int id, CompanyModel model)
        {
            var data = _repositoryCompany.Find(id);
            data.Code = model.Code;
            data.Name = model.Name;
            PrepareEntity(data);
            _repositoryCompany.Update(data);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            _repositoryCompany.Delete(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CompanyListModel>> GetList()
        {
            var res = await _repositoryCompany.Query().GetAsync();
            return res.ToModel();
        }
    }
}