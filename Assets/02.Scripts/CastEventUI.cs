﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

public class CastEventUI : MonoBehaviour
{

    private SteamVR_LaserPointer laserPointer;

    void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        
        //이벤트 연결
        laserPointer.PointerIn += OnPointerEnter;
        laserPointer.PointerOut += OnPointerExit;
        laserPointer.PointerClick += OnPointerClick;
        Debug.Log("Event +++++");
    }

    void OnDisable()
    {
        //이벤트 해제
        laserPointer.PointerIn -= OnPointerEnter;
        laserPointer.PointerOut -= OnPointerExit;
        laserPointer.PointerClick -= OnPointerClick;
        Debug.Log("Event -----");
    }

    void OnPointerEnter(object sender, PointerEventArgs e)
    {
        IPointerEnterHandler enter = e.target.GetComponent<IPointerEnterHandler>();
        if (enter == null) return;

        enter.OnPointerEnter(new PointerEventData(EventSystem.current));
    }

    void OnPointerExit(object sender, PointerEventArgs e)
    {
        IPointerExitHandler enter = e.target.GetComponent<IPointerExitHandler>();
        if (enter == null) return;

        enter.OnPointerExit(new PointerEventData(EventSystem.current));
    }

    void OnPointerClick(object sender, PointerEventArgs e)
    {
        IPointerClickHandler enter = e.target.GetComponent<IPointerClickHandler>();
        if (enter == null) return;

        enter.OnPointerClick(new PointerEventData(EventSystem.current));
    }    
}
