using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_scalarSemantics
    {
        public static Func<string, bool> isNumberLiteral
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Tsonic.CSharp.Js.JSArray<string> longWeekdays
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.JSArray<string>)!;
        public static Tsonic.CSharp.Js.JSArray<string> shortWeekdays
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.JSArray<string>)!;
        public static Tsonic.CSharp.Js.JSArray<string> longMonths
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.JSArray<string>)!;
        public static Tsonic.CSharp.Js.JSArray<string> shortMonths
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.JSArray<string>)!;
        public static Func<string, string> stripLeadingZero
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<double, int> weekdayIndex
        {
            get;
            private set;
        } = default(Func<double, int>)!;
        public static Func<string, int, int, int, string?> addCalendarDate
        {
            get;
            private set;
        } = default(Func<string, int, int, int, string?>)!;
        public static Func<string, string, bool?> isDateAfter
        {
            get;
            private set;
        } = default(Func<string, string, bool?>)!;
        public static Func<string, string, string?> formatDateTime
        {
            get;
            private set;
        } = default(Func<string, string, string?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_int32.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_textBuilder.__tsonic_module_init();
            isNumberLiteral = (string token) =>
            {
                if (token == "")
                {
                    return false;
                }
                return Utils_int32.parseInt32(token) is not null;
            };
            longWeekdays = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            shortWeekdays = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" });
            longMonths = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });
            shortMonths = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" });
            stripLeadingZero = (string value) =>
            {
                return Tsonic.CSharp.Js.String.startsWith(value, "0") ? Tsonic.CSharp.Js.String.slice(value, 1) : value;
            };
            weekdayIndex = (double milliseconds) =>
            {
                double value = (Tsonic.CSharp.Js.Math.floor(milliseconds / 86400000) + 4) % 7;
                if (value < 0)
                {
                    value += 7;
                }
                return (int)value;
            };
            addCalendarDate = (string value, int years, int months, int days) =>
            {
                double milliseconds = Tsonic.CSharp.Js.Date.parse(value);
                if (Tsonic.CSharp.Js.Number.isNaN(milliseconds))
                {
                    return null;
                }
                string iso = new Tsonic.CSharp.Js.Date(milliseconds).toISOString();
                int? sourceYear = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 0, 4));
                int? sourceMonth = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 5, 2));
                int? sourceDay = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 8, 2));
                int? hour = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 11, 2));
                int? minute = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 14, 2));
                int? second = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 17, 2));
                int? millisecond = Utils_int32.parseInt32(Utils_strings.substringCount(iso, 20, 3));
                if (sourceYear is null || sourceMonth is null || sourceDay is null || hour is null || minute is null || second is null || millisecond is null)
                {
                    return null;
                }
                double sourceYearValue = sourceYear.Value;
                double sourceMonthValue = sourceMonth.Value;
                double yearsValue = years;
                double monthsValue = months;
                double totalMonths = sourceYearValue * 12 + sourceMonthValue - 1 + yearsValue * 12 + monthsValue;
                double targetYearValue = Tsonic.CSharp.Js.Math.floor(totalMonths / 12);
                if (targetYearValue < 1 || targetYearValue > 9999)
                {
                    return null;
                }
                int? targetYear = Utils_int32.toInt32(targetYearValue);
                int? targetMonth = Utils_int32.toInt32(totalMonths - targetYearValue * 12);
                if (targetYear is null || targetMonth is null)
                {
                    return null;
                }
                string yearText = Utils_strings.zeroPadInteger(targetYear.Value, 4);
                string monthText = Utils_strings.zeroPadInteger(targetMonth.Value + 1, 2);
                string hourText = Utils_strings.zeroPadInteger(hour.Value, 2);
                string minuteText = Utils_strings.zeroPadInteger(minute.Value, 2);
                string secondText = Utils_strings.zeroPadInteger(second.Value, 2);
                string millisecondText = Utils_strings.zeroPadInteger(millisecond.Value, 3);
                string monthStartText = yearText + "-" + monthText + "-01T" + hourText + ":" + minuteText + ":" + secondText + "." + millisecondText + "Z";
                double monthStart = Tsonic.CSharp.Js.Date.parse(monthStartText);
                if (Tsonic.CSharp.Js.Number.isNaN(monthStart))
                {
                    return null;
                }
                double sourceDayValue = sourceDay.Value;
                double daysValue = days;
                double dayOffset = sourceDayValue - 1 + daysValue;
                double result = monthStart + dayOffset * 86400000;
                if (!Tsonic.CSharp.Js.Number.isFinite(result) || Tsonic.CSharp.Js.Math.abs(result) > 8640000000000000)
                {
                    return null;
                }
                return new Tsonic.CSharp.Js.Date(result).toISOString();
            };
            isDateAfter = (string left, string right) =>
            {
                double leftMilliseconds = Tsonic.CSharp.Js.Date.parse(left);
                double rightMilliseconds = Tsonic.CSharp.Js.Date.parse(right);
                if (Tsonic.CSharp.Js.Number.isNaN(leftMilliseconds) || Tsonic.CSharp.Js.Number.isNaN(rightMilliseconds))
                {
                    return null;
                }
                return leftMilliseconds > rightMilliseconds;
            };
            formatDateTime = (string value, string layout) =>
            {
                double milliseconds = Tsonic.CSharp.Js.Date.parse(value);
                if (Tsonic.CSharp.Js.Number.isNaN(milliseconds))
                {
                    return null;
                }
                string iso = new Tsonic.CSharp.Js.Date(milliseconds).toISOString();
                string year = Utils_strings.substringCount(iso, 0, 4);
                string month = Utils_strings.substringCount(iso, 5, 2);
                string day = Utils_strings.substringCount(iso, 8, 2);
                string hour24 = Utils_strings.substringCount(iso, 11, 2);
                string minute = Utils_strings.substringCount(iso, 14, 2);
                string second = Utils_strings.substringCount(iso, 17, 2);
                int monthIndex = (Utils_int32.parseInt32(month) ?? 1) - 1;
                int hourValue = Utils_int32.parseInt32(hour24) ?? 0;
                double hour12Value = hourValue % 12 == 0 ? 12 : hourValue % 12;
                string hour12 = hour12Value < 10 ? $"0{hour12Value}" : $"{hour12Value}";
                int weekday = weekdayIndex(milliseconds);
                TextBuilder output = new TextBuilder();
                int index = 0;
                while (index < layout.Length)
                {
                    string remaining = Tsonic.CSharp.Js.String.slice(layout, index);
                    if (Tsonic.CSharp.Js.String.startsWith(remaining, "Monday"))
                    {
                        output.append(longWeekdays[weekday]);
                        index += 6;
                    }
                    else
                    {
                        if (Tsonic.CSharp.Js.String.startsWith(remaining, "January"))
                        {
                            output.append(longMonths[monthIndex]);
                            index += 7;
                        }
                        else
                        {
                            if (Tsonic.CSharp.Js.String.startsWith(remaining, "2006"))
                            {
                                output.append(year);
                                index += 4;
                            }
                            else
                            {
                                if (Tsonic.CSharp.Js.String.startsWith(remaining, "Mon"))
                                {
                                    output.append(shortWeekdays[weekday]);
                                    index += 3;
                                }
                                else
                                {
                                    if (Tsonic.CSharp.Js.String.startsWith(remaining, "Jan"))
                                    {
                                        output.append(shortMonths[monthIndex]);
                                        index += 3;
                                    }
                                    else
                                    {
                                        if (Tsonic.CSharp.Js.String.startsWith(remaining, "PM"))
                                        {
                                            output.append(hourValue < 12 ? "AM" : "PM");
                                            index += 2;
                                        }
                                        else
                                        {
                                            if (Tsonic.CSharp.Js.String.startsWith(remaining, "pm"))
                                            {
                                                output.append(hourValue < 12 ? "am" : "pm");
                                                index += 2;
                                            }
                                            else
                                            {
                                                if (Tsonic.CSharp.Js.String.startsWith(remaining, "06"))
                                                {
                                                    output.append(Tsonic.CSharp.Js.String.slice(year, 2));
                                                    index += 2;
                                                }
                                                else
                                                {
                                                    if (Tsonic.CSharp.Js.String.startsWith(remaining, "01"))
                                                    {
                                                        output.append(month);
                                                        index += 2;
                                                    }
                                                    else
                                                    {
                                                        if (Tsonic.CSharp.Js.String.startsWith(remaining, "02"))
                                                        {
                                                            output.append(day);
                                                            index += 2;
                                                        }
                                                        else
                                                        {
                                                            if (Tsonic.CSharp.Js.String.startsWith(remaining, "15"))
                                                            {
                                                                output.append(hour24);
                                                                index += 2;
                                                            }
                                                            else
                                                            {
                                                                if (Tsonic.CSharp.Js.String.startsWith(remaining, "03"))
                                                                {
                                                                    output.append(hour12);
                                                                    index += 2;
                                                                }
                                                                else
                                                                {
                                                                    if (Tsonic.CSharp.Js.String.startsWith(remaining, "04"))
                                                                    {
                                                                        output.append(minute);
                                                                        index += 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Tsonic.CSharp.Js.String.startsWith(remaining, "05"))
                                                                        {
                                                                            output.append(second);
                                                                            index += 2;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Tsonic.CSharp.Js.String.startsWith(remaining, "1"))
                                                                            {
                                                                                output.append(stripLeadingZero(month));
                                                                                index += 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Tsonic.CSharp.Js.String.startsWith(remaining, "2"))
                                                                                {
                                                                                    output.append(stripLeadingZero(day));
                                                                                    index += 1;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Tsonic.CSharp.Js.String.startsWith(remaining, "3"))
                                                                                    {
                                                                                        output.append($"{hour12Value}");
                                                                                        index += 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        output.append(Utils_strings.substringCount(layout, index, 1));
                                                                                        index += 1;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return output.toString();
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
