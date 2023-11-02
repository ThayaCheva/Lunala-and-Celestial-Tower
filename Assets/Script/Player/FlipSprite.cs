using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    public static FlipSprite instance;
    private void Awake() {
        instance = this;
    }
    // Update is called once per frame
    void Update() {
        // Flip the character based on its direction
        if (!LunalaController.instance.isAttacking) {
            if (LunalaController.instance.inputH > 0 && LunalaController.instance.isFlipped) {
                Flip();
            }
            else if (LunalaController.instance.inputH < 0 && !LunalaController.instance.isFlipped) {
                Flip();
            }
        }
    }

     // Altered Code from https://www.youtube.com/watch?v=Cr-j7EoM8bg
    void Flip() {
        Vector3 currScale = gameObject.transform.localScale;
        currScale.x *= -1;
        gameObject.transform.localScale = currScale;
        LunalaController.instance.isFlipped = !LunalaController.instance.isFlipped;
        gameObject.BroadcastMessage("setDirectionRight", LunalaController.instance.isFlipped);
    }

    // Flip character based on the mouse location
    public void flipToMouse() {
        if (LunalaController.instance.inputH == 0) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = transform.position.z - Camera.main.transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if (mousePos.x > transform.position.x && LunalaController.instance.isFlipped) {
                Flip();
            }
            else if (mousePos.x < transform.position.x && !LunalaController.instance.isFlipped) {
                Flip();
            }
        }
    }

}
