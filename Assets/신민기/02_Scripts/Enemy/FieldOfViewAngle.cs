using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] float viewAngle;
    [SerializeField] float viewDistance; // 시야 거리
    [SerializeField] LayerMask tarGetMask; // 타겟 마스터

    Enemys monster;

    private void Start()
    {
        monster = GetComponent<Enemys>();
    }

    private void Update()
    {
        if (monster.enemyType == EnemyType.Animal)
            View();
        else
            HouseCheck();
    }

    Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    void View()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, tarGetMask);
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.CompareTag("Player"))
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.CompareTag("Player"))
                        {
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            Vector3 newPos = _hit.transform.position;
                            monster.Run(newPos);
                        }
                    }
                }
            }
        }
    }

    void HouseCheck()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, tarGetMask);
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.CompareTag("HOUSE"))
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.CompareTag("HOUSE"))
                        {
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            Vector3 newPos = _hit.transform.position;
                            monster.Run(newPos);
                        }
                    }
                }
            }
        }
    }
}
