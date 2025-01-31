using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI 출력용

public class NPCChatTest : MonoBehaviour
{
    private DatabaseReference dbReference;
    public TextMeshProUGUI dialogueText; // NPC 대사를 표시할 UI

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoadNPCDialogue(string npcID)
    {
        dbReference.Child("NPC_Dialogues").Child(npcID).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        // JSON 데이터를 C# 객체로 변환
                        string json = snapshot.GetRawJsonValue();
                        DialogueData data = JsonUtility.FromJson<DialogueData>(json);

                        if (data != null && data.dialogues.Length > 0)
                        {
                            // 첫 번째 대사 출력
                            dialogueText.text = data.dialogues[0];                            
                        }
                        else
                        {
                            dialogueText.text = "대사가 없습니다.";
                            Debug.LogError("대사 데이터가 비어 있습니다.");
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
