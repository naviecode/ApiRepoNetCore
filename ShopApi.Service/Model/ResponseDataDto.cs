﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models
{
    public class ResponseDataDto<T>
    {
        public int TotalItem { get; set; }
        public List<T> Items { get; set; }

        public ResponseDataDto(List<T> items, int totalItem)
        {
            TotalItem = totalItem;
            Items = items;
        }
    }
}
