using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class TellerController : NPCController
{
    protected DatabaseReference dbRef = null;
    public string npcID;
    protected bool Chating = false;


    public MatDetChange MDC_collider;

    public bool isVIP = false;
    public bool isPlayer = false;
    public bool isOLDMan = false;
    
    public override void Start()
    {
        base.Start();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        isPlayer = MDC_collider.isPlayer;
        isVIP = MDC_collider.isVIP;
        isOLDMan = MDC_collider.isOLDMan;
    }
    public void LoadTellerDialogue(string npcID, int number)
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
                                ShowDialogue(dialogues[0]);
                            }
                            else
                            {
                                ShowDialogue("��簡 �����ϴ�");
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
        dialogueText.text = text;
        Debug.Log($"[Teller] ��� ���: {text}");
    }
}
[Serializable]
public class TellerDialogueData
{
    public string[] OldMan;
    public string[] RichMan;
    public string[] Player;
}