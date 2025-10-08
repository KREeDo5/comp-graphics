using OpenTK;
using OpenTK.Input;

namespace Task2
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _front;
        private Vector3 _right;
        private Vector3 _up;

        private float _dist = 5.0f;
        private float _pitch = 0.0f;
        private float _yaw = -90.0f;

        private float _sensitivity = 0.2f;

        public Camera()
        {
            UpdateVectors();
        }

        public Matrix4 ViewMatrix => Matrix4.LookAt(_position, _position + _front, _up);

        private void UpdateVectors()
        {
            _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));
            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));

            _position = -_front * _dist;
        }

        public void Rotate(float offsetX, float offsetY)
        {
            _yaw += offsetX * _sensitivity;
            _pitch -= offsetY * _sensitivity;
            _pitch = MathHelper.Clamp(_pitch, -89.0f, 89.0f);

            UpdateVectors();
        }
    }
}
