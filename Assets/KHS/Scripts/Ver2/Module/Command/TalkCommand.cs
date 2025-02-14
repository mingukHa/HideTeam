using System.Collections.Generic;
using UnityEngine;


public class TalkCommand : ICommand
{
    private NPCController npcController;
    private string rawText;
    private string filteredText;
    private bool finished = false;
    private int i = 0;

    public TalkCommand(NPCController _npc)
    {
        npcController = _npc;
        rawText = _npc.dialogue.Dequeue();
        filteredText = ProcessText(rawText);
    }

    // ���ڿ��� ���͸��Ͽ� /E�� �����ϰ� �̺�Ʈ ����
    private string ProcessText(string text)
    {
        char[] chars = text.ToCharArray();
        string result = "";

        for (int i = 0; i < chars.Length; i++)
        {
            if (i + 1 < chars.Length && chars[i] == '/' && chars[i + 1] == 'E')
            {
                npcController.TriggerScriptEvent(); // Ư�� �̺�Ʈ ����
                i++; // '/E' ���� �ǳʶٱ�
            }
            else
            {
                result += chars[i];
            }
        }
        return result;
    }

    public void Execute()
    {
        npcController.ShowDialogue(filteredText, End);
    }

    public bool IsFinished()
    {
        return finished;
    }

    public void End()
    {
        finished = true;
    }
}