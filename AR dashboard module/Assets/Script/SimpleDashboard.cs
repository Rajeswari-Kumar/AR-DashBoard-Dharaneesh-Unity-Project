using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SimpleDashboard : MonoBehaviour
{
    private string apiUrl = "https://ar-dashboard-nm.onrender.com/api"; 
    //private string apiUrl = "http://127.0.0.1:5000/api";

    [Header("UI Elements")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI dashboardInfoText;
    public Slider growthRateSlider;

    [Header("Region Text Elements")]
    public TextMeshProUGUI northText;
    public TextMeshProUGUI southText;
    public TextMeshProUGUI eastText;
    public TextMeshProUGUI westText;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color topRegionColor = Color.green;

    private DashboardData currentData;

    void Start()
    {
        statusText.text = "Connecting to API...";
        InitializeUI();
        StartCoroutine(FetchDashboardData());
    }

    void InitializeUI()
    {
        if (growthRateSlider != null)
        {
            growthRateSlider.minValue = 0f;
            growthRateSlider.maxValue = 30f;
            growthRateSlider.value = 0f;
        }
        ResetRegionColors();
    }

    IEnumerator FetchDashboardData()
    {
        while (true)
        {
            yield return StartCoroutine(FetchKPIData());
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator FetchKPIData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/ar-dashboard"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                currentData = JsonUtility.FromJson<DashboardData>(request.downloadHandler.text);
                UpdateKPIDisplay();
                statusText.text = "Connected - Data Updated";
            }
            else
            {
                statusText.text = $"API Error: {request.error}";
                Debug.LogError($"API Error: {request.error}");
            }
        }
    }

    void UpdateKPIDisplay()
    {
        if (currentData?.kpis == null) return;
        if (dashboardInfoText != null)
        {
            dashboardInfoText.text = $"Total Sales: {currentData.kpis.total_sales:N0}\n" +
                                   $"Top Region: {currentData.kpis.region_leader}\n" +
                                   $"Growth Rate: {currentData.kpis.growth_rate}";
        }
        UpdateGrowthRateSlider();
        UpdateRegionDisplay();
    }

    void UpdateGrowthRateSlider()
    {
        if (growthRateSlider != null && currentData?.kpis?.growth_rate != null)
        {
            string growthStr = currentData.kpis.growth_rate.Replace("%", "");
            if (float.TryParse(growthStr, out float growthValue))
            {
                growthRateSlider.value = growthValue;
            }
        }
    }

    void UpdateRegionDisplay()
    {
        ResetRegionColors();
        string topRegion = currentData.kpis.region_leader;

        switch (topRegion)
        {
            case "North":
                if (northText != null) northText.color = topRegionColor;
                break;
            case "South":
                if (southText != null) southText.color = topRegionColor;
                break;
            case "East":
                if (eastText != null) eastText.color = topRegionColor;
                break;
            case "West":
                if (westText != null) westText.color = topRegionColor;
                break;
        }
    }

    void ResetRegionColors()
    {
        if (northText != null) northText.color = normalColor;
        if (southText != null) southText.color = normalColor;
        if (eastText != null) eastText.color = normalColor;
        if (westText != null) westText.color = normalColor;
    }
}
