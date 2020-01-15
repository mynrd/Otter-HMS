using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otter.Core.Company
{
    using Otter.Data;

    public static class Extensions
    {
        public static CompanyModel ToModel(this Company data)
        {
            return new CompanyModel()
            {
                Code = data.Code,
                Id = data.Id,
                Name = data.Name,
            };
        }

        public static IEnumerable<CompanyListModel> ToModel(this IQueryable<Company> collection)
        {
            return collection.Select(x => new CompanyListModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });
        }

        public static IEnumerable<CompanyListModel> ToModel(this IEnumerable<Company> collection)
        {
            return  collection.Select(x => new CompanyListModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });
        }
    }
}
