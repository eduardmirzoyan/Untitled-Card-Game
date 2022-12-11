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

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update() {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Get a copy of a random card
                var randomCard = Instantiate(debugCards[Random.Range(0, debugCards.Count)]);

                // Create the card on the board
                game.board.CreateCard(randomCard);
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
