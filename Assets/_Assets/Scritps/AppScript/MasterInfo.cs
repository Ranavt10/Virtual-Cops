
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MasterInfo : Singleton<MasterInfo>
{
    [SerializeField]
    private int version = 1;
    [SerializeField]
    private string webServiceUrl;

    private MasterInfoResponse response;
    //private UnityWebRequest www;
    private float timeFetchedData;
    private float timeStartFetchData;
    private bool isWaitingResponse;
    private string s;

    public bool IsDataFetched { get; private set; }
    public int Version { get {return version;} }

    private Stack<UnityAction<MasterInfoResponse>> waitingCallbacks = new Stack<UnityAction<MasterInfoResponse>>();

    void Awake()
    {
        StartGetData(true, data =>
        {
            DebugCustom.Log("MasterInfo current: " + GetCurrentDateTime());
            DebugCustom.Log("MasterInfo week range: " + GetCurrentWeekRangeString());
            DebugCustom.Log("MasterInfo previous week range " + GetPreviousWeekRangeString());
            TimeSpan t = GetTournamentTimeleft();
            DebugCustom.Log(string.Format("MasterInfo tournament timeleft {0}D {1}H:{2}M:{3}S", t.Days, t.Hours, t.Minutes, t.Seconds));
        });

        DontDestroyOnLoad(this);
    }

    public void StartGetData(bool forceRenew = false, UnityAction<MasterInfoResponse> callback = null)
    {
        if (callback != null)
        {
            DebugCustom.Log("MasterInfo add callback to waiting Stack");
            waitingCallbacks.Push(callback);
        }

        if (forceRenew == false && response != null)
        {
            DebugCustom.Log("MasterInfo callback from CACHE");
            ProcessCallbacks(response);
            return;
        }

        if (!isWaitingResponse)
        {
           // StartCoroutine(GetData());
        }
    }

    public DateTime GetCurrentDateTime()
    {
        if (response == null)
        {
            return DateTime.Now;
        }
        else
        {
            DateTime current = response.data.dateTime;
            current.AddSeconds(Time.realtimeSinceStartup - timeFetchedData);
            return current;
        }
    }

    public string GetWeekRangeString(DateTime date)
    {
        int delta = date.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)date.DayOfWeek - 1;
        DateTime firstDay = date.AddDays(-delta);
        DateTime lastDay = firstDay.AddDays(6);

        string weekRange = string.Format("{0:00}{1:00}{2:00}{3:00}{4}", firstDay.Day, firstDay.Month, lastDay.Day, lastDay.Month, lastDay.Year);
        return weekRange;
    }

    public string GetCurrentWeekRangeString()
    {
        return GetWeekRangeString(GetCurrentDateTime());
    }

    public string GetPreviousWeekRangeString()
    {
        DateTime date = GetCurrentDateTime().AddDays(-7);
        int delta = date.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)date.DayOfWeek - 1;
        DateTime firstDay = date.AddDays(-delta);
        DateTime lastDay = firstDay.AddDays(6);

        string weekRange = string.Format("{0:00}{1:00}{2:00}{3:00}{4}", firstDay.Day, firstDay.Month, lastDay.Day, lastDay.Month, lastDay.Year);
        return weekRange;
    }

    public double GetTournamentTimeleftInSecond()
    {
        return GetTournamentTimeleft().TotalSeconds;
    }

    public TimeSpan GetTournamentTimeleft()
    {
        DateTime date = GetCurrentDateTime();

        int delta = date.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)date.DayOfWeek - 1;
        DateTime lastDay = date.AddDays(6 - delta);
        lastDay = new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);

        return TimeSpan.FromTicks(lastDay.Ticks - date.Ticks);
    }

    public void CountDownTimer(TimeSpan t, out int days, out int hours, out int minutes, out int seconds)
    {
        days = t.Days;
        hours = t.Hours;
        minutes = t.Minutes;
        seconds = t.Seconds;
    }

    public void CountDownTimer(TimeSpan t, out int hours, out int minutes, out int seconds)
    {
        hours = t.Hours;
        minutes = t.Minutes;
        seconds = t.Seconds;
    }

    public void CountDownTimer(TimeSpan t, out int minutes, out int seconds)
    {
        minutes = t.Minutes;
        seconds = t.Seconds;
    }



    private void ProcessCallbacks(MasterInfoResponse response)
    {
        while (waitingCallbacks.Count > 0)
        {
            UnityAction<MasterInfoResponse> callback = waitingCallbacks.Pop();
            callback(response);
        }
    }
}
