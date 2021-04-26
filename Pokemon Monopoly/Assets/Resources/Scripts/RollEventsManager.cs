﻿using System.Linq;
using UnityEngine.Events;

public class RollEventsManager
{
    public event UnityAction<DiceRoll> StandardRoll;
    public event UnityAction EarnedBonusRoll;
    public event UnityAction EnteredJailWithDoubles;
    public event UnityAction ExitedJailWithDoubles;
    public event UnityAction FailedEscapeJailWithDouble;

    private readonly DiceRollHistory rollHistory;

    public RollEventsManager()
    {
        rollHistory = new DiceRollHistory(3);
    }

    public void AddRoll(DiceRoll roll, bool inJail)
    {
        rollHistory.AddRoll(roll);
        if (rollHistory.Count == 3) OnThreeRollsInHistory(inJail);
        else
        {
            if (roll.IsDoubleRoll)
            {
                if (inJail)
                {
                    rollHistory.Clear();
                    ExitedJailWithDoubles?.Invoke();
                }
                else
                {
                    EarnedBonusRoll?.Invoke();
                    StandardRoll?.Invoke(roll);
                }
            }
            else
            {
                StandardRoll?.Invoke(roll);
            }
        }
    }

    public void ClearRollHistory()
    {
        rollHistory.Clear();
    }

    private void OnThreeRollsInHistory(bool inJail)
    {
        bool allDoubles = rollHistory.Last3Rolls.All(r => r.IsDoubleRoll);
        if (inJail && !allDoubles)
        {
            rollHistory.Clear();
            FailedEscapeJailWithDouble?.Invoke();
        }
        else if (!inJail)
        {
            if (allDoubles)
            {
                rollHistory.Clear();
                EnteredJailWithDoubles?.Invoke();
            }
            else
            {
                if (rollHistory.LastRoll.IsDoubleRoll)
                {
                    EarnedBonusRoll?.Invoke();
                }
                StandardRoll?.Invoke(rollHistory.LastRoll);
            }
        }
    }
}