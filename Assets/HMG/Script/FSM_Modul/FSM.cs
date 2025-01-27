using Unity.VisualScripting;
using UnityEngine;

public class FSM //���� ���� ��ũ��Ʈ
{
    private IState currentState;

    public void ChangeState(IState newState, NomalNPC npc)
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

    public void Update(NomalNPC npc)
    {
        currentState?.UpdateState(npc);
    }
}
