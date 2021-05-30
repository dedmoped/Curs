using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Help
{

    public class SaveImage
    {
        public List<string> str = new List<string>();
        public List<IFormFile> files;
        public SaveImage(List<IFormFile> file1)
        {
            files = file1;
        }


        public async Task Run()
        {
            using (var dbx = new DropboxClient("rPWXczyuKPcAAAAAAAAAATZo9acve3FbYnA1Fwol8Flroqb8HBX9iBapvJGePNud"))
            {
                foreach (var file in files)
                {
                    //string file = "appsettings.json";
                    string url = "";
                    string folder = "/UploadPhoto";
                    using (var memory = file.OpenReadStream())
                    {
                        var upload = dbx.Files.UploadAsync(folder + "/" + file.FileName, WriteMode.Overwrite.Instance, body: memory);
                        upload.Wait();
                        try
                        {

                            var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + file.FileName);
                            tx.Wait();
                            url = tx.Result.Url;

                        }
                        catch
                        {
                            var c = dbx.Sharing.ListSharedLinksAsync(folder + "/" + file.FileName);
                            c.Wait();
                            url = c.Result.Links[0].Url;
                        }
                        str.Add(url.Remove(url.Length - 1) + '1');
                    }
                }
            }
        }
    }

}
