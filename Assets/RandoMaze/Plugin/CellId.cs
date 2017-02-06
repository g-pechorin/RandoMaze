using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellId
{
	public readonly int i;
	public readonly int j;

	public CellId(int i, int j)
	{
		this.i = i;
		this.j = j;
	}

	public override bool Equals(object obj)
	{
		var other = obj as CellId;

		return null != other && i == other.i && j == other.j;
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public override string ToString() { return "CellId(" + i + "," + j + ")"; }

	public HashSet<CellId> Neighbors
	{
		get
		{
			return new HashSet<CellId>(new CellId[]
			{
				new CellId(i + 1, j),
				new CellId(i - 1, j),
				new CellId(i, j + 1),
				new CellId(i, j - 1),
			});
		}
	}

	public Vector3 ToVector3(float span)
	{
		return new Vector3(i * span, 0, j * span);
	}
}
