using System;

namespace MyGame
{
    public class Enemy : GameObject, IDaPuntaje, IColisionable
    {
        private static Image enemyImage = Engine.LoadImage("assets/barcoChico.png");

        public bool IsDestroyed { get; private set; } = false;
        public bool TriggerExplosion { get; private set; } = false;

        private float width = 100f;
        private float height = 60f;
        private float speed = 1.5f;

        public Enemy(float x, float y) : base(x, y)
        {
            Angle = 180f; // apunta hacia la izquierda
        }

        public override void Update()
        {
            Transform.Position.X -= speed;

            if (Transform.Position.X < -200)
                IsDestroyed = true;
        }

        public override void Render(float cameraX)
        {
            Engine.DrawRotated(enemyImage, X - cameraX, Y, Angle);
           // CollisionHelper.DrawRotatedRect(X, Y, width, height, Angle, cameraX); // debug colisión
        }

        public bool CollidesWithPlayer(Player player)
        {
            return CollisionHelper.PointInRotatedRect(
                player.X, player.Y,
                X, Y,
                width, height,
                Angle
            );
        }

        public bool CollidesWithBullet(Bullet bullet)
        {
            bool impact = CollisionHelper.PointInRotatedRect(
                bullet.X, bullet.Y,
                X, Y,
                width, height,
                Angle
            );

            if (impact)
            {
                IsDestroyed = true;
                TriggerExplosion = true;
            }

            return false;
        }

        public int OtorgarPuntos()
        {
            return 100;
        }
    }
}


