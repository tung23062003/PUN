using UnityEngine;

public class AlignCamera : TickBehaviour
{
    private Camera _camera { get; set; }
    protected Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }
    public bool IsAlignWithCamera { get; set; } = true;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnUpdate()
    {
        if (!IsAlignWithCamera)
        {
            return;
        }
        if (Camera == null)
        {
            return;
        }
        base.OnUpdate();
        AlignWithCamera();
    }

    private void AlignWithCamera()
    {
        transform.forward = _camera.transform.forward;
    }
}
