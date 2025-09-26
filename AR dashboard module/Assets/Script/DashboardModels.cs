using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DashboardData
{
    public KPIData kpis;
    public Dictionary<string, RegionalData> regional_data;
    public Dictionary<string, int> trend_data;
}

[Serializable]
public class KPIData
{
    public int total_sales;
    public string region_leader;
    public string growth_rate;
}

[Serializable]
public class RegionalData
{
    public int total_sales;
    public float avg_sales;
    public int transactions;
    public PositionData position;
}

[Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class VisualizationData
{
    public BarData[] bars;
    public int max_value;
}

[Serializable]
public class BarData
{
    public string region;
    public int month;
    public int value;
    public float height;
    public string color;
}
