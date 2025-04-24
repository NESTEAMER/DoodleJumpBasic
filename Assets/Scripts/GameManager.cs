using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject platformRefPoint;

    [SerializeField]
    private GameObject platformPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Get platform width in screen space coordinates
        var refPoint = Camera.main.WorldToScreenPoint(platformRefPoint.transform.position);
        var min = platformRefPoint.GetComponent<SpriteRenderer>().bounds.min;
        var max = platformRefPoint.GetComponent<SpriteRenderer>().bounds.max;
        var lebar = max - min;

        for (var i = 1; i <= 100; i++)
        {
            var x = Random.Range(-Camera.main.WorldToScreenPoint(lebar).x, Screen.width +
           Camera.main.WorldToScreenPoint(lebar).x);
            var randomPos = Camera.main.ScreenToWorldPoint(new Vector3(x,
           refPoint.y + (i * 1500.0f)), 0f);
            Instantiate(platformPrefab, randomPos, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
