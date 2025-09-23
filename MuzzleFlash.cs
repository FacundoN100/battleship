using System;

namespace MyGame
{
    public class MuzzleFlash : IPoolable
    {
        private static Image flashImage = Engine.LoadImage("assets/flash.png");

        private float posX;
        private float posY;
        private float angle;
        private float timer;

        public bool IsActive { get; private set; }

        public MuzzleFlash() { }

        public void Initialize(float x, float y, float angle)
        {
            this.posX = x;
            this.posY = y;
            this.angle = angle;
            this.timer = 10f;
            this.IsActive = true;
        }

        public void Update()
        {
            if (!IsActive) return;

            timer -= 1f;
            if (timer <= 0f)
                OnDeactivate();
        }

        public void Render(float cameraX)
        {
            if (!IsActive) return;

            Engine.DrawRotatedScaled(flashImage, posX - cameraX, posY, angle, 0.3f);
        }

        public bool IsExpired() => !IsActive;

        public void OnActivate() => IsActive = true;
        public void OnDeactivate() => IsActive = false;
    }
}
