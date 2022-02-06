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

    public void SetPathCreator(PathCreator creator)
    {
        _pathCreator = creator;
    }
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Play || GameManager.Instance.GameState == GameState.FinishLine)
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

    public void OnGameStart()
    {
        _distanceTravelled = 0;
        _pathCreator = GameManager.Instance.CurrentLevel.GetPathCreator();
    }

    public void OnGameRestart()
    {
        _distanceTravelled = 0;
        var pos = _pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        var rot = _pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
        transform.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);

        _pathCreator = GameManager.Instance.CurrentLevel.GetPathCreator();
        //_pathCreator.TriggerPathUpdate();
    }
}
