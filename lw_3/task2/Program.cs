namespace task2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Window window = new Window(500, 500, "lab3 task1"))
            {
                window.Run(60.0);
            }
        }
    }
}
