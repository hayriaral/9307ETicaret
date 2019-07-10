using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Common
{
    //Bu class generic typeta bir classtır. Parametre değeri ne gelirse ona bürünür.
    //Belli tasklar altında çalışır. Helper class olarak da bilinir.
    //Tamamen yardımcı işler icin kullanılır.
    public class Result<T>
    {
        public string UserMessage { get; set; }
        public bool IsSucceeded { get; set; }
        public T ProcessResult { get; set; } //Categori çekmek icin kullanacağız.
    }
}
