using UnityEngine;

public class AfterPopTrigger : MonoBehaviour
{
    public NPCController NPCController;

    private GameObject _parent;

    private void Awake()
    {
        _parent = NPCController.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NPCController.tryPatrol)
        {
            if (!other.CompareTag("Player"))
            {
                NPCController.patrolIndicator.SetActive(true);
                NPCController.canPatrol = true;
                GameManager.Instance.patrolledNPC = NPCController;
                NPCController.patrolFeedback.PlayFeedbacks();
                void DelayResettingBools()
                {
                    NPCController.tryPatrol = false;
                    NPCController.canPatrol = false;
                    GameManager.Instance.patrolledNPC = null;
                    NPCController.patrolIndicator.SetActive(false);
                }
                DelayUtility.ExecuteAfterSeconds(DelayResettingBools, NPCController.patrolFeedback.TotalDuration);
                NPCController.afterPopZone.SetActive(false);
            }
            else
            {
                NPCController.tryPatrol = false;
                NPCController.canPatrol = false;
                NPCController.patrolIndicator.SetActive(false);
                NPCController.patrolZone.SetActive(false);
                NPCController.afterPopZone.SetActive(false);
            }
        }
        else
        {
            if (other.CompareTag("NPC") && other != _parent)
            {
                if (!NPCController.otherNearNPCs.Contains(other.GetComponent<NPCController>()))
                {
                    NPCController.otherNearNPCs.Add(other.GetComponent<NPCController>());
                }
            }
        }
    }
}
