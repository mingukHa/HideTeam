using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class TellerController : NPCController
{
    protected DatabaseReference dbRef = null;
    public string npcID;
    protected bool isChatting = false;


    public MatDetChange MDC_collider;

    public bool isVIP = false;
    public bool isPlayer = false;
    public bool isOLDMan = false;
    public bool isInterPlayer = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // 대사 큐
    private bool isDialoguePlaying = false; // 대사가 진행 중인지 확인
    private bool isWaitingForEvent = false; // 이벤트 완료 대기 상태
    private bool isPlayerInRange = false; // 플레이어가 대화 범위 안에 있는지 확인

    public List<EventManager.GameEventType> convEnvList;

    public override void Start()
    {
        base.Start();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;
    }

    private void FixedUpdate()
    {
        isPlayer = MDC_collider.isPlayer;
        isVIP = MDC_collider.isVIP;
        isOLDMan = MDC_collider.isOLDMan;


        if (isVIP)
        {
            EventManager.Trigger(EventManager.GameEventType.RichmanTalkTeller);
            LoadTellerDialogue(npcID);
        }
        else if (isOLDMan)
        {
            EventManager.Trigger(EventManager.GameEventType.OldManGotoTeller);
            LoadTellerDialogue(npcID);
        }

        else if (isPlayer && Input.GetKeyDown(KeyCode.E) && !isDialoguePlaying)
        {
            EventManager.Trigger(EventManager.GameEventType.TellerTalk);
            LoadTellerDialogue(npcID);
            isInterPlayer = true; // 플레이어가 대화 시작
        }

        // 대사 가시성 조정
        UpdateDialogueVisibility();
    }
    private void UpdateDialogueVisibility()
    {
        if (isDialoguePlaying && isPlayerInRange)
        {
            dialogueText.alpha = 255f;
        }
        else
        {
            dialogueText.alpha = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerHolder")
        {
            isPlayerInRange = true;
            if (isDialoguePlaying)
            {
                dialogueText.alpha = 255f; // 대화 중이면 즉시 표시
            }
        }
        else if (other.CompareTag("NPC"))
        {
            isChatting = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerHolder")
        {
            isPlayerInRange = false;
            UpdateDialogueVisibility();
        }
        else if (other.CompareTag("NPC"))
        {
            isChatting = false;
        }
    }

    public void LoadTellerDialogue(string npcID)
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
                        TellerDialogueData data = JsonUtility.FromJson<TellerDialogueData>(json);

                        if (data != null)
                        {
                            string[] dialogues = GetDialogueBasedOnTarget(data);

                            if(dialogues.Length > 0)
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

    private string[] GetDialogueBasedOnTarget(TellerDialogueData data)
    {
        if (isOLDMan && data.OldMan != null) return data.OldMan;
        if (isVIP && data.RichMan != null) return data.RichMan;
        if (isPlayer && data.Player != null) return data.Player;
        return new string[0]; // 기본적으로 빈 배열 반환
    }
    private void ShowDialogue(string text)
    {
        dialogueText.text = Regex.Replace(text, @"/E\d+", ""); // /E숫자 부분 제거
        dialogueText.alpha = isPlayerInRange && isChatting ? 255f : 0f; // 범위 내에 있거나 NPC와 대화 중이면 보임
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

            // 대사에 /E 이벤트가 포함되어 있는 경우
            Match match = Regex.Match(dialogue, @"/E(\d+)");
            if (match.Success)
            {
                int eventIndex = int.Parse(match.Groups[1].Value);
                if (eventIndex < convEnvList.Count)
                {
                    Debug.Log($"[이벤트 {eventIndex} 대기!]");
                    isWaitingForEvent = true;

                    // 이벤트 완료 신호를 받을 때까지 대기
                    EventManager.Subscribe(convEnvList[eventIndex], OnEventCompleted);
                    // EventManager.Trigger(convEnvList[eventIndex]);

                    while (isWaitingForEvent)
                    {
                        yield return null;
                    }

                    EventManager.Unsubscribe(convEnvList[eventIndex], OnEventCompleted);
                }
            }

            yield return new WaitForSeconds(2.0f); // 대사 유지 시간
        }

        isDialoguePlaying = false;
        ShowDialogue("");
    }
    
    private void OnEventCompleted()
    {
        Debug.Log("[이벤트 완료] 다음 대사 출력");
        isWaitingForEvent = false;
    }

    public void TellerGone()
    {
        stateMachine.ChangeState(new GoneState(this));
    }
    public void TellerInteract()
    {
        Debug.Log("플레이어가 Teller에게 상호작용!");
        isInterPlayer = true;
    }
}
[Serializable]
public class TellerDialogueData
{
    public string[] OldMan;
    public string[] RichMan;
    public string[] Player;
}