using System;

namespace MyGame
{
    public class Enemy : GameObject, IDaPuntaje, IColisionable
    {
        private static Image enemyImage = Engine.LoadImage("assets/barcoChico.png");

        public bool IsDestroyed { get; private set; } = false;
        public bool TriggerExplosion { get; private set; } = false;

        private float width = 283f;
        private float height = 99f;
        private float speed = 1.5f;

       
        private float colliderOffsetForward = -141f;  
      
        private float colliderOffsetSide = 0f;

        public Enemy(float x, float y) : base(x, y)
        {
            Angle = 180f; 
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

            
            CollisionHelper.DrawRotatedRect(
                X, Y, width, height, Angle, cameraX,
                colliderOffsetForward, colliderOffsetSide
            );
        }

        public bool CollidesWithPlayer(Player player)
        {
            return CollisionHelper.PointInRotatedRect(
                player.X, player.Y,
                X, Y,
                width, height,
                Angle,
                colliderOffsetForward, colliderOffsetSide
            );
        }

        public bool CollidesWithBullet(Bullet bullet)
        {
            bool impact = CollisionHelper.PointInRotatedRect(
                bullet.X, bullet.Y,
                X, Y,
                width, height,
                Angle,
                colliderOffsetForward, colliderOffsetSide
            );

            if (impact)
            {
                IsDestroyed = true;
                TriggerExplosion = true;
            }

            return impact; 
        }

        public int OtorgarPuntos() => 100;
    }
}


