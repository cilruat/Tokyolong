using UnityEngine;
using System.Collections;

namespace RFLib
{
	/// <summary>
	/// RFUtils is a mish-mash of utility functions that have yet to find a home elsewhere.
	///   
	/// </summary>
	public static class RFUtils 
	{
		/// <summary>
		/// Determines if is specified val is a power of 2
		/// </summary>
		/// <returns><c>true</c> If val is a power of 2; otherwise, <c>false</c>.</returns>
		/// <param name="val">Value to check</param>
		public static bool IsPowerOf2(int val)
		{
			return (val > 0) && ((val & (val - 1)) == 0);
		}
		
		
		// Credit: http://wiki.unity3d.com/index.php?title=HexConverter
		public static string ColorToHex(Color32 color)
		{
			string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
			return hex;
		}
 
		
		

		/// <summary>
		/// Remvoe and destroy children from a given transform.  
		/// Note: If activated from the editor, calls DestroyImmediate
		/// </summary>
		/// <param name="theParent">The parent.</param>
		public static void DestroyTransformChildren(Transform theParent)
		{
			while( theParent.childCount > 0 )
			{
				Transform t = theParent.GetChild( 0 );
				t.SetParent( null );
				#if UNITY_EDITOR
					GameObject.DestroyImmediate(t.gameObject);
				#else
					GameObject.Destroy(t.gameObject);
				#endif
			}
		}

	}
}
