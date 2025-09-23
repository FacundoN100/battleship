using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace MyGame
{
    class Program
    {
        static private Player player1;
        static private Font menuFont;
        static private Image timonImage;
        static private Image winBackground;
        static private Image defeatBackground;
        static private Image fondoMar;
        static private Image muelleImage;
         

        static private List<Bullet> bullets = new List<Bullet>();
        static private List<MuzzleFlash> flashes = new List<MuzzleFlash>();
        //static private List<IColisionable> colisionables = new List<IColisionable>();
        private static readonly List<IColisionable> colisionables = new List<IColisionable>();
        static private List<Enemy> enemies = new List<Enemy>();
        static private List<(float x, float y)> enemigosPendientes = new List<(float x, float y)>();
        static private List<(float x, float y)> zonasValidas = new List<(float x, float y)>();

        static private float cameraX = 0;
        static private float leftShootCooldown = 0f;
        static private float rightShootCooldown = 0f;

        static private ObjectPool<Bullet> bulletPool = new ObjectPool<Bullet>();
        static private ObjectPool<MuzzleFlash> flashPool = new ObjectPool<MuzzleFlash>();

        static private BulletFactory bulletFactory;
        static private MuzzleFlashFactory flashFactory;
        static private EnemyFactory enemyFactory;
        static private ShotController shotController;

        static void Main(string[] args)
        {
            Engine.Initialize();
            menuFont = Engine.LoadFont("assets/ManicSea_19.ttf", 24);
            timonImage = Engine.LoadImage("assets/timon.png");
            fondoMar = Engine.LoadImage("assets/fondoMar.png");
            muelleImage = Engine.LoadImage("assets/muelle.png");
            winBackground = Engine.LoadImage("assets/win.png");
            defeatBackground = Engine.LoadImage("assets/defeat.png");
            //trampaImage = Engine.LoadImage("assets/trap.png"); 

            InitializeLevel();

            bulletFactory = new BulletFactory(bulletPool, bullets);
            flashFactory = new MuzzleFlashFactory(flashPool, flashes);
            enemyFactory = new EnemyFactory(enemies, colisionables);
            shotController = new ShotController(player1, bulletFactory, flashFactory);

            while (true)
            {
                Update();
                Render();
            }
        }

        static public void InitializeLevel()
        {
            player1 = new Player(50, 400);
            bullets.Clear();
            flashes.Clear();
            colisionables.Clear();
            enemies.Clear();
            enemigosPendientes.Clear();

            bulletFactory = new BulletFactory(bulletPool, bullets);
            flashFactory = new MuzzleFlashFactory(flashPool, flashes);
            enemyFactory = new EnemyFactory(enemies, colisionables  );
            shotController = new ShotController(player1, bulletFactory, flashFactory);

            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                var trap = new Trap(rnd.Next(2000, 8000), rnd.Next(0, 768));
                colisionables.Add(trap);
            }

            for (int i = 0; i < 20; i++)
                enemigosPendientes.Add((rnd.Next(2500, 8000), rnd.Next(100, 600)));

            zonasValidas = new List<(float x, float y)>
            {
                (7930, 80), (7930, 180), (7930, 280), (7930, 380)
            };

            GameManager.Instance.Score = 0;
        }
        
    
        public static class Maths
        {
            public static int Clamp(int v, int lo, int hi) => v < lo ? lo : (v > hi ? hi : v);
            public static float Clamp(float v, float lo, float hi) => v < lo ? lo : (v > hi ? hi : v);
        }
    

    static void Update()
        {
            int mouseX, mouseY;
            bool mouseClick = Engine.MouseClick(Engine.MOUSE_LEFT, out mouseX, out mouseY);

            bulletFactory.CleanInactive();

            switch (GameManager.Instance.CurrentState)
            {
                case GameManager.GameState.Menu:
                    string[] opciones = { "Play", "Options", "Credits", "Exit" };
                    for (int i = 0; i < opciones.Length; i++)
                    {
                        int x = 460;
                        int y = 250 + i * 50;
                        int width = 200;
                        int height = 40;

                        if (mouseX >= x && mouseX <= x + width && mouseY >= y && mouseY <= y + height && mouseClick)
                        {
                            switch (i)
                            {
                                case 0: GameManager.Instance.CurrentState = GameManager.GameState.Playing; break;
                                case 1: break;
                                case 2: break;
                                case 3: Environment.Exit(0); break;
                            }
                        }
                    }
                    break;

                case GameManager.GameState.Playing:
                    player1.Update();

                    cameraX = Maths.Clamp(player1.X - 300, 0, 8000 - 1024);

                    leftShootCooldown -= 0.2f;
                    rightShootCooldown -= 0.2f;

                    if (Engine.MouseClick(Engine.MOUSE_LEFT, out mouseX, out mouseY) && leftShootCooldown <= 0)
                    {
                        shotController.ShootFromSide(-90f);
                        leftShootCooldown = 15f;
                    }
                    if (Engine.MouseClick(Engine.MOUSE_RIGHT, out mouseX, out mouseY) && rightShootCooldown <= 0)
                    {
                        shotController.ShootFromSide(90f);
                        rightShootCooldown = 15f;
                    }

                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        bullets[i].Update();
                        if (bullets[i].IsOffScreen())
                            bullets.RemoveAt(i);
                    }

                    for (int i = flashes.Count - 1; i >= 0; i--)
                    {
                        flashes[i].Update();
                        if (flashes[i].IsExpired())
                            flashes.RemoveAt(i);
                    }

                    // Colisiones con entidades
                    foreach (var col in colisionables)
                    {
                        if (col.CheckCollision(player1))
                            GameManager.Instance.CurrentState = GameManager.GameState.Defeat;
                    }

                    foreach (var zona in zonasValidas)
                    {
                        if (CollisionHelper.PointInRotatedRect(zona.x, zona.y, player1.X, player1.Y, 180f, 100f, player1.Heading))
                            GameManager.Instance.CurrentState = GameManager.GameState.Victory;
                    }

                    for (int i = enemigosPendientes.Count - 1; i >= 0; i--)
                    {
                        float distancia = enemigosPendientes[i].x - player1.X;
                        if (distancia <= 1024 && distancia > 0)
                        {
                            Enemy e = enemyFactory.Create(enemigosPendientes[i].x, enemigosPendientes[i].y);
                            enemies.Add(e);
                            colisionables.Add(e);
                            enemigosPendientes.RemoveAt(i);
                        }
                    }

                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        Enemy e = enemies[i];
                        e.Update();

                        if (e.IsDestroyed)
                        {
                            if (e.TriggerExplosion)
                                flashFactory.CreateFlash(e.X, e.Y, 0f);

                            if (e is IDaPuntaje daPuntos)
                                GameManager.Instance.Score += daPuntos.OtorgarPuntos();

                            enemies.RemoveAt(i);
                            colisionables.Remove(e);
                            continue;
                        }

                        if (e.CollidesWithPlayer(player1))
                            GameManager.Instance.CurrentState = GameManager.GameState.Defeat;

                        for (int j = bullets.Count - 1; j >= 0; j--)
                        {
                            if (e.CollidesWithBullet(bullets[j]))
                            {
                                bullets.RemoveAt(j);
                                break;
                            }
                        }
                    }

                    break;

                case GameManager.GameState.Victory:
                case GameManager.GameState.Defeat:
                    if (mouseClick)
                    {
                        GameManager.Instance.ResetGame();
                        InitializeLevel();
                        GameManager.Instance.CurrentState = GameManager.GameState.Playing;
                    }
                    break;
            }
        }


        static void Render()
        {
            Engine.Clear();
            Engine.GetMouseState(out int mouseX, out int mouseY);

            switch (GameManager.Instance.CurrentState)
            {
                case GameManager.GameState.Menu:
                    Engine.DrawText("Battle Ship", 380, 150, 255, 255, 255, menuFont);
                    string[] opciones = { "Play", "Options", "Credits", "Exit" };
                    for (int i = 0; i < opciones.Length; i++)
                    {
                        int x = 460, y = 250 + i * 50, width = 200, height = 40;
                        bool hovering = mouseX >= x && mouseX <= x + width && mouseY >= y && mouseY <= y + height;
                        byte c = hovering ? (byte)200 : (byte)255;
                        Engine.DrawText(opciones[i], x, y, c, c, c, menuFont);
                    }
                    break;

                case GameManager.GameState.Playing:
                    DrawBackground();
                    Engine.Draw(muelleImage, 7800 - cameraX, 0);
                    player1.Render(cameraX);

                    foreach (var e in enemies)
                        e.Render(cameraX);
                    foreach (var obj in colisionables)
                        ((GameObject)obj).Render(cameraX);
                    foreach (var obj in colisionables)
                    obj.Render(cameraX);
                    foreach (var b in bullets)
                        b.Render(cameraX);
                    foreach (var f in flashes)
                        f.Render(cameraX);

                    Engine.DrawRotatedScaled(timonImage, 512, 770, player1.Angle, 0.25f);
                    Engine.DrawText("Points: " + GameManager.Instance.Score, 40, 30, 255, 255, 0, menuFont);

                    // DEBUG: rectángulo del jugador
                    CollisionHelper.DrawRotatedDebugRect(player1.X, player1.Y, 180f, 100f, player1.Heading, 255, 255, 0);
                    break;

                case GameManager.GameState.Victory:
                    Engine.Draw(winBackground, 0, 0);
                    Engine.DrawText("Victory!", 400, 300, 0, 255, 0, menuFont);
                    break;

                case GameManager.GameState.Defeat:
                    Engine.Draw(defeatBackground, 0, 0);
                    Engine.DrawText("Defeat!", 400, 300, 255, 0, 0, menuFont);
                    break;
            }

            Engine.Show();
        }

        static void DrawBackground()
        {
            float scale = 0.9f;
            int fondoOriginalWidth = 1536;
            int fondoScaledWidth = (int)(fondoOriginalWidth * scale);
            int offset = (int)((cameraX * 0.8f) % fondoScaledWidth);

            Engine.DrawScaled(fondoMar, -offset, 0, scale, scale);
            Engine.DrawScaled(fondoMar, -offset + fondoScaledWidth, 0, scale, scale);
        }
    }
}
