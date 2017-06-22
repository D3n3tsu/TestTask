using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security;
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

        private List<string> filesInFolder;

        public FolderService()
        {
            DirectoryInfo fldr = (DirectoryInfo)HttpContext.Current.Application.Contents["currentFolder"];
            
            if (fldr != null)
            {
                currentFolder = fldr;
            }
            else
            {
                GoToDrives();
            }
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
            DirectoryInfo folder = folder = new DirectoryInfo(folderName);
            
                if (currentFolder != null)
                    folder = new DirectoryInfo(Path.Combine(currentFolder.FullName, folderName));

            if (folder.Exists)
            {
                currentFolder = folder;
                    
                    HttpContext.Current.Application.Contents.Set("currentFolder", currentFolder);
                
            }
            await CountFiles();
        }

        public async void GoToParentFolder()
        {
            if (currentFolder != null && currentFolder.Parent != null)
            {
                try
                {
                    currentFolder = currentFolder.Parent;
                }
                catch (SecurityException) { }
                await CountFiles();
                HttpContext.Current.Application.Contents.Set("currentFolder", currentFolder);
                await Task.CompletedTask;
            }
            else
            {
                GoToDrives();
                await Task.CompletedTask;
            }

        }

        private void GoToDrives()
        {
            DriveInfo[] drives = new DriveInfo[0];
            try
            {
                drives = DriveInfo.GetDrives();
            }
            catch (SecurityException) { }
            Clear();
            //Work around
            foreach (var item in drives)
            {
                filesInFolder.Add(item.RootDirectory.FullName);
            }
            HttpContext.Current.Application.Contents.Set("currentFolder", null);
        }

        public async Task<IEnumerable<string>> GetFiles()
        {
            return await Task.FromResult(filesInFolder as IEnumerable<string>);
        }

        public async Task<string> GetCurrentFolder()
        {
            if (currentFolder == null) return await Task.FromResult("");
            return await Task.FromResult(currentFolder.FullName);
        }

        private async Task CountFiles()
        {
            IEnumerable<DirectoryInfo> subfolders = currentFolder.GetDirectories();

            IEnumerable<FileInfo> files = currentFolder.GetFiles();

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
            await Task.CompletedTask;
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
            var files = new FileInfo[0];
            try
            {
                files = folder.GetFiles();
            }
            catch (UnauthorizedAccessException) {}
            if (files.Count() > 0)
            {
                foreach (var file in files)
                {
                    Size += (ulong)file.Length;
                }
            }

            var folders = new DirectoryInfo[0];
            try
            {
                folders = folder.GetDirectories();
            }
            catch { }
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