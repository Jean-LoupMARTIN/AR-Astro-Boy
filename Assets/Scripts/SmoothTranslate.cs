using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTranslate : MonoBehaviour
{
    public float t = 0.2f;
    float crt_t;

    public AnimationCurve curve;

    Vector3 target, lastPos;

    void Awake() { crt_t = t; }

    void Update()
    {
        if (crt_t < t) {
            crt_t += Time.deltaTime;
            float progress = Tool.Progress(crt_t, t);
            if (progress >= 1) transform.position = target;
            else transform.position = lastPos + (target - lastPos) * curve.Evaluate(progress);
        }
    }

    public void SetTarget(Vector3 target) {
        if (gameObject.active) {
            this.target = target;
            lastPos = transform.position;
            crt_t = 0;
        }
        else transform.position = target;
    }
}
