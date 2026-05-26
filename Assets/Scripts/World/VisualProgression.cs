using System.Collections.Generic;
using UnityEngine;

public class VisualProgression : MonoBehaviour
{
    private CurrencySystem currency;
    private readonly List<GameObject> levels = new();
    public int VisualLevel { get; private set; }

    public void Initialize(CurrencySystem currencySystem)
    {
        currency = currencySystem;
        CreateWorld();
        currency.OnMoneyChanged += _ => Evaluate();
    }

    private void CreateWorld()
    {
        Camera.main.backgroundColor = new Color(0.95f, 0.88f, 0.74f);

        for (int i = 0; i < 5; i++)
        {
            var kiosk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            kiosk.transform.position = new Vector3(0f, -2f + (i * 0.15f), 0f);
            kiosk.transform.localScale = new Vector3(2f + i * 0.5f, 1f + i * 0.3f, 1f);
            kiosk.GetComponent<Renderer>().material.color = Color.Lerp(new Color(0.5f, 0.25f, 0.1f), Color.gray, i / 4f);
            kiosk.SetActive(i == 0);
            levels.Add(kiosk);
        }
    }

    private void Evaluate()
    {
        int target = currency.Money switch
        {
            > 5000 => 4,
            > 2000 => 3,
            > 700 => 2,
            > 200 => 1,
            _ => 0
        };

        if (target == VisualLevel) return;
        levels[VisualLevel].SetActive(false);
        VisualLevel = target;
        levels[VisualLevel].SetActive(true);
    }

    public void Save(SaveData data) => data.visualLevel = VisualLevel;

    public void Load(SaveData data)
    {
        VisualLevel = Mathf.Clamp(data.visualLevel, 0, 4);
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(i == VisualLevel);
        }
    }
}
