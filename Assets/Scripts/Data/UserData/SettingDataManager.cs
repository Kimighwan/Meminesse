using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using System.Collections.Generic;
using static ScreenModeSetting;

[Serializable]
public class SettingData
{
    public float BGMValue;
    public float SFXValue;

    public ScreenModeOptions screenMode;

    public SettingData()
    {
        BGMValue = 0.5f;
        SFXValue = 0.5f;
        screenMode = ScreenModeOptions.FullScreenWindow;
    }
    public SettingData(SettingData data)
    {
        BGMValue = data.BGMValue;
        SFXValue = data.SFXValue;
        screenMode = data.screenMode;
    }
}

[Serializable]
public class KeyData
{
    public string keyName;      // 버튼 이름 또는 버튼 사용처
    public KeyCode keyCode;     // KeyCode
    
    public KeyData(KeyData data)
    {
        keyName = data.keyName;
        keyCode = data.keyCode;
    }
    public KeyData(string name, KeyCode keyCode)
    {
        keyName = name;
        this.keyCode = keyCode;
    }
}

// wrapper class to parse data to JSON using JSONUtility
[Serializable]
public class WrapperClassKeyDataList
{
    public List<KeyData> keyDataList;
}

public class SettingDataManager : SingletonBehaviour<SettingDataManager>
{
    private SettingData settingData = new();

    // 테스트로 인스펙터 창에서 학인을 위해 public 선언
    public /*private*/ List<KeyData> keyDataList = new();

    // persistentDataPath로 변경 예정
    private string PATH_SETTING = Path.Combine(Application.dataPath, "settingData.json");
    private string PATH_KEY = Path.Combine(Application.dataPath, "keyData.json");
    private const string KEY = "Ikhwan@@ZZang!!";

    protected override void Init()
    {
        base.Init();

        Load();
    }

    #region Key

    /// <summary>
    /// 필요한 키를 keyName으로 매개변수로 전달하여 함수를 사용
    /// 가령 esc가 무슨 키인지 확인하려면 -> GetKeyCode("ESC");
    /// 이는 밑에 ResetKeyData 함수를 보면 특정 string에 매칭되는 keyCode를 설정 중임
    /// 그냥 초기 설정값이고 키 이름(string 값)에 해당하는 KeyCode가 1:1 형식으로 Data를 Save하고 Load 한다.
    /// </summary>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public KeyCode GetKeyCode(string keyName)
    {
        foreach (var keyData in keyDataList)
        {
            if(keyData.keyName == keyName)
                return keyData.keyCode;
        }
        return KeyCode.None;
    }
    public void ResetKeyData()   // 키 값들을 기본값으로 설정, 추가적인 작업 필요
    {
        keyDataList.Clear();

        // Button Number
        // 0:up , 1:down, 2:right, 3:left, 4:attack, 5:jump, 6:dash, 7:skill1, 8:skill2, 9:skill3, 10:map, 11:inventory, 12:skilltree, 13:interact, 14:heal
        keyDataList.Add(new KeyData("Up", KeyCode.UpArrow));
        keyDataList.Add(new KeyData("Down", KeyCode.DownArrow));
        keyDataList.Add(new KeyData("Right", KeyCode.RightArrow));
        keyDataList.Add(new KeyData("Left", KeyCode.LeftArrow));

        keyDataList.Add(new KeyData("BasicAttack", KeyCode.Z));
        keyDataList.Add(new KeyData("Jump", KeyCode.X));
        keyDataList.Add(new KeyData("Dash", KeyCode.LeftShift));
        keyDataList.Add(new KeyData("BackDash", KeyCode.LeftAlt));
        keyDataList.Add(new KeyData("Skill1", KeyCode.A));
        keyDataList.Add(new KeyData("Skill2", KeyCode.S));
        keyDataList.Add(new KeyData("Skill3", KeyCode.D));
        keyDataList.Add(new KeyData("Map", KeyCode.Tab));
        keyDataList.Add(new KeyData("Inventory", KeyCode.E)); 
        keyDataList.Add(new KeyData("SkillTree", KeyCode.K));
        keyDataList.Add(new KeyData("Interact", KeyCode.F));
        keyDataList.Add(new KeyData("Heal", KeyCode.C)); 

        KeyDataSave();
    }
    public bool ChangeKey(string targetKeyName, KeyCode newKeyCode)    // 변경 성공시 True 실패시 False 반환
    {
        // 예외 처리, 조건 검사는 추후에 추가한다.
        // 1) 설정되면 안 되는 Key인지 검사 - 일단 존재하지 않음
        // 2) 이미 설정된 키가 있는지 검사
        foreach (var keyData in keyDataList)
            if (keyData.keyCode == newKeyCode)
                return false;

        foreach (var keyData in keyDataList)
        {
            if (keyData.keyName == targetKeyName)
            {
                keyData.keyCode = newKeyCode;
                KeyDataSave();
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Get Setting Data
    public float GetBGMValue()
    {
        return settingData.BGMValue;
    }
    public float GetSFXValue()
    {
        return settingData.SFXValue;
    }
    public ScreenModeOptions GetScreenMode()
    {
        return settingData.screenMode;
    }
    #endregion
    #region Set Setting Data
    public void SetBGMValue(float value)
    {
        settingData.BGMValue = value;
        SettingDataSave();
    }
    public void SetSFXValue(float value)
    {
        settingData.SFXValue = value;
        SettingDataSave();
    }
    public void SetScreenMode(ScreenModeOptions mode)
    {
        settingData.screenMode = mode;
        SettingDataSave();
    }
    #endregion
    #region Save - Load

    /*-------------
          SAVE
     -------------*/
    public void SetDefaultData()
    {
        InitSettingData();
        ResetKeyData();
    }
    public void Save()
    {
        SettingDataSave();
        KeyDataSave();
    }
    public void SettingDataSave()
    {
        string jsonData = JsonUtility.ToJson(settingData);
        File.WriteAllText(PATH_SETTING, Encrypt(jsonData, KEY));
    }
    public void KeyDataSave()
    {
        WrapperClassKeyDataList keyDataListWrapper = new WrapperClassKeyDataList();
        keyDataListWrapper.keyDataList = keyDataList;
        string jsonKeyDataListWrapper = JsonUtility.ToJson(keyDataListWrapper);
        File.WriteAllText(PATH_KEY, Encrypt(jsonKeyDataListWrapper, KEY));
    }

    /*-------------
          LOAD
     -------------*/
    public void Load() // 2개의 Data를 load, 파일이 없다면 새로 생성, 있다면 load
    {
        if (File.Exists(PATH_SETTING)) // 저장된 세팅 Data 파일이 존재
        {
            var jsonData = File.ReadAllText(PATH_SETTING);
            settingData = JsonUtility.FromJson<SettingData>(Decrypt(jsonData, KEY));
            SettingDataSave();
        }
        else
        {
            InitSettingData();
        }

        if (File.Exists(PATH_KEY)) // 저장된 키 Data 파일이 존재
        {
            string jsonKeyDataListWrapper = File.ReadAllText(PATH_KEY);
            WrapperClassKeyDataList keyDataListWrapper = JsonUtility.FromJson<WrapperClassKeyDataList>(Decrypt(jsonKeyDataListWrapper, KEY));
            keyDataList = keyDataListWrapper.keyDataList;
            KeyDataSave();
        }
        else
        {
            ResetKeyData();
        }
    }

    private void InitSettingData()
    {
        settingData = null;
        settingData = new SettingData();
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
