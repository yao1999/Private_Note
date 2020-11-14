﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Private_Note.Data;
using Private_Note.Models;

namespace Private_Note.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private readonly PrivateNoteDBContext _context;
        public UserHomeController(PrivateNoteDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Files> AllFiles = _context.Files;
            return View(AllFiles);
        }

        public IActionResult FileUpload(IFormFile files)
        {
            if (files != null)
            {
                var FileExtension = Path.GetExtension(Path.GetFileName(files.FileName));
                if(AllFileType().ContainsKey(FileExtension))
                {
                    if (files.Length > 0)
                    {
                        var objfiles = new Files()
                        {
                            FileName = Path.GetFileNameWithoutExtension(files.FileName), //Getting FileName 
                            FileType = Path.GetExtension(Path.GetFileName(files.FileName)), //Getting Extension
                            CreatedDate = DateTime.Now //Getting current time
                        };

                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            objfiles.File = target.ToArray(); //Getting file data
                        }

                        _context.Files.Add(objfiles);
                        _context.SaveChanges();

                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "UserHome");
            }

            return RedirectToAction("Index", "UserHome");
        }

        public IActionResult FileDownload(string givenFileName)
        {
            //var currentFile = _context.Files.SingleOrDefault(r => r.FileName == givenFileName);
            var currentFile = _context.Files.SingleOrDefault(r => r.FileName == "privateNoteTester");
            if(currentFile != null)
            {
                var memory = new MemoryStream(currentFile.File);
                memory.Position = 0;
                var fileNameAndExtension = string.Concat(currentFile.FileName, currentFile.FileType);
                return File(memory, AllFileType()[currentFile.FileType], fileNameAndExtension);
            }
            return RedirectToAction("Index", "UserHome");
        }

        //public IActionResult FileSearch() // return filename to copy thing 
        //{
        //    return RedirectToAction("Index", "UserHome");
        //}

        private Dictionary<string, string> AllFileType()
        {
            var fileTypes = new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".docx", "application/vnd.ms-word" },
                { ".doc", "application/vnd.ms-word" },
                { ".png", "image/png" },
                { ".jpg", "image/jpg" },
                { ".jpeg", "image/jpeg" },
                { ".pptx", "application/vnd.ms-powerpoint" }
            };
            return fileTypes;
        }
    }
}