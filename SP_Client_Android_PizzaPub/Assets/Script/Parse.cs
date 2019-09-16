using UnityEngine;
using System.Collections;

public class Parse
{
	static string[] orderTailHigh = { "ST", "ND", "RD", "TH" };
	static string[] orderTailLow = { "st", "nd", "rd", "th" };

	static public string OrderTailHigh( int order0 )
	{
		return orderTailHigh [Mathf.Clamp (order0, 0, 3)]; 
	}

	static public string OrderTailLow( int order0 )
	{
		return orderTailLow [Mathf.Clamp (order0, 0, 3)];
	}

	static public int[] Point2( string str )
	{
		int[] point = new int[2];
		string[] word = str.Split (',');
		point [0] = int.Parse (word [0]);
		point [1] = int.Parse (word [1]);
		return point;
	}

	static string[] DIGIT = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

	static public string Digit( int digit ) 
	{
		return DIGIT [digit];
	}

	static public string Vector2( Vector2 v )
	{
		return Vector2 (v, 3);
	}

	static public string Vector2( Vector2 v, int cntUnderPoint )
	{
		string format = "0";
		if (cntUnderPoint > 0)
			format += ".";

		for (int i = 0; i < cntUnderPoint; i++)
			format += "0";

		return
			v.x.ToString (format) + ", " +
			v.y.ToString (format);
	}

	static public Vector2 Vector2( string str )
	{
		string[] word = str.Split (',');
		return new Vector2( float.Parse ( word[0] ),
		                   float.Parse ( word[1] ) );
	}

	static public string Vector3( Vector3 v )
	{
		return Vector3 (v, 3);
	}

	static public string Vector3( Vector3 v, int cntUnderPoint )
	{
		string format = "0";
		if (cntUnderPoint > 0)
			format += ".";
		
		for (int i = 0; i < cntUnderPoint; i++)
			format += "0";

		return
			v.x.ToString (format) + ", " +
			v.y.ToString (format) + ", " +
			v.z.ToString (format);
	}
	
	static public Vector3 Vector3( string str )
	{
		string[] word = str.Split (',');
		return new Vector3( float.Parse ( word[0] ),
		                   float.Parse ( word[1] ),
		                   float.Parse ( word[2] ) );
	}
	
	static public void RangeVector2( string str, out Vector2 s, out Vector2 e )
	{
		string[] word = str.Split (',');
		string[] x = word[0].Split ('~');
		string[] y = word[1].Split ('~');
		
		s = new Vector2( float.Parse( x[0] ), float.Parse( y[0] ) ) ;
		e = new Vector2( x.Length > 1 ? float.Parse (x [1]) : s.x,
		                y.Length > 1 ? float.Parse (y [1]) : s.y);
	}
	
	static public void RangeVector3( string str, out Vector3 s, out Vector3 e )
	{
		string[] word = str.Split (',');
		string[] x = word[0].Split ('~');
		string[] y = word[1].Split ('~');
		string[] z = word[2].Split ('~');
		
		s = new Vector3( float.Parse( x[0] ), float.Parse( y[0] ), float.Parse( z[0] ) ) ;
		e = new Vector3( x.Length > 1 ? float.Parse (x [1]) : s.x,
		                y.Length > 1 ? float.Parse (y [1]) : s.y,
		                z.Length > 1 ? float.Parse (z [1]) : s.z );
	}
	
	static public Color Color( string str )
	{
		string[] word = str.Split (',');

		float r = float.Parse (word [0]);

		if (word.Length == 1)
			return new Color (r, r, r, r);
		else if (word.Length == 2)
			return new Color (r, r, r, float.Parse (word [1]));
				
		return new Color( r,
		                  float.Parse ( word[1] ),
		                  float.Parse ( word[2] ),
		                  float.Parse ( word[3] ));
	}
	
	static public Color Color32( string str )
	{
		string[] word = str.Split (',');
		return new Color32( byte.Parse ( word[0] ),
		                   byte.Parse ( word[1] ),
		                   byte.Parse ( word[2] ),
		                   byte.Parse ( word[3] ));
	}

	static public T ToEnum<T> (string strName)
	{
		return (T)System.Enum.Parse(typeof(T), strName);
	}		

	const string KOREAN_TIME_KEY = "KoreanTime";
	const string KOREAN_DATE_KEY = "KoreanDate";

	static public string ConvertKoreanTimeToLocal(string strInput)
	{		
		string strOut = "";

		if (string.IsNullOrEmpty (strInput))
			return strOut;
		
		while (true) {
			int idx = strInput.IndexOf (KOREAN_TIME_KEY + "'");
			if (idx < 0)
				break;

			string old = strInput.Substring (idx, KOREAN_TIME_KEY.Length + "'00:00'".Length);
			string hour = old.Substring (KOREAN_TIME_KEY.Length + "'".Length, 2);
			string minute = old.Substring (KOREAN_TIME_KEY.Length + "'00:".Length, 2);

			int h, m;
			if (int.TryParse (hour, out h) == false)				
				break;

			if (int.TryParse (minute, out m) == false)				
				break;

			System.DateTime utcNow = System.DateTime.UtcNow;
			System.DateTime utcTime = new System.DateTime (
				utcNow.Year, utcNow.Month, utcNow.Day, h, m, 0, System.DateTimeKind.Utc);

			System.DateTime utcPlusTime = utcTime.AddHours (-9);
			System.DateTime localTime = utcPlusTime.ToLocalTime ();
			string time = localTime.Hour.ToString( "00" ) + ":" + localTime.Minute.ToString( "00" );

			strInput = strInput.Replace (old, time);
		}

		return strInput;
	}

	static public string ConvertKoreanDateToLocal(string strInput, bool showTime = true)
	{
		string strOut = "";

		if (string.IsNullOrEmpty (strInput))
			return strOut;

		while (true) {
			int idx = strInput.IndexOf (KOREAN_DATE_KEY + "'");
			if (idx < 0)
				break;

			string old = strInput.Substring (idx, KOREAN_DATE_KEY.Length + "'0000. 00. 00'".Length + " ".Length + "'00:00'".Length);
			string year = old.Substring(KOREAN_DATE_KEY.Length + "'".Length, 4);
			string month = old.Substring(KOREAN_DATE_KEY.Length + "'0000. ".Length, 2);
			string day = old.Substring(KOREAN_DATE_KEY.Length + "'0000. 00. ".Length, 2);

			string hour = old.Substring (KOREAN_DATE_KEY.Length + "'0000. 00. 00'".Length + " ".Length + "'".Length, 2);
			string minute = old.Substring (KOREAN_DATE_KEY.Length + "'0000. 00. 00'".Length + " ".Length + "'00:".Length, 2);

			int y, mon, d, h, m;
			if(int.TryParse(year, out y) == false)
				break;
			if(int.TryParse(month, out mon) == false)
				break;
			if(int.TryParse(day, out d) == false)
				break;
			if (int.TryParse (hour, out h) == false)				
				break;
			if (int.TryParse (minute, out m) == false)				
				break;

			System.DateTime utcNow = System.DateTime.UtcNow;
			System.DateTime utcTime = new System.DateTime (
				y, mon, d, h, m, 0, System.DateTimeKind.Utc);

			System.DateTime utcPlusTime = utcTime.AddHours (-9);
			System.DateTime localTime = utcPlusTime.ToLocalTime ();

/*#if UNITY_EDITOR
			string date = localTime.ToString("D", new System.Globalization.CultureInfo("ko-kr"));
#else
			string date = localTime.ToString("D", System.Globalization.CultureInfo.CurrentCulture);
#endif*/
			string date = localTime.Year.ToString("0000") + ". " + localTime.Month.ToString("00") + ". " + localTime.Day.ToString("00");
			if(showTime)
				date +=  " " + localTime.Hour.ToString( "00" ) + ":" + localTime.Minute.ToString( "00" );

			strInput = strInput.Replace (old, date);
		}

		return strInput;
	}
}
