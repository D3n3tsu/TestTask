using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmpeekTest1.Infrastructure
{
    public interface IFolderNavigationService
    {
        void GoToFolder(string folderName);

        void GoToParentFolder();

        Task<int> GetFilesUnderTen();

        Task<int> GetFilesTenToFifty();

        Task<int> GetFilesAboveHundred();

        Task<IEnumerable<string>> GetFiles();

        Task<string> GetCurrentFolder();
        
    }
}