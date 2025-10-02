using System;

public class TimeHelper
{
    public string GetFormatDate(DateTime dt, char Separator)
    {
        if(dt != null && !dt.Equals(DBNull.Value))
        {
            string tem = string.Format("dd{0}MM{1}yyyy", Separator, Separator);
            return dt.ToString(tem);
        }
        else
        {
            return GetFormatDate(DateTime.Now, Separator);
        }
    }
    
    public string GetFormatTime(DateTime dt, char Separator)
    {
        if(dt != null && !dt.Equals(DBNull.Value))
        {
            string tem = string.Format("hh{0}mm{1}ss", Separator, Separator);
            return dt.ToString(tem);
        }
        else
        {
            return GetFormatDate(DateTime.Now, Separator);
        }
    }
    public static int SecondToMinute(int Second)
    {
        decimal mm = (decimal)((decimal)Second / (decimal)60);
        return Convert.ToInt32(Math.Ceiling(mm));
    }
  
    public static int GetMonthLastDate(int year, int month)
    {
        DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
        int Day = lastDay.Day;
        return Day;
    }   

    public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
    {
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        return ts;
    }  
}