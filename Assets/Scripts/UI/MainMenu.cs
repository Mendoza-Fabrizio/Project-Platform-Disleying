using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int con;

    private void Start()
    {
        // Inicializamos 'con' con el índice de la escena actual
        con = SceneManager.GetActiveScene().buildIndex;
    }

    public void PlayGame()
    {
        LoadNextScene();
    }

    public void NextTest1()
    {
        LoadNextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadNextScene()
    {
        con++; // Incrementamos el índice
        if (IsValidSceneIndex(con))
        {
            SceneManager.LoadSceneAsync(con);
        }
        else
        {
            Debug.LogError("Índice de escena no válido.");
        }
    }

    private bool IsValidSceneIndex(int index)
    {
        return index >= 0 && index < SceneManager.sceneCountInBuildSettings;
    }
}
