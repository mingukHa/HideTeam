using UnityEngine;
using System.Collections;
public class TalkToCommand : ICommand
{
    private NPCController npcController;
    private Vector3 targetPosition;
    private bool finished = false;
    private bool isTalking = false;
    //private Transform player; 플레이어 선언
    public TalkToCommand(NPCController _npc, Vector3 _targetPosition)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform; 플레이어 값을 받아와야 동작 합니다
    }

    public void End()
    {
        
    }

    public void Execute()
    {
        //플레이어가 다가와서 e키를 누르면 상호작용 시작 해야 합니다
        npcController.animator.ResetTrigger("Talk"); //토크 상태로 넘어갑니다
       // StartCoroutine(TalkView()); //플레이어가 e키를 누르면 저를 바라보며 시작합니다
    }

    public bool IsFinished() => finished;

    private IEnumerator TalkView()
    {
        if (isTalking) yield break;

        isTalking = true;

        while (isTalking == true)
        {
            //Vector3 direction = (player.position - transform.position).normalized; //플레이어 위치 - 내 위치를 빼고 정규화
            //direction.y = 0; y고정
           // Quaternion lookRotation = Quaternion.LookRotation(direction); //나를 바라보게 하기
            //npcController.transform.rotation = Quaternion.Slerp(npcController.transform.rotation, lookRotation, Time.deltaTime * 5f); //보간 해주기

            yield return null;
        }

    }
    //StopCoroutine(TalkView()); 플레이어가 콜라이더 밖으로 나가면 종료 시켜줘야 합니다
    //transform.rotation = initrotation; 기존에 바라 보던 곳 복구
    //isTalking = false;
}
