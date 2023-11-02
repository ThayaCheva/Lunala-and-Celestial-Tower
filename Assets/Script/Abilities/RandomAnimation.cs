using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    void Start()
    {
        if (animator != null)
        {
            // animator = GetComponent<Animator>();
            // Get a random animation index based on the number of animations in the Animator Controller
            int randomAnimationIndex = Random.Range(0, animator.runtimeAnimatorController.animationClips.Length);
            Debug.Log(randomAnimationIndex);
            // Play the selected animation
            animator.SetInteger("AnimationIndex", randomAnimationIndex);
        }
    }
}
