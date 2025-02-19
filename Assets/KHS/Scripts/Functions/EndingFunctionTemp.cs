using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class EndingFunctionTemp : MonoBehaviour
{
    private DatabaseReference dbRef = null;
    public string npcID = "Ending";

    public MatDetChange MDC_Collider;
    public MatDetChange MDC_2Collider;

    public GameObject correctDisguse;

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

    private Queue<string> dialogueQueue = new Queue<string>(); // ��� ť
    private bool isDialoguePlaying = false; // ��簡 ���� ������ Ȯ��
    private bool isWaitingForEvent = false;

    public EventManager.GameEventType convEnv;
    public List<EventManager.GameEventType> EndingEnvList;
    public int EndingIdx = 0;

    private void OnEnable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.RichHide, FlowEnd);
        EventManager.Subscribe(EventManager.GameEventType.RichHide, FlowEnd);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.RichHide, FlowEnd);
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
        yield return new WaitForSeconds(8.0f);
        Debug.Log("8�� ���");
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
                        // JSON �����͸� C# ��ü�� ��ȯ
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
                            ShowDialogue("��� �����Ͱ� ��� �ֽ��ϴ�.");
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
        return new string[0]; // �⺻������ �� �迭 ��ȯ
    }
    private void ShowDialogue(string text)
    {
        text = text.Replace("/B", "");
        dialogueText.text = text.Replace("/G", ""); // /G ���� �� ���
        Debug.Log($"[Ending] ��� ���: {text}");
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
            if(dialogue.Contains("/B"))
            {
                EventManager.Trigger(EndingEnvList[0]);
            }
            if(dialogue.Contains("/G"))
            {
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
