using UnityEngine;
using UnityEngine.Scripting;
using System;
//using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[Serializable]
public class PlayerData
{
    public int hp;
    public int maxHp;
    public float additionalHealingProbability;

    public float damage;
    public float addDamage;
    public float defenceIgnore;

    public float dashCoolDown;
    public float skillCoolDownDecrease;

    public float itemDropRate;
    public float goldDropRate;

    public int weaponLevel;

    public PlayerData()
    {
        hp = 100;  //한칸이 20 반칸이 10 풀피가 100 /////////////////
        maxHp = 100;
        damage = 10f;
        addDamage = 0f;
        itemDropRate = 1f;
        goldDropRate = 1f;
        additionalHealingProbability = 0;
        defenceIgnore = 0f;
        dashCoolDown = 2f;
        skillCoolDownDecrease = 0f;
        weaponLevel = 1;
    }
    public PlayerData(PlayerData data)
    {
        hp = data.hp;
        maxHp = data.maxHp;
        damage = data.damage;
        addDamage = data.addDamage;
        itemDropRate = data.itemDropRate;
        goldDropRate = data.goldDropRate;
        additionalHealingProbability = data.additionalHealingProbability;
        defenceIgnore = data.defenceIgnore;
        dashCoolDown = data.dashCoolDown;
        skillCoolDownDecrease = data.skillCoolDownDecrease;
        weaponLevel = data.weaponLevel;
    }
}

public class PlayerDataManager : Security, ISaveAndLoad
{
    private PlayerData playerData;

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "playerData.json");
    //PATH = Path.Combine(Application.persistentDataPath, "playerData.json");

    public void Init()
    {
        Load();
    }

    #region Set Value
    public void UpgradeWeaponLevel()
    {
        playerData.weaponLevel += 1;
    }
    public void AddHP()  // 무시하셈 스킬 트리에서 사용하는 이벤트 등록용 함수다
    {
        playerData.maxHp += 20;
    }
    public void SetHp(int value)
    {
        playerData.hp = Mathf.Clamp(playerData.hp + value, 0, playerData.maxHp);
    }
    public void AddMaxHp(int value)
    {
        playerData.maxHp += value;
    }

    public void HealingProbabilityIncrease()
    {
        playerData.additionalHealingProbability += 0.25f;
    }

    public void DashCoolDownDecrease()
    {
        playerData.dashCoolDown -= 0.5f;
    }

    public void SkillCoolDownDecrease()
    {
        playerData.skillCoolDownDecrease += 0.1f;
    }

    public void ItemDropRate()
    {
        playerData.itemDropRate += 1.25f;
    }

    public void GoldDropRate()
    {
        playerData.goldDropRate += 1.25f;
    }

    public void defenceIgnore()
    {
        playerData.defenceIgnore += 0.5f;
    }

    public void DamageIncrease()
    {
        playerData.addDamage += 0.1f;
    }
    #endregion

    #region Get Value
    public int GetWeaponLevel()
    {
        return playerData.weaponLevel;
    }
    public int GetHp()
    {
        return playerData.hp;
    }
    public int GetMaxHp()
    {
        return playerData.maxHp;
    }
    public float GetAdditionalHealingProbability()
    {
        return playerData.additionalHealingProbability;
    }
    public float GetDashCoolDown()
    {
        return playerData.dashCoolDown;
    }

    public float GetSkillCoolDown()
    {
        return playerData.skillCoolDownDecrease;
    }

    public float GetItemDropRate()
    {
        return playerData.itemDropRate;
    }

    public float GetGoldDropRate()
    {
        return playerData.goldDropRate;
    }

    public float GetDefenseIgnore()
    {
        return playerData.defenceIgnore;
    }

    public float GetDamage()
    {
        return playerData.damage + (playerData.damage * playerData.addDamage);
    }
    public float GetAddDamage()
    {
        return playerData.addDamage;
    }
    #endregion

    #region Save-Load

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log(jsonData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if(!File.Exists(PATH)) // Create
        {
            playerData = new PlayerData();
            Save();
        }
        else // Load
        {
            string loadJson = File.ReadAllText(PATH);
            playerData = JsonUtility.FromJson<PlayerData>(Decrypt(loadJson, KEY));
        }
    }
    #endregion
}
