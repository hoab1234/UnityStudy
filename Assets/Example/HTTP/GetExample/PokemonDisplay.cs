using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using TMPro;

[Serializable]
public class PokemonData
{
    public int id;
    public int height;
    public int base_experience;
    public Ability[] abilities;
    public Sprites sprites;
}

[Serializable]
public class Ability
{
    public AbilityDetail ability;
    public bool is_hidden;
}

[Serializable]
public class AbilityDetail
{
    public string name;
}

[Serializable]
public class Sprites
{
    public string front_default;
}

public class PokemonDisplay : MonoBehaviour
{

    [SerializeField] private Image pokemonImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text heightText;
    [SerializeField] private TMP_Text baseExpText;
    [SerializeField] private TMP_Text abilitiesText;

    private void Start()
    {
        StartCoroutine(GetPokemonData());
    }

    IEnumerator GetPokemonData()
    {
        // 피카츄 데이터 가져오기

        // API 엔드포인트 URL을 지정합니다
        string url = "https://pokeapi.co/api/v2/pokemon/pikachu";

        // UnityWebRequest를 사용하여 HTTP GET 요청을 생성합니다.
        // UnityWebRequest: Unity에서 웹 요청을 처리하기 위한 클래스
        // Get(): HTTP GET 요청을 생성하는 메서드
        // SendWebRequest(): 실제 웹 요청을 보내는 메서드
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest(); // 웹 요청을 보내고 응답을 기다립니다.

            // 요청이 성공적으로 완료되었는지 확인
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("피카츄 데이터: " + request.downloadHandler.text);
                // JSON 파싱 및 데이터 활용

                  // JSON을 PokemonData 객체로 변환
                PokemonData pokemonData = JsonUtility.FromJson<PokemonData>(request.downloadHandler.text);
                
                // UI 업데이트
                UpdatePokemonInfo(pokemonData);
                
                // 이미지 다운로드
                if (!string.IsNullOrEmpty(pokemonData.sprites.front_default))
                {
                    StartCoroutine(GetPokemonSprite(pokemonData.sprites.front_default));
                }
            }
            else
            {
                Debug.LogError("에러: " + request.error);
            }
        } // using 블록이 끝나면 request 객체가 자동으로 정리됩니다.
    }

    void UpdatePokemonInfo(PokemonData data)
    {
        // 기본 정보 업데이트
        nameText.text = "Name: Pikachu";
        idText.text = "ID: " + data.id;
        heightText.text = "Height: " + data.height * 10 + "cm";  // API에서는 데시미터 단위로 제공
        baseExpText.text = "Base Experience: " + data.base_experience;

        // 특성 정보 업데이트
        string abilities = "Abilities:\n";
        foreach (var ability in data.abilities)
        {
            abilities += $"- {ability.ability.name} {(ability.is_hidden ? "(Hidden)" : "")}\n";
        }
        abilitiesText.text = abilities;
    }

    IEnumerator GetPokemonSprite(string spriteUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(spriteUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                pokemonImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
            else
            {
                Debug.LogError("이미지 다운로드 실패: " + request.error);
            }
        }
    }
}
