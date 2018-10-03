namespace Ecard.Services
{
    public class NavAndPrintPrinterService : IPrinterService
    {
        public string Name
        {
            get { return "标题栏打印"; }
        }
    }
    public class NavPrinterService : IPrinterService
    {
        public string Name
        {
            get { return "页面跳转"; }
        }
    }
}