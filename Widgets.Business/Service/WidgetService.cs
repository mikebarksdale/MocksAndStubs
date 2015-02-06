using System;
using System.Collections.Generic;
using System.Linq;
using Widgets.Business.DataAccess;
using Widgets.Business.Models;

namespace Widgets.Business.Service
{
    public class WidgetService : IWidgetService
    {
        private readonly IWidgetData _widgetData;
        private readonly HashSet<WidgetModel> _widgetCache; 

        public WidgetService(IWidgetData widgetData)
        {
            _widgetData = widgetData;
            _widgetCache = new HashSet<WidgetModel>();
        }

        public IEnumerable<WidgetModel> GetAllWidgets()
        {
            if (_widgetCache.Any())
                return _widgetCache.AsEnumerable();

            var widgets = _widgetData.GetWidgets();
            LoadCache(widgets);

            return _widgetCache.AsEnumerable();
        }

        public WidgetModel GetWidget(int widgetId)
        {
            if (_widgetCache.Any())
                return _widgetCache.FirstOrDefault(w => w.Id == widgetId);

            var widgets = GetAllWidgets();
            return widgets.FirstOrDefault(w => w.Id == widgetId);
        }

        public ReceiptModel PurchaseWidgets(IEnumerable<WidgetModel> widgets, string paymentType, decimal amount)
        {
            if(string.IsNullOrEmpty(paymentType))
                throw new ArgumentNullException("paymentType");

            if(amount <= 0.00m) //no freebies!
                throw new ArgumentException("Amount must be greater than zero. No freebies!");

            if(widgets == null)
                throw new ArgumentException("Must buy at least one widget!");

            if (paymentType.Equals("creditcard", StringComparison.InvariantCultureIgnoreCase) && amount > 1000.00m)
                return new ReceiptModel
                {
                    Message = "Authorization required"
                };

            var widgetModels = widgets as WidgetModel[] ?? widgets.ToArray();
            return new ReceiptModel
            {
                PurchaseDate = DateTime.Now,
                Widgets = widgetModels.ToList(),
                Tax = widgetModels.Sum(w => w.Price) * 0.07m
            };
        }

        private void LoadCache(IEnumerable<WidgetModel> widgets)
        {
            widgets.ToList().ForEach(w => _widgetCache.Add(w));
        }
    }
}
