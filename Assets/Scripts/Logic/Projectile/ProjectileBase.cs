using UnityEngine;
using System.Collections;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _speedCurve;
    [SerializeField]
    private AnimationCurve _heightCurve;

    public Sdb.ProjectileInfo ProjectileInfo { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 StartPosition { get; private set; }
    public virtual void Init(Sdb.ProjectileInfo projectileInfo)
    {
        this.ProjectileInfo = projectileInfo;
    }

    public void Go(Vector2 direction)
    {
        StartPosition = this.transform.position;
        this.Direction = direction;
    }

    private IEnumerator MoveProcess()
    {
        float moveTime = ProjectileInfo.MoveTime;
        float elapsedTime = 0f;
        while(elapsedTime < moveTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            float speed = _speedCurve.Evaluate(elapsedTime / moveTime);
            float moveX = Time.deltaTime * speed;
            float y = StartPosition.y + _heightCurve.Evaluate(elapsedTime / moveTime);

            transform.position = new Vector2(transform.position.x + moveX, y); ;
        }
    }
}