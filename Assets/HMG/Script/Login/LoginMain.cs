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
    private TMP_InputField ID;
    [SerializeField]
    private TMP_InputField PW;
    [SerializeField]
    private GameObject AcountUI;
    [SerializeField]
    private Button LoginButton;
    [SerializeField]
    private Button Acount;
    [SerializeField]
    private Button Exit;
    [SerializeField]
    private TextMeshProUGUI logintext;
    [SerializeField]
    private Button Sound;
    [SerializeField]
    private GameObject SoundBar;
    private bool SoundbarOnOff = false;
    private DatabaseReference database;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(InitializeFirebase());
        LoginButton.onClick.AddListener(() =>  Login(ID.text, PW.text));
        Acount.onClick.AddListener(() => OnAcountUI(true));
        Sound.onClick.AddListener(OnSoindUI);
        Exit.onClick.AddListener(GameOff);
    }
    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase �ʱ�ȭ ��...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase �ʱ�ȭ ����!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database �ʱ�ȭ ����!");
            else
                Debug.LogError("Firebase Database �ʱ�ȭ ����!");
        }
        else
        {
            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyTask.Result}");
            yield break;
        }
    }
    private void Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("���̵�� ��й�ȣ�� �Է��ϼ���.");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Firebase Database�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"�����ͺ��̽� ���� ����: {task.Exception}");
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
                        PlayerPrefs.SetString("username", username); //�÷��̾� �̸� ����
                        PlayerPrefs.Save(); //�÷��̾� �̸� ����
                        Debug.Log("�α��� ����!");
                        SceneManager.LoadScene("LobbyScene"); // �α��� ���� �� ���� ������ �̵�
                    }
                    else
                    {
                         logintext.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
                    }
                }
                else
                {
                    logintext.text = "������ ����";
                }
            }
            else
            {
                logintext.text = "����ڸ� ã�� �� �����ϴ�.";
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
    private void GameOff()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private void OnAcountUI(bool Acount)
    {
        AcountUI.SetActive(Acount);
    }
}
