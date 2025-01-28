using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueTextAnimations : MonoBehaviour
{
    public Animator animator;

    public Image textImage;
    public List<Sprite> textSprites = new List<Sprite>();
    public List<Sprite> badTextSprites = new List<Sprite>();

    private int imageIndex;

    private bool isBad;
    
    private void Start()
    {
        StartCoroutine(PlayText());
    }

    IEnumerator PlayText()
    {
        while (true)
        {
            textImage.sprite = isBad ? badTextSprites[imageIndex] : textSprites[imageIndex];
            imageIndex = (imageIndex + 1) % textSprites.Count;
            yield return new WaitForSeconds(1/9f);
        }
    }

    public void PlayNormalAnimation()
    {
        animator.SetInteger("DialogueOption", Random.Range(0, 2));
        animator.SetBool("Activate", true);
    }

    public void PlayBadAnimation()
    {
        animator.SetBool("Activate", false);
        animator.SetBool("DialogueBad", true); 
    }

    public void PlayAnimation(bool isBad = false)
    {
        this.isBad = isBad;
    }
}
