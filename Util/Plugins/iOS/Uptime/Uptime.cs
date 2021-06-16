using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Uptime : MonoBehaviour {

#if UNITY_IOS

    [DllImport("__Internal")]
    private static extern long iosDeviceUptime ();
#endif

    public static long RequestiOSDeviceUptime(){
#if UNITY_IOS
        return iosDeviceUptime();
#else
        throw new NotImplementedException ("RequestiOSDeviceUptime() not implemented on this platform. It is an iOS function only!");
#endif
    }

}
