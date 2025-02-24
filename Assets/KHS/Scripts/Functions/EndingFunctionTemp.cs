using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class EndingFunctionTemp : MonoBehaviour
{
    private DatabaseReference dbRef = null;
    public string npcID = "Ending";

    public PlayerController player;
    public MatDetChange MDC_Collider;
    public MatDetChange MDC_2Collider;

    public GameObject correctDisguse;
    public TellerController tellerCon;

    public TextMeshProUGUI dialogueText;

    [SerializeField]
    private bool isGcode = false;
    [SerializeField]
    private bool isDisguse = false;
    [SerializeField]
    private bool isFlowClear = false;
    [SerializeField]
    private bool isDet = false;
    [SerializeField]
    private bool isDefault = true;
    [SerializeField]
    private bool alreadyStarted = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // 대사 큐
    private bool isDialoguePlaying = false; // 대사가 진행 중인지 확인
    private bool isWaitingForEvent = false;

    public EventManager.GameEventType convEnv;
    public List<EventManager.GameEventType> EndingEnvList;
    public int EndingIdx = 0;

    private void OnEnable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.RichKill, FlowEnd);
        EventManager.Subscribe(EventManager.GameEventType.RichKill, FlowEnd);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.RichKill, FlowEnd);
    }

    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;

        isDefault = true;
        isGcode = false;
        isDisguse = false;
        isFlowClear = false;
        isDet = false;
    }

    private void FixedUpdate()
    {
        isDisguse = correctDisguse.activeSelf;
        isGcode = MDC_2Collider.gcode;

        if (isFlowClear && isDisguse && isDet && Input.GetKeyDown(KeyCode.E) && !alreadyStarted)
        {
            player.LockMoving();
            tellerCon.talkCollider.radius = 0f;
            tellerCon.stateMachine.ChangeState(new TalkState(tellerCon));
            alreadyStarted = true;
            StartCoroutine(EndingCoroutine());
        }
    }

    private void FlowEnd()
    {
        isFlowClear = true;
    }

    private IEnumerator EndingCoroutine()
    {
        dialogueText.alpha = 255f;
        LastEndingDialogue(npcID);
        yield return new WaitForSeconds(9.0f);
        Debug.Log("9초 경과");
        StopAllCoroutines();
        isDefault = false;
        LastEndingDialogue(npcID);
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
        if(isDefault && data.Default != null) return data.Default;
        if (!isDefault && isGcode && data.Good != null) return data.Good;
        if (!isDefault && !isGcode && data.Bad != null) return data.Bad;
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
        text = Regex.Replace(text, @"/F[A-Z]", "");
        text = text.Replace("/B", "");
        dialogueText.text = text.Replace("/G", ""); // /G 제거 후 출력
        Debug.Log($"[Ending] 대사 출력: {text}");
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
            
            yield return new WaitForSeconds(3.0f);
            if(dialogue.Contains("/B"))
            {
                EventManager.Trigger(EndingEnvList[0]);
            }
            if(dialogue.Contains("/G"))
            {
                player.UnlockMoving();
                EventManager.Trigger(EndingEnvList[1]);
            }
        }
        isDialoguePlaying = false;
        ShowDialogue("");
    }
}
public class EndingDialogueData
{
    public string[] Default;
    public string[] Bad;
    public string[] Good;
}
