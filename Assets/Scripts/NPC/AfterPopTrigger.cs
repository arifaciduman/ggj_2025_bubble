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
        if (other.CompareTag("NPC") && other != _parent)
        {
            if (!NPCController.otherNearNPCs.Contains(other.GetComponent<NPCController>()))
            {
                NPCController.otherNearNPCs.Add(other.GetComponent<NPCController>());
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("NPC") && other != _parent)
    //    {
    //        if (!NPCController.otherNearNPCs.Contains(other.GetComponent<NPCController>()))
    //        {
    //            NPCController.otherNearNPCs.Add(other.GetComponent<NPCController>());
    //        }
    //    }
    //}
}
