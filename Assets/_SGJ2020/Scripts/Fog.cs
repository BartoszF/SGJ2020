using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public GameObject fogPrefab;


    public float parallaxFactorX = 1f;
    public float parallaxFactorY = 1f;

    public float scrollingFactorY;
    public float scrollingFactorX;

    public float z = 1000;

    private Camera _camera;

    private float _currentScrollX;
    private float _currentScrollY;

    private GameObject _fogA;
    private GameObject _fogB;
    private GameObject _fogC;
    private GameObject _fogD;

    public float fogTileSize = 20f;

    public void Awake()
    {
        _camera = Camera.main;
        _fogA = Instantiate(fogPrefab, transform);
        _fogB = Instantiate(fogPrefab, transform);
        _fogC = Instantiate(fogPrefab, transform);
        _fogD = Instantiate(fogPrefab, transform);
    }

    private void Update()
    {
        var position = _camera.transform.position;
        var posA = new Vector3(
            (_currentScrollX + (position.x) * parallaxFactorX) % fogTileSize,
            (_currentScrollY + (position.y) * parallaxFactorY) % fogTileSize,
            z
        );
        var posB = new Vector3(
            (_currentScrollX +( position.x) * parallaxFactorX) % fogTileSize + fogTileSize,
            (_currentScrollY +( position.y) * parallaxFactorY) % fogTileSize,
            z
        );
        var posC = new Vector3(
            (_currentScrollX + (position.x) * parallaxFactorX) % fogTileSize,
            (_currentScrollY + (position.y) * parallaxFactorY) % fogTileSize + fogTileSize,
            z
        );
        var posD = new Vector3(
            (_currentScrollX + (position.x) * parallaxFactorX) % fogTileSize + fogTileSize,
            (_currentScrollY + (position.y) * parallaxFactorY) % fogTileSize + fogTileSize,
            z
        );
        _fogA.transform.position = posA + position - Vector3.one * fogTileSize;
        _fogB.transform.position = posB + position - Vector3.one * fogTileSize;
        _fogC.transform.position = posC + position - Vector3.one * fogTileSize;
        _fogD.transform.position = posD + position - Vector3.one * fogTileSize;

        _currentScrollX += scrollingFactorX * Time.deltaTime;
        _currentScrollY += scrollingFactorY * Time.deltaTime;
    }
}