using UnityEngine;
using UnityEngine.UI;

public class NPCMiniMaps                                                                                                                                                                                                                                                                               : MonoBehaviour
{
    public enum NPCState { Idle, Talk, Run, Walk }

    [SerializeField] private Image minimapIcon; // 미니맵 아이콘 (Inspector에서 할당)

    private NPCState currentState;

    private void Start()
    {
        ChangeState(NPCState.Idle);
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
            case NPCState.Idle:
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
