using HtmlAgilityPack;
using JSharp.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JSharpLocalizer.API
{
    public class LocalizationService : ILocalizationService
    {
        public string UpdateCshtmlFiles(string PagesPath, IEnumerable<LocalizerPage> localizerPages)
        {
            DirectoryInfo d = new DirectoryInfo(PagesPath);
            FileInfo[] Files = d.GetFiles("*.cshtml");


            foreach (FileInfo file in Files)
            {
                var localizerPage = new LocalizerPage();
                var pageName = file.Name.Replace(".cshtml", string.Empty);
                localizerPage.PageName = pageName;

                var fileFullPath = d + @"\" + file.Name;
                var fileContent = System.IO.File.ReadAllText(fileFullPath);

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(fileContent);

                var docNodes = pageDoc.DocumentNode?.SelectNodes("//text()[normalize-space()]");
                if (docNodes is not null)
                {
                    foreach (HtmlNode node in docNodes)
                    {
                        if (!string.IsNullOrEmpty(node.InnerText) &&
                        !node.InnerText.Contains("@") &&
                        !node.InnerText.Contains("{") &&
                        !node.InnerText.Contains("}") &&
                        !node.InnerText.All(char.IsDigit) &&
                        !node.InnerText.Contains("$") &&
                        !node.InnerText.Contains("€")
                        )
                        {
                            var oldText = node.InnerText;
                            //fileContent = fileContent.Replace(oldText, newText);
                            var key = localizerPages.FirstOrDefault(x => x.PageName == pageName)?.Data?.FirstOrDefault(x => x.Active && x.Value == oldText)?.Key;
                            if (key != null)
                            {
                                var newText = $"<localize key='{ key.Trim() }' default-text='{ oldText.Trim() }' />";
                                fileContent = fileContent.Replace(oldText, newText);
                            }
                        }
                    }
                }
                System.IO.File.WriteAllText(fileFullPath, fileContent);
            }


            /*if (string.IsNullOrEmpty(PagesPath))
            {
                return "Please send a valid directory path";
            }

            DirectoryInfo d = new DirectoryInfo(PagesPath);
            //todo: get files from localizer page not all files from directory
            FileInfo[] Files = d.GetFiles("*.cshtml");


            foreach (var file in Files.Select((value, i) => new { value, i }))
            {

                var fileFullPath = d + @"\" + file.value.Name;
                var fileContent = System.IO.File.ReadAllText(fileFullPath);

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(fileContent);

                LocalizerPage loc = (dynamic)localizerPages.ToList()[file.i];
                foreach (var x in loc.Data)
                {
                    if (x.Active)
                    {
                        var oldText = x.Value.Trim();
                        var newText = $"<localize key='{ x.Key.Replace(" ", string.Empty) }' default-text='{ oldText }' />";
                        fileContent = fileContent.Replace(oldText, newText);
                    }
                }
                System.IO.File.WriteAllText(fileFullPath, fileContent);

            }*/

            return "Done";
        }
    }
}
