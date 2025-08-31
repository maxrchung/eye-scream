using System;
using UnityEngine;

namespace Utils
{
    public class RenderUtils
    {
        private Texture2D _whiteTexture;

        public Rect NormToScreen(Rect normRect) => new(
            normRect.x * Screen.width,
            (1f - normRect.y - normRect.height) * Screen.height,
            normRect.width * Screen.width,
            normRect.height * Screen.height
        );

        public int PercentToPixels(float percent) => Math.Min(Mathf.RoundToInt(percent / 100 * Screen.width), 1);

        public void DrawRectOutline(Rect position, Color color, int thickness = 1, int margin = 0)
        {
            position.xMin += margin;
            position.xMax -= margin;
            position.yMin += margin;
            position.yMax -= margin;
            var oldColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(position.xMin, position.yMin, position.width, thickness), _whiteTexture); // Top
            GUI.DrawTexture(new Rect(position.xMin, position.yMax - thickness, position.width,
                thickness), _whiteTexture); // Bottom
            GUI.DrawTexture(new Rect(position.xMin, position.yMin, thickness, position.height),
                _whiteTexture); // Left
            GUI.DrawTexture(new Rect(position.xMax - thickness, position.yMin, thickness, position.height),
                _whiteTexture); // Right
            GUI.color = oldColor;
        }

        public RenderUtils()
        {
            _whiteTexture = new Texture2D(1, 1);
            _whiteTexture.SetPixel(0, 0, Color.white);
            _whiteTexture.Apply();
        }
    }
}