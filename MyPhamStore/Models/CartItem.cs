namespace MyPhamStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartItem")]
    public partial class CartItem
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public int Quantity { get; set; }

        public decimal Money
        {
            get;set;
        }
    }
}
