using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public Animator animatior;
    public SmoothTranslate sm;

    [HideInInspector] public Cross previousCross;
    public GameObject point;
    public float dPoint = 0.01f;
    public int nbPointLine = 10;
    public float speedPoints = 1;


    List<GameObject> points = new List<GameObject>();
    float deltaDist = 0;
    float dline, d2lines;
    [HideInInspector] public bool showPoint = true;

    private void Awake()
    {
        dline = nbPointLine * dPoint;
        d2lines = 2 * dline;
    }

    void Update()
    {
        deltaDist += Time.deltaTime * 0.1f * speedPoints;
        deltaDist %= d2lines;

        if(showPoint) UpdatePoints();
    }

    public void hidePoints() {
        ClearPoints();
        showPoint = false;
    }

    void UpdatePoints() {
        ClearPoints();
        SetPoints();
    }

    void ClearPoints() {
        while (points.Count > 0) {
            Destroy(points[0]);
            points.RemoveAt(0);
        }
    }

    void SetPoints()
    {
        Vector3 start;
        if (previousCross) start = previousCross.transform.position;
        else start = GameMan.inst.player.transform.position;

        Vector3 dir = Tool.Dir(start, this, false);
        float dist = dir.magnitude;
        if (dist == 0) return;

        int iPointLine = 0;

        for (float crtDist = deltaDist; crtDist < dist; crtDist += dPoint)
        {
            // pos
            Vector3 pos = start;
            float progress = Tool.Progress(crtDist, dist);
            pos += progress * dir;
            pos += GameMan.inst.player.jumpCurve.Evaluate(progress) * GameMan.inst.player.heightJump * dist * Vector3.up;
            Quaternion rot = Quaternion.LookRotation(Tool.Dir(GameMan.inst.camUsed.transform, pos, false));
            points.Add(Instantiate(point, pos, rot));

            // - - - - 
            iPointLine++;
            if (iPointLine == nbPointLine) {
                iPointLine = 0;
                crtDist += dline;
            }
        }


        if (deltaDist > dline) {
            for (float crtDist = deltaDist%dPoint; crtDist < deltaDist-dline; crtDist += dPoint)
            {
                // pos
                Vector3 pos = start;
                float progress = Tool.Progress(crtDist, dist);
                pos += progress * dir;
                pos += GameMan.inst.player.jumpCurve.Evaluate(progress) * GameMan.inst.player.heightJump * dist * Vector3.up;
                Quaternion rot = Quaternion.LookRotation(Tool.Dir(GameMan.inst.camUsed.transform, pos, false));
                points.Add(Instantiate(point, pos, rot));
            }
        }
    }
}
