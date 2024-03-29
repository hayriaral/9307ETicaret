﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Common;
using ETicaret.Entity;
using System.IO;

namespace ETicaret.Repository {
    public class ProductRepository : DataRepository<Product, int>, GetLatestObject<Product> {
        public static ECommerceEntities db = new ECommerceEntities();
        ResultProcess<Product> result = new ResultProcess<Product>();

        public override Result<int> Delete(int id) {
            Product silinecek = db.Products.SingleOrDefault(t => t.ProductID == id);
            db.Products.Remove(silinecek);
            return result.GetResult(db);
        }

        public Result<List<Product>> GetLatestObjects(int Quantity) {
            return result.GetListResult(db.Products.OrderByDescending(t => t.ProductID).Take(Quantity).ToList());
            //Product'ları ProductIDsi en büyük olandan küçüğe doru sıralattık. En tepedeki son eklenen ürünler oldu. bu ürünlerden quantity kadarını listeye attık.
        }

        public override Result<Product> GetObjByID(int id) {
            return result.GetT(db.Products.SingleOrDefault(t => t.ProductID == id));
        }

        public override Result<int> Insert(Product item) {
            Product newProduct = db.Products.Create();
            newProduct.ProductName = item.ProductName;
            newProduct.Stock = item.Stock;
            newProduct.Price = item.Price;
            newProduct.Photo = item.Photo;
            newProduct.Photo2 = item.Photo2;
            newProduct.Photo3 = item.Photo3;
            newProduct.CategoryID = item.CategoryID;
            newProduct.BrandID = item.BrandID;

            db.Products.Add(newProduct);

            return result.GetResult(db);
        }

        public override Result<List<Product>> List() {
            return result.GetListResult(db.Products.ToList());
        }

        public override Result<int> Update(Product item) {
            Product guncellenecek = db.Products.SingleOrDefault(t => t.ProductID == item.ProductID);

            guncellenecek.BrandID = item.BrandID;
            guncellenecek.CategoryID = item.CategoryID;
            guncellenecek.ProductName = item.ProductName;
            guncellenecek.Price = item.Price;
            guncellenecek.Stock = item.Stock;
            guncellenecek.Photo = item.Photo;
            guncellenecek.Photo2 = item.Photo2;
            guncellenecek.Photo3 = item.Photo3;

            return result.GetResult(db);
        }

        public void StockAzalt(Order o) {
            foreach (OrderDetail item in o.OrderDetails) {

                item.Product.Stock = item.Product.Stock - item.Quantitiy;
                Update(item.Product);
            }
        }
    }
}
