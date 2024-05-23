using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Move : MonoBehaviour
{

    [SerializeField] private float timeTaken = 0.2f;//到完了为止花费的时间
    private float timeErapsed;//经过时间
    private Vector3 destination;//目的地
    private Vector3 origin;//出发地

    private void Start()
    {
        destination = transform.position;
        origin = destination;
    }

    private void Update()
    {
        //如果目的地和出发地相同，则不移动
        if (origin == destination) { return; }
        //时间经过加算
        timeErapsed += Time.deltaTime;
        //时间比率
        float timeRate = timeErapsed / timeTaken;
        //限制时间比率
        if (timeRate > 1) { timeRate = 1; }
        //计算easing
        float easing = timeRate;
        //计算坐标
        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
        //代入坐标
        transform.position = currentPosition;
    }

    public void MoveTo(Vector3 newDestination)
    {
        timeErapsed = 0;

        origin = destination;

        transform.position = origin;

        destination = newDestination;
    }
}
