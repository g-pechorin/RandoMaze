using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class RandomE
{
	public static E SelectRandom<E>(this System.Random random, ICollection<E> set)
	{
		return (new List<E>(set))[random.Next() % set.Count];
	}
}

public class RandoMaze : MonoBehaviour
{
	public float span = 3.83f;
	public int w = 14;
	public int h = 19;

	public int seed = 314;

	public GameObject[] cells = new GameObject[0];
	public GameObject[] links = new GameObject[0];

	void Start()
	{
		// create all cell-ids
		var maze = new HashSet<CellId>();
		for (int i = 0; i < w; ++i)
			for (int j = 0; j < h; ++j)
				maze.Add(new CellId(i, j));

		// start here
		var start = new CellId(0, 0);

		// seen these
		var seen = new HashSet<CellId>();
		seen.Add(start);
		var todo = new HashSet<CellId>(seen);

		var random = new System.Random(seed);

		// make the connections
		var links = new List<CellId[]>();
		while (seen.Count < maze.Count) // while there are unvisited cells
		{
			var next = random.SelectRandom(todo);

			var neighbors = next.Neighbors;
			neighbors.RemoveWhere(e => seen.Contains(e));
			neighbors.RemoveWhere(e => !maze.Contains(e));

			if (0 == neighbors.Count)
			{
				todo.Remove(next);
				continue;
			}

			var link = random.SelectRandom(neighbors);

			links.Add(new CellId[] { next, link });
			seen.Add(link);
			todo.Add(link);
		}

		// create cell floors
		foreach (var next in seen)
		{
			var floor = Instantiate<GameObject>(random.SelectRandom(cells));

			floor.transform.parent = transform;
			floor.transform.localPosition = next.ToVector3(span);

			floor.name = next.ToString();
		}

		// create cell bridges
		foreach (var next in links)
		{
			var where = 0.5f * (next[0].ToVector3(span) + next[1].ToVector3(span));

			var bridge = Instantiate<GameObject>(random.SelectRandom(this.links));

			bridge.transform.parent = transform;
			bridge.transform.localPosition = where;

			bridge.name = next[0] + "<=>" + next[1];
		}
	}
}
