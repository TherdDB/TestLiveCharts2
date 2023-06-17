using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLiveCharts2.Models;

public class MyModel
{
    public MyModel()
    {
        MaxLimit_ECG = 450;
        MaxLimit_SpO2 = 200;
        MaxLimit_Resp = 100;

        MinLimit_ECG = -450;
        MinLimit_SpO2 = -200;
        MinLimit_Resp = -100;

        Sync_ECG = new();
        Sync_SpO2 = new();
        Sync_Resp = new();

        WaveData_ECG = new List<int>();
        WaveData_SpO2 = new List<int>();
        WaveData_Resp = new List<int>();

        Values_ECG = new ObservableCollection<int>();
        Values_SpO2 = new ObservableCollection<int>();
        Values_Resp = new ObservableCollection<int>();

        for (int i = 0; i < 2000; i++)
        {
            Values_Resp.Add(0);
            Values_ECG.Add(0);
            if (i < 480) Values_SpO2.Add(0);
        }

        XAxes_ECG = new Axis[] { new Axis { IsVisible = false, } };
        XAxes_SpO2 = new Axis[] { new Axis { IsVisible = false, } };
        XAxes_Resp = new Axis[] { new Axis { IsVisible = false, } };

        YAxes_ECG = new Axis[] { new Axis { IsVisible = false, MaxLimit = MaxLimit_ECG, MinLimit = MinLimit_ECG, TicksAtCenter = false, SeparatorsAtCenter = false, ShowSeparatorLines = false, ForceStepToMin = true } };
        YAxes_SpO2 = new Axis[] { new Axis { IsVisible = false, MaxLimit = MaxLimit_SpO2, MinLimit = MinLimit_SpO2, TicksAtCenter = false, SeparatorsAtCenter = false, ShowSeparatorLines = false, ForceStepToMin = true } };
        YAxes_Resp = new Axis[] { new Axis { IsVisible = false, MaxLimit = MaxLimit_Resp, MinLimit = MinLimit_Resp, TicksAtCenter = false, SeparatorsAtCenter = false, ShowSeparatorLines = false, ForceStepToMin = true } };


        Series_ECG = new ObservableCollection<ISeries>() { new LineSeries<int>() { Values = Values_ECG, Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, Stroke = new SolidColorPaint(SKColors.LimeGreen, 1), EasingFunction = null, AnimationsSpeed = TimeSpan.FromMilliseconds(10), } };
        Series_SpO2 = new ObservableCollection<ISeries>() { new LineSeries<int>() { Values = Values_SpO2, Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, Stroke = new SolidColorPaint(SKColors.Aqua, 1), EasingFunction = null, AnimationsSpeed = TimeSpan.FromMilliseconds(10), } };
        Series_Resp = new ObservableCollection<ISeries>() { new LineSeries<int>() { Values = Values_Resp, Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, Stroke = new SolidColorPaint(SKColors.Gold, 1), EasingFunction = null, AnimationsSpeed = TimeSpan.FromMilliseconds(10), } };
        StartThreadForECGData();
    }


    public string Name { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;

    public ObservableCollection<ISeries> Series_ECG { get; set; }
    public ObservableCollection<ISeries> Series_SpO2 { get; set; }
    public ObservableCollection<ISeries> Series_Resp { get; set; }




    // X轴
    public Axis[] XAxes_ECG { get; set; }
    public Axis[] XAxes_SpO2 { get; set; }
    public Axis[] XAxes_Resp { get; set; }

    // Y轴
    public Axis[] YAxes_SpO2 { get; set; }
    public Axis[] YAxes_ECG { get; set; }
    public Axis[] YAxes_Resp { get; set; }

    public object Sync_ECG { get; set; } = new object();
    public object Sync_SpO2 { get; set; } = new object();
    public object Sync_Resp { get; set; } = new object();

    // 数据
    public ObservableCollection<int> Values_ECG { get; set; }
    public ObservableCollection<int> Values_SpO2 { get; set; }
    public ObservableCollection<int> Values_Resp { get; set; }
    public List<int> WaveData_ECG { get; set; }
    public List<int> WaveData_SpO2 { get; set; }
    public List<int> WaveData_Resp { get; set; }



    // X轴 最高数值
    public int MaxLimit_ECG { get; set; }
    public int MaxLimit_SpO2 { get; set; }
    public int MaxLimit_Resp { get; set; }

    // Y轴 最低数值
    public int MinLimit_ECG { get; set; }
    public int MinLimit_SpO2 { get; set; }
    public int MinLimit_Resp { get; set; }


    #region 波形相关

    private void StartThreadForECGData()
    {
        //_taskState = true;
        //// 数据来的时候刷新， 或者 用 开关按钮 控制  进程刷新 和 进程数量
        for (var i = 0; i < 8; i++)
            Task.Run(StartTask_ECG);
        for (int i = 0; i < 2; i++)
            Task.Run(StartTask_SpO2);
        for (int i = 0; i < 4; i++)
            Task.Run(StartTask_Resp);

    }




    private async void StartTask_ECG()
    {
        while (true)
        {
            await Task.Delay(1);
            lock (Sync_ECG)
            {
                if (WaveData_ECG.Count == 0)
                    continue;

                if (WaveData_ECG[0] == 32767)
                    WaveData_ECG[0] = 0;
                if (WaveData_ECG[0] > MaxLimit_ECG)
                    WaveData_ECG[0] = MaxLimit_ECG - 2;
                if (WaveData_ECG[0] < MinLimit_ECG)
                    WaveData_ECG[0] = MinLimit_ECG + 2;

                Values_ECG.Add(WaveData_ECG[0]);
                Values_ECG.RemoveAt(0);
                WaveData_ECG.RemoveAt(0);
            }
        }
    }



    private async void StartTask_SpO2()
    {
        while (true)
        {
            await Task.Delay(1);
            lock (Sync_ECG)
            {
                if (WaveData_SpO2.Count == 0)
                    continue;

                if (WaveData_SpO2[0] == 32767)
                    WaveData_SpO2[0] = 0;
                if (WaveData_SpO2[0] > MaxLimit_SpO2)
                    WaveData_SpO2[0] = MaxLimit_SpO2 - 2;
                if (WaveData_SpO2[0] < MinLimit_SpO2)
                    WaveData_SpO2[0] = MinLimit_SpO2 + 2;

                Values_SpO2.Add(WaveData_SpO2[0]);
                Values_SpO2.RemoveAt(0);
                WaveData_SpO2.RemoveAt(0);
            }
        }
    }


    private async void StartTask_Resp()
    {
        while (true)
        {
            await Task.Delay(1);
            lock (Sync_ECG)
            {
                if (WaveData_Resp.Count == 0)
                    continue;

                if (WaveData_Resp[0] == 32767)
                    WaveData_Resp[0] = 0;
                if (WaveData_Resp[0] > MaxLimit_Resp)
                    WaveData_Resp[0] = MaxLimit_Resp - 2;
                if (WaveData_Resp[0] < MinLimit_Resp)
                    WaveData_Resp[0] = MinLimit_Resp + 2;

                Values_Resp.Add(WaveData_Resp[0]);
                Values_Resp.RemoveAt(0);
                WaveData_Resp.RemoveAt(0);
            }
        }
    }
    #endregion
}
