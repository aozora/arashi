using System.Web.Mvc;
using xVal.ServerSide;

namespace xVal.ClientSidePlugins.MvcTestSite
{
    public class TestController : Controller
    {
        public ViewResult RenderSpecificView(string viewPath)
        {
            return View("~" + viewPath);
        }

        public RemoteValidationResult EvaluateAbcRule(FormCollection form)
        {
            var fieldValue = form["myprefix.RemotelyValidated_Field"];

            if (!fieldValue.StartsWith("abc"))
                return RemoteValidationResult.Failure("We don't allow '" + fieldValue + "', because it doesn't start with 'abc'");
            else
                return RemoteValidationResult.Success;
        }
    }
}