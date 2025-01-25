using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public NPCController NPCController;

    public RectTransform bubbleImage;

    public Animator bubbleAnim;//bubble alert anim
    public Animator redBubbleAnim;//bubble alert anim

    public Collider bubbleCollider;

    private float _alertValue;

    public void EnableBubble()
    {
        bubbleImage.localScale = Vector3.one;
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
        bubbleAnim.SetBool("isEaten", false);
        bubbleAnim.SetBool("isRedEaten", false);
        redBubbleAnim.SetBool("isRedEaten", false);
        bubbleImage.localScale = Vector3.zero;
        IncreaseDangerLevel();
    }

    public void SetRedBubbleAnimMotion()
    {
        if (!bubbleAnim.GetBool("isEaten"))
        {
            if (_alertValue >= 1)
            {
                //SPREAD RED BUBBLE
                NPCController.EnableRadar();
            }
            else
            {
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
