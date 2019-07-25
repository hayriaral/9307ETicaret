using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Common;

namespace ETicaret.Repository
{
    public interface DeleteObjectByDoubleID<T>
    {
        Result<T> DeleteObjects(int id1, int id2);
    }
}
