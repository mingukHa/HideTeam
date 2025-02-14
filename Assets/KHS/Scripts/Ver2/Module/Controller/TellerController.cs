using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TellerNPC))]
public class TellerController : NPCController
{
    public int interactType = 0;
    public List<string> dialogue;
    public List<string> playerDialogue;
    public List<string> oldManDialogue;
    public List<string> richManDialogue;

    public override void Start()
    {
        base.Start();
    }
    public void Update()
    {
        if (interactType == 1) // 플레이어
            dialogue = playerDialogue;
        else if (interactType == 2) // OldMan
            dialogue = oldManDialogue;
        else if (interactType == 3) // RichMan
            dialogue = richManDialogue;

        Debug.Log(interactType);
    }

    public void Interact()
    {
        stateMachine.ChangeState(new InteractState(this));
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            interactType = 1;
        else if (other.CompareTag("NPC"))
        {
            if (other.name == "OldMan")
                interactType = 2;
            else if (other.name == "RichMan")
                interactType = 3;
            else
                interactType = 0;
        }
    }

    public IEnumerator DialogueCoroutine()
    {
        foreach(string str in dialogue)
        {
            Debug.Log(str);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
