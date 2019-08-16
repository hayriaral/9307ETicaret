using ETicaret.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret.MVC.Models.ResultModel {
    public class UserInstanceResult<T> {
        public Result<int> ResultInt { get; set; }
        public Result<T> ResultT { get; set; }
        public Result<List<T>> ResultList { get; set; }
    }
}