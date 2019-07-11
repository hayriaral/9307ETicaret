using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETicaret.Common;

namespace ETicaret.MVC.Areas.Admin.Models.ResultModel
{
    public class InstanceResult<T>
    {
        public Result<int> ResultInt { get; set; }
        public Result<T> ResultT { get; set; }
        public Result<List<T>> ResultList { get; set; }
    }
}