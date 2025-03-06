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

    public MatDetChange MDC_collider;
    public MatDetChange MDC_VisualTalk;

    public SphereCollider talkCollider;
    private float initalRad = 3f;

    public bool isVIP = false;
    public bool isPlayer = false;
    public bool isOldMan = false;
    public bool isPlayerDisg = false;
    public bool isInterPlayer = false;
    public bool isInterDisPlayer = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // 대사 큐
    private bool isDialoguePlaying = false; // 대사가 진행 중인지 확인
    private bool isWaitingForEvent = false; // 이벤트 완료 대기 상태
    private bool isPlayerInRange = false; // 플레이어가 대화 범위 안에 있는지 확인

    private bool inAction = false;

    public List<EventManager.GameEventType> convEnvList;
    public int convEnvIdx = 0;

    public EventManager.GameEventType convEnv;
    public EventManager.GameEventType convEndEnv;

    public override void Start()
    {
        base.Start();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;
        convEnvIdx = 0;

        MDC_collider.OnTriggerEnterCallback += OnTiggerIn;
        MDC_collider.OnTriggerExitCallback += OnTiggerOut;

        initalRad = 3f;
    }

    private void FixedUpdate()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.E) && !isInterPlayer)
        {
            EventManager.Trigger(EventManager.GameEventType.TellerTalk);
            LoadTellerDialogue(npcID);
            isInterPlayer = true; // 플레이어가 대화 시작
        }
        else if(isPlayerDisg && Input.GetKeyDown(KeyCode.E) && !isInterDisPlayer)
        {
            EventManager.Trigger(EventManager.GameEventType.PlayerTalkTeller);
            LoadTellerDialogue(npcID);
            isInterDisPlayer = true;
        }
        isPlayerInRange = MDC_VisualTalk.isPrDet;

        // 대사 가시성 조정
        UpdateDialogueVisibility();
    }

    private void OnTiggerIn(MatDetChange.interType _interType)
    {
        if (_interType == MatDetChange.interType.Player)
            isPlayer = true;
        else if (_interType == MatDetChange.interType.RichMan && !inAction)
        {
            inAction = true;
            isVIP = true;
            EventManager.Trigger(EventManager.GameEventType.RichmanTalkTeller);
            LoadTellerDialogue(npcID);
        }
        else if (_interType == MatDetChange.interType.OldMan && !inAction)
        {
            inAction = true;
            isOldMan = true;
            EventManager.Trigger(EventManager.GameEventType.OldManTalkTeller);
            LoadTellerDialogue(npcID);
        }
    }
    private void OnTiggerOut(MatDetChange.interType _interType)
    {
        if (_interType == MatDetChange.interType.Player)
            isPlayer = false;
        else if (_interType == MatDetChange.interType.RichMan)
            isVIP = false;
        else if (_interType == MatDetChange.interType.OldMan)
        {
            isOldMan = false;
            EventManager.Trigger(EventManager.GameEventType.OldManOut);
        }
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

    private string[] GetDialogueBasedOnTarget(TellerDialogueData data)
    {
        if (isOldMan && data.OldMan != null) return data.OldMan;
        if (isVIP && data.RichMan != null) return data.RichMan;
        if (isPlayer && data.Player != null) return data.Player;
        if (isPlayerDisg && data.PlayerRichman != null) return data.PlayerRichman;
        return new string[0]; // 기본적으로 빈 배열 반환
    }
    private void ShowDialogue(string text)
    {
        Match match = Regex.Match(text, @"/F([A-Z])");
        if (match.Success)
        {
            string convTarget = match.Groups[1].Value;
            ChangeConv(convTarget);
        }
        if (text.Contains("/T"))
        {
            Debug.Log($"[이벤트 {convEnv} 호출");
            EventManager.Trigger(convEnv);
            StartCoroutine(EventCallCoroutine());
        }
        if (text.Contains("/C"))
        {
            Debug.Log($"[대화종료 이벤트 {convEndEnv} 호출");
            TellerReset();
        }
        text = Regex.Replace(text, @"/F[A-Z]", "");
        text = text.Replace("/T", "");
        text = text.Replace("/C", "");
        dialogueText.text = text.Replace("/E", ""); // /E 제거 후 출력
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
            if (dialogue.Contains("/E"))
            {

                Debug.Log($"[이벤트 {convEnvIdx} 대기]");
                isWaitingForEvent = true;

                // 이벤트 완료 신호를 받을 때까지 대기
                EventManager.Subscribe(convEnvList[convEnvIdx], OnEventCompleted);
                // EventManager.Trigger(convEnvList[convEnvIdx]);

                while (isWaitingForEvent)
                {
                    yield return null;
                }

                EventManager.Unsubscribe(convEnvList[convEnvIdx], OnEventCompleted);
                convEnvIdx = (convEnvIdx + 1) % convEnvList.Count;
            }

                yield return new WaitForSeconds(2.0f); // 대사 유지 시간
        }

        isDialoguePlaying = false;
        ShowDialogue("");
        inAction = false;
    }

    private void OnEventCompleted()
    {
        Debug.Log("[이벤트 완료] 다음 대사 출력");
        isWaitingForEvent = false;
    }

    public void TellerGone()
    {
        TellerTalkDisable();
        StartCoroutine(moutline.EventOutLine());
        isInterPlayer = true;
        isInterDisPlayer = true;
        gameObject.tag = "NPCEND";
        stateMachine.ChangeState(new TellerGoneState(this));
    }
    public void TellerInteract()
    {
        TellerTalkDisable();
        stateMachine.ChangeState(new TalkState(this));
        StartCoroutine(moutline.EventOutLine());
        Debug.Log("플레이어가 Teller에게 상호작용!");
        isInterPlayer = true;
    }
    public void TellerInteractOldMan()
    {
        TellerTalkDisable();
        stateMachine.ChangeState(new TalkState(this));
        StartCoroutine(moutline.EventOutLine());
    }
    public void TellerReset()
    {
        EventManager.Trigger(convEndEnv);
    }
    public void TellerTalkDisable()
    {
        talkCollider.radius = 0f;
    }
    public void TellerTalkAble()
    {
        EventManager.Trigger(convEndEnv);
        talkCollider.radius = initalRad;
    }
    public void ChangeConv(string _target)
    {
        Debug.Log($"체인지 콘버세이션 : {_target}");
        switch (_target)
        {
            case "R":
                EventManager.Trigger(EventManager.GameEventType.RichManTalkUI);
                break;
            case "O":
                EventManager.Trigger(EventManager.GameEventType.OldManTalkUI);
                break;
            case "P":
                EventManager.Trigger(EventManager.GameEventType.PlayerTalkUI);
                break;
            case "T":
                EventManager.Trigger(EventManager.GameEventType.TellerTalkUI);
                break;
            case "C":
                EventManager.Trigger(EventManager.GameEventType.CleanerTalkUI);
                break;
            case "G":
                EventManager.Trigger(EventManager.GameEventType.GuardTalkUI);
                break;
            case "Z":
                EventManager.Trigger(EventManager.GameEventType.ResetTalkUI);
                break;
        }
    }
    private IEnumerator EventCallCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            EventManager.Trigger(convEnv);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
[Serializable]
public class TellerDialogueData
{
    public string[] OldMan;
    public string[] RichMan;
    public string[] Player;
    public string[] PlayerRichman;
}
