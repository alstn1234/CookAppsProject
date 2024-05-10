using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader
{
    public static List<Dictionary<string, string>> Read(string filePath)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        string data = Resources.Load<TextAsset>(filePath).text;
        string[] lines = data.Split('\n');

        string[] key = lines[0].Trim().Split(',');

        for (int i = 1; i < lines.Length - 1; i++)
        {
            var temp = new Dictionary<string, string>();
            string[] line = lines[i].Trim().Split(',');
            for(int n = 0; n < line.Length; n++)
            {
                temp[key[n]] = line[n];
            }
            result.Add(temp);
        }
        return result;
    } 
}
