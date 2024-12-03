using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    private TextMeshPro textMesh;
    [SerializeField] private float disappearTime;
    private Color textColor;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void indicateDamage(float damageAmount){
        int damage=Mathf.RoundToInt(damageAmount);
        textMesh.SetText(damage.ToString());
    }
    
    public void Update(){
        float upwardSpeed = 0.5f;
        transform.position += new Vector3(0,upwardSpeed,0) * Time.deltaTime;
        disappearTime -= Time.deltaTime;
        if(disappearTime <= 0){
            StartCoroutine(fade());
            Destroy(gameObject);
        }


    }

    public IEnumerator fade(){
        textMesh.CrossFadeAlpha(0,2f,true);
        yield return new WaitForSeconds(1);
    }

    
}
