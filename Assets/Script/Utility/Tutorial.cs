using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject keyText;
    [SerializeField] private GameObject tutorialText;
    private bool showText;
    // Start is called before the first frame update
    void Start()
    {
        keyText.gameObject.SetActive(false);
        if (tutorialText != null) {
            tutorialText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showText && Input.GetKeyDown(KeyCode.F)) {
            DisplayTutorial();
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
            tutorialText.gameObject.SetActive(false);
            showText = false;
        }
    }

    private void DisplayTutorial() {
        tutorialText.gameObject.SetActive(true);
    }
}
