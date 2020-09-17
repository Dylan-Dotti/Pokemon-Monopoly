using System.Collections.Generic;
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
                return Instantiate(pokeBallSquare, spawnPos, rotation, parent);
            case 15:
                return Instantiate(greatBallSquare, spawnPos, rotation, parent);
            case 25:
                return Instantiate(ultraBallSquare, spawnPos, rotation, parent);
            case 35:
                return Instantiate(masterBallSquare, spawnPos, rotation, parent);
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
                return Instantiate(legendarySquares[0], spawnPos, rotation, parent);
            case 28:
                return Instantiate(legendarySquares[1], spawnPos, rotation, parent);
            // gym squares
            // gym 1
            case 1:
                return Instantiate(gym1Squares[0], spawnPos, rotation, parent);
            case 3:
                return Instantiate(gym1Squares[1], spawnPos, rotation, parent);
            // gym 2
            case 6:
                return Instantiate(gym2Squares[0], spawnPos, rotation, parent);
            case 8:
                return Instantiate(gym2Squares[1], spawnPos, rotation, parent);
            case 9:
                return Instantiate(gym2Squares[2], spawnPos, rotation, parent);
            // gym 3
            case 11:
                return Instantiate(gym3Squares[0], spawnPos, rotation, parent);
            case 13:
                return Instantiate(gym3Squares[1], spawnPos, rotation, parent);
            case 14:
                return Instantiate(gym3Squares[2], spawnPos, rotation, parent);
            // gym 4
            case 16:
                return Instantiate(gym4Squares[0], spawnPos, rotation, parent);
            case 18:
                return Instantiate(gym4Squares[1], spawnPos, rotation, parent);
            case 19:
                return Instantiate(gym4Squares[2], spawnPos, rotation, parent);
            // gym 5
            case 21:
                return Instantiate(gym5Squares[0], spawnPos, rotation, parent);
            case 23:
                return Instantiate(gym5Squares[1], spawnPos, rotation, parent);
            case 24:
                return Instantiate(gym5Squares[2], spawnPos, rotation, parent);
            // gym 6
            case 26:
                return Instantiate(gym6Squares[0], spawnPos, rotation, parent);
            case 27:
                return Instantiate(gym6Squares[1], spawnPos, rotation, parent);
            case 29:
                return Instantiate(gym6Squares[2], spawnPos, rotation, parent);
            // gym 7
            case 31:
                return Instantiate(gym7Squares[0], spawnPos, rotation, parent);
            case 32:
                return Instantiate(gym7Squares[1], spawnPos, rotation, parent);
            case 34:
                return Instantiate(gym7Squares[2], spawnPos, rotation, parent);
            // gym 8
            case 37:
                return Instantiate(gym8Squares[0], spawnPos, rotation, parent);
            case 39:
                return Instantiate(gym8Squares[1], spawnPos, rotation, parent);
            default:
                return Instantiate(trainerBattleSquare, spawnPos, rotation, parent);
        }
    }

    private BoardSquare GetBoardSquare(BoardSquare squarePrefab, string propertyName)
    {
        return null;
    }
}
