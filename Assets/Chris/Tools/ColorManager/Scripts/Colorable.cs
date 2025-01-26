using UnityEngine;
using UnityEngine.UI;

namespace MKColorManager
{
    public class Colorable : IColorable
    {
        public override void SetColor(Color color)
        {
            if (TryGetComponent(out Text text))
                text.color = color;
            if (TryGetComponent(out Image image))
                image.color = color;
            if (TryGetComponent(out SpriteRenderer sr))
                sr.color = color;
            if (TryGetComponent(out Camera cam))
                cam.backgroundColor = color;

            #if UNITY_2018_1_OR_NEWER
            if (TryGetComponent(out TMPro.TMP_Text tmp))
                tmp.color = color;
            #endif
        }
    }
}

