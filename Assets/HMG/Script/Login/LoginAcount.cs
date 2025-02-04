using UnityEngine;
using UnityEngine.UI;

public class LoginAcount : MonoBehaviour
{
    [SerializeField] private Button CloseButton;
    [SerializeField] private GameObject AcountUI;

    private void Start()
    {
        CloseButton.onClick.AddListener(()=> AcountUI.SetActive(false));
    }
}
