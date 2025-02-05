using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    private SceneDataStack sceneDataStack = new SceneDataStack();
    private GameObject Player;
    public FadeOut fadeout;
    [SerializeField]
    private GameObject Post;
    [SerializeField]
    private PlayerController mouse;
    private CharacterController PlayerControllers;
    
    private Vector3 Playerposition;
    private Vector3 Npcposition;
    private Quaternion Npcquaternion;
    private Quaternion Playerquaternion;
    
    private Animator npcAnimator;    
    private Animator playerAnimator;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerControllers = Player.GetComponent<CharacterController>();
        npcAnimator = GetComponent<Animator>();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ReturnStack(3f));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneData popReturn = sceneDataStack.PopSceneData();

            if (popReturn != null)
            {
                PlayerOff();
                StartCoroutine(ReturnPlay());
            }
            else
            {
                Debug.Log("스택이 비어 있음");
            }
        }
    }

    public IEnumerator ReturnPlay()
    {
        
        while (sceneDataStack.GetSceneCount() > 0)
        {
            SceneData popReturn = sceneDataStack.PopSceneData();
            float FadeTime = popReturn.Duration;
            if (popReturn != null)
            {
                Post.SetActive(true);
                transform.position = popReturn.NpcPosition;
                transform.rotation = popReturn.NpcRotation;
                Player.transform.position = popReturn.PlayerPosition;
                Player.transform.rotation = popReturn.PlayerRotation;

                yield return null;
                
            }
        }
        SceneManager.LoadScene("MGRealTest");     
    }
    public void PlayerOff()
    {
        PlayerControllers.enabled = false;
        mouse.enabled = false;
    }

    public IEnumerator ReturnStack(float MaxTime)
    {
        Debug.Log("리턴 코루틴 실행");
        float CurrentTime = 0f;

        while (CurrentTime < MaxTime) 
        {
            CurrentTime += Time.deltaTime; 

            Npcposition = transform.position;
            Playerposition = Player.transform.position;
            Npcquaternion = transform.rotation;
            Playerquaternion = Player.transform.rotation;

            SceneData Return = new SceneData(Npcposition, Playerposition, Npcquaternion, Playerquaternion, "Talk", "Walk"/*플레이어 애니메이션*/, MaxTime);
            sceneDataStack.PushSceneData(Return);

            yield return null;
        }

        Debug.Log("리턴 코루틴 종료");
    }
}
