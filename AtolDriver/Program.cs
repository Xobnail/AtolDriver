namespace AtolDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 4;
            int baudRate = 115200;

            string operatorName = "Xobnail";
            string operatorInn = null;

            Interface printer = new Interface(port, baudRate);
            printer.OpenConnection();
            printer.SetOperator(operatorName, operatorInn);
            printer.OpenShift();
            printer.OpenReceipt();
            printer.AddPosition("Шуба", 1500, 1, "piece", "commodity");
            //printer.PrintStatus();
            printer.AddPosition("Банан", 60, 4, "kilogram", "commodity");
            //printer.PrintStatus();
            printer.Pay("cash", 2000);
            printer.CloseReceipt();
            //printer.PrintStatus();
            //printer.CloseShift();
        }
    }
}
