using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionWaypoint : MonoBehaviour
{
    public Image img;
    public List<Transform> targets;
    public TextMeshProUGUI meter;
    public Vector3 offset;
    public float interactionDistance = 3f;

    private Transform currentTarget;

    private void Update()
    {
        if (targets == null || targets.Count == 0)
        {
            img.enabled = false;
            meter.text = "";
            currentTarget = null;
            return;
        }

        UpdateNearestTarget(); // Dynamically find the closest one

        if (currentTarget == null) return;

        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(currentTarget.position + offset);

        if (Vector3.Dot((currentTarget.position - transform.position), transform.forward) < 0)
        {
            pos.x = (pos.x < Screen.width / 2) ? maxX : minX;
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;
        float distance = Vector3.Distance(currentTarget.position, transform.position);
        meter.text = ((int)distance).ToString() + "m";

        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            targets.Remove(currentTarget);
            currentTarget = null;
        }
    }

    private void UpdateNearestTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Transform t in targets)
        {
            if (t == null) continue;

            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = t;
            }
        }

        currentTarget = nearest;
    }
}
