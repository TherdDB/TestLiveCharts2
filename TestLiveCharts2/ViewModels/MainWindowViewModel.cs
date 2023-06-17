using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLiveCharts2.Models;

namespace TestLiveCharts2.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        ListCharts = new ObservableCollection<MyModel>();
    }

    public ObservableCollection<MyModel> ListCharts { get; set; }

    [ObservableProperty] int _count = 5;

    Random _r = new Random();
    int[] data1 = new int[500];
    int[] data2 = new int[60];
    string ecg = $"-4^-4^-4^-4^-4^-4^-4^-4^-8^-8^-4^-8^-8^-8^-8^-4^-4^-4^-4^-4^-4^-4^-4^-4^-8^-8^-4^-4^-4^-4^-8^-8^-4^0^0^0^4^8^8^4^4^4^4^4^4^4^4^8^8^8^8^8^4^4^4^4^4^4^12^20^24^20^12^12^12^12^12^12^8^8^8^8^4^4^8^4^0^0^0^0^0^-4^-4^-4^-4^-4^-4^-4^-4^-4^-4^-4^-4^-4^-4^0^0^0^0^-4^-4^-4^-8^-8^-8^-4^-4^-4^-4^-4^0^0^4^8^24^44^68^92^116^140^168^192^220^244^268^292^300^304^292^268^236^200^156^104^56^12^-24^-68^-104^-128^-148^-164^-172^-172^-168^-160^-148^-132^-116^-104^-92^-76^-60^-52^-48^-40^-32^-24^-20^-20^-16^-12^-12^-12^-12^-12^-12^-16^-16^-12^-12^-8^-8^-8^-12^-8^-4^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^4^12^20^24^24^24^24^24^24^20^20^20^24^24^24^24^24^24^24^20^20^28^36^44^48^48^48^44^44^44^44^44^44^44^44^44^44^44^36^28^24^20^20^20^16^16^12^12^12^12^12^12^8^0^0^-4^-8^-8^-8^-8^-8^-8^-12^-16^-16^-20^-24^-24^-24^-24^-24^-24^-20^-20^-20^-20^-20^-20^-24^-24^-24^-24^-20^-20^-20^-24^-24^-24^-24^-24^-24^-20^-20^-16^-16^-16^-16^-20^-20^-20^-20^-20^-16^-16^-16^-16^-16^-20^-20^-20^-20^-20^-20^-20^-20^-20^-20^-20^-20^-20^-24^-24^-20^-16^-16^-20^-16^-16^-16^-16^-16^-16^-16^-16^-16^-16^-16^-12^-12^-16^-16^-16^-12^-12^-12^-16^-16^-16^-16^-16^-16^-12^-12^-12^-12^-12^-12^-12^-16^-16^-16^-16^-16^-12^-12^-12^-12^-12^-12^-12^-12^-8^-8^-8^-8^-8^-12^-12^-12^-12^-12^-12^-8^-8^-8^-8^-12^-12^-12^-12^-12^-12^-8^-8^-8^-12^-12^-12^-12^-12^-12^-12^-12^-8^-8^-8^-8^-8^-8^-8^-12^-12^-12^-12^-12^-12^-12^-12^-12^-12^-12^-8^-8^-8^-4^-4^-8^-8^-8^-8^-8^-4^-4^-4^-4^-4^-4^-4^-4^-4^-8^-8^-8^-4^-4^-8^-8^-8^-8^-8^-4^-4^-4^-4^-4^-4^-4^-4^-8^-8^-8^-8^-4^-4^-4^-4^-4^-8^-8^-8^-8^-8^-8^-4^-4^-4^-8^-8^-4^-4^-4^-4^-4^-4^-4^0^0^-4^-4^-4^-4";
    string spo2 = $"39^56^62^68^72^76^78^79^80^80^80^79^78^77^75^74^72^70^68^65^63^60^57^55^51^45^41^38^34^30^25^20^15^10^5^1^0^4^13^20^20^29^38^47^56^64^71^76^80^84^83^82^80^78^75^73^71^69^67^66";
    bool _disposed = false;
    [RelayCommand]
    void AddItem()
    {
       
        for (int i = 0; i < Count; i++)
        {
            ListCharts.Add(new MyModel() { Name = $"图例：{i}" });
        }
    }


    [RelayCommand]
    void Start()
    {
        _disposed = true;
        data1 = ecg.Split('^').Select(int.Parse).ToArray();
        data2 = spo2.Split('^').Select(int.Parse).ToArray();

        Task.Run( () =>
        {
            for (int i = 0; i < 50; i++)
            {
                foreach (var item in ListCharts)
                {
                    item.WaveData_ECG.AddRange(data1);
                    item.WaveData_SpO2.AddRange(data2);
                    //item.WaveData_Resp.AddRange(Lis);
                }
            }
        });





        //Task.Run(async () =>
        //{
        //    while (_disposed)
        //    {
        //        await Task.Delay(1000);
        //        for (int i = 0; i < 500; i++)
        //        {
        //            foreach (var item in ListCharts)
        //            {
        //                item.WaveData_ECG.Add(_r.Next(-500, 500));
        //                item.WaveData_SpO2.Add(_r.Next(-500, 500));
        //                item.WaveData_Resp.Add(_r.Next(-500, 500));
        //            }
        //        }
        //    }
        //});


    }

    [RelayCommand]
    void Stop()
    {
        _disposed = false;
    }


}
