
public abstract class ImageTransferNotification : SimpleImageNotification
{
    protected TransferDisplay TransDisplay { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        TransDisplay = transform.Find("Panel Content")
            .Find("Transfer Display").GetComponent<TransferDisplay>();
    }
}
