using EmpeekTest1.Infrastructure;
using EmpeekTest1.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace EmpeekTest1.Controllers
{
    public class FoldersController : ApiController
    {
        private IFolderNavigationService _service = new FolderService();
        
        

        // GET api/folders
        public async Task<JsonResult<FolderViewModel>> Get()
        {
            
            return Json<FolderViewModel>(new FolderViewModel
            {
                CurrentFolder = await _service.GetCurrentFolder(),
                FilesAboveHundred = await _service.GetFilesAboveHundred(),
                FilesUnderTen = await _service.GetFilesUnderTen(),
                FilesTenToFifty = await _service.GetFilesTenToFifty(),
                Files = await _service.GetFiles()
            });
        }

        // GET api/folders?newFolder=
        public async Task<JsonResult<FolderViewModel>> Get([FromUri]string newFolder)
        {
            if(newFolder == "parentFolder")
            {
                _service.GoToParentFolder();
            }
            else
            {
                _service.GoToFolder(newFolder);
            }
            return Json<FolderViewModel>(new FolderViewModel
            {
                CurrentFolder = await _service.GetCurrentFolder(),
                FilesAboveHundred = await _service.GetFilesAboveHundred(),
                FilesUnderTen = await _service.GetFilesUnderTen(),
                FilesTenToFifty = await _service.GetFilesTenToFifty(),
                Files = await _service.GetFiles()
            });
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
