using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float duration = 300.0f; // 5 minutes in seconds
    public float currentTime;
    public static Timer instance;
    public GameObject warpDoor;
    private float lastSecond;
    public GameObject gateText;
    void Awake()
    {
        instance=this;
    }

    void Start()
    {
        currentTime = duration;
    }

    void Update()
    {   
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if(SceneManager.GetActiveScene().name == "Tutorial"){
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);    
        }
        else if (currentTime > 0)
        {
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            currentTime -= Time.deltaTime;
            // Play the ticking sound
            if (currentTime <= 5 && Mathf.FloorToInt(Time.time) != lastSecond)
            {
                FindObjectOfType<SoundManager>().Play("Button Click");
                lastSecond = Mathf.FloorToInt(Time.time);
            }
        }
        else{
            currentTime = 0;
            countdownText.text = string.Format("{0:00}:{1:00}", 0, 0);
            if (SceneManager.GetActiveScene().name != "Level3") {
                warpDoor.SetActive(true);
            }
            gateText.SetActive(true);
        }
    }
}
