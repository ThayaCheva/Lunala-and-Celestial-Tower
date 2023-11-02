using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelNumber : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<ManageScene>().levelCount == 0) {
            levelText.text = "Tutorial Floor";

        }
        else if (SceneManager.GetActiveScene().name == "Level3") {
            levelText.text = "Moon Altar";
        }
        else {
            levelText.text = "Floor " + FindObjectOfType<ManageScene>().levelCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
