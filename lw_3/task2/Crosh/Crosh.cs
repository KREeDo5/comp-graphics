using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace task2
{
    public class Crosh : BaseSmesharik
    {
        public Crosh(float x, float y, float size, Color4 bodyColor)
        : base(x, y, size, bodyColor)
        {

        }

        protected override void DrawBody()
        {
            GL.Color4(borderColor);
            Helper.DrawCircle(0f, 0f, 5.3f);

            GL.Color4(bodyColor);
            Helper.DrawCircle(0f, 0f, 5f);
        }

        protected override void DrawEyes()
        {
            float eyeWidth = 1.2f;
            float eyeHeight = 1.6f;

            Color4 eyeColor = new Color4(1f, 1f, 1f, 1);
            Color4 pupilColor = new Color4(0f, 0f, 0f, 1);


            GL.Color4(borderColor);
            Helper.DrawEllipse(-1.2f, 2, eyeWidth + 0.2f, eyeHeight + 0.2f);
            Helper.DrawEllipse(1.2f, 2, eyeWidth + 0.2f, eyeHeight + 0.2f);

            GL.Color4(eyeColor);
            Helper.DrawEllipse(-1.2f, 2, eyeWidth, eyeHeight);
            Helper.DrawEllipse(1.2f, 2, eyeWidth, eyeHeight);

            GL.Color4(pupilColor);
            Helper.DrawEllipse(-0.6f, 1.7f, eyeWidth / 3, eyeHeight / 3);
            Helper.DrawEllipse(0.6f, 1.7f, eyeWidth / 3, eyeHeight / 3);
        }

        protected override void DrawMouth()
        {
            Color4 mouthColor = borderColor;
            GL.Color4(mouthColor);
            GL.LineWidth(5);
            List<Vector2> mouth = new List<Vector2>
            {
                new Vector2(-2f, 0.0f),
                new Vector2(0.0f, -1.9f),
                new Vector2(2f, 0.0f),
            };

            Helper.DrawCasteljauBezier(mouth, false);

            Color4 toothColor = new Color4(1f, 1f, 1f, 1);
            GL.Color4(toothColor);

            List<Vector2> leftTeeth = new List<Vector2>
            {
                new Vector2(-0.6f, -1f),
                new Vector2(-0.6f, -2f),
                new Vector2(-0.02f, -2f),
                new Vector2(-0.02f, -1.1f),
            };

            Helper.DrawCasteljauBezier(leftTeeth, true);
            List<Vector2> rightTeeth = new List<Vector2>
            {
                new Vector2(0.6f, -1f),
                new Vector2(0.6f, -2f),
                new Vector2(0.02f, -2f),
                new Vector2(0.02f, -1.1f),
            };

            Helper.DrawCasteljauBezier(rightTeeth, true);
        }

        protected override void DrawHands()
        {
            GL.Color4(borderColor);
            List<Vector2> leftHandBorder = new List<Vector2>
            {
                new Vector2(-5f, 0f),
                new Vector2(-6f, -1f),
                new Vector2(-6f, -2f),
                new Vector2(-5.5f, -4f),
                new Vector2(-7.5f, -4f),
                new Vector2(-7.5f, -2f),
                new Vector2(-7.5f, -1.5f),
                new Vector2(-7f, -1f),
                new Vector2(-6f, -0.5f),
                new Vector2(-5f, 1.5f),
            };

            Helper.DrawCasteljauBezier(leftHandBorder, true);

            GL.Color4(borderColor);
            List<Vector2> rightHandBorder = new List<Vector2>
            {
                new Vector2(5f, 0f),
                new Vector2(6f, -1f),
                new Vector2(6f, -2f),
                new Vector2(5.5f, -4f),
                new Vector2(7.5f, -4f),
                new Vector2(7.5f, -2f),
                new Vector2(7.5f, -1.5f),
                new Vector2(7f, -1f),
                new Vector2(6f, -0.5f),
                new Vector2(5f, 1.5f),
            };

            Helper.DrawCasteljauBezier(rightHandBorder, true);

            GL.Color4(bodyColor);
            List<Vector2> leftHand = new List<Vector2>
            {
                new Vector2(-5.2f, 0.1f),
                new Vector2(-6.2f, -0.9f),
                new Vector2(-6.2f, -1.9f),
                new Vector2(-6.5f, -3.6f),
                new Vector2(-6.8f, -3.6f),
                new Vector2(-7f, -2.1f),
                new Vector2(-7f, -1.6f),
                new Vector2(-6.9f, -1.2f),
                new Vector2(-5.9f, -0.7f),
                new Vector2(-5.2f, 1.3f),
            };
            Helper.DrawCasteljauBezier(leftHand, true);

            List<Vector2> rightHand = new List<Vector2>
            {
                new Vector2(5.2f, 0.1f),
                new Vector2(6.2f, -0.9f),
                new Vector2(6.2f, -1.9f),
                new Vector2(6.5f, -3.6f),
                new Vector2(6.8f, -3.6f),
                new Vector2(7f, -2.1f),
                new Vector2(7f, -1.6f),
                new Vector2(6.9f, -1.2f),
                new Vector2(5.9f, -0.7f),
                new Vector2(5.2f, 1.3f),
            };
            Helper.DrawCasteljauBezier(rightHand, true);
        }

        protected override void DrawLegs()
        {
            GL.Color4(borderColor);
            List<Vector2> leftLegBorder = new List<Vector2>
            {
                new Vector2(-0.4f, -5f),
                new Vector2(-0.4f, -7.5f),
                new Vector2(-1f, -7.5f),
                new Vector2(-2f, -7.5f),
                new Vector2(-4f, -7.5f),
                new Vector2(-3.8f, -6.5f),
                new Vector2(-0.4f, -5f),
            };

            Helper.DrawCasteljauBezier(leftLegBorder, true);

            List<Vector2> rightLegBorder = new List<Vector2>
            {
                new Vector2(0.4f, -5f),
                new Vector2(0.4f, -7.5f),
                new Vector2(1f, -7.5f),
                new Vector2(2f, -7.5f),
                new Vector2(4f, -7.5f),
                new Vector2(3.8f, -6.5f),
                new Vector2(0.4f, -5f),
            };

            Helper.DrawCasteljauBezier(rightLegBorder, true);

            GL.Color4(bodyColor);
            List<Vector2> leftLeg = new List<Vector2>
            {
                new Vector2(-0.6f, -5.1f),
                new Vector2(-0.6f, -7.3f),
                new Vector2(-1.1f, -7.3f),
                new Vector2(-2.1f, -7.3f),
                new Vector2(-3.7f, -7.3f),
                new Vector2(-3.5f, -6.5f),
                new Vector2(-0.6f, -5.4f),
            };

            Helper.DrawCasteljauBezier(leftLeg, true);

            List<Vector2> rightLeg = new List<Vector2>
            {
                new Vector2(0.6f, -5.1f),
                new Vector2(0.6f, -7.3f),
                new Vector2(1.1f, -7.3f),
                new Vector2(2.1f, -7.3f),
                new Vector2(3.7f, -7.3f),
                new Vector2(3.5f, -6.5f),
                new Vector2(0.6f, -5.4f),
            };

            Helper.DrawCasteljauBezier(rightLeg, true);
        }

        protected override void DrawEars()
        {
            GL.Color4(borderColor);
            List<Vector2> leftEarBorder = new List<Vector2>
            {
                new Vector2(-0.3f, 5f),
                new Vector2(-0.3f, 5.5f),
                new Vector2(-0.5f, 6f),
                new Vector2(-0.5f, 7f),
                new Vector2(-0.5f, 9f),
                new Vector2(-0.3f, 9.5f),
                new Vector2(0f, 10f),
                new Vector2(-1f, 11f),
                new Vector2(-3f, 13.5f),
                new Vector2(-5f, 14f),
                new Vector2(-4.5f, 13f),
                new Vector2(-4f, 13f),
                new Vector2(-3.5f, 12f),
                new Vector2(-2.5f, 10f),
                new Vector2(-2f, 9f),
                new Vector2(-1f, 7f),
                new Vector2(-2f, 4.9f),
            };

            Helper.DrawCasteljauBezier(leftEarBorder, true);

            List<Vector2> rightEarBorder = new List<Vector2>
            {
                new Vector2(0.3f, 5f),
                new Vector2(0.3f, 5.5f),
                new Vector2(0.5f, 6f),
                new Vector2(0.5f, 7f),
                new Vector2(0.5f, 9f),
                new Vector2(0.3f, 9.5f),
                new Vector2(0f, 10f),
                new Vector2(1f, 11f),
                new Vector2(3f, 13.5f),
                new Vector2(5f, 14f),
                new Vector2(4.5f, 13f),
                new Vector2(4f, 13f),
                new Vector2(3.5f, 12f),
                new Vector2(2.5f, 10f),
                new Vector2(2f, 9f),
                new Vector2(1f, 7f),
                new Vector2(2f, 4.9f),
            };

            Helper.DrawCasteljauBezier(rightEarBorder, true);

            GL.Color4(bodyColor);
            List<Vector2> leftEar = new List<Vector2>
            {
                new Vector2(-0.42f, 5f),
                new Vector2(-0.5f, 5.6f),
                new Vector2(-0.52f, 6f),
                new Vector2(-0.52f, 7f),
                new Vector2(-0.52f, 8.8f),
                new Vector2(-0.24f, 9.3f),
                new Vector2(-0.34f, 9.7f),
                new Vector2(-1.5f, 10.6f),
                new Vector2(-3f, 12.9f),
                new Vector2(-4.6f, 13.5f),
                new Vector2(-4.2f, 12.7f),
                new Vector2(-3.7f, 12.7f),
                new Vector2(-3.2f, 11.8f),
                new Vector2(-2.3f, 9.9f),
                new Vector2(-1.7f, 8.9f),
                new Vector2(-0.95f, 7.1f),
                new Vector2(-1.8f, 4.7f),
            };
            Helper.DrawCasteljauBezier(leftEar, true);

            List<Vector2> rightEar = new List<Vector2>
            {
                new Vector2(0.42f, 5f),
                new Vector2(0.5f, 5.6f),
                new Vector2(0.52f, 6f),
                new Vector2(0.52f, 7f),
                new Vector2(0.52f, 8.8f),
                new Vector2(0.24f, 9.3f),
                new Vector2(0.34f, 9.7f),
                new Vector2(1.5f, 10.6f),
                new Vector2(3f, 12.9f),
                new Vector2(4.6f, 13.5f),
                new Vector2(4.2f, 12.7f),
                new Vector2(3.7f, 12.7f),
                new Vector2(3.2f, 11.8f),
                new Vector2(2.3f, 9.9f),
                new Vector2(1.7f, 8.9f),
                new Vector2(0.95f, 7.1f),
                new Vector2(1.8f, 4.7f),
            };
            Helper.DrawCasteljauBezier(rightEar, true);
        }
    }
}
