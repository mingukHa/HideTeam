using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NamedNPC))]
public class NamedController : NPCController
{
    public List<string> dialogue;
    public List<string> AngryEvent1Dialogue;
    public List<string> AngryEvent2Dialogue;
    private bool dialogueend = false;

    public override void Start()
    {
        base.Start();
    }

    public void StartKar()
    {
        stateMachine.ChangeState(new EventWaitState(this));
    }
    public void PlayerEnter()
    {
        stateMachine.ChangeState(new RoutineState(this));
    }
    public void VIPAngry()
    {
        dialogue = AngryEvent1Dialogue;
        stateMachine.ChangeState(new AngryState(this));
    }

    public IEnumerator DialogueCoroutine()
    {
        foreach (string str in dialogue)
        {
            Debug.Log(str);
            yield return new WaitForSeconds(2.0f);
        }
        dialogueend = true;
    }
    public bool DialogueEnd()
    {
        return dialogueend;
    }
}
