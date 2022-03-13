using ICSharpCode.Decompiler.Util;
using Microsoft.AspNetCore.Mvc;

namespace JSharpLocalizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizersController : ControllerBase
    {
        [HttpPost]
        public ActionResult StoreNewFile([FromBody] string newFileContent)
        {
            //System.IO.File.WriteAllText(@"E:\Tools\J# localizer\test.html", string.Empty);
            return Ok(123);
            System.IO.File.WriteAllText(@"E:\Tools\J# localizer\test.cshtml", newFileContent);

            // Define a resource file named CarResources.resx.
            using (ResXResourceWriter resx = new ResXResourceWriter(@".\wwwroot\Resources\test.en-GB.resx"))
            {
                resx.AddResource("Title", "Classic American Cars");
                resx.AddResource("HeaderString1", "Make");
                resx.AddResource("HeaderString2", "Model");
            }

                return Ok(newFileContent);
        }

        //[HttpPost("test")]
        //public ActionResult ChangeText([FromBody] IEnumerable<Request> model)
        //{
        //    string html = System.IO.File.ReadAllText("Html/test.html");

        //    //string text = html.Replace(model.First().Old, model.First().New);
        //    //System.IO.File.WriteAllText("Html/test.html", text);

        //    foreach (var item in model)
        //    {
        //        string text = html.Replace(item.Old.ToString(), item.New.ToString());
        //        System.IO.File.WriteAllText("Html/test.html", text);
        //    }
        //    return Ok("done");
        //}

    }
}
