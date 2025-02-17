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

    private Queue<string> dialogueQueue = new Queue<string>(); // ��� ť
    private bool isDialoguePlaying = false; // ��簡 ���� ������ Ȯ��
    private bool isWaitingForEvent = false; // �̺�Ʈ �Ϸ� ��� ����
    private bool isPlayerInRange = false; // �÷��̾ ��ȭ ���� �ȿ� �ִ��� Ȯ��

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
            isInterPlayer = true; // �÷��̾ ��ȭ ����
        }

        // ��� ���ü� ����
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
                dialogueText.alpha = 255f; // ��ȭ ���̸� ��� ǥ��
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
                        // JSON �����͸� C# ��ü�� ��ȯ
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
                            ShowDialogue("��� �����Ͱ� ��� �ֽ��ϴ�.");
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
        return new string[0]; // �⺻������ �� �迭 ��ȯ
    }
    private void ShowDialogue(string text)
    {
        dialogueText.text = Regex.Replace(text, @"/E\d+", ""); // /E���� �κ� ����
        dialogueText.alpha = isPlayerInRange && isChatting ? 255f : 0f; // ���� ���� �ְų� NPC�� ��ȭ ���̸� ����
        Debug.Log($"[Teller] ��� ���: {text}");
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

            // ��翡 /E �̺�Ʈ�� ���ԵǾ� �ִ� ���
            Match match = Regex.Match(dialogue, @"/E(\d+)");
            if (match.Success)
            {
                int eventIndex = int.Parse(match.Groups[1].Value);
                if (eventIndex < convEnvList.Count)
                {
                    Debug.Log($"[�̺�Ʈ {eventIndex} ���!]");
                    isWaitingForEvent = true;

                    // �̺�Ʈ �Ϸ� ��ȣ�� ���� ������ ���
                    EventManager.Subscribe(convEnvList[eventIndex], OnEventCompleted);
                    // EventManager.Trigger(convEnvList[eventIndex]);

                    while (isWaitingForEvent)
                    {
                        yield return null;
                    }

                    EventManager.Unsubscribe(convEnvList[eventIndex], OnEventCompleted);
                }
            }

            yield return new WaitForSeconds(2.0f); // ��� ���� �ð�
        }

        isDialoguePlaying = false;
        ShowDialogue("");
    }
    
    private void OnEventCompleted()
    {
        Debug.Log("[�̺�Ʈ �Ϸ�] ���� ��� ���");
        isWaitingForEvent = false;
    }

    public void TellerGone()
    {
        stateMachine.ChangeState(new GoneState(this));
    }
    public void TellerInteract()
    {
        Debug.Log("�÷��̾ Teller���� ��ȣ�ۿ�!");
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