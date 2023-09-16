using ConsoleAppNoteServer.Model;

internal class Program
{
    private static void Main(string[] args)
    {
        AsyncServer server = new AsyncServer("127.0.0.1", 1024);
        server.StartServer();
        Console.Read();
    }
}