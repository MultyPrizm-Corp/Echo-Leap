using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public class GameData{
    public string playerName;
    public List<LevelData> levels = new List<LevelData>();

    public LevelData GetLevel(string name)
    {
        foreach(LevelData i in levels)
        {
            if(i.name == name)
            {
                return i;
            }
        }

        return null;
    }

    public void SetLevel(string name)
    {
        foreach (LevelData i in levels)
        {
            if (i.name == name)
            {
                return;
            }
        }

        LevelData levelData = new LevelData {name = name};

        levels.Add(levelData);
    }
}

[System.Serializable]
public class LevelData
{
    public string name;

}

public class GameDataStore : MonoBehaviour
{
    [SerializeField] private string criptoKey;

    public static GameData gameData;
    private static string EncryptionKey = "your-encryption-key";

    void Start()
    {
        EncryptionKey = criptoKey;
        LoadData();
        Debug.Log(gameData.playerName);
        gameData.playerName = "Test";
        SaveData();
    }

    public void LoadData()
    {

        if (File.Exists("Data/GameData.json"))
        {
            string json = Decrypt(File.ReadAllText("Data/GameData.json"));

            if (json != null)
            {
                gameData = JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                gameData = new GameData();
            }
        }
        else
        {
            gameData = new GameData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(gameData, prettyPrint: true);

        if (!Directory.Exists("Data"))
        {
            Directory.CreateDirectory("Data");
        }

        File.WriteAllText("Data/GameData.json", Encrypt(json));
    }

    

    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            byte[] iv = aes.IV;

            using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                byte[] result = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);
                return Convert.ToBase64String(result);
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        try
        {
            byte[] fullBytes = Convert.FromBase64String(encryptedText);
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.BlockSize / 8];
                Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = iv;

                byte[] encryptedBytes = new byte[fullBytes.Length - iv.Length];
                Buffer.BlockCopy(fullBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        catch (FormatException)
        {
            return null;
        }
        catch (CryptographicException)
        {
            return null;
        }
    }
}
