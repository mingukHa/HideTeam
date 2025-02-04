using UnityEngine;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour
{
    [SerializeField] private Slider soundSlider; // UI에서 연결할 사운드 슬라이더
    private const string VolumeKey = "MasterVolume"; // PlayerPrefs 키값

    private void Start()
    {
        // 저장된 볼륨 값 불러오기 (없으면 기본값 1)
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        soundSlider.value = savedVolume;
        AudioListener.volume = savedVolume; // 전체 볼륨 적용

        // 슬라이더 값 변경 시 볼륨 조절
        soundSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        AudioListener.volume = volume; // 전체 볼륨 변경
        PlayerPrefs.SetFloat(VolumeKey, volume); // 볼륨 값 저장
        PlayerPrefs.Save();
    }
}
