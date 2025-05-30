using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;
using Firebase;
using System.Collections;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
public class LoginMain : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField ID; //ID입력 칸
    [SerializeField]
    private TMP_InputField PW; //PW입력 칸
    [SerializeField]
    private GameObject AcountUI; //회원가입 UI
    [SerializeField]
    private Button LoginButton; //로그인 버튼
    [SerializeField]
    private Button Acount; //회원가입 버튼
    [SerializeField]
    private Button Exit; //나가기 버튼
    [SerializeField]
    private TextMeshProUGUI logintext; //로그인 안내 텍스트
    [SerializeField]
    private Button Sound; //사운드 버튼
    [SerializeField]
    private GameObject SoundBar; //사운드 바

    private bool SoundbarOnOff = false; //사운드바 활성화 여부

    private DatabaseReference database; 

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(InitializeFirebase()); //파이어 베이스 초기화 시작
        LoginButton.onClick.AddListener(() =>  Login(ID.text, PW.text)); //버튼 이벤트 등록
        Acount.onClick.AddListener(() => OnAcountUI(true));
        Sound.onClick.AddListener(OnSoindUI);
        Exit.onClick.AddListener(GameOff);
    }
    private IEnumerator InitializeFirebase() //파이어 베이스 초기화 코루틴 (초기화를 꼭 해줘야 합니다)
    {
        Debug.Log("Firebase 초기화 중...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database 초기화 성공!");
            else
                Debug.LogError("Firebase Database 초기화 실패!");
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyTask.Result}");
            yield break;
        }
    }
    private void Login(string username, string password) //회원 정보 조회
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("아이디와 비밀번호를 입력하세요.");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Firebase Database가 초기화되지 않았습니다.");
            return;
        }

        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"데이터베이스 접근 실패: {task.Exception}");
                return;
            }

            if (task.Result.Exists)
            {
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    if (storedPassword == password)
                    {
                        PlayerPrefs.SetString("username", username); //플레이어 이름 저장
                        PlayerPrefs.Save(); //플레이어 이름 저장
                        Debug.Log("로그인 성공!");
                        SceneManager.LoadScene("LobbyScene"); // 로그인 성공 시 다음 씬으로 이동
                    }
                    else
                    {
                         logintext.text = "비밀번호가 일치하지 않습니다.";
                    }
                }
                else
                {
                    logintext.text = "데이터 오류";
                }
            }
            else
            {
                logintext.text = "사용자를 찾을 수 없습니다.";
            }
        });
    }
    private void OnSoindUI()
    {
        if (SoundbarOnOff == false)
        {
            SoundbarOnOff = true;
            SoundBar.SetActive(true);
        }
        else
        {
            SoundbarOnOff = false;
            SoundBar.SetActive(false);
        }
    }
    private void GameOff() //게임 종료 버튼
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //유니티 플레이 모드면 종료하기
#endif
    }
    private void OnAcountUI(bool Acount) //회원가입 UI 켜기
    {
        AcountUI.SetActive(Acount);
    }
}
