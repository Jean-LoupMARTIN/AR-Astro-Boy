using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class GameMan : MonoBehaviour
{
    public static GameMan inst;

    public static int NARgroundLM;

    public static bool android;
    public GameObject AR, NAR;
    public ARRaycastManager arRaycastManager;
    public Camera ARcam, NARcam;
    [HideInInspector] public Camera camUsed;

    public Cross crossPrefab;
    [HideInInspector] public List<Cross> crosses = new List<Cross>();

    public Player player;
    public UnityEvent activePlayerEvent, placePlayerEvent;
    bool initPlayer = true;

    



    void Awake() {
        android = SystemInfo.deviceName != "MacBook Pro de Jean-Loup";
        AR.SetActive(android);
        NAR.SetActive(!android);
        if (android) camUsed = ARcam;
        else camUsed = NARcam;

        NARgroundLM = LayerMask.GetMask("NAR Ground");

        inst = this;
    }


    void Update()
    {
        if (initPlayer) InitPlayer();
        else  UpdateCrosses();
    }

    private void UpdateCrosses()
    {
        Vector3 pos;
        Cross lastCross = Tool.Last(crosses);
        if ((android && Tool.ScreenCenterHitAR(arRaycastManager, out pos)) ||
            (!android && Tool.MouseHit(camUsed, out pos, NARgroundLM)))
        {
            // rotation
            Vector3 camPosProj = camUsed.transform.position;
            camPosProj.y = pos.y;
            Tool.Look(lastCross.transform, Tool.Dir(pos, camPosProj, false));

            // position
            lastCross.sm.SetTarget(pos);

            // active
            if (!lastCross.gameObject.active)
                lastCross.gameObject.SetActive(true);
        }

        if (lastCross.gameObject.active && Tool.Click()) {
            lastCross.animatior.Play("NoOscillation");
            AddCross();
        }
    }

    void AddCross() {
        Cross cross =  Instantiate(crossPrefab);
        cross.gameObject.SetActive(false);
        if (crosses.Count > 0) cross.previousCross = Tool.Last(crosses);
        crosses.Add(cross);
    }

    void InitPlayer()
    {
        Vector3 pos;

        if ((android && Tool.ScreenCenterHitAR(arRaycastManager, out pos)) ||
            (!android && Tool.MouseHit(camUsed, out pos, NARgroundLM)))
        {
            // rotation
            Vector3 camPosProj = camUsed.transform.position;
            camPosProj.y = pos.y;
            Tool.Look(player.transform, Tool.Dir(pos, camPosProj, false));

            // position
            player.st.SetTarget(pos);

            // active
            if (!player.gameObject.active) {
                player.gameObject.SetActive(true);
                activePlayerEvent.Invoke();
            }
        }

        if (player.gameObject.active && Tool.Click()) {
            initPlayer = false;
            AddCross();
            placePlayerEvent.Invoke();
        }
    }




}
