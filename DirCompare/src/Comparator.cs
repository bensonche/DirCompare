using System.IO;
using System.Collections.Generic;
using DirCompare.model;
using System.Text;
using System.Linq;

namespace DirCompare.src
{
    public class Comparator
    {
        private DirectoryInfo dir1;
        private DirectoryInfo dir2;

        public Comparator(string _dir1, string _dir2)
        {
            dir1 = new DirectoryInfo(_dir1);
            dir2 = new DirectoryInfo(_dir2);
        }

        public string Compare()
        {
            var dict1 = GetDictionary(dir1);
            var dict2 = GetDictionary(dir2);

            StringBuilder sb = new StringBuilder();

            dict1.OrderByDescending(x => x.Value.Count)
                  .ToList()
                  .ForEach(x => sb.AppendLine($"{x.Value.Count} - {x.Key.Name}"));

            //  x => sb.AppendLine($"{x.Value.Count} - {x.Key.Name}")
            return sb.ToString();
        }

        private Dictionary<FileMetadata, List<string>> GetDictionary(DirectoryInfo dir, Dictionary<FileMetadata, List<string>> list = null)
        {
            if (list == null)
                list = new Dictionary<FileMetadata, List<string>>();

            foreach (var f in dir.GetFiles())
            {
                FileMetadata fm = new FileMetadata
                {
                    Name = f.Name,
                    ModifyDate = f.LastWriteTime,
                    Size = f.Length
                };

                if (!list.ContainsKey(fm))
                {
                    var value = new List<string>();
                    value.Add(f.FullName);
                    list.Add(fm, value);
                }
                else
                    list[fm].Add(f.FullName);
            }

            foreach(var d in dir.GetDirectories())
            {
                GetDictionary(d, list);
            }
            
            return list;
        }
    }
}
