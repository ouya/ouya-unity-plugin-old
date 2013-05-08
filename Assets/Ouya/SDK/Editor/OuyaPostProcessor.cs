using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OuyaPostProcessor : AssetPostprocessor
{
    // detected files
    private static Dictionary<string, DateTime> m_detectedFiles = new Dictionary<string, DateTime>();

    // post processor event
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                                               string[] movedFromPath)
    {
        if (!OuyaPanel.UsePostProcessor)
        {
            return;
        }

        if (EditorApplication.isCompiling)
        {
            return;
        }

        bool detectedCPlusPlus = false;
        bool detectedJava = false;
        foreach (string path in importedAssets)
        {
            if (Path.GetExtension(path).ToUpper().Equals(".CPP"))
            {
                if (!m_detectedFiles.ContainsKey(path) ||
                    (m_detectedFiles[path] + TimeSpan.FromSeconds(5)) < File.GetLastWriteTime(path))
                {
                    m_detectedFiles[path] = File.GetLastWriteTime(path);
                    Debug.Log(string.Format("{0} C++ change: {1}", File.GetLastWriteTime(path), path));
                    detectedCPlusPlus = true;
                }
            }
            else if (Path.GetExtension(path).ToUpper().Equals(".JAVA") &&
                !Path.GetFileName(path).ToUpper().Equals("R.JAVA"))
            {
                if (!m_detectedFiles.ContainsKey(path) ||
                    (m_detectedFiles[path] + TimeSpan.FromSeconds(5)) < File.GetLastWriteTime(path))
                {
                    m_detectedFiles[path] = File.GetLastWriteTime(path);
                    Debug.Log(string.Format("{0} Java change: {1}", File.GetLastWriteTime(path), path));
                    detectedJava = true;
                }
            }
        }

        if (detectedCPlusPlus)
        {
            //compile NDK
            OuyaPanel.CompileNDK();
        }

        if (detectedJava)
        {
            //compile Plugin
            OuyaMenuAdmin.MenuGeneratePluginJar();

            //compile Application Java
            OuyaPanel.CompileApplicationJava();
        }
    }
}