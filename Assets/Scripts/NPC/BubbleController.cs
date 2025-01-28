using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BubbleController : MonoBehaviour
{
    public NPCController NPCController;
    public DialogueTextAnimations dialogueTextAnimations;

    public SpeechAudio SpeechAudio;
    
    public RectTransform bubbleImage;
    public RectTransform bubbleRedImage;
    public RectTransform dialogueText;

    public Image bubbleImageComponent;

    public Animator bubbleAnim;//bubble alert anim
    public Animator redBubbleAnim;//bubble alert anim

    public AnimationClip bubbleOpening;
    
    public Collider bubbleCollider;

    private float _alertValue;

    private bool _isBubbleUp;

    private void Update()
    {
        if (_isBubbleUp)
        {
            SpeechAudio.canPlay = true;
        }
        else
        {
            SpeechAudio.canPlay = false;
        }
    }

    public void EnableBubble()
    {
        SpeechAudio._isReadyForNext = false;
        bubbleCollider.enabled = true;
        _alertValue = 0;
        _isBubbleUp = true;
        dialogueTextAnimations.PlayAnimation();
        if (NPCController.isAlerted)
        {
            //redBubbleAnim.SetBool("isActivated", true);
            //bubbleImage.localScale = Vector3.zero;
            // dialogueTextAnimations.PlayBadAnimation();
            
            bubbleImageComponent.enabled = false;
            bubbleRedImage.localScale = Vector3.one;
            SpeechAudio.badClipsPlaying = true;
            NPCController.NpcFeedbacks.evilBubbleSpawn.PlayFeedbacks();
            dialogueTextAnimations.PlayAnimation(true);
            dialogueText.localScale = Vector3.zero;

            void DelayDialogue()
            {
                dialogueText.localScale = Vector3.one;
                //dialogueTextAnimations.PlayBadAnimation();
                
            }
            DelayUtility.ExecuteAfterSeconds(DelayDialogue, bubbleOpening.length, true);
        }
        else
        {
            bubbleAnim.SetBool("isActivated", true);
            //bubbleImage.localScale = Vector3.one;
            bubbleImageComponent.enabled = true;
            bubbleRedImage.localScale = Vector3.zero;
            
            SpeechAudio.badClipsPlaying = false;
            NPCController.NpcFeedbacks.bubbleSpawn.PlayFeedbacks();
            dialogueText.localScale = Vector3.zero;
            
            // dialogueTextAnimations.PlayNormalAnimation();
            void DelayDialogue()
            { 
                dialogueText.localScale = Vector3.one;
               //dialogueTextAnimations.PlayNormalAnimation();

                
            }
            DelayUtility.ExecuteAfterSeconds(DelayDialogue, bubbleOpening.length, true);
        }
    }

    public void StartEatenAnim()
    {
        string whichAnim = NPCController.isAlerted ? "isRedEaten" : "isEaten";
        //bubbleImage.localScale = Vector3.one;
        dialogueText.localScale = Vector3.zero;
        bubbleImageComponent.enabled = whichAnim != "isRedEaten";
        bubbleImageComponent.enabled = true;
        // bubbleRedImage.localScale = whichAnim != "isRedEaten" ? Vector3.zero : Vector3.one;
       bubbleRedImage.localScale = Vector3.zero;
        bubbleAnim.SetBool(whichAnim, true);
        if(whichAnim == "isEaten") NPCController.NpcFeedbacks.eat.PlayFeedbacks();
        else NPCController.NpcFeedbacks.evilEat.PlayFeedbacks();
        DelayUtility.ExecuteAfterSeconds(DisableBubble, 1f);
    }

    public void DisableBubble()
    {
        dialogueTextAnimations.animator.SetBool("Activate", false);
        bubbleAnim.Rebind();
        bubbleImageComponent.enabled = false;
        bubbleRedImage.localScale = Vector3.zero;
        dialogueText.localScale = Vector3.zero;
        _isBubbleUp = false;
        NPCController.isAlerted = false;
        IncreaseDangerLevel();
    }

    public void SetRedBubbleAnimMotion()
    {
        if (!bubbleAnim.GetBool("isEaten") && !bubbleAnim.GetBool("isRedEaten"))
        {
            if (_alertValue >= 1)
            {
                //SPREAD RED BUBBLE
                NPCController.EnableRadar();
                bubbleCollider.enabled = true;
                if(bubbleRedImage.localScale == Vector3.one) dialogueText.localScale = Vector3.one;
            }
            else
            {
                bubbleImageComponent.enabled = false;
                dialogueTextAnimations.PlayAnimation(true);
                bubbleRedImage.localScale = Vector3.one;
                dialogueText.localScale = Vector3.zero;
                // void ChangeBubble()
                // {
                //     
                // }
                //
                // DelayUtility.ExecuteAfterSeconds(ChangeBubble, bubbleOpening.length, true);
                _alertValue += Time.deltaTime * .5f;
                redBubbleAnim.SetFloat("alertValue", _alertValue);
            }
        }
    }

    public void IncreaseDangerLevel()
    {
        GameManager.Instance.danger.AddDanger();
    }
}
