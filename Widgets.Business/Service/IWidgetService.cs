using System.Collections.Generic;
using Widgets.Business.Models;

namespace Widgets.Business.Service
{
    public interface IWidgetService
    {
        IEnumerable<WidgetModel> GetAllWidgets();
        WidgetModel GetWidget(int widgetId);
        ReceiptModel PurchaseWidgets(IEnumerable<WidgetModel> widgets, string paymentType, decimal amount);
        void Boom();
    }
}
