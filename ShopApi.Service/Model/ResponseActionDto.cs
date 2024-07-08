using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models
{
    public class ResponseActionDto<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Attr { get; set; }

        public ResponseActionDto(T data, int code, string message, string attr)
        {
            Data = data;
            Code = code;
            Message = message;
            Attr = attr;
        }
    }
}
