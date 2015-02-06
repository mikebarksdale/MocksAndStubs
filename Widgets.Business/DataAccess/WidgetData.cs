using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Widgets.Business.Models;

namespace Widgets.Business.DataAccess
{
    public interface IWidgetData
    {
        IEnumerable<WidgetModel> GetWidgets();
    }

    public class WidgetData : IWidgetData
    {
        public IEnumerable<WidgetModel> GetWidgets()
        {
            Thread.Sleep(10000);

            yield return new WidgetModel
            {
                Id = 1,
                Name = "Toy",
                Price = 9.99m
            };

            yield return new WidgetModel
            {
                Id = 2,
                Name = "Shoes",
                Price = 39.99m
            };


            yield return new WidgetModel
            {
                Id = 3,
                Name = "Shirt",
                Price = 25.49m
            };


            yield return new WidgetModel
            {
                Id = 1,
                Name = "Pants",
                Price = 49.99m
            };


            yield return new WidgetModel
            {
                Id = 1,
                Name = "Hat",
                Price = 12.99m
            };
        }
    }
}
