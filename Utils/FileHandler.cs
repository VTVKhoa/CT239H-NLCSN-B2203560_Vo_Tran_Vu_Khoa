using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace A23017_Cloud.Utils
{
    internal class FileHandler
    {
        private readonly Random random;
        private readonly string rootPath;

        public FileHandler()
        {
            random = new Random();
            rootPath = HostingEnvironment.MapPath("/");
        }

        public string Save(HttpPostedFileBase file, string path)
        {
            string tenFile = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            int doDaiTenFile = tenFile.Length > 230 ? 230 : tenFile.Length;
            string time = DateTime.Now.ToString("HHmmssddMMyyyy");
            string rand = random.Next(100, 1000).ToString();
            string tenFileMoi = $"{tenFile.Substring(0, doDaiTenFile)}_{time}_{rand}{extension}";
            string duongDan = Path.Combine(rootPath, path, tenFileMoi);
            file.SaveAs(duongDan);
            return tenFileMoi;
        }

        public void Delete(string fileName, string path)
        {
            string duongDan = Path.Combine(rootPath, path, fileName);
            if (File.Exists(duongDan))
            {
                File.Delete(duongDan);
            }
        }
    }
}