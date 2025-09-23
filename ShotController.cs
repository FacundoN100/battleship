using System;

namespace MyGame
{
    public class ShotController
    {
        private Player player;
        private BulletFactory bulletFactory;
        private MuzzleFlashFactory flashFactory;

        public ShotController(Player player, BulletFactory bulletFactory, MuzzleFlashFactory flashFactory)
        {
            this.player = player;
            this.bulletFactory = bulletFactory;
            this.flashFactory = flashFactory;
        }


        public void ShootFromSide(float sideOffsetAngle)
        {
            float headingRad = player.Heading * (float)Math.PI / 180f;

            // Dirección perpendicular
            float dirX = (float)Math.Cos(headingRad + sideOffsetAngle * (float)Math.PI / 180f);
            float dirY = (float)Math.Sin(headingRad + sideOffsetAngle * (float)Math.PI / 180f);

            float spawnX = player.X + dirX * 40f;
            float spawnY = player.Y + dirY * 40f;
            float bulletAngle = player.Heading + sideOffsetAngle;

            bulletFactory.CreateBullet(spawnX, spawnY, bulletAngle);
            flashFactory.CreateFlash(spawnX, spawnY, bulletAngle);
        }
    }
}
