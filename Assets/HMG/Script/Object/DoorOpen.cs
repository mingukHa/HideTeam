﻿using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false; // 문 상태 (열림/닫힘)
    public bool isPrOpen = false;
    public float openAngle = 90f; // 문이 열릴 각도
    public float animationTime = 1f; // 문 열림/닫힘 애니메이션 시간
    public GameObject DoorOpenUI;

    private GameObject Player;
    private Animator PlayerAnimator;
    private Quaternion closedRotation; // 닫힌 상태의 회전값
    private Quaternion openRotation; // 열린 상태의 회전값
    private bool playerdooropen = false;
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
       // Debug.Log($"{Player}문이 받아 옴");
        PlayerAnimator = Player.GetComponent<Animator>();
    }
    private void Start()
    {
        closedRotation = transform.rotation;
    }
    private void Update()
    {
        if (playerdooropen == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isOpen == false)
                {
                    PlayerAnimator.SetTrigger("DoorOpen");

                    isPrOpen = true;
                    OpenDoorBasedOnView(Player.transform);
                }
                else
                {
                    CloseDoor();
                }
            }
           
        }
        
    }
    public void OpenDoorBasedOnView(Transform entity)
    {
        if (entity == null) return;

        // 플레이어(NPC)의 위치
        Vector3 entityPos = entity.transform.position;

        // 문의 위치 (문이 바라보는 방향)
        Vector3 doorPos = transform.position;

        // 플레이어 -> 문 방향의 벡터
        Vector3 openVec = doorPos - entityPos;

        float dotDoor = Vector3.Dot(openVec, transform.forward);

        if(dotDoor >= 0)
        {
            openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        }
        else
        {
            openRotation = Quaternion.Euler(0, -openAngle, 0) * closedRotation;
        }

        ToggleDoor();
    }

    // 문 열기/닫기 토글
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    // 문 열기
    public void OpenDoor()
    {
        SoundManager.instance.SFXPlay("DoorSound", this.gameObject);
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(openRotation));
        isOpen = true;
    }

    // 문 닫기
    public void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(closedRotation));
        isOpen = false;
        isPrOpen = false;
    }

    // 문 애니메이션 처리
    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        yield return new WaitForSeconds(0.2f);
        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < animationTime)
        {
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    // 플레이어나 NPC가 문 가까이에 왔을 때 실행
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            OpenDoorBasedOnView(other.transform);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("문열기 활성화");
            playerdooropen = true;
            DoorOpenUI.SetActive(true);
        }
                
    }
   
    private void OnTriggerExit(Collider other)
   {
       if (other.CompareTag("NPC"))
       {
            Invoke("CloseDoor", 3f);
        }
        else if (other.CompareTag("Player"))
        {
            DoorOpenUI.SetActive(false);
            playerdooropen = false;
            Invoke("CloseDoor", 2f);
        }
    }
    
}
