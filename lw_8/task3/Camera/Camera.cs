using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;

namespace task3_elParabaloid
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _front = Vector3.UnitZ;
        private Vector3 _right = Vector3.UnitX;
        private Vector3 _up = Vector3.UnitY;

        private float _distance = 5.0f;
        private float _pitch;
        private float _yaw = -90.0f;
        private float _sensitivity = 0.2f;

        public Camera()
        {
            UpdateVectors();
        }

        public Matrix4 ViewMatrix => Matrix4.LookAt(_position, _position + _front, _up);

        private void UpdateVectors()
        {
            _front.X = (float)Math.Cos(MathHelper.DegreesToRadians(_pitch)) *
                       (float)Math.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(_pitch)) *
                       (float)Math.Sin(MathHelper.DegreesToRadians(_yaw));
            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));

            _position = -_front * _distance;
        }

        public void Rotate(float deltaX, float deltaY)
        {
            _yaw += deltaX * _sensitivity;
            _pitch -= deltaY * _sensitivity;
            _pitch = MathHelper.Clamp(_pitch, -89.0f, 89.0f);

            UpdateVectors();
        }
    }
}