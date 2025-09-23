using System;
using Tao.Sdl;


public static class CollisionHelper
{
    // ✅ Verifica si un punto (px, py) está dentro de un rectángulo rotado con offsets opcionales
    public static bool PointInRotatedRect(
        float px, float py,    // Punto a chequear
        float cx, float cy,    // Centro del rectángulo (normalmente centro del sprite)
        float width, float height,
        float angleDegrees,
        float offsetX = 0f, float offsetY = 0f)
    {
        float rad = angleDegrees * (float)Math.PI / 180f;
        float cos = (float)Math.Cos(rad);
        float sin = (float)Math.Sin(rad);

        // Offset desde el centro (por default, 0,0 es el centro)
        float originX = cx + offsetX * cos - offsetY * sin;
        float originY = cy + offsetX * sin + offsetY * cos;

        // Llevar punto a espacio local del rectángulo
        float dx = px - originX;
        float dy = py - originY;

        // Rotar punto al sistema local
        float localX = dx * cos + dy * sin;
        float localY = -dx * sin + dy * cos;

        return (localX >= 0 && localX <= width &&
                localY >= -height / 2 && localY <= height / 2);
    }

    // ✅ Dibuja un rectángulo rotado con offsets y flecha de dirección (para debug)
    public static void DrawRotatedRect(
        float cx, float cy,
        float width, float height,
        float angleDegrees,
        float cameraX,
        float offsetX = 0f, float offsetY = 0f)
    {
        float angleRad = angleDegrees * (float)Math.PI / 180f;
        float cos = (float)Math.Cos(angleRad);
        float sin = (float)Math.Sin(angleRad);

        // Offset para alinear el rectángulo con el sprite
        float originX = cx + offsetX * cos - offsetY * sin;
        float originY = cy + offsetX * sin + offsetY * cos;

        // Definir los vértices locales (arranca en esquina superior izquierda)
        float[] localX = { 0, width, width, 0 };
        float[] localY = { -height / 2, -height / 2, height / 2, height / 2 };

        float[] worldX = new float[4];
        float[] worldY = new float[4];

        for (int i = 0; i < 4; i++)
        {
            worldX[i] = originX + (localX[i] * cos - localY[i] * sin);
            worldY[i] = originY + (localX[i] * sin + localY[i] * cos);
        }

        // 🔴 Dibujar rectángulo
        for (int i = 0; i < 4; i++)
        {
            int next = (i + 1) % 4;
            Engine.DrawLine((int)(worldX[i] - cameraX), (int)worldY[i], (int)(worldX[next] - cameraX), (int)worldY[next], 255, 0, 0);
        }

        // ✅ Flecha de dirección principal (proa)
        float tipX = originX + width * cos;
        float tipY = originY + width * sin;
        Engine.DrawLine((int)(originX - cameraX), (int)originY, (int)(tipX - cameraX), (int)tipY, 0, 255, 0);

        // Flechita decorativa (punta de la flecha)
        float arrowLength = 15f;
        float arrowAngle = 30f * (float)Math.PI / 180f;
        float dx = tipX - originX;
        float dy = tipY - originY;
        float len = (float)Math.Sqrt(dx * dx + dy * dy);
        dx /= len; dy /= len;
        float baseAngle = (float)Math.Atan2(dy, dx);

        float leftX = tipX - arrowLength * (float)Math.Cos(baseAngle - arrowAngle);
        float leftY = tipY - arrowLength * (float)Math.Sin(baseAngle - arrowAngle);
        float rightX = tipX - arrowLength * (float)Math.Cos(baseAngle + arrowAngle);
        float rightY = tipY - arrowLength * (float)Math.Sin(baseAngle + arrowAngle);

        Engine.DrawLine((int)(tipX - cameraX), (int)tipY, (int)(leftX - cameraX), (int)leftY, 0, 255, 0);
        Engine.DrawLine((int)(tipX - cameraX), (int)tipY, (int)(rightX - cameraX), (int)rightY, 0, 255, 0);
    }

    public static void DrawRotatedDebugRect(
            float cx, float cy, float width, float height, float angleDeg,
            byte r, byte g, byte b)
    {
        
        //Engine.DebugLog($"[DBG Rect] c=({cx},{cy}) size=({width}x{height}) angle={angleDeg} color=({r},{g},{b})");
        //EngineGDI.Engine.DebugLog($"[DBG Rect] c=({cx},{cy}) size=({width}x{height}) angle={angleDeg} color=({r},{g},{b})");
    }
}
