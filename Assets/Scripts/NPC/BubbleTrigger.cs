using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public NPCController NPCController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPCController.AfterEatingBubble();
            GetComponent<Collider>().enabled = false;
        }
    }
}
