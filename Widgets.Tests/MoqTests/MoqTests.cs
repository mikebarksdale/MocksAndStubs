using System;
using System.Collections.Generic;
using System.Linq;
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
            //arrange
            var mockData = GetStubData();

            var service = new WidgetService(mockData.Object);

            //act
            var widget = service.GetWidget(5);

            //assert
            Assert.IsNull(widget);
        }

        [TestMethod]
        public void WidgetCacheIsUsed()
        {
            var mockData = GetStubData();

            var service = new WidgetService(mockData.Object);

            var widgets1 = service.GetAllWidgets();
            var widgets2 = service.GetAllWidgets();

            //check behavior: cache should be used in second call
            mockData.Verify(d => d.GetWidgets(), Times.Once);
            Assert.AreEqual(widgets1.Count(), widgets2.Count());
        }

        [TestMethod]
        public void AuthorizationRequiredForCreditPurchasesOver1000UsDollars()
        {
            var mockData = GetStubData();
            var service = new WidgetService(mockData.Object);

            var receiptModel =
                service.PurchaseWidgets(new[] {new WidgetModel {Id = 0, Name = "Something", Price = 1001.00m}},
                    "creditcard", 1001.00m);

            Assert.IsTrue(receiptModel.Message.Contains("Authorization"));
        }

        [TestMethod]
        public void GetWidgetExpected()
        {
            var mockData = GetStubData();
            var service = new WidgetService(mockData.Object);

            var widget = service.GetWidget(1);

            Assert.IsNotNull(widget);
        }

        [TestMethod]
        public void ConnectToExpensiveResourceAndGetAWidget()
        {
            var service = new WidgetService(new WidgetData());

            var widget = service.GetWidget(1); //gotta make sure my data has this widget!

            Assert.IsNotNull(widget);
        }

        [TestMethod]
        public void BuyWidgetsAndDetermineTaxCalculation()
        {
            //arrange
            var mockData = GetStubData();
            var service = new WidgetService(mockData.Object);

            var widgets = service.GetAllWidgets();

            var widgetsToBuy = widgets.Take(2).ToList();
            
            //act
            var receiptModel = service.PurchaseWidgets(widgetsToBuy, "cash", widgetsToBuy.Sum(w => w.Price));

            Assert.IsTrue(widgetsToBuy.Sum(w => w.Price) * 0.07m == receiptModel.Tax);
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void TestTheBoom()
        {
            var mockData = GetStubData();
            var service = new WidgetService(mockData.Object);

            service.Boom();
        }

        private Mock<IWidgetData> GetStubData()
        {
            var mockData = new Mock<IWidgetData>();
            mockData.Setup(d => d.GetWidgets()).Returns(_mockWidgets);
            return mockData;
        }


    }
}
