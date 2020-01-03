using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

namespace PackageAnalyzer
{
    public class PackageHasher
    {
        public async Task<Dictionary<string, string>> HashFoldersAsync(List<string> folders, string rootPath)
        {
            Dictionary<string, string> hashedFolders = new Dictionary<string, string>();

            IEnumerable<Task<(string folder, string folderwithhash)>> hashFolderTasksQuery = 
                folders.Select( f => HashFolderAsync(new DirectoryInfo(f), rootPath));

            List<Task<(string folder, string folderwithhash)>> hashFolderTasks = hashFolderTasksQuery.ToList();

            while (hashFolderTasks.Count > 0)
            {
                Task<(string folder, string folderwithhash)> firstTask = await Task.WhenAny(hashFolderTasks);

                hashFolderTasks.Remove(firstTask);

                (string folder, string folderwithhash) HashFolderValue = await firstTask;

                hashedFolders.Add(HashFolderValue.folder, HashFolderValue.folderwithhash);
            }

            return hashedFolders;
        }


        async Task<(string folder, string folderwithhash)> HashFolderAsync(DirectoryInfo folder, string rootPath = "", string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            using(var alg = MD5.Create())
            {
                var result = await alg.ComputeHashAsync(folder.EnumerateFiles(searchPattern, searchOption), rootPath);

                // Folder has starts with folder name.  
                // Format - name.hash
                StringBuilder sb = new StringBuilder();
                sb.Append(folder.Name);
                sb.Append(".");

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return (folder: folder.Name, folderwithhash: sb.ToString());

            }
        }
    }
}
