using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardSquareFactory : MonoBehaviour
{
    [Header("Square Primitives")]
    [SerializeField] private GameObject cornerSquare;
    [SerializeField] private GameObject streetSquare;

    [Header("Balls")]
    [SerializeField] private BallSquare pokeBallSquare;
    [SerializeField] private BallSquare greatBallSquare;
    [SerializeField] private BallSquare ultraBallSquare;
    [SerializeField] private BallSquare masterBallSquare;

    [Header("Corners")]
    [SerializeField] private GoSquare goSquare;
    [SerializeField] private JailSquare jailSquare;
    [SerializeField] private FreeParkingSquare freeParkingSquare;
    [SerializeField] private GoToJailSquare goToJailSquare;

    [Header("Cards")]
    [SerializeField] private CardSquare trainerBattleSquare;
    [SerializeField] private CardSquare professorOakSquare;

    [Header("Attacks")]
    [SerializeField] private BoardSquare rivalAttacksSquare;
    [SerializeField] private BoardSquare rocketAttacksSquare;

    [Header("Legendaries")]
    [SerializeField] private List<LegendarySquare> legendarySquares;

    [Header("Gyms")]
    [SerializeField] private List<GymSquare> gym1Squares;
    [SerializeField] private List<GymSquare> gym2Squares;
    [SerializeField] private List<GymSquare> gym3Squares;
    [SerializeField] private List<GymSquare> gym4Squares;
    [SerializeField] private List<GymSquare> gym5Squares;
    [SerializeField] private List<GymSquare> gym6Squares;
    [SerializeField] private List<GymSquare> gym7Squares;
    [SerializeField] private List<GymSquare> gym8Squares;

    public float CornerSquareSize => cornerSquare.transform.lossyScale.x;
    public float StreetSquareWidth => streetSquare.transform.lossyScale.x;
    public float StreetSquareHeight => streetSquare.transform.lossyScale.y;

    public BoardSquare SpawnSquare(
        int index, Vector3 spawnPos, Quaternion rotation, Transform parent)
    {
        switch (index)
        {
            // corner squares
            case 0:
                return Instantiate(goSquare, spawnPos, rotation, parent);
            case 10:
                return Instantiate(jailSquare, spawnPos, rotation, parent);
            case 20:
                return Instantiate(freeParkingSquare, spawnPos, rotation, parent);
            case 30:
                return Instantiate(goToJailSquare, spawnPos, rotation, parent);
            // ball squares
            case 5:
                return GetPropertySquare(pokeBallSquare, spawnPos, rotation, parent);
            case 15:
                return GetPropertySquare(greatBallSquare, spawnPos, rotation, parent);
            case 25:
                return GetPropertySquare(ultraBallSquare, spawnPos, rotation, parent);
            case 35:
                return GetPropertySquare(masterBallSquare, spawnPos, rotation, parent);
            // trainer battle squares
            case 2:
            case 17:
            case 33:
                return Instantiate(trainerBattleSquare, spawnPos, rotation, parent);
            // professor squares
            case 7:
            case 22:
            case 36:
                return Instantiate(professorOakSquare, spawnPos, rotation, parent);
            // attacks! squares
            case 4:
                return Instantiate(rivalAttacksSquare, spawnPos, rotation, parent);
            case 38:
                return Instantiate(rocketAttacksSquare, spawnPos, rotation, parent);
            // legendary squares
            case 12:
                return GetPropertySquare(legendarySquares[0], spawnPos, rotation, parent);
            case 28:
                return GetPropertySquare(legendarySquares[1], spawnPos, rotation, parent);
            // gym squares
            // gym 1
            case 1:
                return GetPropertySquare(gym1Squares[0], spawnPos, rotation, parent);
            case 3:
                return GetPropertySquare(gym1Squares[1], spawnPos, rotation, parent);
            // gym 2
            case 6:
                return GetPropertySquare(gym2Squares[0], spawnPos, rotation, parent);
            case 8:
                return GetPropertySquare(gym2Squares[1], spawnPos, rotation, parent);
            case 9:
                return GetPropertySquare(gym2Squares[2], spawnPos, rotation, parent);
            // gym 3
            case 11:
                return GetPropertySquare(gym3Squares[0], spawnPos, rotation, parent);
            case 13:
                return GetPropertySquare(gym3Squares[1], spawnPos, rotation, parent);
            case 14:
                return GetPropertySquare(gym3Squares[2], spawnPos, rotation, parent);
            // gym 4
            case 16:
                return GetPropertySquare(gym4Squares[0], spawnPos, rotation, parent);
            case 18:
                return GetPropertySquare(gym4Squares[1], spawnPos, rotation, parent);
            case 19:
                return GetPropertySquare(gym4Squares[2], spawnPos, rotation, parent);
            // gym 5
            case 21:
                return GetPropertySquare(gym5Squares[0], spawnPos, rotation, parent);
            case 23:
                return GetPropertySquare(gym5Squares[1], spawnPos, rotation, parent);
            case 24:
                return GetPropertySquare(gym5Squares[2], spawnPos, rotation, parent);
            // gym 6
            case 26:
                return GetPropertySquare(gym6Squares[0], spawnPos, rotation, parent);
            case 27:
                return GetPropertySquare(gym6Squares[1], spawnPos, rotation, parent);
            case 29:
                return GetPropertySquare(gym6Squares[2], spawnPos, rotation, parent);
            // gym 7
            case 31:
                return GetPropertySquare(gym7Squares[0], spawnPos, rotation, parent);
            case 32:
                return GetPropertySquare(gym7Squares[1], spawnPos, rotation, parent);
            case 34:
                return GetPropertySquare(gym7Squares[2], spawnPos, rotation, parent);
            // gym 8
            case 37:
                return GetPropertySquare(gym8Squares[0], spawnPos, rotation, parent);
            case 39:
                return GetPropertySquare(gym8Squares[1], spawnPos, rotation, parent);
            default:
                throw new System.ArgumentException("Unexpected index: " + index);
        }
    }

    private PropertySquare GetPropertySquare(PropertySquare squarePrefab,
        Vector3 spawnPos, Quaternion spawnRotation, Transform parent)
    {
        PropertySquare newSquare = Instantiate(squarePrefab, spawnPos, spawnRotation, parent);
        newSquare.Property = PropertyManager.Instance.GetPropertyByName(
            GetPropertySquareName(squarePrefab));
        return newSquare;
    }

    private string GetPropertySquareName(PropertySquare propSquare)
    {
        return propSquare.gameObject.name.Split(
            new string[] { " Square" }, System.StringSplitOptions.None).First();
    }
}
