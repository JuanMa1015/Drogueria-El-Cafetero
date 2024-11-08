using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailsProduct
    {

           [Key] public int IdProduct { get; set; }
            public string ProductName { get; set; }
            public string SupplierName { get; set; }
            public string CategoryName { get; set; }
            public decimal Price { get; set; }
            public int UnitsInStock { get; set; }
            public DateTime ExpirationDate { get; set; }
            public bool Active { get; set; }
            public string Image { get; set; }
  

    }
}
