using UnityEngine;
using System;
using System.Collections;

/* 
 * This class was created to store all relevant data in the list of scores.
 * This class can be used as it is with name and score. It can also have additional values added to it to increase the amount of storable information
 * 
 * For example, 
 * If your implementation included level speed runs (float value), the time could be stored and shown within the scoreboard by simply adding a new variable and possibly a comparison function
 * If problems occur with the addition of new variables make sure the comparison functions check the correct values against one and other and not the same as the other functions
 * Using the current existing functions as a starting point would help overcome any initial problems
 */
 
public class ScoreHolder : IEquatable<ScoreHolder> , IComparable<ScoreHolder>
{
	public string Name {get; set;}
	public int Value {get; set;}

	public ScoreHolder(string name, int value)
	{
		this.Name = name;
		this.Value = value;
	}

	/*
     * The following set of functions are implemented to help with sorting the list of values numerically in Highscore.cs
     */
	public override bool Equals(object obj)
	{
		if (obj == null) 
			return false;

		ScoreHolder objAsPart = obj as ScoreHolder;

		if (objAsPart == null) 
			return false;

		else 
			return Equals(objAsPart);
	}

	public int SortByNameAscending(string name, string otherName)
	{
		return name.CompareTo(otherName);
	}
	
	// Default comparer for Part type. 
	public int CompareTo(ScoreHolder comparePlayer)
	{
		// A null value means that this object is greater. 
		if (comparePlayer == null)
			return 1;
		
		else 
			return this.Value.CompareTo(comparePlayer.Value);
	}

	public override int GetHashCode()
	{
		return Value;
	}

	public bool Equals(ScoreHolder other)
	{
		if (other == null) 
			return false;

		return (this.Value.Equals(other.Value));
	}
}

