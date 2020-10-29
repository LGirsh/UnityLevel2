using UnityEngine;

public class MiniCamera : MonoBehaviour
{

    [SerializeField] private static Transform _player;
    [SerializeField] private Shader _replShader;

    private Camera _mapCam;
    private Vector3 newPosition;

    public static Transform Player { get => _player; set => _player = value; }

    void Start()
    {
        Player = FindObjectOfType<SinglePlayer>().transform;

        _mapCam = GetComponent<Camera>();
        _replShader = Shader.Find("Toon/Basic Outline");

        if (_replShader)
        {
            _mapCam.SetReplacementShader(_replShader, "RenderType");
        }
    }
    private void OnDisable()
    {
        _mapCam.ResetReplacementShader();
    }

    void LateUpdate()
    {
        if (_player)
        {
            newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, _player.eulerAngles.y, 0f);

        }
    }
}
