using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollDragMonitor : MonoBehaviour
{
    private bool isDragging;

    public void startDrag() => isDragging = true;
    public void endDrag() => isDragging = false;

    public bool drag() { return isDragging; }
}
