using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RichmanCallTemp : MonoBehaviour
{
    private DatabaseReference dbRef = null;
    public string npcID;

    public TextMeshProUGUI dialogueText;
    public MatDetChange MDC_Collider;

    [SerializeField]
    private bool isDet = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // 대사 큐
    private bool isDialoguePlaying = false; // 대사가 진행 중인지 확인
    private bool alreadyStarted = false;
    private bool isWaitingForEvent = false;

    public EventManager.GameEventType convEnv;


    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;

        isDet = false;
    }

    private void OnEnable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.Conversation4, RichmanCall);
        EventManager.Subscribe(EventManager.GameEventType.Conversation4, RichmanCall);
        EventManager.Subscribe(EventManager.GameEventType.RichKill, RichmanCallStop);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.Conversation4, RichmanCall);
        EventManager.Unsubscribe(EventManager.GameEventType.RichKill, RichmanCallStop);
    }

    private void FixedUpdate()
    {
        bool newIsDet = MDC_Collider.isPrDet;
        if (newIsDet != isDet)
        {
            isDet = newIsDet;
            dialogueText.alpha = isDet ? 255f : 0f;
        }
    }
    private void RichmanCall()
    {
        if (!alreadyStarted)
        {
            alreadyStarted = true;
            StartCoroutine(LastEndingDialogue(npcID));
        }
    }
    private void RichmanCallStop()
    {
        StopCoroutine(LastEndingDialogue(npcID));
        ShowDialogue("");
    }

    private IEnumerator LastEndingDialogue(string npcID)
    {
        bool dialogueLoaded = false;

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
                        DialogueData data = JsonUtility.FromJson<DialogueData>(json);

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
        yield return new WaitUntil(() => dialogueLoaded);

        alreadyStarted = false;
    }
    private string[] GetDialogueBasedOnTarget(DialogueData data)
    {
        if (data.dialogues != null) return data.dialogues;
        return new string[0]; // 기본적으로 빈 배열 반환
    }
    private void ShowDialogue(string text)
    {
        dialogueText.text = text.Replace("/E", ""); // /E 제거 후 출력
        Debug.Log($"[RichMan] 대사 출력: {text}");
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

            // 대사에 /E 이벤트가 포함되어 있는 경우
            if (dialogue.Contains("/E"))
            {

                Debug.Log($"[이벤트 {convEnv} 호출]");
                isWaitingForEvent = true;

                // 이벤트 완료 신호를 받을 때까지 대기
                EventManager.Subscribe(convEnv, OnEventCompleted);
                // EventManager.Trigger(convEnvList[convEnvIdx]);

                while (isWaitingForEvent)
                {
                    yield return null;
                }

                EventManager.Unsubscribe(convEnv, OnEventCompleted);
            }

            yield return new WaitForSeconds(2.0f);
        }
        isDialoguePlaying = false;
        ShowDialogue("");
    }
    private void OnEventCompleted()
    {
        Debug.Log("[이벤트 완료] 다음 대사 출력");
        isWaitingForEvent = false;
    }
}
