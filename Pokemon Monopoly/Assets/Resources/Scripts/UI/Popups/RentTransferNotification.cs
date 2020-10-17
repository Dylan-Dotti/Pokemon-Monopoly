using UnityEngine;
using UnityEngine.UI;

public class RentTransferNotification : ImageTransferNotification
{
    [SerializeField] private Text rentValueText;

    private MonopolyPlayer payingPlayer;
    private PropertyData rentedProperty;

    public MonopolyPlayer PayingPlayer
    {
        get => payingPlayer;
        set
        {
            payingPlayer = value;
            var avatarImage = payingPlayer.GetNewAvatarImage(scale: Vector3.one * 0.5f);
            TransDisplay.LeftContent = avatarImage;
            UpdateText();
        }
    }

    public PropertyData RentedProperty
    {
        get => rentedProperty;
        set
        {
            rentedProperty = value;
            NotificationSprite = rentedProperty.PropertySprite;
            var avatarImage = rentedProperty.Owner.GetNewAvatarImage(scale: Vector3.one * 0.5f);
            TransDisplay.RightContent = avatarImage;
            UpdateText();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void UpdateText()
    {
        if (payingPlayer != null && rentedProperty != null)
        {
            MonopolyPlayer receivingPlayer = rentedProperty.Owner;
            string payingPlayerName = payingPlayer.IsLocalPlayer ?
                "You" : payingPlayer.PlayerName;
            string receivePlayerName = receivingPlayer.IsLocalPlayer ?
                "you" : receivingPlayer.PlayerName;
            NotificationText = $"{payingPlayerName} " +
                $"landed on {rentedProperty.PropertyName} and paid " +
                $"{SpecialStrings.POKEMONEY_SYMBOL + rentedProperty.CurrentRent} " +
                $"in rent to {receivePlayerName}";
            rentValueText.text = rentedProperty.CurrentRent.ToPokeMoneyString();
        }
    }
}
