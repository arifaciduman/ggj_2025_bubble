using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public NPCController NPCController;

    public RectTransform bubbleImage;

    public Animator bubbleAnim;//bubble alert anim
    public Animator redBubbleAnim;//bubble alert anim

    public Collider bubbleCollider;

    public void EnableBubble()
    {
        bubbleImage.localScale = Vector3.one;
        bubbleCollider.enabled = true;
    }

    public void StartEatenAnim()
    {
        string whichAnim = NPCController.isAlerted ? "isRedEaten" : "isEaten";
        bubbleAnim.SetBool(whichAnim, true);
        DelayUtility.ExecuteAfterSeconds(DisableBubble, 1f);
    }

    public void DisableBubble()
    {
        bubbleAnim.SetBool("isEaten", false);
        bubbleAnim.SetBool("isRedEaten", false);
        bubbleImage.localScale = Vector3.zero;
        IncreaseDangerLevel();
    }

    public void SetRedBubbleAnimMotion(float alertValue)
    {
        if (!bubbleAnim.GetBool("isEaten"))
        {
            if (alertValue >= 100)
            {
                //SPREAD RED BUBBLE
                NPCController.EnableRadar();
            }
            else
            {
                alertValue += Time.deltaTime;
                redBubbleAnim.SetFloat("alertValue", alertValue / 100);
            }
        }
    }

    public void IncreaseDangerLevel()
    {
        GameManager.Instance.danger.AddDanger();
    }
}
