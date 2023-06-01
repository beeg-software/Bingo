using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Bingo.UI.Shared.UIElements.Input
{
    public partial class CustomInputNullableTimeSpan
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public Expression<Func<long?>> ValidationFor { get; set; }
        [Parameter] public EventCallback<string> OnChange { get; set; }

        private TimeSpan _localeTimeSpan = TimeSpan.Zero;

        #region TimeProperties
        private int _days = 0;
        public int days
        {
            get => _days;
            set
            {
                if (value < 0)
                {
                    _days = 0;
                }
                else
                {
                    if (value > 999)
                    {
                        _days = 999;
                    }
                    else
                    {
                        _days = value;
                    }
                }
                CurrentValue = ConvertInputToTimeSpanTicks();
                OnChange.InvokeAsync("");
            }
        }

        private int _hours = 0;
        public int hours
        {
            get => _hours;
            set
            {
                if (value < 0)
                {
                    _hours = 0;
                }
                else
                {
                    if (value > 23)
                    {
                        _hours = 23;
                    }
                    else
                    {
                        _hours = value;
                    }
                }
                CurrentValue = ConvertInputToTimeSpanTicks();
                OnChange.InvokeAsync("");
            }
        }

        private int _minutes = 0;
        public int minutes
        {
            get => _minutes;
            set
            {
                if (value < 0)
                {
                    _minutes = 0;
                }
                else
                {
                    if (value > 59)
                    {
                        _minutes = 59;
                    }
                    else
                    {
                        _minutes = value;
                    }
                }
                CurrentValue = ConvertInputToTimeSpanTicks();
                OnChange.InvokeAsync("");
            }
        }

        private int _seconds = 0;
        public int seconds
        {
            get => _seconds;
            set
            {
                if (value < 0)
                {
                    _seconds = 0;
                }
                else
                {
                    if (value > 59)
                    {
                        _seconds = 59;
                    }
                    else
                    {
                        _seconds = value;
                    }
                }
                CurrentValue = ConvertInputToTimeSpanTicks();
                OnChange.InvokeAsync("");
            }
        }

        private int _milliseconds = 0;
        public int milliseconds
        {
            get => _milliseconds;
            set
            {
                if (value < 0)
                {
                    _milliseconds = 0;
                }
                else
                {
                    if (value > 999)
                    {
                        _milliseconds = 999;
                    }
                    else
                    {
                        _milliseconds = value;
                    }
                }
                CurrentValue = ConvertInputToTimeSpanTicks();
                OnChange.InvokeAsync("");
            }
        }
        #endregion

        string _tempId = "";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (CurrentValue.HasValue)
            {
                _localeTimeSpan = TimeSpan.FromTicks(CurrentValue ?? 0L);
            }
            else
            {
                _localeTimeSpan = TimeSpan.MinValue;
            }

            _days = _localeTimeSpan.Days;
            _hours = _localeTimeSpan.Hours;
            _minutes = _localeTimeSpan.Minutes;
            _seconds = _localeTimeSpan.Seconds;
            _milliseconds = _localeTimeSpan.Milliseconds;
        }

        protected override bool TryParseValueFromString(string value, out long? result, out string validationErrorMessage)
        {

            TimeSpan _tempResult = default(TimeSpan);


            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field is empty.";
                return false;
            }

            var _tRes = TimeSpan.TryParse(value, out var parsedValue);

            if (_tRes)
            {
                result = parsedValue.Ticks;
                validationErrorMessage = null;
                return true;
            }
            else
            {
                result = null;
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not in the correct format.";
                return false;
            }
        }

        private long? ConvertInputToTimeSpanTicks()
        {
            if (days == default(int) || hours == default(int) || minutes == default(int) || seconds == default(int) || milliseconds == default(int))
            {
                return null;
            }

            if (_days < 0 || _hours < 0 || _minutes < 0 || _seconds < 0 || _milliseconds < 0)
            {
                return null;
            }

            string days_s = FillString(days.ToString(), 2);
            string hours_s = FillString(hours.ToString(), 2);
            string minutes_s = FillString(minutes.ToString(), 2);
            string seconds_s = FillString(seconds.ToString(), 2);
            string milliseconds_s = FillString(milliseconds.ToString(), 3);

            string tempDateString = days_s + ":" + hours_s + ":" + minutes_s + ":" + seconds_s + "." + milliseconds_s;
            TimeSpan _tempResult = default(TimeSpan);
            var _tRes = TimeSpan.TryParse(tempDateString, out var parsedValue);

            if (_tRes)
            {
                return parsedValue.Ticks;
            }
            else
            {
                return null;
            }
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
    }
}
