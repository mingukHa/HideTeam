using UnityEngine;
using UnityEngine.UI;

public class NPCMiniMaps                                                                                                                                                                                                                                                                               : MonoBehaviour
{
    public enum NPCState { Idel, Talk, Run, Walk } //NPC�� ���¿� �°� ���� ����

    [SerializeField] private Image minimapIcon; // �̴ϸ� ������

    private NPCState currentState;

    private void Start()
    {
        ChangeState(NPCState.Idel); //Idel�� ����
    }

    public void ChangeState(NPCState newState) 
    {
        if (currentState == newState) return;

        currentState = newState;
        UpdateMinimapIconColor();
    }

    private void UpdateMinimapIconColor()
    {
        switch (currentState)
        {
            case NPCState.Idel:
                minimapIcon.color = Color.white;
                break;
            case NPCState.Talk:
                minimapIcon.color = Color.yellow;
                break;
            case NPCState.Run:
                minimapIcon.color = Color.red;
                break;
            case NPCState.Walk:
                minimapIcon.color = Color.blue;
                break;
        }
    }
}
