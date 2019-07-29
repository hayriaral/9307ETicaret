using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Entity;
using ETicaret.Common;

namespace ETicaret.Repository
{
    public class OrderDetailRepository : DataRepository<OrderDetail, int>, DeleteObjectByDoubleID<int>, GetObjectByTwoID<OrderDetail>
        //buradaki int OrderID'yi temsil ediyor.
    {
        public static ECommerceEntities db = Tool.GetConnection();
        ResultProcess<OrderDetail> result = new ResultProcess<OrderDetail>();

        public override Result<int> Delete(int id) //OrderID'yi siler.
        {
            //Aynı OrderID ile birden fazla OrderDetail olabilir. O sebeple List tipiyle data çektik.
            List<OrderDetail> silinecekler = db.OrderDetails.Where(t => t.OrderID == id).ToList();
            db.OrderDetails.RemoveRange(silinecekler);
            return result.GetResult(db);
        }

        public Result<int> DeleteObjects(int id1, int id2)
        {
            OrderDetail ord = db.OrderDetails.SingleOrDefault(t => t.OrderID == id1 && t.ProductID == id2);
            db.OrderDetails.Remove(ord);
            return result.GetResult(db);
        }

        public override Result<OrderDetail> GetObjByID(int id)
        {
            throw new NotImplementedException();
        }

        public Result<OrderDetail> GetObjectByTwoID(int id1, int id2)
        {
            //id1=orderid id2)productid
            OrderDetail od = db.OrderDetails.SingleOrDefault(t => t.OrderID == id1 && t.ProductID == id2);
            return result.GetT(od);
        }


        public override Result<int> Insert(OrderDetail item)
        {
            OrderDetail newOD = db.OrderDetails.Create();
            newOD.ProductID = item.ProductID;
            newOD.OrderID = item.OrderID;
            newOD.Price = item.Price;
            newOD.Quantitiy = item.Quantitiy;

            db.OrderDetails.Add(newOD);
            return result.GetResult(db);
        }

        public override Result<List<OrderDetail>> List()
        {
            return result.GetListResult(db.OrderDetails.ToList());
        }

        public override Result<int> Update(OrderDetail item)
        {
            OrderDetail od = GetObjectByTwoID(item.OrderID, item.ProductID).ProcessResult;

            od.Price = item.Price;
            od.Quantitiy = item.Quantitiy;

            return result.GetResult(db);
        }
    }
}
