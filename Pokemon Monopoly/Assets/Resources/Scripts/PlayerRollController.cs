using UnityEngine;

public class PlayerRollController : MonoBehaviour
{
    [SerializeField] private DiceRoller roller;

    private MonopolyPlayer friendlyPlayer;
    private RollEventsManager rollEvents;

    private void Awake()
    {
        rollEvents = new RollEventsManager();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    public void RollDice()
    {
        roller.RollComplete += OnRollComplete;
        roller.RollDice(friendlyPlayer == null ? "Player" : friendlyPlayer.PlayerName);
    }

    public void RollDice(int result1, int result2)
    {
        roller.RollComplete += OnRollComplete;
        roller.RollDice(friendlyPlayer == null ? 
            "Player" : friendlyPlayer.PlayerName,
            result1, result2);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer)
        {
            friendlyPlayer = player;
            rollEvents.StandardRoll += friendlyPlayer.OnStandardRoll;
            rollEvents.EarnedBonusRoll += friendlyPlayer.OnEarnedAdditionalMove;
            rollEvents.EnteredJailWithDoubles += friendlyPlayer.OnEnterJailWithDoubles;
            rollEvents.ExitedJailWithDoubles += friendlyPlayer.OnExitJailWithDoubles;
            rollEvents.FailedEscapeJailWithDouble += friendlyPlayer.OnFailExitJailWithDoubles;
        }
    }

    private void OnRollComplete(DiceRoll roll)
    {
        roller.RollComplete -= OnRollComplete;
        if (friendlyPlayer != null)
        {
            rollEvents.AddRoll(roll, friendlyPlayer.InJail);
        }
    }
}
