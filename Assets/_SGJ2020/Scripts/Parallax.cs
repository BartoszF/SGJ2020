using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxFactorX = 1f;
    public float parallaxFactorY = 1f;

    private Camera _camera;
    
    public void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        var position = _camera.transform.position;
        transform.position = new Vector3(position.x * parallaxFactorX, position.y * parallaxFactorY);
    }
}
