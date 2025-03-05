using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public Slider masterSlider;     //Master 볼륨 조절 슬라이더
    public Slider bgmSlider;        //BGM 볼륨 조절 슬라이더
    public Slider sfxSlider;        //SFX 볼륨 조절 슬라이더
    public AudioMixer mixer;        //볼륨 조절하기 위한 중앙 mixer변수
    public AudioSource bgmSound;    //bgm용 AudioSource컴퍼넌트 관련
    public AudioSource sfxSound;    //sfx용 AudioSource컴퍼넌트 관련
    public static SoundManager instance;    //싱글톤 위한 변수

    public AudioClip[] bgmList;     //리스트에 넣을 bgm자료들
    public AudioClip[] sfxList;     //리스트에 넣을 sfx자료들

    private void Awake()
    {
        transform.SetParent(null); // 부모에서 분리
        DontDestroyOnLoad(gameObject); // DontDestroyOnLoad 적용

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene name0, LoadSceneMode name1)
    {
        //LoginScene이란 이름이 동일한 경우 해당 BGM 발생
        //bgmList[0]에 들어갈 bgm이름 LoginScene으로 설정
        if (name0.name == bgmList[0].name)
            BGMPlay(bgmList[0]);

        else if (name0.name == "LobbyScene")
            BGMPlay(bgmList[0]);

        //MainScene 용
        else if (name0.name == bgmList[1].name)
            BGMPlay(bgmList[1]);

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterSlider masterSliderObject = FindAnyObjectByType<MasterSlider>(FindObjectsInactive.Include);
            Debug.Log("Master Slider가 할당되었다.");
            if (masterSliderObject)
            {
                masterSlider = masterSliderObject.GetComponentInChildren<Slider>();
                if (masterSlider)
                {
                    float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
                    masterSlider.value = savedVolume;
                    MasterSoundVolume(savedVolume);
                }
            }
        }
        masterSlider?.onValueChanged.RemoveAllListeners();
        masterSlider?.onValueChanged.AddListener(MasterSoundVolume);

        //게임 실행 시 저장된 BGM 볼륨값을 불러와 슬라이더, 믹서에 반영
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            //BGMSlider 스크립트를 가지고있는 오브젝트를 찾는다.
            BGMSlider bgmSliderObject = FindAnyObjectByType<BGMSlider>(FindObjectsInactive.Include);
            Debug.Log("BGM Slider가 할당되었다.");

            if (bgmSliderObject)
            {
                //bgmSliderObject 자식들 중 Slider를 찾는다.
                bgmSlider = bgmSliderObject.GetComponentInChildren<Slider>();

                if (bgmSlider)
                {
                    //BGMVolume 키에 저장된 값을 가져오는데 없으면 1f 기본값
                    float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
                    //슬라이더 UI의 값을 savedVolume값으로 설정
                    bgmSlider.value = savedVolume;
                    BGMSoundVolume(savedVolume);
                }
            }
        }
        //기존에 추가된 이벤트 리스너 모두 제거 (중복호출방지)
        bgmSlider?.onValueChanged.RemoveAllListeners();
        //bgmSlider값이 변경될 때마다 BGMSoundVolume 함수 호출
        bgmSlider?.onValueChanged.AddListener(BGMSoundVolume);

        //게임 실행 시 저장된 SFX 볼륨값을 불러와 슬라이더, 믹서에 반영
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            //SFXSlider 스크립트를 가지고있는 오브젝트를 찾는다.
            SFXSlider sfxSliderObject = FindAnyObjectByType<SFXSlider>(FindObjectsInactive.Include);
            Debug.Log("SFX Slider가 할당되었다.");

            if (sfxSliderObject)
            {
                //sfxSliderObject 자식들 중 Slider를 찾는다.
                sfxSlider = sfxSliderObject.GetComponentInChildren<Slider>();

                if (sfxSlider)
                {
                    //SFXVolume 키에 저장된 값을 가져오는데 없으면 1f 기본값
                    float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
                    //슬라이더 UI의 값을 savedVolume값으로 설정
                    sfxSlider.value = savedVolume;
                    SFXSoundVolume(savedVolume);
                }
            }
        }

        //기존에 추가된 이벤트 리스너 모두 제거 (중복호출방지)
        sfxSlider?.onValueChanged.RemoveAllListeners();
        //sfxSlider값이 변경될 때마다 BGMSoundVolume 함수 호출
        sfxSlider?.onValueChanged.AddListener(SFXSoundVolume);
    }

    public void MasterSoundVolume(float val)
    {
        float volume = Mathf.Log10(val) * 20;
        mixer.SetFloat("Mastersound", volume);
        PlayerPrefs.SetFloat("MasterVolume", val);
        PlayerPrefs.Save();
    }

    //효과음 플레이, 속성값
    public void SFXPlay(string sfxName, GameObject targetObject)
    {
        //이름으로 찾기
        AudioClip clip = System.Array.Find(
            sfxList, sound => sound.name == sfxName);   //이름으로 찾기

        if (clip != null)
        {
            AudioSource audiosource = targetObject.GetComponent<AudioSource>();  // 오브젝트에서 AudioSource를 찾음

            if (audiosource == null) // 만약 문에 AudioSource가 없다면 추가
            {
                audiosource = targetObject.AddComponent<AudioSource>();
            }
            
            //사운드파일 가져옴
            audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX 믹서 추가

            // 3D 효과 설정
            audiosource.spatialBlend = 1f;         // 1: 3D 사운드, 0: 2D 사운드
            audiosource.minDistance = 1f;          // 최소 거리 (풀 볼륨)
            audiosource.maxDistance = 10f;         // 최대 거리 (감쇠 시작)
            audiosource.rolloffMode = AudioRolloffMode.Linear;  // 선형 감쇠

            audiosource.clip = clip;    //clip 파일
            audiosource.volume = 1f;    //volume값
            audiosource.Play();         //그걸 실행
        }
    }

    //효과음 슬라이더에 적용되는 값
    public void SFXSoundVolume(float val)
    {
        // mixer는 log값을 사용하기 때문에 이렇게 안하면 문제생김
        float volume = Mathf.Log10(val) * 20;

        // mixer에 볼륨을 적용
        mixer.SetFloat("SFXsound", volume);

        // 슬라이더의 값을 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("SFXVolume", val);
        PlayerPrefs.Save();
    }

    //배경음 플레이, 속성값
    public void BGMPlay(AudioClip clip)
    {
        // 현재 재생 중인 BGM과 동일한 경우 새로 재생하지 않음
        if (bgmSound.clip == clip && bgmSound.isPlaying)
        {
            return;
        }

        bgmSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];    //Mixer
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 1f;
        //2D 사운드로 설정 (카메라 회전 영향 제거)
        bgmSound.spatialBlend = 0f;
        bgmSound.Play();
    }

    //배경음 슬라이더에 적용되는 값
    public void BGMSoundVolume(float val)
    {
        // mixer는 log값을 사용하기 때문에 이렇게 안하면 문제생김
        float volume = Mathf.Log10(val) * 20;

        // mixer에 볼륨을 적용
        mixer.SetFloat("BGMsound", volume);

        // 슬라이더의 값을 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("BGMVolume", val);
        PlayerPrefs.Save();
    }
}
