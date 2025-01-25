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
                NPCController.canPatrol = true;
                NPCController.MMF_Player.PlayFeedbacks();
                void DelayResettingBools()
                {
                    NPCController.tryPatrol = false;
                    NPCController.canPatrol = false;
                }
                DelayUtility.ExecuteAfterSeconds(DelayResettingBools, NPCController.MMF_Player.TotalDuration);
                NPCController.afterPopZone.SetActive(false);
            }
            else
            {
                NPCController.tryPatrol = false;
                NPCController.canPatrol = false;
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
