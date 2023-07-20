using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierArrow : MonoBehaviour
{
    //!!! NOT WORKING !!!
    //https://www.youtube.com/watch?v=FpuH303FYYU
    //WATCH LINK TO UNDERSTAND PROPERLY!

    public GameObject ArrowHead;
    public GameObject ArrowBody;

    public int arrowNodeNum;
    public float scaleFactor = 1f;

    private RectTransform origin;
    private List<RectTransform> arrowNodes = new();
    private List<Vector2> controlPoints = new();

    private readonly List<Vector2> controlPointAnchors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };
    private void Awake()
    {
        this.origin = this.GetComponent<RectTransform>();
        Debug.Log(this.origin.position.x);
        for (int i = 0; i < this.arrowNodeNum; i++)
        {
            this.arrowNodes.Add(Instantiate(this.ArrowBody, this.transform).GetComponent<RectTransform>());
        }

        this.arrowNodes.Add(Instantiate(this.ArrowHead, this.transform).GetComponent<RectTransform>());

        this.arrowNodes.ForEach(x => x.GetComponent<RectTransform>().position = new Vector2(0, 0));

        for (int i = 0; i < 4; i++)
        {
            this.controlPoints.Add(Vector2.zero);
        }
    }
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 arrowPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);
        this.controlPoints[3] = new Vector2(arrowPosition.x, arrowPosition.y);

        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0] * this.controlPointAnchors[0]);
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0] * this.controlPointAnchors[1]);

        for (int i = 0; i < this.arrowNodes.Count; i++)
        {
            var x = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);

            this.arrowNodes[i].position =
                Mathf.Pow(1 - x, 3) * this.controlPoints[0] +
                3 * Mathf.Pow(1 - x, 2) * x * this.controlPoints[1] +
                3 * (1 - x) * Mathf.Pow(x, 2) * this.controlPoints[2] +
                Mathf.Pow(x, 3) * this.controlPoints[3];

            if (i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);
            }

            var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
        }

        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
}
