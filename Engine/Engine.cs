using System;
using System.Threading;
using System.Drawing;
using Tao.Sdl;
using System.Collections.Generic;

class Engine
{
    static IntPtr screen;
    static int ancho, alto;

    public static void Initialize()
    {
        Initialize(1024, 768);
    }

    public static void Initialize(int an, int al)
    {
        ancho = an;
        alto = al;
        int colores = 24;
        int flags = (Sdl.SDL_HWSURFACE | Sdl.SDL_DOUBLEBUF | Sdl.SDL_ANYFORMAT);

        Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
        screen = Sdl.SDL_SetVideoMode(ancho, alto, colores, flags);

        Sdl.SDL_Rect rect = new Sdl.SDL_Rect(0, 0, (short)ancho, (short)alto);
        Sdl.SDL_SetClipRect(screen, ref rect);

        SdlTtf.TTF_Init();
    }

    public static void Debug(string text)
    {
        Console.WriteLine(text);
    }

    public static void Clear()
    {
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, (short)ancho, (short)alto);
        Sdl.SDL_FillRect(screen, ref origin, 0);
    }

    public static void Show()
    {
        Sdl.SDL_Flip(screen);
    }

    public static Image LoadImage(string imagePath)
    {
        return new Image(imagePath);
    }

    public static void Draw(Image image, float x, float y)
    {
        Draw(image.Pointer, x, y);
    }

    public static void Draw(IntPtr imagen, float x, float y)
    {
        Sdl.SDL_Surface surface = (Sdl.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(
            imagen, typeof(Sdl.SDL_Surface));

        short width = (short)surface.w;
        short height = (short)surface.h;

        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, width, height);

        Sdl.SDL_BlitSurface(imagen, ref origin, screen, ref dest);
    }

    public static void DrawLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b)
    {
        int color = (r << 24) | (g << 16) | (b << 8) | 255; // RGBA como int
        SdlGfx.aalineColor(
            screen,
            (short)x1, (short)y1,
            (short)x2, (short)y2,
            color
        );
    }

    public static void Draw(Image image, float x, float y, float width, float height)
    {
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, (short)width, (short)height);
        Sdl.SDL_BlitSurface(image.Pointer, ref origin, screen, ref dest);
    }

    public static void DrawRotated(Image image, float x, float y, float angle)
    {
        IntPtr rotated = SdlGfx.rotozoomSurface(image.Pointer, angle, 1.0, 1);

        if (rotated == IntPtr.Zero)
        {
            Console.WriteLine("No se pudo rotar la imagen");
            return;
        }

        Sdl.SDL_Surface surface = (Sdl.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(
            rotated, typeof(Sdl.SDL_Surface));

        short width = (short)surface.w;
        short height = (short)surface.h;

        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)(x - width / 2), (short)(y - height / 2), width, height);
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);

        Sdl.SDL_BlitSurface(rotated, ref origin, screen, ref dest);
        Sdl.SDL_FreeSurface(rotated);
    }

    public static void DrawRotatedScaled(Image image, float x, float y, float angle, float scale)
    {
        IntPtr rotated = SdlGfx.rotozoomSurface(image.Pointer, angle, scale, 1);

        if (rotated == IntPtr.Zero)
        {
            Console.WriteLine("No se pudo rotar y escalar la imagen");
            return;
        }

        Sdl.SDL_Surface surface = (Sdl.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(
            rotated, typeof(Sdl.SDL_Surface));

        short width = (short)surface.w;
        short height = (short)surface.h;

        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)(x - width / 2), (short)(y - height / 2), width, height);
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);

        Sdl.SDL_BlitSurface(rotated, ref origin, screen, ref dest);
        Sdl.SDL_FreeSurface(rotated);
    }

    // 🔥 NUEVO método para escalar una imagen sin rotarla
    public static void DrawScaled(Image image, float x, float y, float scaleX, float scaleY)
    {
        Sdl.SDL_Surface surface = (Sdl.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(
            image.Pointer, typeof(Sdl.SDL_Surface));

        short originalWidth = (short)surface.w;
        short originalHeight = (short)surface.h;

        short scaledWidth = (short)(originalWidth * scaleX);
        short scaledHeight = (short)(originalHeight * scaleY);

        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, originalWidth, originalHeight);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, scaledWidth, scaledHeight);

        Sdl.SDL_BlitSurface(image.Pointer, ref origin, screen, ref dest);
    }

    public static Font LoadFont(string path, short size)
    {
        return new Font(path, size);
    }

    public static void DrawText(string text, int x, int y, byte r, byte g, byte b, Font font)
    {
        DrawText(text, x, y, r, g, b, font.ReadPointer());
    }

    public static void DrawText(string text, int x, int y, byte r, byte g, byte b, IntPtr font)
    {
        Sdl.SDL_Color color = new Sdl.SDL_Color(r, g, b);
        IntPtr textSurface = SdlTtf.TTF_RenderText_Solid(font, text, color);

        if (textSurface == IntPtr.Zero)
        {
            Environment.Exit(5);
        }

        Sdl.SDL_Surface surface = (Sdl.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(
            textSurface, typeof(Sdl.SDL_Surface));

        short width = (short)surface.w;
        short height = (short)surface.h;

        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, width, height);

        Sdl.SDL_BlitSurface(textSurface, ref origin, screen, ref dest);
        Sdl.SDL_FreeSurface(textSurface);
    }

    public static IntPtr LoadFont2(string file, int size)
    {
        IntPtr font = SdlTtf.TTF_OpenFont(file, size);
        if (font == IntPtr.Zero)
        {
            Console.WriteLine("Fuente inexistente: " + file);
            Environment.Exit(6);
        }
        return font;
    }

    public static bool GetKey(int c)
    {
        Sdl.SDL_PumpEvents();
        Sdl.SDL_Event e;
        Sdl.SDL_PollEvent(out e);
        int numkeys;
        byte[] keys = Sdl.SDL_GetKeyState(out numkeys);

        return keys[c] == 1;
    }

    private static List<Sdl.SDL_Event> eventQueue = new List<Sdl.SDL_Event>();

    public static bool MouseClick(int button, out int mouseX, out int mouseY)
    {
        bool click = false;
        mouseX = 0;
        mouseY = 0;

        Sdl.SDL_PumpEvents();
        Sdl.SDL_Event e;

        for (int i = 0; i < eventQueue.Count; i++)
        {
            e = eventQueue[i];
            if (e.type == Sdl.SDL_MOUSEBUTTONDOWN && e.button.button == button)
            {
                click = true;
                mouseX = e.button.x;
                mouseY = e.button.y;
                eventQueue.RemoveAt(i);
                return click;
            }
        }





        while (Sdl.SDL_PollEvent(out e) != 0)
        {
            if (e.type == Sdl.SDL_MOUSEBUTTONDOWN)
            {
                if (e.button.button == button)
                {
                    click = true;
                    mouseX = e.button.x;
                    mouseY = e.button.y;
                    return click;
                }
                else
                {
                    eventQueue.Add(e);
                }
            }
        }

        return click;
    }
        public static int GetMouseState(out int mouseX, out int mouseY)
    {
        Sdl.SDL_PumpEvents();
        return Sdl.SDL_GetMouseState(out mouseX, out mouseY);
    }

    public static void ErrorFatal(string texto)
    {
        Console.WriteLine(texto);
        Environment.Exit(1);
    }

    // Definiciones de teclas
    public static int KEY_ESC = Sdl.SDLK_ESCAPE;
    public static int KEY_ESP = Sdl.SDLK_SPACE;
    public static int KEY_A = Sdl.SDLK_a;
    public static int KEY_D = Sdl.SDLK_d;
    public static int KEY_W = Sdl.SDLK_w;
    public static int KEY_S = Sdl.SDLK_s;
    public static int KEY_Q = Sdl.SDLK_q;
    public static int KEY_E = Sdl.SDLK_e;
    public static int KEY_UP = Sdl.SDLK_UP;
    public static int KEY_DOWN = Sdl.SDLK_DOWN;
    public static int KEY_LEFT = Sdl.SDLK_LEFT;
    public static int KEY_RIGHT = Sdl.SDLK_RIGHT;

    public static int MOUSE_LEFT = Sdl.SDL_BUTTON_LEFT;
    public static int MOUSE_RIGHT = Sdl.SDL_BUTTON_RIGHT;
    public static int MOUSE_MIDDLE = Sdl.SDL_BUTTON_MIDDLE;
    public static int MOUSE_WHEELUP = Sdl.SDL_BUTTON_WHEELUP;
    public static int MOUSE_WHEELDOWN = Sdl.SDL_BUTTON_WHEELDOWN;
}
