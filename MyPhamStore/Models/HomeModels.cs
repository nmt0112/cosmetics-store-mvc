using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhamStore.Models
{
    public class HomeModels
    {
        public List<Brand> listBarnds { get; set; }
        public List<MyPham> listMyPham { get; set; }
        public List<Category> listCategory { get; set; }
    }
}