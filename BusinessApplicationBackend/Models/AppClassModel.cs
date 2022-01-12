namespace BusinessApplicationBackend.Models
{
    public class AppClass
    {
        public string? BusinessName { get; set; }
        public string? Email { get; set; }
        public string? Industry { get; set; }
        public int AnnualSales { get; set; }
        public int AnnualPayroll { get; set; }
        public int NumberOfEmployees { get; set; }
        public string? ZipCode { get; set; }
    }

    public class AppResponse
    {
        public AppResponse()
        {
            Apps = new List<AppClass>();
        }

        public IList<AppClass> Apps { get; set; }
    }
}
