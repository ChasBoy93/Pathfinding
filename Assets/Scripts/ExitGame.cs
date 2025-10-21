using UnityEngine;

public class ExitGame : MonoBehaviour
{

    public void ExitTheGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
