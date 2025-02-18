using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class TellerController : NPCController
{
    protected DatabaseReference dbRef = null;
    public string npcID;

    public MatDetChange MDC_collider;

    public bool isVIP = false;
    public bool isPlayer = false;
    public bool isOldMan = false;
    public bool isInterPlayer = false;

    private Queue<string> dialogueQueue = new Queue<string>(); // ��� ť
    private bool isDialoguePlaying = false; // ��簡 ���� ������ Ȯ��
    private bool isWaitingForEvent = false; // �̺�Ʈ �Ϸ� ��� ����
    private bool isPlayerInRange = false; // �÷��̾ ��ȭ ���� �ȿ� �ִ��� Ȯ��

    private bool inAction = false;

    public List<EventManager.GameEventType> convEnvList;
    public int convEnvIdx = 0;

    public override void Start()
    {
        base.Start();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dialogueText.alpha = 0f;
        convEnvIdx = 0;

        MDC_collider.OnTriggerEnterCallback += OnTiggerIn;
        MDC_collider.OnTriggerExitCallback += OnTiggerOut;
    }

    private void FixedUpdate()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.E) && !isInterPlayer)
        {
            EventManager.Trigger(EventManager.GameEventType.TellerTalk);
            LoadTellerDialogue(npcID);
            isInterPlayer = true; // �÷��̾ ��ȭ ����
        }

        // ��� ���ü� ����
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
            isOldMan = false;
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
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerHolder")
        {
            isPlayerInRange = false;
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
                            ShowDialogue("��� �����Ͱ� ��� �ֽ��ϴ�.");
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
        return new string[0]; // �⺻������ �� �迭 ��ȯ
    }
    private void ShowDialogue(string text)
    {
        dialogueText.text = text.Replace("/E", ""); // /E ���� �� ���
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
            if (dialogue.Contains("/E"))
            {

                Debug.Log($"[�̺�Ʈ {convEnvIdx} ȣ��]");
                isWaitingForEvent = true;

                // �̺�Ʈ �Ϸ� ��ȣ�� ���� ������ ���
                EventManager.Subscribe(convEnvList[convEnvIdx], OnEventCompleted);
                // EventManager.Trigger(convEnvList[convEnvIdx]);

                while (isWaitingForEvent)
                {
                    yield return null;
                }

                EventManager.Unsubscribe(convEnvList[convEnvIdx], OnEventCompleted);
                convEnvIdx = (convEnvIdx + 1) % convEnvList.Count;
            }

            yield return new WaitForSeconds(2.0f); // ��� ���� �ð�
        }

        isDialoguePlaying = false;
        ShowDialogue("");
        inAction = false;
    }

    private void OnEventCompleted()
    {
        Debug.Log("[�̺�Ʈ �Ϸ�] ���� ��� ���");
        isWaitingForEvent = false;
    }

    public void TellerGone()
    {
        stateMachine.ChangeState(new TellerGoneState(this));
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
