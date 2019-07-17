using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Common;
using ETicaret.Entity;

namespace ETicaret.Repository
{
    public class ProductRepository : DataRepository<Product, int>
    {
        public static ECommerceEntities db = new ECommerceEntities();
        ResultProcess<Product> result = new ResultProcess<Product>();

        public override Result<int> Delete(int id)
        {
            Product silinecek = db.Products.SingleOrDefault(t => t.ProductID == id);
            db.Products.Remove(silinecek);

            return result.GetResult(db);
        }

        public override Result<Product> GetObjByID(int id)
        {
            return result.GetT(db.Products.SingleOrDefault(t => t.ProductID == id));
        }

        public override Result<int> Insert(Product item)
        {
            Product newProduct = db.Products.Create();
            newProduct.ProductName = item.ProductName;
            newProduct.Stock = item.Stock;
            newProduct.Price = item.Price;
            newProduct.Photo = item.Photo;
            newProduct.CategoryID = item.CategoryID;
            newProduct.BrandID = item.BrandID;

            db.Products.Add(newProduct);

            return result.GetResult(db);
        }

        public override Result<List<Product>> List()
        {
            return result.GetListResult(db.Products.ToList());
        }

        public override Result<int> Update(Product item)
        {
            Product guncellenecek = db.Products.SingleOrDefault(t => t.ProductID == item.ProductID);

            guncellenecek.BrandID = item.BrandID;
            guncellenecek.CategoryID = item.CategoryID;
            guncellenecek.ProductName = item.ProductName;
            guncellenecek.Price = item.Price;
            guncellenecek.Stock = item.Stock;
            guncellenecek.Photo = item.Photo;

            return result.GetResult(db);
        }

    }
}
