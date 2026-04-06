namespace smartStock.Domain.Enums;

public enum EstadoUsuario
{
    Suspendido = 0,
    Activo     = 1,
    Conectado  = 2   // reservado para SignalR (presencia en tiempo real)
}
