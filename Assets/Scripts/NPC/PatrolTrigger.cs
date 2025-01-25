using UnityEngine;

public class PatrolTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.vibe.isFull)
        {
            GameManager.Instance.isGameOver = true;
            GameManager.Instance.vibe.currentValue = 0;
        }
    }
}
