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

    public int topPassiveA;
    public int topPassiveB;
    public int topPassiveC;

    public bool[] IsSkillActive;
    public int[] SelectedActive;

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
        // expert
        topPassiveA = 0;
        // normal
        topPassiveB = 0;
        // beginner
        topPassiveC = 0;
        IsSkillActive = new bool[24];
        SelectedActive = new int[3] { 0, 0, 0 };
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
        topPassiveA = data.topPassiveA;
        topPassiveB = data.topPassiveB;
        topPassiveC = data.topPassiveC;
        IsSkillActive = data.IsSkillActive;
        SelectedActive = data.SelectedActive;
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
    public void SetTopPassive(int number, int index)
    {
        if (index == 1) playerData.topPassiveA++;
        else if (index == 2) playerData.topPassiveB++;
        else playerData.topPassiveC++;

        playerData.SelectedActive[number] = index;
    }
    public void UpgradeWeaponLevel()
    {
        playerData.weaponLevel += 1;

        switch (playerData.weaponLevel)
        {
            case 2:
                playerData.damage = 14f;
                break;
            case 3:
                playerData.damage = 20f;
                break;
            case 4:
                playerData.damage = 28f;
                break;
            case 5:
                playerData.damage = 40f;
                break;
            default:
                break;
        }
    }
    public void AddHP()
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

    public void DefenceIgnoreIncrease()
    {
        playerData.defenceIgnore += 50f;
    }

    public void DamageIncrease()
    {
        playerData.addDamage += 0.1f;
    }
    #endregion

    #region Get Value
    public int GetTopPassive(int index)
    {
        if (index == 1) 
            return playerData.topPassiveA;
        else if (index == 2) 
            return playerData.topPassiveB;
        else 
            return playerData.topPassiveC;
    }
    public int GetTopNumber(int number) => playerData.SelectedActive[number - 1];
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

    public void SetDefaultData()
    {
        playerData = new PlayerData();
        Save();
    }
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log(jsonData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            SetDefaultData();
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
