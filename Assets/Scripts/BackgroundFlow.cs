using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFlow : MonoBehaviour {

    public Transform BG_1;
    public Transform BG_2;
    public Transform Star_1;
    public Transform Star_2;

    public float BG_Speed = 5f;
    public float Star_Speed = 3f;

    private const float ICON_DIS = 40.96f;
    private static Vector3 BASE_ICON_VECTOR3 = new Vector3(0, ICON_DIS, 0);

    // Use this for initialization
    void Start () {
        InitUIPos();
    }

    private void InitUIPos ()
    {
        BG_1.localPosition = Vector3.zero;
        BG_2.localPosition = -BASE_ICON_VECTOR3;

        Star_1.localPosition = Vector3.zero;
        Star_2.localPosition = -BASE_ICON_VECTOR3;
    }
	
	// Update is called once per frame
	void Update () {
        BG_1.localPosition += Time.deltaTime * new Vector3(0, BG_Speed, 0);
        BG_2.localPosition += Time.deltaTime * new Vector3(0, BG_Speed, 0);

        if (BG_1.localPosition.y >= ICON_DIS)
        {
            BG_1.localPosition -= 2 * BASE_ICON_VECTOR3;
        }
        if (BG_2.localPosition.y >= ICON_DIS)
        {
            BG_2.localPosition -= 2 * BASE_ICON_VECTOR3;
        }

        // -----------------------------------------------------------------------------------

        Star_1.localPosition += Time.deltaTime * new Vector3(0, Star_Speed, 0);
        Star_2.localPosition += Time.deltaTime * new Vector3(0, Star_Speed, 0);

        if (Star_1.localPosition.y >= ICON_DIS)
        {
            Star_1.localPosition -= 2 * BASE_ICON_VECTOR3;
        }
        if (Star_2.localPosition.y >= ICON_DIS)
        {
            Star_2.localPosition -= 2 * BASE_ICON_VECTOR3;
        }
    }
}
