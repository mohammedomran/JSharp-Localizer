using HtmlAgilityPack;
using ICSharpCode.Decompiler.Util;
using JSharp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSharp.Core.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string PagesPath { get; set; }
        public void OnGet(string PagesPath)
        {
            if (PagesPath is null)
            {
                return;
            }

            DirectoryInfo d = new DirectoryInfo(PagesPath);
            FileInfo[] Files = d.GetFiles("*.cshtml");

            foreach (FileInfo file in Files)
            {
                var fileFullPath = d + @"\" + file.Name;
                var fileContent = System.IO.File.ReadAllText(fileFullPath);

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(fileContent);

                List<Localizer> localizers = new List<Localizer>();
                foreach (HtmlNode node in pageDoc.DocumentNode.SelectNodes("//text()[normalize-space()]"))
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {
                        var oldText = node.InnerText;
                        var newText = $"<localize key='{ oldText.Replace(" ", string.Empty) }' default-text='{ oldText }' />";
                        fileContent = fileContent.Replace(oldText, newText);

                        localizers.Add( new Localizer { Key= oldText.Replace(" ", string.Empty), Value= oldText } );
                    }
                }

                using (ResXResourceWriter resx = new ResXResourceWriter(@".\wwwroot\Resources\" + Path.GetFileNameWithoutExtension(file.Name) + ".en-GB.resx"))
                {
                    foreach(var localizer in localizers)
                        resx.AddResource(localizer.Key, localizer.Value);
                }

                System.IO.File.WriteAllText(fileFullPath, fileContent);

            }
            
        }
    }
}