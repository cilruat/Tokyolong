using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class STR
{
	static StringBuilder builder = new StringBuilder();
	static string[] COLOR_PRESET	= new string[] { "ff0000", "ffff00", "ffffff", "33ff33", "0088ff", "33ccff", "cc44ff", "ff6600", "999999", "000000", "cccccc", "77ff77", };

	static string _AddColorTag (string text, int presetIdx)
	{
		builder.Clear();
		builder.AppendFormat("<color='#{0}'>{1}</color>", COLOR_PRESET[presetIdx], text);
		return builder.ToString();
	}

	static string _AddSizeTag (string text, int size)
	{
		builder.Clear();
		builder.AppendFormat("<size={0}>{1}</size>", size.ToString(), text);
		return builder.ToString();
	}
	    
	static public string SIZE (string text, int size) { return _AddSizeTag(text, size); }
	static public string RED (string text)			{ return _AddColorTag (text, 0); }
	static public string YELLOW (string text)		{ return _AddColorTag (text, 1); }
	static public string WHITE (string text)		{ return _AddColorTag (text, 2); }
	static public string GREEN (string text)		{ return _AddColorTag (text, 3); }
	static public string BLUE (string text)			{ return _AddColorTag (text, 4); }
	static public string SKY (string text)			{ return _AddColorTag (text, 5); }
	static public string PURPLE (string text)		{ return _AddColorTag (text, 6); }
	static public string ORANGE (string text)		{ return _AddColorTag (text, 7); }
	static public string GRAY (string text)			{ return _AddColorTag (text, 8); }
	static public string BLACK (string text)		{ return _AddColorTag (text, 9); }
	static public string LIGHTGRAY (string text)	{ return _AddColorTag (text, 10); }
	static public string BOOL (bool val)			{ return val ? GREEN(val.ToString()) : RED(val.ToString()); }

	static public string P2P (string text)		{ return STR.GREEN("[P2P] ") + STR.WHITE(text); }

	static public string TIME_HHMMSS (int totalSec)
	{
		int hour = (int)(totalSec / 3600);
		int min = (int)((totalSec % 3600) / 60);
		int sec = totalSec % 60;

		builder.Clear();
		builder.AppendFormat("{0}:{1}:{2}", hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
		return builder.ToString();
	}

	static public string TIME_MMSS (int totalSec)
	{
		int min = (int)(totalSec / 60);
		int sec = totalSec % 60;

		builder.Clear();
		builder.AppendFormat("{0}:{1}", min.ToString("00"), sec.ToString("00"));
		return builder.ToString();
	}		

	const float MS_FONT_SIZE_RATIO = .75f;
	static public string TIME_MMSSMM (float elapsed, int fontSize = -1)
	{
		int min = (int)(elapsed * (1 / 60f));
		int sec = (int)elapsed % 60;
		int ms = (int)(elapsed * 100f) % 100;

		builder.Clear();
		builder.AppendFormat("{0}:{1}", min.ToString("00"), sec.ToString("00"));

		if (fontSize > 0)
			builder.AppendFormat("<size={0}>.{1}</size>", Mathf.CeilToInt(fontSize * MS_FONT_SIZE_RATIO).ToString(), ms.ToString("00"));
		else
			builder.AppendFormat(".{0}", ms.ToString("00"));

		return builder.ToString();
	}

	static public int GetStringLengthForUTF16(string target)
	{
		string str = "";
		int cnt = 0;
		foreach(char ch in target)
		{
			if(ch < 128)	// ascii
				cnt += 1;
			else
				str += ch;
		}

		return cnt + Encoding.Unicode.GetByteCount(str);
	}
}

public static class StringBuilderExtention
{
	public static void Clear (this StringBuilder sb)
	{
		sb.Remove(0, sb.Length);
	}
}   
