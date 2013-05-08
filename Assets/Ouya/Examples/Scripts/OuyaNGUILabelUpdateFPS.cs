using System;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OuyaNGUILabelUpdateFPS : MonoBehaviour
{
    public UILabel Label = null;
    public bool ExecuteInEditMode = false;

    private int m_lastCount = 0;
    private int m_count = 0;
    private DateTime m_timer = DateTime.MinValue;

    public void OnPostRender()
    {
        if (!ExecuteInEditMode)
        {
            if (!Application.isPlaying)
            {
                return;
            }
        }

        ++m_count;
        if (m_timer < DateTime.Now)
        {
            m_timer = DateTime.Now + TimeSpan.FromSeconds(1);
            m_lastCount = m_count;
            m_count = 0;

            if (Label)
            {
                Label.text = m_lastCount.ToString();
            }
        }
    }
}