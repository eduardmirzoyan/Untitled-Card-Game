using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Game game;

    [Header("Game Settings")]
    [SerializeField] private int boardWidth;
    [SerializeField] private int boardHeight;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private List<Card> debugCards;
    [SerializeField] private List<ResourceToken> debugTokens;

    public bool isPaused;

    public static GameHandler instance;
    private void Awake()
    {
        // Singleton Logic
        if (GameHandler.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Get a copy of a random card
                var randomCard = Instantiate(debugCards[Random.Range(0, debugCards.Count)]);

                // Create the card on the board
                game.board.CreateCard(randomCard);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                // Get a copy of a random token
                var randomToken = game.tokenTable.GetRandomToken(); // Instantiate(debugTokens[Random.Range(0, debugTokens.Count)]);

                // Set name
                randomToken.name = randomToken.ToString();

                // Create the token on the board
                game.board.CreateToken(randomToken);
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                // Toggle pause
                isPaused = !isPaused;
            }
        }
    }

    private IEnumerator StartGame()
    {
        // Wait
        yield return new WaitForSeconds(0.1f);

        // Start by creating a new game
        game = ScriptableObject.CreateInstance<Game>();
        game.Initialize(boardWidth, boardHeight);
    }
}
