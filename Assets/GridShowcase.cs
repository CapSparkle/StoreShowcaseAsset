
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

public class GridShowcase : MonoBehaviour
{
    public int columns = 3;
    public float spacing = 1f;
    public Vector3 startingPosition = Vector3.zero;
    public float moveDuration = 1f;
    public float scaleDuration = 0.5f;
    public Ease moveEase = Ease.OutQuad;
    public Ease scaleEase = Ease.OutQuad;

    [SerializeField]
    BundleLoader bundleLoader;

    [SerializeField]
    GameObject go;
    private void Start()
    {
        bundleLoader.updateDone += SetTheSetOfGameObjects;

        var cube = go;
        var testGos = new List<GameObject>();
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);
        testGos.Add(cube);

        SetTheSetOfGameObjects(
            testGos    
        );
    }

    List<GameObject> goods = null;
    public void SetTheSetOfGameObjects(List<GameObject> objects)
    {
       

        int index = 0;
        Vector3 position = startingPosition;
        float maxBoundsSize = 0f;

        // Calculate the maximum bounds size of the objects to use for scaling
        foreach (GameObject obj in objects)
        {
            maxBoundsSize = Mathf.Max(maxBoundsSize, obj.GetComponent<MeshRenderer>().bounds.size.magnitude);
        }

        if (goods != null)
            foreach (var commodity in goods)
                Destroy(commodity);

        goods = new List<GameObject>();

        // Arrange the objects in a grid
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < Mathf.CeilToInt((float)objects.Count / columns); j++)
            {
                if (index >= objects.Count) break;

                // Instantiate the object
                GameObject obj = Instantiate(objects[index], transform);
                goods.Add(obj);

                // Scale the object to be non-intersecting

                var exents = obj.GetComponent<MeshRenderer>().bounds.extents;
                float scale = 0.65f / ((new List<float> { exents.x, exents.y, exents.z }).Max() * 2);
                obj.transform.localScale = Vector3.zero;

                // Move and scale the object smoothly using DOTween
                obj.transform.DOLocalMove(position, moveDuration).SetEase(moveEase);
                obj.transform.DOScale(scale, scaleDuration).SetEase(scaleEase);

                // Increment the position for the next object
                position.x += spacing;
                index++;
            }
            position.x = startingPosition.x;
            position.z += spacing;
        }
    }
}
