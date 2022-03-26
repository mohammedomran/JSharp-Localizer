using HtmlAgilityPack;
using ICSharpCode.Decompiler.Util;
using JSharp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace JSharpLocalizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Serializable]

    public class LocalizationController : ControllerBase
    {
        [HttpPost]
        public ActionResult<IEnumerable<Localizer>> GetLocalizationKeysValues([FromQuery]string PagesPath)
        {
            if (string.IsNullOrEmpty(PagesPath))
            {
                return BadRequest("Please send a valid directory path");
            }

            DirectoryInfo d = new DirectoryInfo(PagesPath);
            FileInfo[] Files = d.GetFiles("*.cshtml");


            List<LocalizerPage> LocalizerPages = new List<LocalizerPage>();
            foreach (FileInfo file in Files)
            {
                var localizerPage = new LocalizerPage();
                localizerPage.PageName = file.Name.Replace(".cshtml", string.Empty);

                var fileFullPath = d + @"\" + file.Name;
                var fileContent = System.IO.File.ReadAllText(fileFullPath);

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(fileContent);

                List<Localizer> localizers = new List<Localizer>();
                foreach (HtmlNode node in pageDoc.DocumentNode.SelectNodes("//text()[normalize-space()]"))
                {
                    if (!string.IsNullOrEmpty(node.InnerText) && 
                        !node.InnerText.Contains("@") &&
                        !node.InnerText.Contains("{") &&
                        !node.InnerText.Contains("}")
                        )
                    {
                        var oldText = node.InnerText;
                        var newText = $"<localize key='{ oldText.Replace(" ", string.Empty) }' default-text='{ oldText }' />";
                        fileContent = fileContent.Replace(oldText, newText);

                        localizers.Add(new Localizer { Key = oldText.Replace(" ", string.Empty), Value = oldText });
                    }
                }
                localizerPage.Data = localizers;
                LocalizerPages.Add(localizerPage);

                /*using (ResXResourceWriter resx = new ResXResourceWriter(@".\wwwroot\Resources\" + Path.GetFileNameWithoutExtension(file.Name) + ".en-GB.resx"))
                {
                    foreach (var localizer in localizers)
                        resx.AddResource(localizer.Key, localizer.Value);
                }

                System.IO.File.WriteAllText(fileFullPath, fileContent);*/

            }
            return Ok(LocalizerPages);

        }
    }

}
