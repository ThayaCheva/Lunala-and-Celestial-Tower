using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// Altered code from https://www.youtube.com/watch?v=8oTYabhj248&t=15s
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image portrait;
    public GameObject floorText;
    public string[] lines;
    public float textSpeed;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        floorText.SetActive(false);
        if((SceneManager.GetActiveScene().name == "Level3")){
                BossSpawner.instance.enabled=false;
        }
        LunalaController.instance.enabled = false;
        text.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (text.text == lines[index]) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            text.text += c;
            if (c != ' ') {
                FindObjectOfType<SoundManager>().Play("Typing");
            }
            yield return new WaitForSeconds(textSpeed);
        } 
    }

    void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            LunalaController.instance.enabled = true;
            if((SceneManager.GetActiveScene().name == "Level3")){
                BossSpawner.instance.enabled=true;
            }
            gameObject.SetActive(false);
            portrait.gameObject.SetActive(false);
            floorText.SetActive(true);
        }
    }
}
