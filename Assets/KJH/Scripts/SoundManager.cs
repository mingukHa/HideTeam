using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public Slider masterSlider;     //Master ���� ���� �����̴�
    public Slider bgmSlider;        //BGM ���� ���� �����̴�
    public Slider sfxSlider;        //SFX ���� ���� �����̴�
    public AudioMixer mixer;        //���� �����ϱ� ���� �߾� mixer����
    public AudioSource bgmSound;    //bgm�� AudioSource���۳�Ʈ ����
    public AudioSource sfxSound;    //sfx�� AudioSource���۳�Ʈ ����
    public static SoundManager instance;    //�̱��� ���� ����

    public AudioClip[] bgmList;     //����Ʈ�� ���� bgm�ڷ��
    public AudioClip[] sfxList;     //����Ʈ�� ���� sfx�ڷ��

    private void Awake()
    {
        transform.SetParent(null); // �θ𿡼� �и�
        DontDestroyOnLoad(gameObject); // DontDestroyOnLoad ����

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
        //LoginScene�̶� �̸��� ������ ��� �ش� BGM �߻�
        //bgmList[0]�� �� bgm�̸� LoginScene���� ����
        if (name0.name == bgmList[0].name)
            BGMPlay(bgmList[0]);

        else if (name0.name == "LobbyScene")
            BGMPlay(bgmList[0]);

        //MainScene ��
        else if (name0.name == bgmList[1].name)
            BGMPlay(bgmList[1]);

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterSlider masterSliderObject = FindAnyObjectByType<MasterSlider>(FindObjectsInactive.Include);
            Debug.Log("Master Slider�� �Ҵ�Ǿ���.");
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

        //���� ���� �� ����� BGM �������� �ҷ��� �����̴�, �ͼ��� �ݿ�
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            //BGMSlider ��ũ��Ʈ�� �������ִ� ������Ʈ�� ã�´�.
            BGMSlider bgmSliderObject = FindAnyObjectByType<BGMSlider>(FindObjectsInactive.Include);
            Debug.Log("BGM Slider�� �Ҵ�Ǿ���.");

            if (bgmSliderObject)
            {
                //bgmSliderObject �ڽĵ� �� Slider�� ã�´�.
                bgmSlider = bgmSliderObject.GetComponentInChildren<Slider>();

                if (bgmSlider)
                {
                    //BGMVolume Ű�� ����� ���� �������µ� ������ 1f �⺻��
                    float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
                    //�����̴� UI�� ���� savedVolume������ ����
                    bgmSlider.value = savedVolume;
                    BGMSoundVolume(savedVolume);
                }
            }
        }
        //������ �߰��� �̺�Ʈ ������ ��� ���� (�ߺ�ȣ�����)
        bgmSlider?.onValueChanged.RemoveAllListeners();
        //bgmSlider���� ����� ������ BGMSoundVolume �Լ� ȣ��
        bgmSlider?.onValueChanged.AddListener(BGMSoundVolume);

        //���� ���� �� ����� SFX �������� �ҷ��� �����̴�, �ͼ��� �ݿ�
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            //SFXSlider ��ũ��Ʈ�� �������ִ� ������Ʈ�� ã�´�.
            SFXSlider sfxSliderObject = FindAnyObjectByType<SFXSlider>(FindObjectsInactive.Include);
            Debug.Log("SFX Slider�� �Ҵ�Ǿ���.");

            if (sfxSliderObject)
            {
                //sfxSliderObject �ڽĵ� �� Slider�� ã�´�.
                sfxSlider = sfxSliderObject.GetComponentInChildren<Slider>();

                if (sfxSlider)
                {
                    //SFXVolume Ű�� ����� ���� �������µ� ������ 1f �⺻��
                    float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
                    //�����̴� UI�� ���� savedVolume������ ����
                    sfxSlider.value = savedVolume;
                    SFXSoundVolume(savedVolume);
                }
            }
        }

        //������ �߰��� �̺�Ʈ ������ ��� ���� (�ߺ�ȣ�����)
        sfxSlider?.onValueChanged.RemoveAllListeners();
        //sfxSlider���� ����� ������ BGMSoundVolume �Լ� ȣ��
        sfxSlider?.onValueChanged.AddListener(SFXSoundVolume);
    }

    public void MasterSoundVolume(float val)
    {
        float volume = Mathf.Log10(val) * 20;
        mixer.SetFloat("Mastersound", volume);
        PlayerPrefs.SetFloat("MasterVolume", val);
        PlayerPrefs.Save();
    }

    //ȿ���� �÷���, �Ӽ���
    public void SFXPlay(string sfxName, GameObject targetObject)
    {
        //�̸����� ã��
        AudioClip clip = System.Array.Find(
            sfxList, sound => sound.name == sfxName);   //�̸����� ã��

        if (clip != null)
        {
            AudioSource audiosource = targetObject.GetComponent<AudioSource>();  // ������Ʈ���� AudioSource�� ã��

            if (audiosource == null) // ���� ���� AudioSource�� ���ٸ� �߰�
            {
                audiosource = targetObject.AddComponent<AudioSource>();
            }
            
            //�������� ������
            audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX �ͼ� �߰�

            // 3D ȿ�� ����
            audiosource.spatialBlend = 1f;         // 1: 3D ����, 0: 2D ����
            audiosource.minDistance = 1f;          // �ּ� �Ÿ� (Ǯ ����)
            audiosource.maxDistance = 10f;         // �ִ� �Ÿ� (���� ����)
            audiosource.rolloffMode = AudioRolloffMode.Linear;  // ���� ����

            audiosource.clip = clip;    //clip ����
            audiosource.volume = 1f;    //volume��
            audiosource.Play();         //�װ� ����
        }
    }

    //ȿ���� �����̴��� ����Ǵ� ��
    public void SFXSoundVolume(float val)
    {
        // mixer�� log���� ����ϱ� ������ �̷��� ���ϸ� ��������
        float volume = Mathf.Log10(val) * 20;

        // mixer�� ������ ����
        mixer.SetFloat("SFXsound", volume);

        // �����̴��� ���� PlayerPrefs�� ����
        PlayerPrefs.SetFloat("SFXVolume", val);
        PlayerPrefs.Save();
    }

    //����� �÷���, �Ӽ���
    public void BGMPlay(AudioClip clip)
    {
        // ���� ��� ���� BGM�� ������ ��� ���� ������� ����
        if (bgmSound.clip == clip && bgmSound.isPlaying)
        {
            return;
        }

        bgmSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];    //Mixer
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 1f;
        //2D ����� ���� (ī�޶� ȸ�� ���� ����)
        bgmSound.spatialBlend = 0f;
        bgmSound.Play();
    }

    //����� �����̴��� ����Ǵ� ��
    public void BGMSoundVolume(float val)
    {
        // mixer�� log���� ����ϱ� ������ �̷��� ���ϸ� ��������
        float volume = Mathf.Log10(val) * 20;

        // mixer�� ������ ����
        mixer.SetFloat("BGMsound", volume);

        // �����̴��� ���� PlayerPrefs�� ����
        PlayerPrefs.SetFloat("BGMVolume", val);
        PlayerPrefs.Save();
    }
}
