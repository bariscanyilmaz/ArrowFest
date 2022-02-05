using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatformCreator : MonoBehaviour
{

    [SerializeField]
    private Platform _platformPrefab;

    [SerializeField]
    private int _count;

    [SerializeField]
    private float _spacing;

    private List<Platform> _platforms = new List<Platform>();

    [ContextMenu("Create")]
    private void Create()
    {
        for (int i = 0; i < _count; i++)
        {
            var platform = Instantiate(_platformPrefab, new Vector3(transform.position.x, transform.position.y, i * _spacing), Quaternion.identity);
            platform.transform.SetParent(transform);
            platform.transform.localPosition = new Vector3(transform.position.x, transform.position.y, i * _spacing);
            platform.SetText(i);
            _platforms.Add(platform);

        }
    }

    [ContextMenu("Delete")]
    private void Delete()
    {
        foreach (var item in _platforms)
        {
            Destroy(item);
        }
    }


}
