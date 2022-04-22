using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquarePositionsTestManager : MonoBehaviour
{
    [SerializeField] private int maxActiveAvatars = 6;
    [SerializeField] private int maxActiveJailAvatars = 6;

    [Header("Spawn Positions")]
    [SerializeField] private Transform squareSpawn;
    [SerializeField] private Transform avatarSpawn;
    [SerializeField] private Transform avatarDespawn;

    [Header("Controls")]
    [SerializeField] private Button useStreetSquareButton;
    [SerializeField] private Button useCornerSquareButton;
    [SerializeField] private Button useJailSquareButton;
    [SerializeField] private Button addAvatarButton;
    [SerializeField] private Button removeAvatarButton;
    [SerializeField] private Button addJailAvatarButton;
    [SerializeField] private Button removeJailAvatarButton;

    [Header("Prefabs")]
    [SerializeField] private BoardSquare streetSquarePrefab;
    [SerializeField] private BoardSquare cornerSquarePrefab;
    [SerializeField] private BoardSquare jailSquarePrefab;
    [SerializeField] private PlayerAvatar avatarPrefab;

    private BoardSquare boardSquare;
    private Queue<PlayerAvatar> activeAvatars;
    private Queue<PlayerAvatar> activeJailAvatars;
    private PlayerAvatar preparedAvatar;

    private void Awake()
    {
        activeAvatars = new Queue<PlayerAvatar>();
        activeJailAvatars = new Queue<PlayerAvatar>();

        addAvatarButton.onClick.AddListener(OnAddAvatarClicked);
        removeAvatarButton.onClick.AddListener(OnRemoveAvatarClicked);
        addJailAvatarButton.onClick.AddListener(OnAddJailAvatarClicked);
        removeJailAvatarButton.onClick.AddListener(OnRemoveJailAvatarClicked);
        useStreetSquareButton.onClick.AddListener(SpawnStreetSquare);
        useCornerSquareButton.onClick.AddListener(SpawnCornerSquare);
        useJailSquareButton.onClick.AddListener(SpawnJailSquare);

        removeAvatarButton.interactable = false;
        removeJailAvatarButton.interactable = false;
    }

    private void Start()
    {
        SpawnJailSquare();
        SpawnPreparedAvatar();
    }

    private void OnAddAvatarClicked()
    {
        preparedAvatar.LerpToSquare(boardSquare, false, false, false);
        activeAvatars.Enqueue(preparedAvatar);
        if (activeAvatars.Count < maxActiveAvatars || 
            activeJailAvatars.Count < maxActiveJailAvatars) SpawnPreparedAvatar();
        addAvatarButton.interactable = activeAvatars.Count < maxActiveAvatars;
        removeAvatarButton.interactable = activeAvatars.Count > 0;
    }

    private void OnRemoveAvatarClicked()
    {
        StartCoroutine(DespawnAvatarCR(activeAvatars.Dequeue()));
        addAvatarButton.interactable = activeAvatars.Count < maxActiveAvatars;
        removeAvatarButton.interactable = activeAvatars.Count > 0;
    }

    private void OnAddJailAvatarClicked()
    {
        preparedAvatar.InJailOverride = true;
        preparedAvatar.LerpToSquare(boardSquare, false, false, false);
        activeJailAvatars.Enqueue(preparedAvatar);
        if (activeAvatars.Count < maxActiveAvatars ||
            activeJailAvatars.Count < maxActiveJailAvatars) SpawnPreparedAvatar();
        addJailAvatarButton.interactable = activeJailAvatars.Count < maxActiveJailAvatars;
        removeJailAvatarButton.interactable = activeJailAvatars.Count > 0;
    }

    private void OnRemoveJailAvatarClicked()
    {
        StartCoroutine(DespawnAvatarCR(activeJailAvatars.Dequeue()));
        addJailAvatarButton.interactable = activeJailAvatars.Count < maxActiveJailAvatars;
        removeJailAvatarButton.interactable = activeJailAvatars.Count > 0;
    }

    private void SpawnPreparedAvatar()
    {
        preparedAvatar = Instantiate(avatarPrefab, avatarSpawn.position, avatarSpawn.rotation, null);
    }

    private void SpawnStreetSquare()
    {
        SpawnSquare(streetSquarePrefab);
        useStreetSquareButton.interactable = false;
        useCornerSquareButton.interactable = true;
        useJailSquareButton.interactable = true;
    }

    private void SpawnCornerSquare()
    {
        SpawnSquare(cornerSquarePrefab);
        useStreetSquareButton.interactable = true;
        useCornerSquareButton.interactable = false;
        useJailSquareButton.interactable = true;
    }

    private void SpawnJailSquare()
    {
        SpawnSquare(jailSquarePrefab);
        useStreetSquareButton.interactable = true;
        useCornerSquareButton.interactable = true;
        useJailSquareButton.interactable = false;
    }

    private void SpawnSquare(BoardSquare squarePrefab)
    {
        if (boardSquare != null)
        {
            while (activeAvatars.Count > 0)
            {
                Destroy(activeAvatars.Dequeue().gameObject);
            }
            activeAvatars.Clear();
            Destroy(boardSquare.gameObject);
            addAvatarButton.interactable = true;
            removeAvatarButton.interactable = false;
        }
        boardSquare = Instantiate(squarePrefab, squareSpawn.position, squareSpawn.rotation, null);
    }

    private IEnumerator DespawnAvatarCR(PlayerAvatar avatar)
    {
        avatar.RemoveFromSquare();
        yield return avatar.LerpToPosition(avatarDespawn.position);
        Destroy(avatar.gameObject);
    }
}
