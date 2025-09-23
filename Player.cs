using System;

namespace MyGame
{
    public class Player : GameObject
    {
        private Image playerImage = Engine.LoadImage("assets/BarcoPlayer.png");

        public float AngleTimon { get; private set; }
        private float headingAngle;
        public float Heading => headingAngle;

        private float speed = 2.0f;

        public Player(float x, float y) : base(x, y)
        {
            AngleTimon = 0;
            headingAngle = 0;
        }

        public event Action<string> OnCollision;

        public void NotificarColision(string conQue)
        {
            OnCollision?.Invoke(conQue);
        }

        public override void Update()
        {
            int mouseX, mouseY;
            if (Engine.MouseClick(Engine.MOUSE_WHEELUP, out mouseX, out mouseY))
                AngleTimon += 4f;
            if (Engine.MouseClick(Engine.MOUSE_WHEELDOWN, out mouseX, out mouseY))
                AngleTimon -= 4f;

            headingAngle = LerpAngle(headingAngle, AngleTimon, 0.05f);
            Angle = headingAngle;

            float rad = headingAngle * (float)Math.PI / 180f;
            Transform.Position.X += (float)Math.Cos(rad) * speed;
            Transform.Position.Y -= (float)Math.Sin(rad) * speed;

            if (Transform.Position.X < 0) Transform.Position.X = 0;
            if (Transform.Position.Y < 0) Transform.Position.Y = 0;
            if (Transform.Position.Y > 768) Transform.Position.Y = 768;
        }

        public override void Render(float cameraX)
        {
            Engine.DrawRotated(playerImage, X - cameraX, Y, headingAngle);
        }

        private float LerpAngle(float from, float to, float t)
        {
            float delta = (((to - from) % 360f) + 540f) % 360f - 180f;
            return from + delta * t;
        }

        // NUEVO: coordenadas de la popa del barco (atrás)
        public (float x, float y) Popa
        {
            get
            {
                float rad = headingAngle * (float)Math.PI / 180f;
                float offset = 40f; // Ajustá si el sprite tiene otro largo
                float popaX = X - (float)Math.Cos(rad) * offset;
                float popaY = Y - (float)Math.Sin(rad) * offset;
                return (popaX, popaY);
            }
        }
    }
}
