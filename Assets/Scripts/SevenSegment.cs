using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenSegment : MonoBehaviour
{
    [SerializeField] private Color offColor = new Color(0,0,0, 0.5f);
    [SerializeField] private Color onColor = Color.white;
    private Renderer[] segmentRenderers = null;

    private readonly byte[] binaryValues = new byte[] 
    {
        0b_0011_1111,   // 0
        0b_0000_0110,   // 1
        0b_0101_1011,   // 2
        0b_0100_1111,   // 3
        0b_0110_0110,   // 4
        0b_0110_1101,   // 5
        0b_0111_1101,   // 6
        0b_0000_0111,   // 7
        0b_0111_1111,   // 8
        0b_0110_0111,   // 9
    };
    private void Awake()
    {
        segmentRenderers = GetComponentsInChildren<Renderer>();
        Debug.Assert(segmentRenderers.Length == 7, "There must be 7 renderers.");
    }
    public void DisplayDigit(uint? digit)
    {
        Debug.Assert(!digit.HasValue || digit < binaryValues.Length,
            "Seven segment display can only display single digits.");

        var binary = digit.HasValue ? binaryValues[digit.Value] : 0;

        foreach (Renderer segmentRenderer in segmentRenderers)
        {
            segmentRenderer.material.color = (binary & 0b_0000_0001) > 0 ? onColor : offColor;
            binary >>= 1;
        }
    }
}
