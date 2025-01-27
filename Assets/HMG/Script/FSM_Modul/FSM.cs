using Unity.VisualScripting;
using UnityEngine;

public class FSM //���� ���� ��ũ��Ʈ
{
    private IState currentState;

    public void ChangeState(IState newState, NPC npc)
    {
        if (newState == null)
        {
            Debug.LogError("�� ���°� null�Դϴ�. ���¸� Ȯ���ϼ���.");
            return;
        }

        currentState?.ExitState(npc); // ���� ���� ����
        currentState = newState;
        currentState.EnterState(npc); // �� ���� ����
    }

    public void Update(NPC npc)
    {
        currentState?.UpdateState(npc);
    }
}
