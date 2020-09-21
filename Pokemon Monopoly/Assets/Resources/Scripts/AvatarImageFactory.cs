using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarImageFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] avatarImagePrefabs;

    public GameObject GetAvatarImage(string imageName, Transform parent = null,
        Vector3? scale = null)
    {
        GameObject avatarImage = avatarImagePrefabs
            .Where(p => p.name == imageName).FirstOrDefault();
        if (avatarImage == null) throw new System.ArgumentException("Invalid image name");
        avatarImage = Instantiate(avatarImage, parent);
        if (scale.HasValue) avatarImage.transform.localScale = scale.Value;
        return avatarImage;
    }
}
