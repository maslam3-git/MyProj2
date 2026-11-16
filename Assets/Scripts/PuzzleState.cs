
using System.Collections.Generic;

public class PuzzleState
{
    private readonly HashSet<int> collectedCrystals =
        new HashSet<int>();

    public int Count => collectedCrystals.Count;

    public bool Collect(int id)
    {
        return collectedCrystals.Add(id);
    }
}
