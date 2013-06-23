using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace xVal.Tests.TestHelpers
{
    /// <summary>
    /// Just sets up enough context to get a HtmlHelper object (so you can call, e.g., html.ClientSideValidationRules())
    /// </summary>
    /// <typeparam name="TModel">The type parameter used for ViewData</typeparam>
    internal class HtmlHelperMocks<TModel> where TModel: class 
    {
        public readonly Mock<HttpContextBase> MockHttpContext = new Mock<HttpContextBase>();
        public readonly Mock<IView> MockView = new Mock<IView>();
        public readonly Mock<ControllerBase> MockController = new Mock<ControllerBase>();
        public readonly HtmlHelper HtmlHelper;

        public HtmlHelperMocks()
        {
            var viewData = new ViewDataDictionary<TModel>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Expect(x => x.ViewData).Returns(viewData);

            var controllerContext = new ControllerContext(MockHttpContext.Object, new RouteData(), MockController.Object);

            var viewContext = new ViewContext(controllerContext, MockView.Object, viewData, new TempDataDictionary(), controllerContext.HttpContext.Response.Output);
            HtmlHelper = new HtmlHelper(viewContext, mockViewDataContainer.Object);
        }
    }
}