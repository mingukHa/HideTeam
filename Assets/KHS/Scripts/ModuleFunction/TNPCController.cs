using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TNPCController : MonoBehaviour
{
    private StateController stateController;
    private NavMeshAgent agent;
    public Transform target; // 플레이어 위치

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<StateController>();

        // 초기 상태: Idle
        stateController.ChangeState(new IdleState(this));

        // 일정 시간마다 상태 업데이트 (실제 게임에서는 필요 없을 수도 있음)
        StartCoroutine(StateUpdateLoop());
    }

    public void ChangeState(NPCState newState)
    {
        stateController.ChangeState(newState);
    }

    public void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    // 탐지, 추격 관련 메서드
    public void StartDetection() { Debug.Log("탐지 기능 활성화"); }
    public void EndDetection() { Debug.Log("탐지 기능 비활성화"); }

    public void StartChase() { Debug.Log("추격 기능 활성화"); }
    public void EndChase() { Debug.Log("추격 기능 비활성화"); }

    public void StartPatrol() { Debug.Log("순찰 기능 활성화"); }
    public void EndPatrol() { Debug.Log("순찰 기능 비활성화"); }

    // 상태 변경을 위한 조건 체크
    public bool CanDetectPlayer() { return Random.value > 0.7f; } // 예제: 30% 확률로 탐지 시작
    public bool PlayerSpotted() { return Random.value > 0.5f; } // 예제: 50% 확률로 추격 시작
    public bool PlayerVisible() { return Random.value > 0.2f; } // 예제: 80% 확률로 추격 유지
    public bool CanPatrol() { return Random.value > 0.4f; }

    // 상태 업데이트 루프 (테스트용)
    private IEnumerator StateUpdateLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2초마다 상태 업데이트
            stateController.UpdateState();
        }
    }
}
