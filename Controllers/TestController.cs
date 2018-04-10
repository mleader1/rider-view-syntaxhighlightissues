using Microsoft.AspNetCore.Mvc;

namespace rider_view_syntaxhighlightissues.Controllers
{
    public class TestController : Controller
    {
        [Route("{*requestPath}")]
        public ActionResult TestUrl(string requestPath)
        {
            return
                View("index"); //NOTE:  this view param is using a relative path as well, the syntax check shouldn't be use to valid the file path it may be different location depending on the FileProvider implemented.
        }
    }
}