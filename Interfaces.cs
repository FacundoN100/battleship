// Interfaces.cs

namespace MyGame
{
    // 🏆 Si un objeto da puntaje al destruirlo
    //public interface IDaPuntaje
    //{
    //    int OtorgarPuntos();
    //}

    // 💥 Si un objeto puede recibir daño (opcional, para el futuro)
    public interface IDañable
    {
        void RecibirDaño(int cantidad);
        bool EstaDestruido { get; }
    }

    // 🧩 Si se puede dibujar para debug visual de colisiones
    public interface IDebugDrawable
    {
        void DibujarDebug(float cameraX);
    }
}
