using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;
using Firebase;
using System.Collections;
using Firebase.Extensions;

public class LoginAcount : MonoBehaviour
{
    [SerializeField] private Button RegisterButton; //회원가입 버튼
    [SerializeField] private GameObject AcountUI; //회원가입 UI
    [SerializeField] private TextMeshProUGUI StatusText; //상태 메세지
    [SerializeField] private Button CloseRegisterUI; //UI닫기
    [SerializeField] private TMP_InputField ID; //ID 입력
    [SerializeField] private TMP_InputField PW; //PW 입력
    [SerializeField] private TMP_InputField PWCheck; //PW 확인
    private DatabaseReference database;
    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference; //DB초기화
        RegisterButton.onClick.AddListener(() => Register(ID.text, PW.text, PWCheck.text));
        CloseRegisterUI.onClick.AddListener(() => AcountUI.SetActive(false));
    }
    private void Register(string username, string password, string confirmPassword) //회원가입 처리 함수
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            StatusText.text = "모든 항목을 입력하세요.";
            return;
        }

        if (password != confirmPassword)
        {
            StatusText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        if (database == null)
        {
            StatusText.text = "데이터베이스 연결 오류.";
            return;
        }
        //아이디 중복 체크
        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                StatusText.text = "데이터베이스 접근 실패.";
                return;
            }

            if (task.Result.Exists)
            {
                StatusText.text = "이미 존재하는 아이디입니다.";
            }
            else 
            {
                // 새로운 사용자 저장
                database.Child("users").Child(username).SetRawJsonValueAsync($"{{\"password\":\"{password}\"}}")
                    .ContinueWithOnMainThread(registerTask =>
                    {
                        if (registerTask.IsCompleted)
                        {
                            StatusText.text = "회원가입 성공!";
                            AcountUI.SetActive(false);
                        }
                        else
                        {
                            StatusText.text = "회원가입 실패. 다시 시도하세요.";
                        }
                    });
            }
        });
    }
}
