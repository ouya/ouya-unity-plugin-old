package tv.ouya.sdk;

import android.app.Activity;
import android.os.Bundle;
import android.widget.FrameLayout;
import com.unity3d.player.UnityPlayer;

public class IOuyaActivity
{
	// save reference to the activity
	protected static Activity m_activity = null;
	public static Activity GetActivity()
	{
		return m_activity;
	}
	public static void SetActivity(Activity activity)
	{
		m_activity = activity;
	}

	// save reference to the unity player
	protected static UnityPlayer m_unityPlayer = null;
	public static UnityPlayer GetUnityPlayer()
	{
		return m_unityPlayer;
	}
	public static void SetUnityPlayer(UnityPlayer unityPlayer)
	{
		m_unityPlayer = unityPlayer;
	}

	// save reference to the bundle
	protected static Bundle m_savedInstanceState = null;
	public static Bundle GetSavedInstanceState()
	{
		return m_savedInstanceState;
	}
	public static void SetSavedInstanceState(Bundle savedInstanceState)
	{
		m_savedInstanceState = savedInstanceState;
	}

	// save reference to the FrameLayout
	protected static FrameLayout m_layout = null;
	public static FrameLayout GetLayout()
	{
		return m_layout;
	}
	public static void SetLayout(FrameLayout layout)
	{
		m_layout = layout;
	}

	// save reference to the testFacade
	protected static TestOuyaFacade m_testOuyaFacade = null;
	public static TestOuyaFacade GetTestOuyaFacade()
	{
		return m_testOuyaFacade;
	}
	public static void SetTestOuyaFacade(TestOuyaFacade testOuyaFacade)
	{
		m_testOuyaFacade = testOuyaFacade;
	}
}