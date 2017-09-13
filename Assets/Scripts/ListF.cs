using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// List functions
/// </summary>

public static class Listf {

	public static void Shuffle<T>( this IList<T> list )
	{
		int n = list.Count;
		Random rnd = new Random();
		while(n > 1)
		{
			int k = ( rnd.Next(0,n) % n );
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}