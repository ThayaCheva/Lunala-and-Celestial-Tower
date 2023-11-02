using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    [SerializeField] private GameObject keyText;
    public Canvas levelSelection;
    private bool showText;
    // Start is called before the first frame update
    void Start()
    {
        keyText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showText && Input.GetKeyDown(KeyCode.F)) {
            ManageScene sceneManager = FindObjectOfType<ManageScene>();
            if (SceneManager.GetActiveScene().name == "Tutorial") {
                ChangeLevel();  
            }
            else {
                if (sceneManager.levelCount >= 2) { 
                    levelSelection.enabled = !levelSelection.enabled;
                    if (levelSelection.enabled) {
                        Time.timeScale = 0;
                    }
                    else {
                        Time.timeScale = 1;
                    }
                }
                else {
                    ChangeLevel();  
                }
            }
            
            keyText.gameObject.SetActive(false);
            FindObjectOfType<SoundManager>().Play("Button Click");
        }    
    }

    private void OnTriggerEnter(Collider col) {
        if (col.GetComponent<Collider>().tag == "Player") {
            keyText.gameObject.SetActive(true);
            showText = true;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.GetComponent<Collider>().tag == "Player") {
            keyText.gameObject.SetActive(false);
            showText = false;
        }
    }

    public void LoadBossStage() {
        if (SceneManager.GetActiveScene().name != "Tutorial") {
            levelSelection.enabled = false;
            Time.timeScale = 1;
        }
        FindObjectOfType<ManageScene>().LoadLevel(6);
    }

    public void ChangeLevel() {
        int randomLevel = Random.Range(4, 6);
        if (SceneManager.GetActiveScene().name != "Tutorial") {
            levelSelection.enabled = false;
            Time.timeScale = 1;
        }
        if (SceneManager.GetActiveScene().name == "Level1") {
            FindObjectOfType<ManageScene>().LoadLevel(5);
        }
        else if (SceneManager.GetActiveScene().name == "Level2") {
            FindObjectOfType<ManageScene>().LoadLevel(4);
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial") {
            FindObjectOfType<ManageScene>().LoadLevel(randomLevel);
        }
    }
}
