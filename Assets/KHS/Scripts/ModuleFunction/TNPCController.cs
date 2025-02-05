using UnityEngine;

public class TNPCController : MonoBehaviour
{
    private StateController stateController;
    

    private void Start()
    { 
        stateController = GetComponent<StateController>();

        // 초기 상태: Idle
        stateController.ChangeState(new IdleState(this));

    }

    public void ChangeState(NPCState _State)
    {
        stateController.ChangeState(_State);
    }

    public void MoveToTarget()
    {

    }

    // 탐지, 추격 관련 메서드
    public void StartDetection() { Debug.Log("탐지 기능 활성화"); }
    public void EndDetection() { Debug.Log("탐지 기능 비활성화"); }

    public void StartChase() { Debug.Log("추격 기능 활성화"); }
    public void EndChase() { Debug.Log("추격 기능 비활성화"); }

    public void StartPatrol() { Debug.Log("순찰 기능 활성화"); }
    public void EndPatrol() { Debug.Log("순찰 기능 비활성화"); }

    // 상태 변경을 위한 조건 체크
    public bool CanDetectPlayer() { return false; }
    public bool PlayerSpotted() { return false; }
    public bool PlayerVisible() { return false; }
    public bool CanPatrol() { return false; }

}
