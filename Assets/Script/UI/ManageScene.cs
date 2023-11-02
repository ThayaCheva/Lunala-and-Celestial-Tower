using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ManageScene : MonoBehaviour
{
    public int levelCount = 0;
    // Start is called before the first frame update

    public void Update() {
        if (LunalaController.instance != null && !LunalaController.instance.isDead && levelCount>=1) {
            ManageDeath();
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel(int level)
    {
        levelCount++;
        LunalaController.instance.damage+=2;
        SceneManager.LoadSceneAsync(level);
    }

    private void ManageDeath() {
        if (LunalaController.instance != null) {
            if(LunalaController.instance.currenthealth <= 0){
                Debug.Log(LunalaController.instance.currenthealth);
                LunalaController.instance.rb.velocity = new Vector3(0, 0, 0);
                LunalaController.instance.isDead = true;
                LunalaController.instance.anim.SetTrigger("dead");
                GameObject.Find("Background Music").GetComponent<AudioSource>().Stop();
                FindObjectOfType<SoundManager>().Play("Player Death Sound");
                StartCoroutine(GameOverScene());
            }
        }
    }

    private IEnumerator GameOverScene() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(3);
    }
}
