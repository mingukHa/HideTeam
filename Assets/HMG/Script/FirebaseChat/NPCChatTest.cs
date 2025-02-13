using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI ��¿�

public class NPCChatTest : MonoBehaviour
{
    private DatabaseReference dbReference;
    public TextMeshProUGUI dialogueText; // NPC ��縦 ǥ���� UI
    public string npcID; // Firebase���� ������ NPC ID
    private BoxCollider chatColider;
    private bool Chating = false;
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        chatColider = GetComponent<BoxCollider>();
    }

   
    public void LoadNPCDialogue(string npcID , int number)
    {
        dbReference.Child("NPC_Dialogues").Child(npcID).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        // JSON �����͸� C# ��ü�� ��ȯ
                        string json = snapshot.GetRawJsonValue();
                        DialogueData data = JsonUtility.FromJson<DialogueData>(json);

                        if (data != null && data.dialogues.Length > 0)
                        {
                            // ù ��° ��� ���
                            dialogueText.text = data.dialogues[number];                            
                        }
                        else
                        {
                            dialogueText.text = "��簡 �����ϴ�.";
                            Debug.LogError("��� �����Ͱ� ��� �ֽ��ϴ�.");
                        }
                    }
                    
                }
                
            });
    }
}
[System.Serializable]
public class DialogueData
{
    public string[] dialogues;
}
