using System.Linq;
using UnityEngine;

public class AvatarImageFactory : MonoBehaviour
{
    [SerializeField] private RectTransform[] avatarImagePrefabs;

    public static AvatarImageFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public RectTransform GetAvatarImage(string imageName,
        Transform parent = null, Vector3? scale = null)
    {
        RectTransform avatarImage = avatarImagePrefabs
            .Where(p => p.gameObject.name == imageName).Single();
        if (avatarImage == null) throw new System.ArgumentException("Invalid image name");
        avatarImage = Instantiate(avatarImage, parent);
        if (scale.HasValue)
        {
            avatarImage.parent = null;
            avatarImage.transform.localScale = scale.Value;
            avatarImage.parent = parent;
        }
        return avatarImage;
    }
}
