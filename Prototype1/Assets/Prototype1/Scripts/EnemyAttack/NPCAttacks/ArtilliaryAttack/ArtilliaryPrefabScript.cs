using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ArtilliaryPrefabScript : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _time;
    private float _timer;
    private Vector3 _upperCoordinate;
    private SphereCollider _collider;
    private bool _endOfMovement = false;

    public void SetQuadraticCurve(Vector3 pos)
    {
        _startPos = transform.position;
        _endPos = pos;
        _upperCoordinate = (_endPos - _startPos) / 2;
        _upperCoordinate.y = _upperCoordinate.y + Vector3.Magnitude(_endPos - _startPos) * 2;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(_timer<=_time)
        {
            Vector3 pointA = Vector3.Lerp(_startPos, _upperCoordinate, _time);
            Vector3 pointB = Vector3.Lerp(_upperCoordinate, _endPos, _time);
            transform.position = Vector3.Lerp(pointA, pointB, _time);
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _endOfMovement = true;
        _collider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check for enemy;
    }
}
