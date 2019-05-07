using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVReader 
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string path)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

		#if UNITY_ANDROID
		WWW www = new WWW(path);
		while(!www.isDone)
			continue;

		string text = www.text.Trim();
		#else
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string text = sr.ReadToEnd();
		#endif

        string[] lines = Regex.Split(text, LINE_SPLIT_RE);
        if (lines.Length <= 1)
            return list;

        string[] header = Regex.Split(lines[0], SPLIT_RE);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "")
                continue;

            Dictionary<string, object> entry = new Dictionary<string, object>();
            for (int j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                value = value.Replace("#n", "\n");

                object finalValue = value;

                int n = 0;
                float f = 0f;
                if (int.TryParse(value, out n))         finalValue = n;
                else if (float.TryParse(value, out f))  finalValue = f;

                entry[header[j]] = finalValue;
            }

            list.Add(entry);
        }

        return list;
    }
}