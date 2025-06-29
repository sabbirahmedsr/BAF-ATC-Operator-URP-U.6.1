using JetBrains.Annotations;
using System;
using UnityEngine;

public static class ATCTime {
    internal static event Action onATC_SecondUpdate = delegate { };

    internal static float globalSpeed = 1f;


    private static int _clockTimeHH = 0;
    private static int _clockTimeMM = 0;
    private static int _clockTimeSS = 0;
    internal static int _clockTimeInSeconds = 0;

    private static int _elapsedTimeHH = 0;
    private static int _elapsedTimeMM = 0;
    private static int _elapsedTimeSS = 0;
    internal static int _elapsedTimeInSeconds = 0;


    internal static void SetATCTime(int rATCTimeInSeconds, int rElapsedTimeInSeconds) {
        _clockTimeInSeconds = rATCTimeInSeconds;
        _elapsedTimeInSeconds = rElapsedTimeInSeconds;
        //
        _elapsedTimeHH = Mathf.FloorToInt(_elapsedTimeInSeconds / 3600) % 24;
        _elapsedTimeMM = Mathf.FloorToInt(_elapsedTimeInSeconds / 60) % 60;
        _elapsedTimeSS = _elapsedTimeInSeconds % 60;
        //
        _clockTimeHH = Mathf.FloorToInt(_clockTimeInSeconds / 3600) % 24;
        _clockTimeMM = Mathf.FloorToInt(_clockTimeInSeconds / 60) % 60;
        _clockTimeSS = _clockTimeInSeconds % 60;
        //
        onATC_SecondUpdate.Invoke();
    }





    internal static string GetClockTimeInHHMM() {
        return _clockTimeHH.ToString("00") + _clockTimeMM.ToString("00");
    }
    internal static string GetClockTimeInHHMMSS() {
        return _clockTimeHH.ToString("00") + ":" + _clockTimeMM.ToString("00") + ":" + _clockTimeSS.ToString("00");
    }
    internal static string GetElapsedTimeInHHMMSS() {
        return _elapsedTimeHH.ToString("00") + ":" + _elapsedTimeMM.ToString("00") + ":" + _elapsedTimeSS.ToString("00");
    }
    internal static string GetFutureClockTimeInHHMM(int rAddSeconds) {
        int fTimeInSeconds = _clockTimeInSeconds + rAddSeconds;
        int fTimeHH = Mathf.FloorToInt(fTimeInSeconds / 3600) % 24;
        int fTimeMM = Mathf.FloorToInt(fTimeInSeconds / 60) % 60;
        int fTimeSS = fTimeInSeconds % 60;
        return fTimeHH.ToString("00") + fTimeMM.ToString("00");
    }


}
