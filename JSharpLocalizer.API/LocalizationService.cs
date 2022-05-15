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
            if (string.IsNullOrEmpty(PagesPath))
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

            }

            return "Done";
        }
    }
}
