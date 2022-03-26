using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSharp.Core.Models
{
    public class LocalizerPage
    {
        public string PageName { get; set; }
        public IEnumerable<Localizer> Data { get; set; }
    }
}
