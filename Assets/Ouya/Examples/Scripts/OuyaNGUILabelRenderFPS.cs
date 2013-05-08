using System;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OuyaNGUILabelRenderFPS : MonoBehaviour
{
    public UILabel Label = null;
    public bool ExecuteInEditMode = false;

    private DateTime m_timer = DateTime.MinValue;

    private Stopwatch m_stopWatch = new Stopwatch();

    private float m_renderLatency = 0f;

    public void OnPreRender()
    {
        m_stopWatch.Reset();
        m_stopWatch.Start();
    }

    public void OnPostRender()
    {
        if (!ExecuteInEditMode)
        {
            if (!Application.isPlaying)
            {
                return;
            }
        }

        m_stopWatch.Stop();
        Double elapsed = m_stopWatch.Elapsed.TotalMilliseconds;
        if (elapsed != 0f)
        {
            if (m_timer < DateTime.Now)
            {
                m_timer = DateTime.Now + TimeSpan.FromMilliseconds(500);

                m_renderLatency = (float) elapsed;

                if (Label)
                {
                    Label.text = string.Format("{0:F2}",
                        m_renderLatency);
                }
            }
        }
    }
}