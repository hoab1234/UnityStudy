using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using TMPro;
using System.Text;
public class AudioRecorder : MonoBehaviour
{
    public Button recordButton;
    [SerializeField] public AudioSource audioSource;
    public AudioClip soundEffectstart;
    public AudioClip soundEffectend;
    private bool isRecording = false;
    private float lastSoundTime = 0f;
    public TextMeshProUGUI output_text;
    private const string OPENAI_API_KEY = ""; // Replace with your actual API key

    // 시스템 언어 감지용 변수
    private SystemLanguage currentSystemLanguage;
    private string targetLanguageCode = "en"; // 기본값은 영어
    private string targetLanguageName = "English"; // 기본값은 영어

    // 키워드 목록 - 한국어 기준
    [Tooltip("응급 의료 상황 키워드")]
    public string[] EmergencyMedicalKeywords = { "긴급상황", "응급상황", "응급", "cpr", "환자", "구급", "의사", "출혈", "심장마비", "구조" };

    [Tooltip("폭발 테러 상황 키워드")]
    public string[] DisasterKeywords = { "폭발", "테러", "화재", "지진", "붕괴", "재난", "대피", "위험", "폭탄" };

    // 번역된 키워드 목록을 저장할 변수
    private string[] translatedMedicalKeywords;
    private string[] translatedDisasterKeywords;

    void Start()
    {
        currentSystemLanguage = Application.systemLanguage;
        SetTargetLanguage(currentSystemLanguage);

        Debug.Log("시스템 언어: " + currentSystemLanguage.ToString());

        if(currentSystemLanguage is not SystemLanguage.Korean)
        {
            Debug.Log("번역 대상 언어: " + targetLanguageName + " (" + targetLanguageCode + ")");
            StartCoroutine(TranslateKeywords());
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the game object.");
            return;
        }

        recordButton.onClick.AddListener(() => {
            ToggleRecording();
            PlaySoundEffect(); // Play sound when the button is pressed
        });
    }

    private IEnumerator TranslateKeywords()
    {
        Debug.Log("키워드 번역 중...");

        // 의료 응급 키워드 번역
        yield return StartCoroutine(TranslateKeywordArray(EmergencyMedicalKeywords, (translatedKeywords) => {
            translatedMedicalKeywords = translatedKeywords;
            Debug.Log("의료 응급 키워드 번역 완료: " + string.Join(", ", translatedKeywords));
        }));

        //재난 상황 키워드 번역
        yield return StartCoroutine(TranslateKeywordArray(DisasterKeywords, (translatedKeywords) =>
        {
            translatedDisasterKeywords = translatedKeywords;
            Debug.Log("재난 상황 키워드 번역 완료: " + string.Join(", ", translatedKeywords));
        }));

        recordButton.interactable = true;
    }

    // 키워드 배열 번역 헬퍼 코루틴
    private IEnumerator TranslateKeywordArray(string[] keywords, System.Action<string[]> callback)
    {
        string keywordsText = string.Join(", ", keywords);

        // API 요청을 위한 JSON 데이터 구성
        string jsonRequestBody = JsonUtility.ToJson(new ChatGPTRequestData
        {
            model = "gpt-3.5-turbo",
            messages = new ChatGPTMessage[]
            {
                new ChatGPTMessage
                {
                    role = "system",
                    content = $"You are a translator. Translate the following Korean keywords to {targetLanguageName}. " +
                              $"Respond with only the translated keywords, separated by commas. " +
                              $"Don't reduce the number of words and Keep technical terms intact (like 'CPR')."
                },
                new ChatGPTMessage
                {
                    role = "user",
                    content = keywordsText
                }
            },
            temperature = 0.3f
        });

        // API 요청 준비
        UnityWebRequest www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + OPENAI_API_KEY);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("번역 에러: " + www.error);
            Debug.LogError(www.downloadHandler.text);

            // 오류 발생 시 원본 키워드 그대로 사용
            callback(keywords);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            ChatGPTResponseData responseData = JsonUtility.FromJson<ChatGPTResponseData>(jsonResponse);

            if (responseData != null && responseData.choices.Length > 0)
            {
                string translatedText = responseData.choices[0].message.content;
                string[] translatedKeywords = translatedText.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                // 공백 제거
                for (int i = 0; i < translatedKeywords.Length; i++)
                {
                    translatedKeywords[i] = translatedKeywords[i].Trim();
                }

                callback(translatedKeywords);
            }
            else
            {
                Debug.LogError("번역 응답 형식 오류");
                callback(keywords);
            }
        }
    }

    private void SetTargetLanguage(SystemLanguage language)
    {
        switch (language)
        {
            case SystemLanguage.Korean:
                targetLanguageCode = "ko";
                targetLanguageName = "한국어";
                break;
            case SystemLanguage.English:
                targetLanguageCode = "en";
                targetLanguageName = "English";
                break;
            case SystemLanguage.Japanese:
                targetLanguageCode = "ja";
                targetLanguageName = "日本語";
                break;
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.Chinese:
                targetLanguageCode = "zh";
                targetLanguageName = "中文";
                break;
            case SystemLanguage.French:
                targetLanguageCode = "fr";
                targetLanguageName = "Français";
                break;
            default:
                targetLanguageCode = "ko";
                targetLanguageName = "한국어";
                break;
        }
    }

    void Update()
    {
        if (isRecording)
        {
            if (audioSource.clip != null && Microphone.GetPosition(null) > 0 && IsSilent())
            {
                if (Time.time - lastSoundTime > 3.0f)
                {
                    StopRecording();
                }
            }
            else
            {
                lastSoundTime = Time.time;
            }
        }
    }

    void ToggleRecording()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        isRecording = true;
        lastSoundTime = Time.time;
    }
    private void StopRecording()
    {
        if (!isRecording) return;
        Microphone.End(null);
        isRecording = false;


        // 파일 저장 과정 생략하고 바로 API 호출
        StartCoroutine(SendToWhisperAPI(audioSource.clip));
        PlaySoundEffectEnd();
    }

    public string Prompt = "응급상황, 도움 요청, 긴급";
    public string Language = "ko";

    private IEnumerator SendToWhisperAPI(AudioClip clip)
    {
        Debug.Log("API 요청 준비 중...");

        // 오디오 클립에서 샘플 데이터 가져오기
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // WAV 파일 형식으로 변환 (헤더 + 샘플 데이터)
        byte[] wavBytes = ConvertAudioClipToWav(clip, samples);

        // API 요청 준비
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", wavBytes, "audio.wav", "audio/wav");
        form.AddField("model", "whisper-1");
        form.AddField("language", Language);
        form.AddField("prompt", Prompt);

        UnityWebRequest www = UnityWebRequest.Post("https://api.openai.com/v1/audio/transcriptions", form);
        www.SetRequestHeader("Authorization", "Bearer " + OPENAI_API_KEY);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            Debug.LogError(www.downloadHandler.text);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            WhisperResponseData data = JsonUtility.FromJson<WhisperResponseData>(jsonResponse);

            if (output_text != null)
            {
                Debug.Log("output :" + data.text);
                ProcessRecognizedText(data.text);

                if(UseTranslate)
                {
                    // 인식된 텍스트 ChatGPT API로 보내 번역 요청
                    StartCoroutine(TranslateWithChatGPT(data.text));
                }
            }
            else
            {
                Debug.LogError("TMP_InputField is not assigned.");
            }
        }
    }

    public bool UseTranslate = false;

    private IEnumerator TranslateWithChatGPT(string textToTranslate)
    {
        Debug.Log("ChatGPT API로 번역 요청 중...");

        // API 요청을 위한 JSON 데이터 구성
        string jsonRequestBody = JsonUtility.ToJson(new ChatGPTRequestData
        {
            model = "gpt-3.5-turbo",
            messages = new ChatGPTMessage[]
            {
                new ChatGPTMessage
                {
                    role = "system",
                    content = $"You are a translator. Translate the following text to {targetLanguageName}. Only respond with the translation, nothing else."
                },
                new ChatGPTMessage
                {
                    role = "user",
                    content = textToTranslate
                }
            },
            temperature = 0.3f
        });

        // API 요청 준비
        UnityWebRequest www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + OPENAI_API_KEY);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("번역 에러: " + www.error);
            Debug.LogError(www.downloadHandler.text);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            ChatGPTResponseData responseData = JsonUtility.FromJson<ChatGPTResponseData>(jsonResponse);

            if (responseData != null && responseData.choices.Length > 0)
            {
                string translatedText = responseData.choices[0].message.content;

                if (output_text != null)
                {
                    Debug.Log("번역된 텍스트: " + translatedText);
                    output_text.text = translatedText;
                    ProcessRecognizedText(translatedText);
                }
                else
                {
                    Debug.LogError("번역 텍스트 컴포넌트가 할당되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogError("번역 응답 형식 오류");
            }
        }
    }


    // AudioClip을 WAV 바이트 배열로 직접 변환
    private byte[] ConvertAudioClipToWav(AudioClip clip, float[] samples)
    {
        using (var memoryStream = new MemoryStream())
        {
            // WAV 헤더 작성 (44바이트)
            using (var writer = new BinaryWriter(memoryStream))
            {
                int hz = clip.frequency;
                int channels = clip.channels;
                int sampleCount = clip.samples;

                // RIFF 헤더
                writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
                writer.Write(36 + sampleCount * channels * 2); // 파일 크기
                writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));

                // 포맷 청크
                writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
                writer.Write(16); // 포맷 청크 크기
                writer.Write((ushort)1); // PCM 포맷
                writer.Write((ushort)channels);
                writer.Write(hz);
                writer.Write(hz * channels * 2); // 초당 바이트 수
                writer.Write((ushort)(channels * 2)); // 블록 정렬
                writer.Write((ushort)16); // 비트 깊이

                // 데이터 청크
                writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
                writer.Write(sampleCount * channels * 2); // 오디오 데이터 크기

                // 오디오 데이터 쓰기
                short[] intData = new short[samples.Length];
                for (int i = 0; i < samples.Length; i++)
                {
                    intData[i] = (short)(samples[i] * 32767);
                    writer.Write(intData[i]);
                }
            }

            return memoryStream.ToArray();
        }
    }

    private void ProcessRecognizedText(string text)
    {
        string lowerText = text.ToLower();

        bool isMedicalEmergency = CheckForKeywords(lowerText, EmergencyMedicalKeywords);
        bool isDisasterSituation = CheckForKeywords(lowerText, DisasterKeywords);

        if (isMedicalEmergency)
        {
            Debug.Log("1번 상황 감지: 응급 의료 상황");
            HandleMedicalEmergency();
        }
        else if (isDisasterSituation)
        {
            Debug.Log("2번 상황 감지: 재난 상황");
            HandleDisasterSituation();
        }
        else
        {
            Debug.Log("특정 상황이 감지되지 않음");
            HandleNormalSituation();
        }
    }

    // 문자열에서 키워드 검사
    private bool CheckForKeywords(string text, string[] keywords)
    {
        foreach (string keyword in keywords)
        {
            if (text.Contains(keyword))
            {
                Debug.Log("감지된 키워드: " + keyword);
                return true;
            }
        }
        return false;
    }

    // 각 상황별 처리 메서드
    private void HandleMedicalEmergency()
    {
        // 1번 상황 처리 로직
        Debug.Log("cpr 콘텐츠 실행");
    }

    private void HandleDisasterSituation()
    {
        // 2번 상황 처리 로직
        Debug.Log("테러 대응 콘텐츠 실행");
    }

    private void HandleNormalSituation()
    {
        // 일반 상황 처리 로직
    }

    private void PlaySoundEffect()
    {
        if (soundEffectstart != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffectstart); // Play the sound effect
        }
    }
    private void PlaySoundEffectEnd()
    {
        if (soundEffectend != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffectend); // Play the sound effect
        }
    }

    public float SilentThreshold = 0.01f;
    public float SampleWindow = 128f;

    private bool IsSilent()
    {
        int SampleWindow = 128;
        float[] samples = new float[SampleWindow];
        int microphonePosition = Microphone.GetPosition(null) - SampleWindow + 1;
        if (microphonePosition < 0) return false;
        audioSource.clip.GetData(samples, microphonePosition);
        float averageLevel = GetAverageVolume(samples);
        return averageLevel < SilentThreshold;
    }

    private float GetAverageVolume(float[] samples)
    {
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        return sum / samples.Length;
    }

    // JSON 직렬화를 위한 클래스들
    [System.Serializable]
    public class WhisperResponseData
    {
        public string text;
    }

    [System.Serializable]
    public class ChatGPTRequestData
    {
        public string model;
        public ChatGPTMessage[] messages;
        public float temperature;
    }

    [System.Serializable]
    public class ChatGPTMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ChatGPTResponseData
    {
        public ChatGPTChoice[] choices;
    }

    [System.Serializable]
    public class ChatGPTChoice
    {
        public ChatGPTMessage message;
    }
}