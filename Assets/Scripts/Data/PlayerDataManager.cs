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
    public int additionalHealingAmount;

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
        hp = 100;  //한칸이 20 반칸이 10 풀피가 100
        maxHp = 100;
        damage = 10f;
        addDamage = 0f;
        itemDropRate = 1f;
        goldDropRate = 1f;
        additionalHealingAmount = 0;
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
        additionalHealingAmount = data.additionalHealingAmount;
        defenseIgnoreIncrease = data.defenseIgnoreIncrease;
        dashCoolTimeDecrease = data.dashCoolTimeDecrease;
        skillCoolTimeDecrease = data.skillCoolTimeDecrease;
        weaponStep = data.weaponStep;
    }
}

public class PlayerDataManager : SingletonBehaviour<PlayerDataManager>
{
    private PlayerData playerData;

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH;
    protected override void Init() // Awake
    {
        base.Init();

        // 경로는 임시로 dataPath로 지정 - 테스터 용이, 추후 아래 주석으로 적용하기
        PATH = Path.Combine(Application.dataPath, "playerData.json");
        //PATH = Path.Combine(Application.persistentDataPath, "playerData.json");
        Load();
    }

    #region Set Value
    public void UpgradeWeaponStep()
    {
        playerData.weaponStep += 1;
    }
    public void AddHP()  // 무시하셈 스킬 트리에서 사용하는 이벤트 등록용 함수다
    {
        playerData.maxHp += 1;
    }
    public void SetHp(int value)
    {
        playerData.hp = Mathf.Clamp(playerData.hp + value, 0, playerData.maxHp);
    }
    public void AddMaxHp(int value)
    {
        playerData.maxHp += value;
    }

    public void HealingRateIncrease()
    {
        playerData.additionalHealingAmount += 5;
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
    public int GetWeaponSetp()
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
    public int GetAdditionalHealingAmount()
    {
        return playerData.additionalHealingAmount;
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
            Debug.Log("파일 없어서 새로 저장");
            playerData = new PlayerData();
            Save();
        }
        else // Load
        {
            Debug.Log("파일 존재해서 로드");
            string loadJson = File.ReadAllText(PATH);
            playerData = JsonUtility.FromJson<PlayerData>(Decrypt(loadJson, KEY));
        }
    }
    #endregion

    #region Security
    private static string Encrypt(string plainText, string key)
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

    private static string Decrypt(string cipherText, string key)
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

    private static byte[] AdjustKeyLength(byte[] keyBytes)
    {
        byte[] adjustedKey = new byte[32];
        Array.Copy(keyBytes, adjustedKey, Math.Min(keyBytes.Length, 32));
        return adjustedKey;
    }
    #endregion
}
