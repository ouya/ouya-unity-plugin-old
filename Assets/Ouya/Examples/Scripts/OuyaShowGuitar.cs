/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;

public class OuyaShowGuitar : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{
    [Serializable]
    public class CubeLaneItem
    {
        public GameObject StartPosition = null;
        public GameObject EndPosition = null;
        public Color LaneColor = Color.green;
        public OuyaSDK.KeyEnum LaneButton = OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_GREEN;
        public GameObject Instance = null;
    }

    public List<CubeLaneItem> Lanes = new List<CubeLaneItem>();

    [Serializable]
    public class NoteItem
    {
        public CubeLaneItem Parent = null;
        public GameObject Instance = null;
        public DateTime StartTime = DateTime.MinValue;
        public DateTime EndTime = DateTime.MinValue;
    }

    private List<NoteItem> Notes = new List<NoteItem>();

    private int NoteTimeToLive = 3000;

    private int NoteTimeToCreate = 500;

    private DateTime m_timerCreate = DateTime.MinValue;

    void Awake()
    {
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
    }

    void OnDestroy()
    {
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);

        DestroyNotes();
    }

    public void OuyaMenuButtonUp()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaMenuAppearing()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnPause()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnResume()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void DestroyNote(NoteItem note)
    {
        if (note.Instance)
        {
            Object.DestroyImmediate(note.Instance, true);
            note.Instance = null;
        }
    }

    public void DestroyNotes()
    {
        foreach (NoteItem note in Notes)
        {
            DestroyNote(note);
        }
        Notes.Clear();
    }

    public void CreateNote(CubeLaneItem item)
    {
        NoteItem note = new NoteItem();
        note.StartTime = DateTime.Now;
        note.EndTime = DateTime.Now + TimeSpan.FromMilliseconds(NoteTimeToLive);
        note.Parent = item;
        note.Instance = (GameObject)Instantiate(item.StartPosition);
        (note.Instance.renderer as MeshRenderer).material.color = item.LaneColor;
        Notes.Add(note);
    }

    void Update()
    {
        if (m_timerCreate < DateTime.Now)
        {
            m_timerCreate = DateTime.Now + TimeSpan.FromMilliseconds(NoteTimeToCreate);
            int index = UnityEngine.Random.Range(0, Lanes.Count);
            CreateNote(Lanes[index]);
        }

        List<NoteItem> removeList = new List<NoteItem>();
        foreach (NoteItem note in Notes)
        {
            if (note.EndTime < DateTime.Now)
            {
                removeList.Add(note);
                continue;
            }
            float elapsed = (float)(DateTime.Now - note.StartTime).TotalMilliseconds;
            note.Instance.transform.position =
                Vector3.Lerp(
                    note.Parent.StartPosition.transform.position,
                    note.Parent.EndPosition.transform.position,
                    elapsed/(float) NoteTimeToLive);
            if (OuyaExampleCommon.GetButton(note.Parent.LaneButton, OuyaSDK.OuyaPlayer.player1))
            {
                (note.Instance.renderer as MeshRenderer).material.color = Color.Lerp((note.Instance.renderer as MeshRenderer).material.color, Color.white, Time.deltaTime);
                note.Instance.transform.localScale = Vector3.Lerp(note.Instance.transform.localScale, note.Parent.StartPosition.transform.localScale * 2, Time.deltaTime);
            }
        }
        foreach (NoteItem note in removeList)
        {
            Notes.Remove(note);
            DestroyNote(note);
        }
    }

    void FixedUpdate()
    {
        OuyaExampleCommon.UpdateJoysticks();
    }
}