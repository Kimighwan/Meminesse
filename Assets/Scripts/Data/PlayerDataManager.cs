using UnityEngine;
using System;
//using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[Serializable]
public class PlayerData
{
    public float hp;
    public float healingAmount;

    public float damage;
    public float addDamage;
    public float defenseIgnoreIncrease;

    public float dashCoolTimeDecrease;
    public float skillCoolTimeDecrease;

    public float itemDropRate;
    public float goldDropRate;

    public PlayerData()
    {
        hp = 100f;
        damage = 10f;
        addDamage = 0f;
        itemDropRate = 0f;
        goldDropRate = 0f;
        healingAmount = 0f;
        defenseIgnoreIncrease = 0f;
        dashCoolTimeDecrease = 0f;
        skillCoolTimeDecrease = 0f;
        Debug.Log("디폴트 생성자 호출!");
    }
    public PlayerData(PlayerData data)
    {
        hp = data.hp;
        damage = data.damage;
        addDamage = data.addDamage;
        itemDropRate = data.itemDropRate;
        goldDropRate = data.goldDropRate;
        healingAmount = data.healingAmount;
        defenseIgnoreIncrease = data.defenseIgnoreIncrease;
        dashCoolTimeDecrease = data.dashCoolTimeDecrease;
        skillCoolTimeDecrease = data.skillCoolTimeDecrease;
        Debug.Log("복사 생성자 호출!");
    }
}

public class PlayerDataManager : SingletonBehaviour<PlayerDataManager>
{
    // 임시 테스트용
    private PlayerData playerData = new PlayerData();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH;
    protected override void Init() // Awake
    {
        base.Init();

        //if(플레이어 데이터 파일이 없다면 새로 생성)
        //{
        //}
        //else // 파일이 있으므로 불러와서 적용하기
        //{ }

        // 경로는 임시로 dataPath로 지정 - 테스터 용이, 추후 아래 주석으로 적용하기
        PATH = Path.Combine(Application.dataPath, "playerData.json");
        //PATH = Path.Combine(Application.persistentDataPath, "playerData.json");
        Load();
    }

    #region Set Value
    public void AddHP()
    {
        playerData.hp += 1;
    }

    public void HealingAmountIncrease()
    {
        playerData.healingAmount += 0.25f;
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
        playerData.itemDropRate += 0.25f;
    }

    public void GoldDropRate()
    {
        playerData.goldDropRate += 0.25f;
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
    public float GetAddHp()
    {
        return playerData.hp;
    }
    public float GetHealingAmount()
    {
        return playerData.healingAmount;
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

    //[ContextMenu("To Json Data")]
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log(jsonData);
        //File.WriteAllText(PATH, jsonData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if(!File.Exists(PATH)) // Create
        {
            Debug.Log("파일 없어서 새로 저장");
            Save();
        }
        else // Load
        {
            Debug.Log("파일 존재해서 로드");
            string loadJson = File.ReadAllText(PATH);
            //playerData = JsonUtility.FromJson<PlayerData>(loadJson);
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
