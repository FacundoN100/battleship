using System;

namespace MyGame
{
    public static class IColisionableExtensions
    {
      
        public static bool CheckCollision(this IColisionable c, Player player)
            => c.CollidesWithPlayer(player);

     
        public static bool CheckCollision(this IColisionable c, Bullet bullet)
            => c.CollidesWithBullet(bullet);

        
        public static void Render(this IColisionable c, float cameraX)
        {
            if (c is GameObject go)
                go.Render(cameraX);
        }
    }
}