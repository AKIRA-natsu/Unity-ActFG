using UnityEngine;
// 2020-07-09
public class DeviceInfo
{
    public enum DeviceType
    {
        iPad, // 3:4≈1.33 (典型分辨率：2048x2732，iPad Pro,iPad Air等)
        Normal, // 9:16≈1.78 (典型分辨率：1242x2208，iPhone6/7/8，小米6等)
        AndroidNarrow, // 18:37≈2.06 (典型分辨率：1080x2220，三星S系列等)
        iPhoneX, // 6:13≈2.17 (典型分辨率：1242x2688，iPhoneX/Xs/11等)
        SuperNarrow, // >2.2 （超窄屏，极少数）

        Ads, // 特殊处理 广告
    }

    public static DeviceType CurrentDevice
    {
        get
        {
            float hdw = Screen.height * 1.0f / Screen.width;
            if (hdw > 1.3f && hdw < 1.4f)//4:3
            {
                //Debug.Log("iPad");
                return DeviceType.iPad;
            }
            if (hdw > 1.7f && hdw < 1.8f)//16:9
            {
                //Debug.Log("iPhone6/7/8");
                return DeviceType.Normal;
            }
            if (hdw > 1.8f && hdw < 2.1f)//18:37
            {
                //Debug.Log("AndroidNarrow");
                return DeviceType.AndroidNarrow;
            }
            if (hdw > 2.1f && hdw < 2.2f)//6:13
            {
                //Debug.Log("iPhoneX");
                return DeviceType.iPhoneX;
            }
            if (hdw > 2.2f)//>2.2
            {
                //Debug.Log("SuperNarrow");
                return DeviceType.SuperNarrow;
            }
            if (hdw == 1.25f)
            {
                return DeviceType.Ads;
            }
            return DeviceType.Normal;
        }
    }

    public static bool IsEditor
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.LinuxEditor)
            {
                return true;
            }
            return false;
        }
    }
}