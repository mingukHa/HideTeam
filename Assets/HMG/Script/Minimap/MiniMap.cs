using UnityEngine;
using UnityEngine.UI;

public class NPCMiniMaps                                                                                                                                                                                                                                                                               : MonoBehaviour
{
    public enum NPCState { Idel, Talk, Run, Walk } //NPC의 상태에 맞게 색상 변경

    [SerializeField] private Image minimapIcon; // 미니맵 아이콘

    private NPCState currentState;

    private void Start()
    {
        ChangeState(NPCState.Idel); //Idel로 시작
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
