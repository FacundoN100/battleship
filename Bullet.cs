using System;

namespace MyGame
{
    public class Bullet : IPoolable, IDebugDrawable
    {
        private static Image bulletImage = Engine.LoadImage("assets/bullet.png");

        public float X => Transform.Pos.X;
        public float Y => Transform.Pos.Y;

        private float angulo;
        private float velocidad = 2.5f;
        private float distanciaRecorrida = 0f;
        private float distanciaMax = 250f;
        private float escala = 1f;

        public bool IsActive { get; private set; }

        public BulletTransform Transform { get; private set; } = new BulletTransform(0, 0);

        public Bullet() { }

        public void Initialize(float x, float y, float anguloDisparo)
        {
            Transform.Pos.X = x;
            Transform.Pos.Y = y;
            angulo = anguloDisparo;
            distanciaRecorrida = 0;
            IsActive = true;
        }

        public void Update()
        {
            if (!IsActive) return;

            float rad = angulo * (float)Math.PI / 180f;
            Transform.Pos.X += (float)Math.Cos(rad) * velocidad;
            Transform.Pos.Y += (float)Math.Sin(rad) * velocidad;

            distanciaRecorrida += velocidad;

            float mitad = distanciaMax / 2f;
            if (distanciaRecorrida <= mitad)
                escala = 1.2f + (distanciaRecorrida / mitad) * 0.6f;
            else
                escala = 1.8f - ((distanciaRecorrida - mitad) / mitad) * 0.6f;

            if (IsOffScreen())
                OnDeactivate();
        }

        public void Render(float camX)
        {
            if (!IsActive) return;

            Engine.DrawRotatedScaled(bulletImage, X - camX, Y, angulo, escala);
            DibujarDebug(camX);
        }

        public bool IsOffScreen() => distanciaRecorrida >= distanciaMax;

        public void OnActivate() => IsActive = true;
        public void OnDeactivate() => IsActive = false;

        public void DibujarDebug(float cameraX)
        {
            CollisionHelper.DrawRotatedRect(X, Y, 12f, 12f, angulo, cameraX);
        }

        public class BulletVector2
        {
            public float X { get; set; }
            public float Y { get; set; }
            public BulletVector2(float x = 0, float y = 0) { X = x; Y = y; }
        }

        public class BulletTransform
        {
            public BulletVector2 Pos { get; set; } = new BulletVector2();
            public BulletTransform(float x, float y) => Pos = new BulletVector2(x, y);
        }
    }
}
