﻿using System.IO;
using System.Collections.Generic;
using DirCompare.model;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System;

namespace DirCompare.src
{
    public class Comparator
    {
        private DirectoryInfo dir1;
        private DirectoryInfo dir2;

        public string result1;
        public string result2;

        public Comparator(string _dir1, string _dir2)
        {
            dir1 = new DirectoryInfo(_dir1);
            dir2 = new DirectoryInfo(_dir2);
        }

        public string Execute()
        {
            var dict1 = GetDictionary(dir1);
            var dict2 = GetDictionary(dir2);

            StringBuilder sb = new StringBuilder();

            result1 = $"Files found in directory 1 but not in directory 2:\n\n{Compare(dict1, dict2)}";
            result2 = $"Files found in directory 2 but not in directory 1:\n\n{Compare(dict2, dict1)}";

            dict1.OrderByDescending(x => x.Value.Count)
                  .ToList()
                  .ForEach(x => sb.AppendLine($"{x.Value.Count} - {x.Key.Name}"));

            return sb.ToString();
        }

        private string Compare(Dictionary<FileMetadata, List<string>> dict1, Dictionary<FileMetadata, List<string>> dict2)
        {
            StringBuilder result = new StringBuilder();

            int count = 0;

            foreach (var k in dict1.Keys.OrderBy(x => x.Name))
            {
                if (dict2.ContainsKey(k))
                    continue;

                count++;

                result.AppendLine(k.Name);
                dict1[k].OrderBy(x => x).ToList().ForEach(x => result.AppendFormat($"\t{x}\n"));
            }

            StringBuilder finalResult = new StringBuilder();
            finalResult.AppendLine($"{dict1.Keys.Count} files total in directory");
            finalResult.AppendLine($"{count} unique files found");
            finalResult.AppendLine();
            finalResult.AppendLine(result.ToString());


            return finalResult.ToString();
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
                    Size = f.Length,
                    MD5 = GetMD5(f)
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

            foreach (var d in dir.GetDirectories())
            {
                GetDictionary(d, list);
            }

            return list;
        }

        private string GetMD5(FileInfo f)
        {
            if (!FileMetadata.checkContent)
                return null;

            try
            {
                using (var md5Hash = MD5.Create())
                using (var sw = new StreamReader(f.FullName))
                {
                    sw.ReadToEnd();
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sw.ReadToEnd()));

                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        result.Append(data[i].ToString("x2"));
                    }

                    return result.ToString();
                }
            }
            catch(OutOfMemoryException)
            {
                return "";
            }
        }
    }
}
