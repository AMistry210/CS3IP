using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager instance;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method called when the game is over
    public void GameOver()
    {
        // Implement game over behavior here
        Debug.Log("Game Over");
    }

    // Method called when the game is finished
    public void Finished()
    {
        // Implement finished behavior here
        Debug.Log("Finished");
    }
}

