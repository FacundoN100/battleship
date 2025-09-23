using System;

namespace MyGame
{
    public class Trap : GameObject, IColisionable
    {
        private Image trampaImage = Engine.LoadImage("assets/trap.png");

        public Trap(float x, float y) : base(x, y) { }

        public override void Update() { }

        public override void Render(float cameraX)
        {
            Engine.Draw(trampaImage, X - cameraX, Y);
        }

        public bool CollidesWithPlayer(Player player)
        {
            return CollisionHelper.PointInRotatedRect(X, Y, player.X, player.Y, 180f, 100f, player.Heading);
        }

        public bool CollidesWithBullet(Bullet bullet) => false;
    }
}