using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopyText : MonoBehaviour
{
    public TextMeshProUGUI modiferText;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = modiferText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
