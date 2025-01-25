using System.Collections.Generic;
using UnityEngine;

public class DialogueTextAnimations : MonoBehaviour
{
    public List<Animation> normalAnimations;
    public List<Animation> specialAnimations;

    public Animator animator;
    
    public void PlayNormalAnimation()
    {
        animator.SetInteger("DialogueOption", Random.Range(0, 2));
        animator.SetBool("Activate", true);
    }

    public void PlayBadAnimation()
    {
        animator.SetInteger("DialogueBad", Random.Range(0, 2));
        animator.SetBool("Activate", true);
    }
}
