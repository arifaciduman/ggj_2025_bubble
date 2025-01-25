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
        Debug.Log($"other.CompareTag(\"NPC\"): {other.CompareTag("NPC")} other != _parent: {other != _parent}");
        if (other.CompareTag("NPC") && other != _parent)
        {
            if (!NPCController.otherNearNPCs.Contains(other.GetComponent<NPCController>()))
            {
                NPCController.otherNearNPCs.Add(other.GetComponent<NPCController>());
                Debug.Log($"NPCController.otherNearNPCs.Count: {NPCController.otherNearNPCs.Count}");
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
