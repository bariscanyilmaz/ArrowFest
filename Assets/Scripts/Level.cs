
using PathCreation;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    PathCreator _pathCreator;
    public PathCreator GetPathCreator() => _pathCreator;
    
}
