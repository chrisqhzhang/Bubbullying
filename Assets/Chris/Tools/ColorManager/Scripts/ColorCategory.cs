using System;
using UnityEngine;

[Serializable]
public class ColorCategory
{
    [SerializeField]
    private string _category;
    public string Category => _category;

    [SerializeField]
    private Color _color;
    public Color Color => _color;
}
