using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.App.ViewModel
{
    public class StatisticsViewModel
    {
        private readonly MainViewModel _main;

        public StatisticsViewModel(MainViewModel mainViewModel)
        {
            _main = mainViewModel;

            PresenceVsSmartChart = BuildPresenceVsSmartChart();
            WeeklyChart = BuildWeeklyChart();
        }

        public Chart PresenceVsSmartChart { get; }
        public Chart WeeklyChart { get; }

        private Chart BuildPresenceVsSmartChart()
        {
            var entries = new[]
            {
            new ChartEntry((float)_main.CurrentMonthPresenceWorkingHours)
            {
                Label = "Presenza",
                ValueLabel = $"{_main.CurrentMonthPresenceWorkingHours:F1}h",
                Color = SKColor.Parse("#3498db")
            },
            new ChartEntry((float)_main.CurrentMonthSmartWorkingHours)
            {
                Label = "Smart Working",
                ValueLabel = $"{_main.CurrentMonthSmartWorkingHours:F1}h",
                Color = SKColor.Parse("#2ecc71")
            }
        };

            return new DonutChart
            {
                Entries = entries,
                HoleRadius = 0.5f
            };
        }

        private Chart BuildWeeklyChart()
        {
            var entries = new[]
            {
            new ChartEntry((float)_main.CurrentWeekPresenceWorkingHours)
            {
                Label = "Presenza",
                ValueLabel = $"{_main.CurrentWeekPresenceWorkingHours:F1}h",
                Color = SKColor.Parse("#2980b9")
            },
            new ChartEntry((float)_main.CurrentWeekSmartWorkingHours)
            {
                Label = "Smart Working",
                ValueLabel = $"{_main.CurrentWeekSmartWorkingHours:F1}h",
                Color = SKColor.Parse("#27ae60")
            }
        };

            return new BarChart
            {
                Entries = entries,
                LabelTextSize = 32
            };
        }
    }
}
