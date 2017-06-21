using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpeekTest1.Models.ViewModels
{
    public class FolderViewModel
    {
        public IEnumerable<string> Files { get; set; }

        public int FilesAboveHundred { get; set; }

        public int FilesTenToFifty { get; set; }

        public int FilesUnderTen { get; set; }

        public string CurrentFolder { get; set; }
    }
}