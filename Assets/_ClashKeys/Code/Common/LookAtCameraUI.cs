using System;
using UnityEngine;

namespace ClashKeys.Common
{
internal class LookAtCameraUI : MonoBehaviour
{
    [SerializeField] private Camera _cachedCamera;

    private void Start()
    {
        if (_cachedCamera == null)
            _cachedCamera = Camera.main;

        if (_cachedCamera == null)
            throw new ArgumentNullException();
    }

    private void Update() => transform.LookAt(_cachedCamera.transform);

#if UNITY_EDITOR
    private void Reset()
    {
        _cachedCamera = Camera.main;
    }
#endif
}
}