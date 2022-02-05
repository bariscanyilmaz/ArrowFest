using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class FollowPath : MonoBehaviour
{
    [SerializeField]
    private PathCreator _pathCreator;

    [SerializeField]
    private EndOfPathInstruction _endOfPathInstruction;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private UnityEvent _endOfPath;

    private float _distanceTravelled;

    void Update()
    {
        if (_pathCreator != null)
        {
            _distanceTravelled += _speed * Time.deltaTime;
            var pos = _pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
            transform.position = new Vector3(pos.x, transform.position.y, pos.z);
            var rot = _pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
            transform.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);

            if (transform.position == _pathCreator.path.GetPoint(_pathCreator.path.NumPoints - 1))
            {
                _endOfPath.Invoke();
            }

        }
    }
}
