using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public NPCController NPCController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.vibe.isFull)
        {
            NPCController.AfterEatingBubble();
            GetComponent<Collider>().enabled = false;
        }
    }
}
