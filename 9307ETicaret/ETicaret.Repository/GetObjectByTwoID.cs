using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Common;

namespace ETicaret.Repository
{
    interface GetObjectByTwoID<T>
    {
        //kullanımı genel olması için result<orderdetail> yapısına uygun olarak tasarlıyoruz.
        Result<T> GetObjectByTwoID(int id1,int id2);
    }
}
