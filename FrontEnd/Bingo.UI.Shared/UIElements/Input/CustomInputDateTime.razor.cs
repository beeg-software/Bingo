using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.UIElements.Input
{
    public partial class CustomInputDateTime
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public Expression<Func<DateTime>> ValidationFor { get; set; }
        [Parameter] public EventCallback<string> OnChange { get; set; }

        #region TimeProperties
        private int _year = 0;
        public int year
        {
            get => _year;
            set
            {
                if (value < 1)
                {
                    _year = 1;
                }
                else
                {
                    if (value > 9999)
                    {
                        _year = 9999;
                    }
                    else
                    {
                        _year = value;
                    }
                }
                SetMaxDayOfMonth();
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _month = 0;
        public int month
        {
            get => _month;
            set
            {
                if (value < 1)
                {
                    _month = 1;
                }
                else
                {
                    if (value > 12)
                    {
                        _month = 12;
                    }
                    else
                    {
                        _month = value;
                    }
                }
                SetMaxDayOfMonth();
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _day = 0;
        public int day
        {
            get => _day;
            set
            {
                if (value < 1)
                {
                    _day = 1;
                }
                else
                {
                    if (value > maxDayOfMonth)
                    {
                        _day = maxDayOfMonth;
                    }
                    else
                    {
                        _day = value;
                    }
                }
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _hour = 0;
        public int hour
        {
            get => _hour;
            set
            {
                if (value < 0)
                {
                    _hour = 0;
                }
                else
                {
                    if (value > 23)
                    {
                        _hour = 23;
                    }
                    else
                    {
                        _hour = value;
                    }
                }
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _minute = 0;
        public int minute
        {
            get => _minute;
            set
            {
                if (value < 0)
                {
                    _minute = 0;
                }
                else
                {
                    if (value > 59)
                    {
                        _minute = 59;
                    }
                    else
                    {
                        _minute = value;
                    }
                }
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _second = 0;
        public int second
        {
            get => _second;
            set
            {
                if (value < 0)
                {
                    _second = 0;
                }
                else
                {
                    if (value > 59)
                    {
                        _second = 59;
                    }
                    else
                    {
                        _second = value;
                    }
                }
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }

        private int _millisecond = 0;
        public int millisecond
        {
            get => _millisecond;
            set
            {
                if (value < 0)
                {
                    _millisecond = 0;
                }
                else
                {
                    if (value > 999)
                    {
                        _millisecond = 999;
                    }
                    else
                    {
                        _millisecond = value;
                    }
                }
                CurrentValue = ConvertInputToDateTime();
                OnChange.InvokeAsync("");
            }
        }
        #endregion

        int maxDayOfMonth = 31;

        string _tempId = "";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _year = CurrentValue.Year;
            _month = CurrentValue.Month;
            _day = CurrentValue.Day;
            _hour = CurrentValue.Hour;
            _minute = CurrentValue.Minute;
            _second = CurrentValue.Second;
            _millisecond = CurrentValue.Millisecond;

            SetMaxDayOfMonth();
        }

        protected override bool TryParseValueFromString(string value, out DateTime result, out string validationErrorMessage)
        {
            result = DateTime.Parse(value);
            validationErrorMessage = null;
            return true;
        }

        private DateTime ConvertInputToDateTime()
        {
            string year_s = FillString(year.ToString(), 4);
            string month_s = FillString(month.ToString(), 2);
            string day_s = FillString(day.ToString(), 2);
            string hour_s = FillString(hour.ToString(), 2);
            string minute_s = FillString(minute.ToString(), 2);
            string second_s = FillString(second.ToString(), 2);
            string millisecond_s = FillString(millisecond.ToString(), 3);

            string tempDateString = year_s + "-" + month_s + "-" + day_s + "T" + hour_s + ":" + minute_s + ":" + second_s + "." + millisecond_s + "Z";
            return DateTime.Parse(tempDateString).ToUniversalTime();
        }

        private string FillString(string orig_s, int min_length)
        {
            var orig_length = orig_s.Length;
            while(orig_length < min_length)
            {
                orig_s = "0" + orig_s;
                orig_length = orig_s.Length;
            }
            return orig_s;
        }

        private void SetMaxDayOfMonth()
        {
            if(month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                maxDayOfMonth = 31;
            }
            if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                maxDayOfMonth = 30;
            }
            if (month == 2)
            {
                if(year % 4 == 0)
                {
                    if (year % 100 != 0 || year % 400 == 0)
                    {
                        maxDayOfMonth = 29;
                    }
                    else
                    {
                        maxDayOfMonth = 28;
                    }
                }
                else
                {
                    maxDayOfMonth = 28;
                }
            }

            //After changes check if day is still valid
            if (day > maxDayOfMonth)
            {
                day = maxDayOfMonth;
                StateHasChanged();
            }
        }
    }
}
