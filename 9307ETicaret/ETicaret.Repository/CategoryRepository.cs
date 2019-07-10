using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Entity;
using ETicaret.Common;

namespace ETicaret.Repository
{
    //uniqueidentifier karşılığı burda Guiddir.
    class CategoryRepository : DataRepository<Category, Guid>
        //DataRepository ctrl . ile abstracları aktarıyoruz.
    {
        static ECommerceEntities db = Tool.GetConnection();
        ResultProcess<Category> result = new ResultProcess<Category>();

        public override Result<int> Delete(Guid id)
        {
            Category cat = db.Categories.SingleOrDefault(t => t.CategoryID == id);
            db.Categories.Remove(cat);
            return result.GetResult(db);
        }

        public override Result<Category> GetObjByID(Guid id)
        {
            Category cat = db.Categories.SingleOrDefault(t => t.CategoryID == id);
            return result.GetT(cat);
        }

        public override Result<int> Insert(Category item)
        {
            Category newCat = db.Categories.Create();
            newCat.CategoryName = item.CategoryName;
            newCat.CreatedDate = DateTime.Now;
            newCat.Description = item.Description;

            db.Categories.Add(newCat);
            return result.GetResult(db);
        }

        public override Result<List<Category>> List()
        {
            List<Category> CatList = db.Categories.ToList();
            return result.GetListResult(CatList);
        }

        public override Result<int> Update(Category item)
        {
            Category dbdeki = db.Categories.SingleOrDefault(t => t.CategoryID == item.CategoryID);

            dbdeki.CategoryName = item.CategoryName;
            dbdeki.Description = item.Description;

            return result.GetResult(db);
        }
    }
}
