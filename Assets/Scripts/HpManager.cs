using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

// 인게임, 인벤토리, 세이브파일화면에서 쓸려고 만듦
// 체력 하트 UI 관리

public class HpManager : MonoBehaviour
{
    public GameObject[] hearts; // 채워진 하트 이미지들
    private int maxHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHp = PlayerDataManager.Instance.GetMaxHp(); // 저장된 최대 체력 불러오기. 총 하트칸 수
        // hp 불러오기
        UpdateHearts(PlayerDataManager.Instance.GetHp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //현재 hp를 ui에 하트로 표시
    public void UpdateHearts(int currentHp)
    {
        int hp = currentHp; 

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i <= hp)
                hearts[i].SetActive(true); // 하트 보이기
            else
                hearts[i].SetActive(false); // 하트 숨기기
        }
        Debug.Log("현재 HP = " + hp); //임시 확인용
    }

    // 체력 회복 함수(물약 사용)
    public void Heal(int healingAmout)
    {
        int currentHp = PlayerDataManager.Instance.GetHp(); // 현재 체력 가져오기
        //float additionalHealingRate = PlayerDataManager.Instance.GetAdditionalHealingProbability(); // 회복 확률 가져오기 //////////////보류

        //float randomValue = Random.Range(1f, 100f);
        //PlayerDataManager.Instance.SetHp(healingAmout + additionalHealingRate); // 체력을 1 * (1+추가회복비율) 증가  ////////////////보류
        //UpdateHearts(PlayerDataManager.Instance.GetHp());
        //Debug.Log($"HP +{healingAmout * (1 + additionalHealingRate)}");
    }

    // 체력 최대로 회복 함수(특정 지점에 가면)
    public void FullHeal()
    {
        int currentHp = maxHp; // 최대 체력으로 설정
        PlayerDataManager.Instance.SetHp(currentHp); // maxHp를 현재 체력에 더해서 max로 만듦
        UpdateHearts(PlayerDataManager.Instance.GetHp()); 
        Debug.Log("HP Full");
    }


    // 체력 감소 함수(몬스터 피격)
    public void TakeDamage(int damage)
    {
        int currentHp = PlayerDataManager.Instance.GetHp(); // 현재 체력 가져오기
        PlayerDataManager.Instance.SetHp(-damage); // 현재 체력에 damage를 뺌
        UpdateHearts(PlayerDataManager.Instance.GetHp());
        Debug.Log($"HP -{damage}");
    }

    // 최대 체력 증가
    public void IncreaseMaxHp()
    {
        PlayerDataManager.Instance.AddMaxHp(1); 
        UpdateHearts(PlayerDataManager.Instance.GetHp()); // UI 업데이트
        // 실제로 하트 칸 수가 늘어나게 개발 예정
    }
}
