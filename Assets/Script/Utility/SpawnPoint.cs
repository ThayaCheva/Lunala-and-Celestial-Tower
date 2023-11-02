using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    // Code with assistant from ChatGPT (unity call function when scene is loaded and teleport object based on scene name)
    private void Start()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Set the character's initial position
        TeleportCharacterToStartingPosition(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When a new scene is loaded, update the character's position
        TeleportCharacterToStartingPosition(scene.name);
    }

    private void TeleportCharacterToStartingPosition(string sceneName)
    {
        // Determine the starting position based on the scene name
        Vector3 startPosition = Vector3.zero;
        switch (sceneName)
        {
            case "Tutorial":
                startPosition = new Vector3(-21.128f, 0.8858f, -0.59f);
                break;
            case "Level1":
                startPosition = new Vector3(-31.02f, 0.8858f, -0.59f);
                break;
            case "Level2":
                startPosition = new Vector3(-29.35f, 0.8858f, 9.452f);;
                break;
            case "Level3":
                startPosition = new Vector3(-31.147f, 0.8858f, -5.047f);;
                break;
        }

        transform.position = startPosition;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
