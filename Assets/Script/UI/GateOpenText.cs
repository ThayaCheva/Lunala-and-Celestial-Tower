using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GateOpenText : MonoBehaviour
{
    public TextMeshProUGUI gateText;
    // Start is called before the first frame update
    void Start()
    {
        ManageScene sceneManager = FindObjectOfType<ManageScene>();
        if (SceneManager.GetActiveScene().name == "Level3") {
                gateText.text = "You took too long. . .";
        }
        else {
            if (sceneManager.levelCount % 2 == 0) {
                gateText.text = "Path to the Moon Altar has been revealed";
            }
            else {
                gateText.text = "A path has been opened";
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
