using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// 如果使用的是 Text 组件
// 如果使用 TextMeshPro，替换为 using TMPro;

public class NPCQuestGiver : MonoBehaviour
{
    public string questDescription = "Defeat the Slime"; // Quest description
    private bool questGiven = false; // Tracks if the quest has been given
    private bool playerInRange = false; // Tracks if the player is within interaction range
    public TMP_Text messageText; // UI Text component reference
    // 如果使用 TextMeshPro，则替换为 public TMP_Text messageText;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Press "E" to accept the quest
        {
            if (!questGiven)
            {
                GiveQuest();
            }
        }
    }

    private void GiveQuest()
    {
        questGiven = true;
        UpdateMessage("Quest Accepted: " + questDescription); // Outputs the quest details to the UI
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateMessage("Press E to accept the quest"); // Notify the player they can accept the quest
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UpdateMessage("");
        }
    }

    private void UpdateMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message; // Update the UI text
        }
    }
}


