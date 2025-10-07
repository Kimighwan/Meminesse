using UnityEngine;
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
    public float defenseIgnoreIncrease;

    public float dashCoolTimeDecrease;
    public float skillCoolTimeDecrease;

    public float itemDropRate;
    public float goldDropRate;

    public int weaponStep;

    public PlayerData()
    {
        hp = 100;  //한칸이 20 반칸이 10 풀피가 100 /////////////////
        maxHp = 100;
        damage = 10f;
        addDamage = 0f;
        itemDropRate = 1f;
        goldDropRate = 1f;
        additionalHealingProbability = 0;
        defenseIgnoreIncrease = 0f;
        dashCoolTimeDecrease = 0f;
        skillCoolTimeDecrease = 0f;
        weaponStep = 1;
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
        defenseIgnoreIncrease = data.defenseIgnoreIncrease;
        dashCoolTimeDecrease = data.dashCoolTimeDecrease;
        skillCoolTimeDecrease = data.skillCoolTimeDecrease;
        weaponStep = data.weaponStep;
    }
}

public class PlayerDataManager : SingletonBehaviour<PlayerDataManager>
{
    private PlayerData playerData = new PlayerData();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "playerData.json");
    //PATH = Path.Combine(Application.persistentDataPath, "playerData.json");

    protected override void Init()
    {
        base.Init();

        Load();
    }

    #region Set Value
    public void UpgradeWeaponStep()
    {
        playerData.weaponStep += 1;
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

    public void DashCoolTimeDecrease()
    {
        playerData.dashCoolTimeDecrease += 0.5f;
    }

    public void SkillCoolTimeDecrease()
    {
        playerData.skillCoolTimeDecrease += 0.1f;
    }

    public void ItemDropRate()
    {
        playerData.itemDropRate += 1.25f;
    }

    public void GoldDropRate()
    {
        playerData.goldDropRate += 1.25f;
    }

    public void DefenseIgnoreIncrease()
    {
        playerData.defenseIgnoreIncrease += 0.5f;
    }

    public void DamageIncrease()
    {
        playerData.addDamage += 0.1f;
    }
    #endregion

    #region Get Value
    public int GetWeaponStep()
    {
        return playerData.weaponStep;
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
    public float GetDashCoolTime()
    {
        return playerData.dashCoolTimeDecrease;
    }

    public float GetSkillCoolTime()
    {
        return playerData.skillCoolTimeDecrease;
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
        return playerData.defenseIgnoreIncrease;
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
    #region Security
    protected string Encrypt(string plainText, string key)
    {
        byte[] keyBytes = AdjustKeyLength(Encoding.UTF8.GetBytes(key));

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            byte[] iv = aes.IV;
            using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(iv, 0, iv.Length);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    protected string Decrypt(string cipherText, string key)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        byte[] keyBytes = AdjustKeyLength(Encoding.UTF8.GetBytes(key));
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream(cipher))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    protected byte[] AdjustKeyLength(byte[] keyBytes)
    {
        byte[] adjustedKey = new byte[32];
        Array.Copy(keyBytes, adjustedKey, Math.Min(keyBytes.Length, 32));
        return adjustedKey;
    }
    #endregion
}
