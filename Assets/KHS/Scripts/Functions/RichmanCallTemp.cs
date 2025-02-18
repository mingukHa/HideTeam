using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class RichmanCallTemp : MonoBehaviour
{
    private DatabaseReference dbRef = null;
    public string npcID;

    public TextMeshProUGUI dialogueText;

    [SerializeField]
    private bool isGcode = false;
    [SerializeField]
    private bool isDet = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // 대사 큐
    private bool isDialoguePlaying = false; // 대사가 진행 중인지 확인


    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;

        isGcode = false;
        isDet = false;
    }

    private void FixedUpdate()
    {

        
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.name == "PlayerHolder")
        {
            isDet = true;
        }
    }
    private void OnTriggerExit(Collider _collider)
    {
        if (_collider.gameObject.name == "PlayerHolder")
            isDet = false;
    }

    private void LastEndingDialogue(string npcID)
    {
        dbRef.Child("NPC_Dialogues").Child(npcID).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        // JSON 데이터를 C# 객체로 변환
                        string json = snapshot.GetRawJsonValue();
                        EndingDialogueData data = JsonUtility.FromJson<EndingDialogueData>(json);

                        if (data != null)
                        {
                            string[] dialogues = GetDialogueBasedOnTarget(data);

                            if (dialogues.Length > 0)
                            {
                                StartDialogueSequence(dialogues);
                            }
                            else
                            {
                                ShowDialogue("");
                            }
                        }
                        else
                        {
                            ShowDialogue("대사 데이터가 비어 있습니다.");
                        }
                    }

                }

            });
    }
    private string[] GetDialogueBasedOnTarget(EndingDialogueData data)
    {
        return new string[0]; // 기본적으로 빈 배열 반환
    }
    private void ShowDialogue(string text)
    {
        dialogueText.text = text;
        Debug.Log($"[Teller] 대사 출력: {text}");
    }

    private void StartDialogueSequence(string[] dialogues)
    {
        dialogueQueue.Clear();
        foreach (string dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }

        isDialoguePlaying = true;
        StartCoroutine(PlayDialogue());
    }
    private IEnumerator PlayDialogue()
    {
        while (dialogueQueue.Count > 0)
        {
            string dialogue = dialogueQueue.Dequeue();
            ShowDialogue(dialogue);
            yield return new WaitForSeconds(2.0f);
        }
        isDialoguePlaying = false;
        ShowDialogue("");
    }
}
