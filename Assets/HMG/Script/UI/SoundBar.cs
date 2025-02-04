using UnityEngine;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour
{
    [SerializeField] private Slider soundSlider; // UI���� ������ ���� �����̴�
    private const string VolumeKey = "MasterVolume"; // PlayerPrefs Ű��

    private void Start()
    {
        // ����� ���� �� �ҷ����� (������ �⺻�� 1)
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        soundSlider.value = savedVolume;
        AudioListener.volume = savedVolume; // ��ü ���� ����

        // �����̴� �� ���� �� ���� ����
        soundSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        AudioListener.volume = volume; // ��ü ���� ����
        PlayerPrefs.SetFloat(VolumeKey, volume); // ���� �� ����
        PlayerPrefs.Save();
    }
}
