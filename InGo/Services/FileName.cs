using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Services
{
    public class FileName
    {
        private const string filesDir = "wwwroot/sources";
        private uint currentIndex = 0;

        public FileName()
        {
            Directory.CreateDirectory(filesDir);
            string[] files = Directory.GetFiles(filesDir);
            if (files.Length == 0)
            {
                currentIndex = 0;
                return;
            }
            string[] fileNameParts = files.Last().Split("\\");
            string fileName = fileNameParts[fileNameParts.Length - 1].Split(".")[0];
            currentIndex = uint.Parse(fileName) + 1;
        }

        public string GetFileName(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            return $"{filesDir}/{currentIndex++}{extension}";
        }
    }
}
