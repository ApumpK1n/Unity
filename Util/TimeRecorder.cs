using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using UnityEditor;

public class TimeRecorder : SingletonBehaviour<TimeRecorder>
{
    private const string saveKey = "TimeRecorderKey";
    private DateTime gameDateTimeNow;

    private float timeSinceLastSession;
    private int preSavedDay;
    public DateTime GameDateTimeNow
    {
        get
        {
            return gameDateTimeNow.AddSeconds(Time.realtimeSinceStartup);
        }
    }

    public static Action OnNewDay;
    
    private float DayTimer;
    private SaveData saveData;

    public class SaveData
    {
        public string savedGameDateTime;
        public double savedDeviceElapsedTime;
        public int preSavedDay;
    }
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void LoadSaveData()
    {
        if (ES3.KeyExists(saveKey))
        {
            saveData = ES3.Load<SaveData>(saveKey);
        }
    }

    public void OnSave()
    {
        if (saveData == null)
        {
            saveData = new SaveData();
        }

        saveData.savedGameDateTime = GameDateTimeNow.ToString(CultureInfo.InvariantCulture);
        saveData.savedDeviceElapsedTime = GetDeviceElapsedTime();
        saveData.preSavedDay = preSavedDay;

        ES3.Save(saveKey, saveData);

#if UNITY_EDITOR
        Debug.Log("TimeRecorder: savedGameDateTime:" + saveData.savedGameDateTime);
        Debug.Log("TimeRecorder: savedDeviceElapsedTime:" + saveData.savedDeviceElapsedTime);
#endif
    }

    private void Update()
    {
        DayTimer += Time.deltaTime;
        if (DayTimer > 10f)
        {
            DayTimer = 0;
            if (preSavedDay != GameDateTimeNow.DayOfYear)
            {
                UpdateDay();
            }
        }
    }

    private void UpdateDay()
    {
        if (preSavedDay != GameDateTimeNow.DayOfYear)
        {
            preSavedDay = GameDateTimeNow.DayOfYear;
            OnNewDay?.Invoke();
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.pauseStateChanged += OnEditorApplicationPause;
#endif
    }

    private void OnApplicationQuit()
    {
        OnSave();
    }

#if UNITY_EDITOR
    private void OnEditorApplicationPause(PauseState obj)
    {
        if (obj == PauseState.Paused)
        {
            OnSave();
        }
    }
#else
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            OnSave();
        }
    }
#endif

    private void Start()
    {
        LoadSaveData();

        if (saveData != null && !string.IsNullOrEmpty(saveData.savedGameDateTime))
        {
            gameDateTimeNow = StringToDateTimeInvariantCulture(saveData.savedGameDateTime);
            timeSinceLastSession = Mathf.Clamp((float)(GetDeviceElapsedTime() - saveData.savedDeviceElapsedTime), 0, float.MaxValue);

            if (timeSinceLastSession > 0)
            {
                gameDateTimeNow = gameDateTimeNow.AddSeconds(timeSinceLastSession);
            }
            else
            {
                timeSinceLastSession = (float)GetDifferenceBetween(DateTime.Now, gameDateTimeNow).TotalSeconds;
                gameDateTimeNow = DateTime.Now;
            }

        }
        else gameDateTimeNow = DateTime.Now;

        if (saveData == null) preSavedDay = GameDateTimeNow.DayOfYear;
        else preSavedDay = saveData.preSavedDay;

        Debug.Log("Recorder: Time since last session: " + timeSinceLastSession);

        UpdateDay();

        OnSave();
    }

    public static DateTime StringToDateTimeInvariantCulture(string dateTime)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(dateTime, CultureInfo.InvariantCulture);
            return dt;
        }
        catch (Exception e)
        {
            DateTime dt = Convert.ToDateTime(dateTime);
            return dt;
        }
    }

    public static TimeSpan GetDifferenceBetween(DateTime dt1, DateTime dt2, bool inverted = false)
    {
        try
        {
            TimeSpan difference = inverted ? (dt2 - dt1) : (dt1 - dt2);

            return difference;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return default(TimeSpan);
        }
    }

    #region DeviceElapsedTime
    public static double GetDeviceElapsedTime()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return GetAndroidElapsedRealtime();
#elif UNITY_IOS && !UNITY_EDITOR
        return GetAppleElapsedRealtime();
#elif UNITY_EDITOR
        return DateTime.UtcNow.Ticks / 10000000.0;
#endif
    }

    private static double GetAndroidElapsedRealtime()
    {
#if UNITY_ANDROID
        AndroidJavaObject systemClock = new AndroidJavaObject("android.os.SystemClock");
        long systemUptime = systemClock.CallStatic<long>("elapsedRealtime") / 1000;
        return Convert.ToDouble(systemUptime);
#else
    return -1;
#endif
    }

    private static double GetAppleElapsedRealtime()
    {
#if UNITY_IOS
        long elapsedTime = Uptime.RequestiOSDeviceUptime();
        Debug.Log("elapsedTime:" + elapsedTime);
        return elapsedTime;
#else
        return -1;
#endif
    }
    #endregion

#if UNITY_EDITOR
    public void AddExtraSeconds(double seconds)
    {
        gameDateTimeNow = gameDateTimeNow.AddSeconds(seconds);
        Debug.Log("[TIME CONTROLLER] Debug set new time: " + gameDateTimeNow.ToString());
    }

    public void SetTimeToEndOfDay(int secondsBefore)
    {
        DayTimer = 0;
        gameDateTimeNow = gameDateTimeNow.Date.AddDays(1).AddSeconds(-secondsBefore);
        Debug.Log("[TIME CONTROLLER] Debug set time to end of day: " + gameDateTimeNow.ToString());
    }
#endif
}