using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string Key = "shawarma_idle_save";

    public void Save(SaveData data)
    {
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        if (!PlayerPrefs.HasKey(Key)) return new SaveData();
        var json = PlayerPrefs.GetString(Key);
        return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
    }
}
