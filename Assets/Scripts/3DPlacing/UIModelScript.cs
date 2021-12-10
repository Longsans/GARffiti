using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelScript : ModelScript
{
    public float RotationSpeed = 30;
    public GameObject ModelPrefab;

    public bool Spinning = false;

    protected override void Awake()
    {
        base.Awake();
        gameObject.AddComponent<RectTransform>();
        MidAnchor.AddComponent<RectTransform>();
        BottomAnchor.AddComponent<RectTransform>();
    }

    private void Update()
    {
        if (Spinning)
            Rotation += RotationSpeed * Time.deltaTime;
    }

    public void FitToSize(float width, float height)
    {
        float ratioW = width  / (Size.x);
        float ratioH = height / (Size.y);
        float ratioD = width / (Size.z);

        float ratio = Mathf.Min(ratioW, ratioH, ratioD);

        SizeMultiplier = ratio;
    }

    public override void UseMidAnchor()
    {
        base.UseMidAnchor();
        gameObject.transform.localPosition = new Vector3(0, -MidPoint.y, 0);
    }
}
