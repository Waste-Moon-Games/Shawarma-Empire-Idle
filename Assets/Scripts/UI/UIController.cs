using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private CurrencySystem currency;
    private ProductionSystem production;
    private UpgradeSystem upgrades;
    private VisualProgression progression;

    private TextMeshProUGUI moneyLabel;
    private TextMeshProUGUI stockLabel;
    private Slider progressBar;
    private readonly List<TextMeshProUGUI> costLabels = new();

    public void Initialize(CurrencySystem currencySystem, ProductionSystem productionSystem, UpgradeSystem upgradeSystem, VisualProgression visualProgression)
    {
        currency = currencySystem;
        production = productionSystem;
        upgrades = upgradeSystem;
        progression = visualProgression;

        BuildCanvas();

        currency.OnMoneyChanged += _ => RefreshAll();
        production.OnStockChanged += RefreshAll;
        production.OnProgress += progress => progressBar.value = progress;
    }

    private void BuildCanvas()
    {
        var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        moneyLabel = CreateText(canvasGo.transform, "Money: $0", new Vector2(20f, -20f), 28);
        stockLabel = CreateText(canvasGo.transform, "Stock: 0", new Vector2(20f, -60f), 24);

        CreateButton(canvasGo.transform, "Готовить", new Vector2(20f, -110f), production.ManualCookClick);
        progressBar = CreateSlider(canvasGo.transform, new Vector2(20f, -160f));

        CreateUpgradeRow(canvasGo.transform, "Speed", new Vector2(20f, -220f), upgrades.BuySpeed);
        CreateUpgradeRow(canvasGo.transform, "Income", new Vector2(20f, -270f), upgrades.BuyIncome);
        CreateUpgradeRow(canvasGo.transform, "Auto", new Vector2(20f, -320f), upgrades.BuyAuto);
        CreateUpgradeRow(canvasGo.transform, "Spawn", new Vector2(20f, -370f), upgrades.BuySpawn);
    }

    private void CreateUpgradeRow(Transform parent, string title, Vector2 position, Func<bool> buyAction)
    {
        CreateButton(parent, title, position, () =>
        {
            buyAction();
            RefreshAll();
        });

        var label = CreateText(parent, "$0", position + new Vector2(180f, 5f), 20);
        costLabels.Add(label);
    }

    private TextMeshProUGUI CreateText(Transform parent, string text, Vector2 position, int size)
    {
        var go = new GameObject(text, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(320f, 36f);

        var label = go.GetComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = size;
        label.color = Color.black;

        return label;
    }

    private Button CreateButton(Transform parent, string title, Vector2 position, Action onClick)
    {
        var go = new GameObject(title + "Btn", typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(140f, 36f);

        var text = CreateText(go.transform, title, new Vector2(8f, -3f), 20);
        text.alignment = TextAlignmentOptions.Center;
        text.rectTransform.anchorMin = Vector2.zero;
        text.rectTransform.anchorMax = Vector2.one;
        text.rectTransform.offsetMin = Vector2.zero;
        text.rectTransform.offsetMax = Vector2.zero;

        var button = go.GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
        return button;
    }

    private Slider CreateSlider(Transform parent, Vector2 position)
    {
        var go = new GameObject("Progress", typeof(RectTransform), typeof(Slider));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(280f, 24f);

        return go.GetComponent<Slider>();
    }

    public void RefreshAll()
    {
        moneyLabel.text = $"Money: ${currency.Money:0}";
        stockLabel.text = $"Stock: {production.PendingShawarmas} | Tier: {progression.VisualLevel + 1}/5";

        costLabels[0].text = $"${upgrades.SpeedCost():0}";
        costLabels[1].text = $"${upgrades.IncomeCost():0}";
        costLabels[2].text = $"${upgrades.AutoCost():0}";
        costLabels[3].text = $"${upgrades.SpawnCost():0}";
    }
}
