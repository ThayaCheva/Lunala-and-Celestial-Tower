using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI pointsText;
    // Start is called before the first frame update

    void Start()
    {
        pointsText.text = LunalaController.instance.points.ToString();
    }

    void Update() {
        if (pointsText.text != LunalaController.instance.points.ToString()) {
            pointsText.text = LunalaController.instance.points.ToString();
        }
    }

    public void AddPoints() {
        LunalaController.instance.points += 1;
        pointsText.text = LunalaController.instance.points.ToString();
    }
}
