using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceRollHistory
{
    private readonly List<DiceRoll> history;
    private readonly int maxSize;

    public DiceRoll LastRoll => history.FirstOrDefault();
    public IEnumerable<DiceRoll> Last3Rolls => new List<DiceRoll>(history.Take(3));
    public int Count => history.Count;

    public DiceRollHistory(int maxSize)
    {
        this.maxSize = maxSize;
        history = new List<DiceRoll>(maxSize + 1);
    }

    public void AddRoll(DiceRoll roll)
    {
        Debug.Log("DiceRollHistory adding roll");
        history.Insert(0, roll);
        if (history.Count > maxSize)
        {
            history.RemoveAt(history.Count - 1);
        }
    }

    public void Clear()
    {
        history.Clear();
    }
}
