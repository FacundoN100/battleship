using System.Collections.Generic;

namespace MyGame
{
    public class MuzzleFlashFactory
    {
        private ObjectPool<MuzzleFlash> pool;
        private List<MuzzleFlash> activeList;

        public MuzzleFlashFactory(ObjectPool<MuzzleFlash> pool, List<MuzzleFlash> activeList)
        {
            this.pool = pool;
            this.activeList = activeList;
        }

        public void CreateFlash(float x, float y, float angle)
        {
            MuzzleFlash flash = pool.Get();
            flash.Initialize(x, y, angle);
            activeList.Add(flash);
        }

        public void CleanInactive()
        {
            activeList.RemoveAll(f => !f.IsActive);
        }
    }
}
