using UnityEngine;

public class FSW_NomalNPC : MonoBehaviour
{
    private Animator animator;
    private enum NomalNPCState { idle, Walking, Run, Look}
    private NomalNPCState currentState = NomalNPCState.idle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        //각종 이벤트 받을 곳
    }
    private void Update()
    {

    }
}
