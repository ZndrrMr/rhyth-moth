using UnityEngine;

public class MissCounter : MonoBehaviour
{
    public int maxMisses = 5; // Maximum allowed misses
    public int currentMisses = 0; // Current count of misses

    public void IncrementMissCounter()
    {
        currentMisses++;
        Debug.Log($"Misses: {currentMisses}/{maxMisses}");

        if (currentMisses >= maxMisses)
        {
            TriggerPenalty();
            ResetMissCounter();
        }
    }

    public void ResetMissCounter()
    {
        currentMisses = 0;
        Debug.Log("Miss counter reset.");
    }

    private void TriggerPenalty()
    {
        Debug.Log("Max misses reached! Ending the game...");
        EndGame();
    }

    private void EndGame()
    {
        // Implement your game-ending logic here
        Debug.Log("Game Over! You have missed too many times.");
        Time.timeScale = 0; // Pause the game
        // Optionally, display a game over UI or reload the scene
    }
}