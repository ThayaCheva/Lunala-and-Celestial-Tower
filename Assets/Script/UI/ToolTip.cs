using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ToolTip : MonoBehaviour
{
    public string title;
    public string cost;
    public string description;
    public Sprite iconSprite;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI costText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI levelText;
    private Image upgradeIcon;

    public GameObject toolTip;

    public void ShowTooltip()
    {
        FindObjectOfType<SoundManager>().Play("Button Click");
        nameText = toolTip.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        costText = toolTip.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        descriptionText = toolTip.transform.Find("Description").GetComponent<TextMeshProUGUI>(); 
        levelText = toolTip.transform.Find("Level").GetComponent<TextMeshProUGUI>(); 
        upgradeIcon = toolTip.transform.Find("Icon").GetComponent<Image>(); 
        nameText.text = title;
        costText.text = cost;
        descriptionText.text = description;
        upgradeIcon.sprite = iconSprite;
        
        if (title == "Rapid Fire") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock1_level.ToString() + "/5";
        }
        else if (title == "Swift Lance") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock2_level.ToString() + "/5";
        }
        else if (title == "Divine Arrow") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock3_level.ToString() + "/1";
        }
        else if (title == "Enlarging Lance") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock1_R_level.ToString() + "/5";
        }
        else if (title == "Empowered Lance") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock2_R_level.ToString() + "/5";
        }
        else if (title == "Moonlight Bloom") {
            levelText.text = "Level: " + TreeManager.instance.lanceUnlock3_R_level.ToString() + "/1";
        }

        if (title == "Rapid Heal") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock1_level.ToString() + "/5";
        }
        else if (title == "Supermoon") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock2_level.ToString() + "/5";
        }
        else if (title == "Moonlight Barrage") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock3_level.ToString() + "/1";
        }
        
        else if (title == "Rejuvenating Moon") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock1_R_level.ToString() + "/5";
        }
        else if (title == "Divine Protection") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock2_R_level.ToString() + "/5";
        }
        else if (title == "Blood Moon") {
            levelText.text = "Level: " + TreeManager.instance.blessingUnlock3_R_level.ToString() + "/1";
        }

        if (title == "Celestial Assault") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock1_level.ToString() + "/5";
        }
        else if (title == "Engulfing Strike") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock2_level.ToString() + "/5";
        }
        else if (title == "Divine Devastation") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock3_level.ToString() + "/1";
        }
        
        else if (title == "Celestial Storm") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock1_R_level.ToString() + "/5";
        }
        else if (title == "Quad Strike") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock2_R_level.ToString() + "/4";
        }
        else if (title == "Carpet Bomb") {
            levelText.text = "Level: " + TreeManager.instance.celestialUnlock3_R_level.ToString() + "/1";
        }
        
    }
}
