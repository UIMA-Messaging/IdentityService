namespace ContactService.Contracts
{
    public class PaginatedResults
    {
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }
        public object[] Results { get; set; }
    }
}
