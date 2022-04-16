using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBall : MonoBehaviour
{
    private Ball ball;
    private IEnumerator moveCoroutine;
    void Awake()
    {
        ball = GetComponent<Ball>();
    }

    public void Move(int newX, int newY, float time)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = MoveCoroutine(newX, newY, time);
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(int newX, int newY, float time)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = ball.GridRef.GetWorldPosition(newX, newY);

        for (float t = 0; t <= 1 * time; t += 0.01f)
        {
            ball.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }
    }
}
