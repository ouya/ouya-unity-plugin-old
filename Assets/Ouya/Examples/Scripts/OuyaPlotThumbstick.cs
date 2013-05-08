using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;

[RequireComponent(typeof(Camera))]
public class OuyaPlotThumbstick : MonoBehaviour
{
    public Material PlotMaterial = null;

    private int TextureSize = 128;

    private Texture2D m_texture = null;
    private Color32[] m_pixels = null;
    private float[] m_pixelVs = null;

    public Color32 BackgroundColor = Color.clear;

    public string StickX = "LX";
    public string StickY = "LY";

    private static bool m_toggleGraph = true;

    private static Color m_plotBackground = new Color(0f, 0f, 0f, 0.5f);

    private static List<OuyaPlotThumbstick> m_plots = new List<OuyaPlotThumbstick>();

    private int m_lastX = -2;
    private int m_lastY = -2;

    public UILabel m_label = null;

    private int m_pixelCount = 0;
    private int m_updatePixelCount = 0;
    private float m_pixelRatio = 0;

    private DateTime m_timerText = DateTime.MinValue;
    private DateTime m_timerTexture = DateTime.MinValue;

    public void OnEnable()
    {
        m_texture = new Texture2D(TextureSize, TextureSize, TextureFormat.ARGB32, false);
        m_texture.filterMode = FilterMode.Point;
        m_pixels = m_texture.GetPixels32();
        m_pixelVs = new float[m_pixels.Length];
        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = m_texture;
        }

        ClearTexture();

        m_plots.Add(this);
    }

    public void OnDisable()
    {
        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = null;
            PlotMaterial.mainTextureOffset = Vector2.zero;
        }

        if (null != m_texture)
        {
            Object.DestroyImmediate(m_texture, true);
            m_texture = null;
        }

        m_plots.Remove(this);
    }

    void ClearTexture()
    {
        if (null != m_pixels)
        {
            for (int index = 0; index < m_pixels.Length; ++index)
            {
                m_pixels[index] = m_plotBackground;
                m_pixelVs[index] = -1;
            }
            m_texture.SetPixels32(m_pixels);
            m_texture.Apply();

            m_pixelCount = 0;
            m_updatePixelCount = 0;
            m_pixelRatio = 0;
        }
    }

    private void UpdateCounts()
    {
        m_pixelCount = 0;
        m_updatePixelCount = 0;

        for (int index = 0; index < m_pixelVs.Length; ++index)
        {
            if (m_pixelVs[index] == 0f)
            {
                ++m_pixelCount;
            }
            if (m_pixelVs[index] > 0f)
            {
                ++m_updatePixelCount;
            }
        }

        if (m_pixelCount == 0)
        {
            m_pixelRatio = 0f;
        }
        else
        {
            m_pixelRatio = m_updatePixelCount/(float) m_pixelCount;
        }
    }

    private float m_increment = 1/32f;

    void UpdateTexture()
    {
        // range -1 to 1
        float axisX = OuyaExampleCommon.GetAxis(StickX, OuyaExampleCommon.Player);
        float axisY = OuyaExampleCommon.GetAxis(StickY, OuyaExampleCommon.Player);

        // put in 0 to TextureSize range
        int x = (int)((-axisX + 1) * 0.5f * (TextureSize - 1));
        int y = (int)((axisY + 1) * 0.5f * (TextureSize - 1));

        int index = x + y * TextureSize;

        if (x != m_lastX ||
            y != m_lastY)
        {
            m_lastX = x;
            m_lastY = y;
            if (index >= 0 &&
                index < m_pixels.Length)
            {
                if (m_pixelVs[index] < 0f)
                {
                    m_pixelVs[index] = 0f;
                }
                else
                {
                    m_pixelVs[index] = m_pixelVs[index] + m_increment;
                }
                Vector3 c = Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(1, 1, 1), m_pixelVs[index]);
                m_pixels[index].r = (byte) (int) (c.x*255);
                m_pixels[index].g = (byte) (int) (c.y*255);
                m_pixels[index].b = (byte) (int) (c.z*255);
                m_pixels[index].a = 255;
            }
        }
    }

    public void Update()
    {
        if (null == m_texture)
        {
            return;
        }

        UpdateTexture();

        if (m_timerTexture < DateTime.Now)
        {
            m_timerTexture = DateTime.Now + TimeSpan.FromMilliseconds(100);
            m_texture.SetPixels32(m_pixels);
            m_texture.Apply();
        }

        if (m_timerText < DateTime.Now)
        {
            m_timerText = DateTime.Now + TimeSpan.FromMilliseconds(1000);
            
            if (m_label)
            {
                UpdateCounts();
                m_label.text = string.Format("c={0} | u={1} | {2:F2}%",
                                             m_pixelCount, m_updatePixelCount, m_pixelRatio * 100);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.Space(100);
        if (GUILayout.Button("Clear Thumbstick Graph", GUILayout.MaxHeight(40)))
        {
            foreach (OuyaPlotThumbstick plot in m_plots)
            {
                if (plot)
                {
                    plot.ClearTexture();
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(100);
        if (GUILayout.Button("Toggle Thumbstick Graph", GUILayout.MaxHeight(40)))
        {
            m_toggleGraph = !m_toggleGraph;

            if (m_toggleGraph)
            {
                m_plotBackground.a = 0.5f; 
            }
            else
            {
                m_plotBackground.a = 0.99f;
            }
            foreach (OuyaPlotThumbstick plot in m_plots)
            {
                if (plot)
                {
                    plot.ClearTexture();
                }
            }
        }
        GUILayout.EndHorizontal();
    }
}