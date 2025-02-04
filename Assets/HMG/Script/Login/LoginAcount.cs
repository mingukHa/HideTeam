using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;
using Firebase;
using System.Collections;
using Firebase.Extensions;

public class LoginAcount : MonoBehaviour
{
    [SerializeField] private Button RegisterButton;
    [SerializeField] private GameObject AcountUI;
    [SerializeField] private TextMeshProUGUI StatusText;
    [SerializeField] private Button CloseRegisterUI;
    [SerializeField] private TMP_InputField ID;
    [SerializeField] private TMP_InputField PW;
    [SerializeField] private TMP_InputField PWCheck;
    private DatabaseReference database;
    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        RegisterButton.onClick.AddListener(() => Register(ID.text, PW.text, PWCheck.text));
        CloseRegisterUI.onClick.AddListener(() => AcountUI.SetActive(false));
    }
    private void Register(string username, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            StatusText.text = "��� �׸��� �Է��ϼ���.";
            return;
        }

        if (password != confirmPassword)
        {
            StatusText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            return;
        }

        if (database == null)
        {
            StatusText.text = "�����ͺ��̽� ���� ����.";
            return;
        }

        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                StatusText.text = "�����ͺ��̽� ���� ����.";
                return;
            }

            if (task.Result.Exists)
            {
                StatusText.text = "�̹� �����ϴ� ���̵��Դϴ�.";
            }
            else
            {
                // ���ο� ����� ����
                database.Child("users").Child(username).SetRawJsonValueAsync($"{{\"password\":\"{password}\"}}")
                    .ContinueWithOnMainThread(registerTask =>
                    {
                        if (registerTask.IsCompleted)
                        {
                            StatusText.text = "ȸ������ ����!";
                            AcountUI.SetActive(false);
                        }
                        else
                        {
                            StatusText.text = "ȸ������ ����. �ٽ� �õ��ϼ���.";
                        }
                    });
            }
        });
    }
}
