using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Widgets.Business.DataAccess;
using Widgets.Business.Models;
using Widgets.Business.Service;

namespace Widgets.Tests.MoqTests
{
    [TestClass]
    public class MoqTests
    {
        private IList<WidgetModel> _mockWidgets;

        [TestInitialize]
        public void Initialize()
        {
            _mockWidgets = new List<WidgetModel>
                {
                    new WidgetModel
                        {
                            Id = 1,
                            Name = "Foo",
                            Price = 5.00m
                        },
                    new WidgetModel
                        {
                            Id = 2,
                            Name = "Bar",
                            Price = 10.00m
                        }
                };
        }

        [TestMethod]
        public void WidgetNullIfNotFound()
        {
            //stub data
            var mockData = new Mock<IWidgetData>();
            mockData.Setup(d => d.GetWidgets()).Returns(_mockWidgets);

            var service = new WidgetService(mockData.Object);

            var widget = service.GetWidget(5);

            Assert.IsNull(widget);
        }
    }
}
