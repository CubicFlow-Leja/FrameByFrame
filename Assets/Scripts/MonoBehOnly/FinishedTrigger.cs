using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<AbstractController>().ParentController.RewardPanel = true;
            InGameMenu.GameMenu.OpenRewardScreen();
        }
            
    }
}
