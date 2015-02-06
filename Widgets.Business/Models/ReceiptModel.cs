using System;
using System.Collections.Generic;
using System.Linq;

namespace Widgets.Business.Models
{
    public class ReceiptModel
    {
        public ReceiptModel()
        {
            Widgets = new List<WidgetModel>();
        }

        public List<WidgetModel> Widgets { get; set; } 
        public DateTime PurchaseDate { get; set; }
        public decimal Tax { get; set; }
        public decimal Total
        {
            get { return Widgets.Sum(w => w.Price) + Tax; }
        }

        public string Message { get; set; }
    }
}
