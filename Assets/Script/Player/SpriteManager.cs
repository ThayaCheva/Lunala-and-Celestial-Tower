using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    public bool isTransitioning = false;

    private void Awake() {
        instance = this;
    }

    // Set the attack transition of Lunala
    public void setTransition() {
        if (isTransitioning) {
            isTransitioning = false;
        }
        else {
            isTransitioning = true;
        }
    }
}
