using System;

namespace MyGame
{
    public abstract class GameObject
    {
        public Transform Transform { get; protected set; }
        public float Angle { get; protected set; }

        public float X => Transform.Position.X;
        public float Y => Transform.Position.Y;

        public GameObject(float x, float y, float angle = 0f)
        {
            Transform = new Transform(x, y);
            Angle = angle;
        }

        public abstract void Update();
        public abstract void Render(float cameraX);
    }

    public class Vector2
    {
        public float X, Y;
        public Vector2(float x = 0, float y = 0) { X = x; Y = y; }
    }

    public class Transform
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public Transform(float x, float y, float rotation = 0, float scale = 1f)
        {
            Position = new Vector2(x, y);
            Rotation = rotation;
            Scale = scale;
        }
    }
}
