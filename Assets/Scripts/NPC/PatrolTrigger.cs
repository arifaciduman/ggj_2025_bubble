using UnityEngine;

public class PatrolTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.vibe.isFull)
        {
            GameManager.Instance.isGameOver = true;
            GameManager.Instance.vibe.currentValue = -1;
            void caughtPanel()
            {
                GameManager.Instance.canvasManager.SetCaughtPanelActive();
            }

            DelayUtility.ExecuteAfterSeconds(caughtPanel, 3f);
                
            for (int j = 0; j < GameManager.Instance.allNPCs.Count; j++)
            {
                GameManager.Instance.allNPCs[j].PlayerDied();
            }
            Debug.Log("dead from patrol");
        }
    }
}
