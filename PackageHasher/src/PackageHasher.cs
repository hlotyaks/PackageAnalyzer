using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;

namespace PackageAnalyzer
{
    public class PackageHasher
    {
        public Dictionary<string, string> HashFolders(List<string> folders)
        {
            Dictionary<string, string> hasedFolders = new Dictionary<string, string>();


            return hasedFolders;
        }
        async Task<string> HashFolder(DirectoryInfo folder, bool excludeRoot, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            string rootPath = String.Empty;

            if(excludeRoot)
            {
                rootPath = folder.FullName.Substring(0, folder.FullName.Length - folder.FullName.Split('\\').Last().Length );
            }   

            using(var alg = MD5.Create())
            {

                var result = await alg.ComputeHashAsync(folder.EnumerateFiles(searchPattern, searchOption), rootPath);

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();

            }
        }
    }
}
