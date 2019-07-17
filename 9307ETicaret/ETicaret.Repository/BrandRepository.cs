using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Common;
using ETicaret.Entity;

namespace ETicaret.Repository
{
    public class BrandRepository : DataRepository<Brand, int>
    {
        public static ECommerceEntities db = Tool.GetConnection();
        ResultProcess<Brand> result = new ResultProcess<Brand>();

        public override Result<int> Delete(int id)
        {
            Brand silinecek = db.Brands.SingleOrDefault(t => t.BrandID == id);
            db.Brands.Remove(silinecek);
            return result.GetResult(db);
        }

        public override Result<Brand> GetObjByID(int id)
        {
            Brand brand = db.Brands.SingleOrDefault(t => t.BrandID == id);
            return result.GetT(brand);
        }

        public override Result<int> Insert(Brand item)
        {
            Brand newBrand = db.Brands.Create();
            newBrand.BrandName = item.BrandName;
            newBrand.Description = item.Description;
            newBrand.Photo = item.Photo;

            db.Brands.Add(newBrand);

            return result.GetResult(db);
        }

        public override Result<List<Brand>> List()
        {
            return result.GetListResult(db.Brands.ToList());
        }

        public override Result<int> Update(Brand item)
        {
            Brand guncellenecek = db.Brands.SingleOrDefault(t => t.BrandID == item.BrandID);
            guncellenecek.BrandName = item.BrandName;
            guncellenecek.Description = item.Description;
            guncellenecek.Photo = item.Photo;
            return result.GetResult(db);
        }
    }
}
