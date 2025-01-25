using UnityEngine;

public class AfterPopTrigger : MonoBehaviour
{
    public NPCController NPCController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && other != NPCController.gameObject)
        {
            NPCController.otherNearNPCs.Add(other.GetComponent<NPCController>());
        }
    }
}
