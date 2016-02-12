using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ROPCommon
{
    public class DownloadData
    {
        public Task<byte[]> Data(string url)
        {
            return Data(new Uri(url));
        }

        public async Task<byte[]> Data(Uri uri)
        {
            if (uri.IsFile)
            {
                return File.ReadAllBytes(uri.AbsolutePath);
            }
            var wc=new WebClient();
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".csv");
            await wc.DownloadFileTaskAsync(uri, tempFile);
            var ret= File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return ret;

        }
    }
}
