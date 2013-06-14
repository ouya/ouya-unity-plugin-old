#pragma strict

public class OuyaShowJavaScript extends MonoBehaviour implements
    OuyaSDK.IPauseListener,
    OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{
    function Awake()
    {
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
    }

    function OnDestroy()
    {
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    public function OuyaMenuButtonUp()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaMenuAppearing()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaOnPause()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaOnResume()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
}