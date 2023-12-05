namespace InstaHashtagUsage.ClassLibrary.Models;

public class HashtagCountTable
{
	/// <summary>
	/// Each array represents a category of usage count for hashtag.
	/// </summary>
	public List<HashtagCountPair>[] Table { get; private set; }

	/// <summary>
	/// Represent tresholds counts what define categories.
	/// </summary>
	public int[] Thresholds { get; private set; }
	public int MaxCount => Table.Max(list => list.Count);

	public HashtagCountTable(int[] thresholds, List<HashtagCountPair>[] table)
	{
		Thresholds = thresholds;
		Table = table;
	}

	public void Add(HashtagCountPair pair)
	{
		// Checks what column fits hachtag based on thresholds values.
		for (int i = 0; i < Table.Length; i++)
		{
			if (pair.Count < Thresholds[int.Min(i, Thresholds.Length - 1)]) continue;
			Table[i].Add(pair);
			return;
		}
		Table.Last().Add(pair);
	}

	public HashtagCountPair Get(int row, int column)
	{
		if (row < Table[column].Count)
		{
			return Table[column][row];
		}
		return null;
	}
}
