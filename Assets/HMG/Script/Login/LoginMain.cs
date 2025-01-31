using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;

public class LoginMain : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField ID;
    [SerializeField]
    private TMP_InputField PW;
    [SerializeField]
    private Button LoginButton;

    private DatabaseReference database;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;

    }
}
