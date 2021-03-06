﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Material lineMat;
    public float maxLineLength;
    public Gradient clr;
    public float currentLengthPercent = 0;

    #region linestuff
    public void DrawLine(Vector2 start, Vector2 end) {
        //Debug.Log("Drawing line " + start + " => " + end);

        if ((end - start).magnitude > maxLineLength) {
            end = start + (start - end).normalized * maxLineLength;
        }

        Vector3[] arr = { start, end };
        if (lineRenderer == null) {
            GameObject lineObject = new GameObject();
            lineObject.transform.SetParent(this.transform);
            this.lineRenderer = lineObject.AddComponent<LineRenderer>();

        }
        currentLengthPercent = Vector2.Distance(start, end)/maxLineLength;

        this.lineRenderer.startWidth = 0.15f;
        this.lineRenderer.endWidth = 0.05f;
        this.lineRenderer.positionCount = 2;
        lineRenderer.material = lineMat;
        lineRenderer.colorGradient = clr;
        this.lineRenderer.SetPositions(arr);



    }

    public void UpdateLine(Vector2 start, Vector2 end) {
        //Debug.Log("Updating line " + start + " => " + end);
        if ((start - end).magnitude > maxLineLength) {
            end = start + (end - start).normalized * maxLineLength;
        }
        if (this.lineRenderer == null) {
            //Debug.Log("Update line called no line renderer.");
            return;
        }
        Vector3[] arr = { start, end };
        currentLengthPercent = Vector2.Distance(start, end) / maxLineLength;
        this.lineRenderer.SetPositions(arr);

    }

    public void DestroyLine() {
        if (lineRenderer != null) {
            Destroy(lineRenderer.gameObject);
            currentLengthPercent = 0;
        }

    }
    #endregion linestuff
}
