﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Entity;

namespace ETicaret.Common
{
    public class ResultProcess<T>
    {
        public Result<int> GetResult(ECommerceEntities db)
            //db'ye kayıt yapmak icin kullanacağımız yardımcı method.
        {
            Result<int> result = new Result<int>();

            int sonuc = db.SaveChanges();
            if (sonuc > 0)
            {
                result.UserMessage = "Basarili";
                result.IsSucceeded = true;
                result.ProcessResult = sonuc;
            }
            else
            {
                result.UserMessage = "Basarisiz";
                result.IsSucceeded = false;
                result.ProcessResult = sonuc;
            }

            return result;
        }

        public Result<List<T>> GetListResult(List<T> data)
            //db'den liste tipinde data çekerken, datanın olup olmadığını kontrol eden method.
        {
            Result<List<T>> result = new Result<List<T>>();

            if(data != null)
            {
                result.UserMessage = "işlem başarılı";
                result.IsSucceeded = true;
                result.ProcessResult = data;
            }

            else
            {
                result.UserMessage = "işlem basarisiz, data yok";
                result.IsSucceeded = false;
                result.ProcessResult = data;
            }

            return result;
        }

        public Result<T> GetT(T data)
        {
            Result<T> result = new Result<T>();
            if (data != null)
            {
                result.IsSucceeded = true;
                result.UserMessage = "Başarıli";
                result.ProcessResult = data;
            }
            else
            {
                result.IsSucceeded = false;
                result.UserMessage = "başarısız";
                result.ProcessResult = data;
            }
            return result;
        }
    }
}
