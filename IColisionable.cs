using System;

namespace MyGame
{
    public interface IColisionable
    {
        float X { get; }
        float Y { get; }
        bool CollidesWithPlayer(Player player);
        bool CollidesWithBullet(Bullet bullet);
    }
}