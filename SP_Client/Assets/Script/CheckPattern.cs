using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CheckPattern : MonoBehaviour
{
	public delegate bool CorrectPos( float c, float p );

	public struct PATTERN
	{
		public int idxHORIZON;
		public int idxVERTICAL;
		public CorrectPos HORIZON;
		public CorrectPos VERTICAL;
		public PATTERN( int idxHOR, CorrectPos HOR, int idxVER, CorrectPos VER )
		{
			idxHORIZON = idxHOR;
			HORIZON = HOR;
			idxVERTICAL = idxVER;
			VERTICAL = VER;
		}
	}

	PATTERN[] pattern =
	{
		new PATTERN (0, PASS, 0, PASS),
		new PATTERN (0, LEFT, 0, DOWN),	
		new PATTERN (0, RIGHT, 1, DOWN),
		new PATTERN (0, LEFT, 2, DOWN),
		new PATTERN (3, LEFT, 3, UPPER),
		new PATTERN (0, RIGHT, 4, UPPER),
		new PATTERN (5, LEFT, 5, UPPER),
	};

	List<Vector2> approval = new List<Vector2>();
	static bool PASS( float c, float p ) { return true; }
	static bool RIGHT( float c, float p ) { return c < p; }
	static bool LEFT( float c, float p ) { return c > p; }
	static bool UPPER( float c, float p ) { return c < p; }
	static bool DOWN( float c, float p ) { return c > p; }

	public UnityEvent callback;

	void Update ()
	{
		Vector2 m = Input.mousePosition;

		if (Input.GetMouseButtonDown (0)) {			
			approval.Clear ();
			approval.Add ( m );
		} else if( approval.Count > 0 )
		{
			if (Input.GetMouseButton (0))
			{
				int check = approval.Count;
				if (check < pattern.Length) {
					PATTERN p = pattern [check];
					if (p.HORIZON (approval [p.idxHORIZON].x, m.x) && p.VERTICAL (approval [p.idxVERTICAL].y, m.y))
						approval.Add (m);
				} else {				
					if (approval [0].y - approval [3].y > Screen.height * .5f)
					{
						Debug.Log ("Received Pattern Event!");
						callback.Invoke();
					}
					
					approval.Clear ();
				}
			}
		}
	}
}
