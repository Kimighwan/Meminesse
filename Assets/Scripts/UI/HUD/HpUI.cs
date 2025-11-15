using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

// 인게임, 인벤토리, 세이브파일화면에서 쓸려고 만듦
// 체력 하트 UI 관리

public class HpUI : SingletonBehaviour<HpUI>
{
    public GameObject heartPrefab;      
    public Transform heartParent;       
    private List<Heart> hearts = new List<Heart>();

    private int maxHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHearts();
    }

    private void OnEnable()
    {
        UpdateHearts();
    }

    // 총 하트 개수 설정
    private void InitHearts(int newMaxHp)
    {
        int heartCount = newMaxHp / 20;

        // 하트 가져오기
        hearts.Clear();
        foreach (Transform child in heartParent)
        {
            Heart h = child.GetComponent<Heart>();
            if (h != null) hearts.Add(h);
        }

        // 부족하면 새로 생성
        while (hearts.Count < heartCount)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartParent);
            hearts.Add(heartObj.GetComponent<Heart>());
        }
    }

    // HP만큼 하트 채우기 
    public void UpdateHearts()
    {
        maxHp = PlayerDataManager.Instance.GetMaxHp(); // 저장된 최대 체력 불러오기. 총 하트칸 수
        InitHearts(maxHp);
        int hp = PlayerDataManager.Instance.GetHp();

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHp = hp - (i * 20);

            if (heartHp >= 20)
                hearts[i].SetState(2); // 풀칸
            else if (heartHp >= 10)
                hearts[i].SetState(1); // 반칸
            else
                hearts[i].SetState(0); // 빈칸
        }

        Debug.Log("현재 HP = " + hp);
    }

    public void SyncAllHpUI()
    {
        HpUI[] allUIs = Object.FindObjectsByType<HpUI>(FindObjectsSortMode.None);
        foreach (var ui in allUIs)
            ui.UpdateHearts();
    }

    #region hp manage
    // 체력 회복 함수(물약 사용) - 추가회복량 포함 힐
    public void Heal(int healingAmount)
    {
        int currentHp = PlayerDataManager.Instance.GetHp(); 
        float additionalHealingRate = PlayerDataManager.Instance.GetAdditionalHealingProbability(); // 추가 회복 확률 가져오기 //////////////보류
        PlayerDataManager.Instance.SetHp(healingAmount); 

        if (UnityEngine.Random.value < additionalHealingRate)        // 일정 확률로 추가 회복
            PlayerDataManager.Instance.SetHp(20);

        Debug.Log($"HP +{healingAmount * (1 + additionalHealingRate)}");
        UpdateHearts();
        Canvas.ForceUpdateCanvases();
    }

    // 체력 최대로 회복 함수(빨간물약 사용, 특정 지점에 가면)
    public void FullHeal()
    {
        int currentHp = maxHp;
        PlayerDataManager.Instance.SetHp(currentHp); // maxHp를 현재 체력에 더해서 max로 만듦
        Debug.Log("HP Full");
        UpdateHearts();
    }


    // 체력 감소 함수(디버그용)     //// player 부분에서 함
    public void TakeDamage(int damage)
    {
        int currentHp = PlayerDataManager.Instance.GetHp();
        PlayerDataManager.Instance.SetHp(-damage); 
        Debug.Log($"HP -{damage}");
        UpdateHearts();
    }

    // 최대 체력 증가
    public void IncreaseMaxHp()
    {
        PlayerDataManager.Instance.AddMaxHp(20); 
        InitHearts(PlayerDataManager.Instance.GetMaxHp());
        UpdateHearts();
        Canvas.ForceUpdateCanvases();
    }
    #endregion
}
