using JSharp.Core.Models;
using System.Collections.Generic;

namespace JSharpLocalizer.API
{
    public interface ILocalizationService
    {
        string UpdateCshtmlFiles(string PagesPath, IEnumerable<LocalizerPage> localizerPages);
    }
}
