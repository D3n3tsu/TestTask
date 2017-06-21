using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;

namespace EmpeekTest1.Infrastructure
{
    public class FolderService : IFolderNavigationService
    {
        private DirectoryInfo currentFolder;
        private int filesAboveHundred;
        private int filesTenToFifty;
        private int filesUnderTen;
        private IEnumerable<DirectoryInfo> subfolders;
        private IEnumerable<FileInfo> files;

        private List<string> filesInFolder;

        public FolderService()
        {
            DirectoryInfo fldr = (DirectoryInfo)HttpContext.Current.Application.Contents["currentFolder"];
            
            if (fldr == null)
            {
                currentFolder = new DirectoryInfo(Environment.CurrentDirectory);
                HttpContext.Current.Application.Contents.Set("currentFolder", currentFolder);
            }
            else
            {
                currentFolder = fldr;
            }
            CountFiles();
        }

        public async Task<int> GetFilesAboveHundred()
        {
            return await Task.FromResult(filesAboveHundred);
        }

        public async Task<int> GetFilesTenToFifty()
        {
            return await Task.FromResult(filesTenToFifty);
        }

        public async Task<int> GetFilesUnderTen()
        {
            return await Task.FromResult(filesUnderTen);
        }

        public async void GoToFolder(string folderName)
        {
            
            var folder = new DirectoryInfo(Path.Combine(currentFolder.FullName, folderName));

            if (folder.Exists)
            {
                currentFolder = folder;
                CountFiles();
                HttpContext.Current.Application.Contents.Set("currentFolder", currentFolder);
            }

            await Task.CompletedTask;
        }

        public async void GoToParentFolder()
        {
            currentFolder = currentFolder.Parent;
            CountFiles();
            HttpContext.Current.Application.Contents.Set("currentFolder", currentFolder);
            await Task.CompletedTask;
        }
         
        public async Task<IEnumerable<string>> GetFiles()
        {
            return await Task.FromResult(filesInFolder as IEnumerable<string>);
        }

        public async Task<string> GetCurrentFolder()
        {
            return await Task.FromResult(currentFolder.FullName);
        }

        private void CountFiles()
        {
            subfolders = currentFolder.GetDirectories();
            
            files = currentFolder.GetFiles();

            Clear();

            if (subfolders.Count() > 0)
            {
                foreach (var folder in subfolders)
                {
                    filesInFolder.Add(folder.Name);
                    var size = GetFolderSize(folder);
                    if (size < 10485760) { filesUnderTen++; }
                    else if (size < 52428800) { filesTenToFifty++; }
                    else if (size > 104857600) { filesAboveHundred++; }
                }
            }

            if (files.Count() > 0)
            {
                foreach (var file in files)
                {
                    filesInFolder.Add(file.Name);
                    var size = file.Length;
                    if (size < 10485760) { filesUnderTen++; }
                    else if (size < 52428800) { filesTenToFifty++; }
                    else if (size > 104857600) { filesAboveHundred++; }
                }
            }
        }

        private void Clear()
        {
            filesInFolder = new List<string>();
            filesAboveHundred = 0;
            filesTenToFifty = 0;
            filesUnderTen = 0;
        }

        private ulong GetFolderSize(DirectoryInfo folder)
        {
            ulong Size = 0;
            var files = folder.GetFiles();
            if (files.Count() > 0)
            {
                foreach (var file in files)
                {
                    Size += (ulong)file.Length;
                }
            }

            var folders = folder.GetDirectories();
            if (folders.Count() > 0)
            {
                foreach (var fold in folders)
                {
                    Size += GetFolderSize(fold);
                }
            }

            return Size;
        }

        
    }
}