using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLevel : GapiMonoBehaviour
{
    public static ScrollLevel instance;
    public static string nameMap;

    [SerializeField] protected List<Transform> listMap = new List<Transform>();
    [SerializeField] protected Vector3 pos;

    //[SerializeField] protected Text txtTargetOfMap;

    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private Vector2 lastPosition = Vector2.zero;
    [SerializeField] private bool isScrolling = false;
    [SerializeField] private float decelerationRate = 0.1f;

    private Vector3 smoothDampVelocity = Vector3.zero;
    private float smoothTime = 0.2f;

    protected override void Reset()
    {
        base.Reset();
        this.pos = new Vector3(0, transform.position.y, 0);

        this.listMap.Clear();
        foreach (Transform child in transform)
        {
            this.listMap.Add(child);
        }

        //this.txtTargetOfMap = transform.parent.Find("BtnPlay/Panel/_TxtTarget").GetComponent<Text>();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
        {
            this.ScrollMap();
        }
    }

    protected void ScrollMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isScrolling = true;
            lastPosition = Input.mousePosition;
            velocity = Vector2.zero;
        }
        else if (Input.GetMouseButton(0) && isScrolling)
        {
            Vector2 currentPosition = Input.mousePosition;
            velocity = (currentPosition - lastPosition) / Time.deltaTime;
            lastPosition = currentPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false;
        }

        if (!isScrolling && velocity.sqrMagnitude > 0.01f)
        {
            // Chỉ scroll theo trục Y
            transform.position += new Vector3(0, velocity.y * Time.deltaTime, 0);
            velocity *= decelerationRate;
        }

        Transform nearestObject = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform child in listMap)
        {
            float distance = Mathf.Abs(pos.y - child.position.y);

            float scale = Mathf.Lerp(0.6f, 1f, 1.1f - Mathf.Clamp01(distance / 5f));
            child.localScale = new Vector3(scale, scale, scale);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestObject = child;
            }
        }

        if (nearestObject != null && !isScrolling)
        {
            Vector3 offset = new Vector3(0, pos.y - nearestObject.position.y, 0);
            Vector3 targetPosition = transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothDampVelocity, smoothTime);

            nameMap = nearestObject.gameObject.name;

            //this.CheckTarget();
        }
    }

    /*
    protected void CheckTarget()
    {
        this.txtTargetOfMap.text = MainScore.instance.LoadInt("reward_" + nameMap, 0).ToString() + "/4";
    }
    */
}
