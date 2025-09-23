using System.Collections.Generic;

namespace MyGame
{
    public class BulletFactory
    {
        private ObjectPool<Bullet> pool;
        private List<Bullet> activeList;

        public BulletFactory(ObjectPool<Bullet> pool, List<Bullet> activeList)
        {
            this.pool = pool;
            this.activeList = activeList;
        }

        public void CreateBullet(float x, float y, float angle)
        {
            Bullet bullet = pool.Get();
            bullet.Initialize(x, y, angle);
            activeList.Add(bullet);
        }

        public void CleanInactive()
        {
            activeList.RemoveAll(b => !b.IsActive);
        }
    }
}
