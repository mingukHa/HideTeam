using UnityEngine;
using System.Collections;
public class TalkToCommand : ICommand
{
    private NPCController npcController;
    private Vector3 targetPosition;
    private bool finished = false;
    private bool isTalking = false;
    //private Transform player; �÷��̾� ����
    public TalkToCommand(NPCController _npc, Vector3 _targetPosition)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform; �÷��̾� ���� �޾ƿ;� ���� �մϴ�
    }

    public void End()
    {
        
    }

    public void Execute()
    {
        //�÷��̾ �ٰ��ͼ� eŰ�� ������ ��ȣ�ۿ� ���� �ؾ� �մϴ�
        npcController.animator.ResetTrigger("Talk"); //��ũ ���·� �Ѿ�ϴ�
       // StartCoroutine(TalkView()); //�÷��̾ eŰ�� ������ ���� �ٶ󺸸� �����մϴ�
    }

    public bool IsFinished() => finished;

    private IEnumerator TalkView()
    {
        if (isTalking) yield break;

        isTalking = true;

        while (isTalking == true)
        {
            //Vector3 direction = (player.position - transform.position).normalized; //�÷��̾� ��ġ - �� ��ġ�� ���� ����ȭ
            //direction.y = 0; y����
           // Quaternion lookRotation = Quaternion.LookRotation(direction); //���� �ٶ󺸰� �ϱ�
            //npcController.transform.rotation = Quaternion.Slerp(npcController.transform.rotation, lookRotation, Time.deltaTime * 5f); //���� ���ֱ�

            yield return null;
        }

    }
    //StopCoroutine(TalkView()); �÷��̾ �ݶ��̴� ������ ������ ���� ������� �մϴ�
    //transform.rotation = initrotation; ������ �ٶ� ���� �� ����
    //isTalking = false;
}
