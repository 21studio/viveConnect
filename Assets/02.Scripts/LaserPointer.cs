﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    private SteamVR_Behaviour_Pose pose;
    [SerializeField]
    private SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean trigger;

    //라인렌더러의 속성 설정
    private LineRenderer line;
    public float maxDistance = 10.0f; //광선의 최대거리
    public Color defaultColor = Color.white; //광선의 기본 색상
    public Color clickedColor = Color.green; //클릭했을 때의 색상

    //레이캐스트
    private Ray ray;
    private RaycastHit hit;
    private int layerButton;

    private Transform controller;

    //델리게이트 선언
    public delegate void PointerInHandler(GameObject button);
    public delegate void PointerOutHandler(GameObject button);
    public delegate void PointerClickHandler(GameObject button);
    //이벤트 선언
    public static event PointerInHandler OnPointerIn;
    public static event PointerOutHandler OnPointerOut;
    public static event PointerClickHandler OnPointerClick;

    private GameObject prevButton;

    void Start()
    {
        trigger = SteamVR_Actions.default_InteractUI;

        pose = GetComponent<SteamVR_Behaviour_Pose>()        ;
        hand = pose.inputSource;
        controller = GetComponent<Transform>();

        layerButton = 1 << LayerMask.NameToLayer("BUTTON_UI");
        // | 1 << LayerMask.NameToLayer("OBSTACLE");
        // 1<<8 = 256(십진수, 비트연산)

        CreateLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (prevButton != null)
        {
            OnPointerOut(prevButton);
            prevButton = null;
        }

        ray = new Ray(controller.position, controller.forward);

        if (Physics.Raycast(ray, out hit, maxDistance, layerButton))
        {
            line.SetPosition(1, new Vector3(0, 0, hit.distance));
            
            OnPointerIn(hit.collider.gameObject);
            prevButton = hit.collider.gameObject;            
            
            if (trigger.GetStateDown(hand))
            {
                ExecuteEvents.Execute(hit.collider.gameObject, 
                            new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
        }
        else
        {
            line.SetPosition(1, new Vector3(0, 0, maxDistance));            
        }
    }

    void CreateLine()
    {
        //라인렌더러를 생성한 후 변수에 저장
        line = this.gameObject.AddComponent<LineRenderer>();
        //로컬좌표계 기준으로 라인렌더러를 드로잉하는 속성
        line.useWorldSpace = false;
        line.receiveShadows = false;

        //시작점, 끝점의 갯수
        line.positionCount = 2;
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, new Vector3(0, 0, maxDistance));

        //라인렌더러의 폭
        line.widthMultiplier = 0.03f;
        line.numCapVertices = 10;
        //라인렌더러의 머터리얼 적용
        line.material = new Material(Shader.Find("Unlit/Color"));
        line.material.color = defaultColor;
    }
}