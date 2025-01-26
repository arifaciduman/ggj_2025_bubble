using System;
using UnityEngine;
using UnityEngine.UI;

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
        if (NPCController.isAlerted)
        {
            //redBubbleAnim.SetBool("isActivated", true);
            //bubbleImage.localScale = Vector3.zero;
            bubbleImageComponent.enabled = false;
            bubbleRedImage.localScale = Vector3.one;
            SpeechAudio.badClipsPlaying = true;
            NPCController.NpcFeedbacks.evilBubbleSpawn.PlayFeedbacks();
            void DelayDialogue()
            {
                dialogueText.localScale = Vector3.one;
                dialogueTextAnimations.PlayBadAnimation();
                
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

            void DelayDialogue()
            {
                dialogueText.localScale = Vector3.one;
                dialogueTextAnimations.PlayNormalAnimation();

                
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
        bubbleRedImage.localScale = whichAnim != "isRedEaten" ? Vector3.zero : Vector3.one;
        bubbleAnim.SetBool(whichAnim, true);
        if(whichAnim == "isEaten") NPCController.NpcFeedbacks.eat.PlayFeedbacks();
        else NPCController.NpcFeedbacks.evilEat.PlayFeedbacks();
        DelayUtility.ExecuteAfterSeconds(DisableBubble, 1f);
    }

    public void DisableBubble()
    {
        bubbleAnim.Rebind();
        bubbleImageComponent.enabled = false;
        bubbleRedImage.localScale = Vector3.zero;
        dialogueText.localScale = Vector3.zero;
        dialogueTextAnimations.animator.SetBool("Activate", false);
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
            }
            else
            {
                bubbleImageComponent.enabled = false;
                bubbleRedImage.localScale = Vector3.one;
                
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
