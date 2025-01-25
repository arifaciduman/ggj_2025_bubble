using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public NPCController NPCController;
    public DialogueTextAnimations dialogueTextAnimations;

    public RectTransform bubbleImage;
    public RectTransform bubbleRedImage;
    public RectTransform dialogueText;

    public Animator bubbleAnim;//bubble alert anim
    public Animator redBubbleAnim;//bubble alert anim

    public AnimationClip bubbleOpening;
    
    public Collider bubbleCollider;

    private float _alertValue;

    public void EnableBubble()
    {
        bubbleAnim.SetBool("isActivated", true);
        bubbleImage.localScale = Vector3.one;

        void DelayDialogue()
        {
            dialogueText.localScale = Vector3.one;
            dialogueTextAnimations.PlayNormalAnimation();   
        }
        
        DelayUtility.ExecuteAfterSeconds(DelayDialogue, bubbleOpening.length, true);
        bubbleCollider.enabled = true;
        _alertValue = 0;
    }

    public void StartEatenAnim()
    {
        string whichAnim = NPCController.isAlerted ? "isRedEaten" : "isEaten";
        bubbleAnim.SetBool(whichAnim, true);
        DelayUtility.ExecuteAfterSeconds(DisableBubble, 1f);
    }

    public void DisableBubble()
    {
        bubbleAnim.Rebind();
        //bubbleAnim.SetBool("isEaten", false);
        //bubbleAnim.SetBool("isRedEaten", false);
        redBubbleAnim.SetBool("isRedEaten", false);
        bubbleImage.localScale = Vector3.zero;
        bubbleRedImage.localScale = Vector3.zero;
        dialogueText.localScale = Vector3.zero;
        dialogueTextAnimations.animator.SetBool("Activate", false);

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
                bubbleRedImage.localScale = Vector3.one;
                
                _alertValue += Time.deltaTime * .75f;
                //print($"alertValue: {_alertValue}");
                
                redBubbleAnim.SetFloat("alertValue", _alertValue);
            }
        }
    }

    public void IncreaseDangerLevel()
    {
        GameManager.Instance.danger.AddDanger();
    }
}
