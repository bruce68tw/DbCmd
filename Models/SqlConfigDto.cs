namespace DbCmd.Models
{
    /// <summary>
    /// get from appSettings.json MyConfig section
    /// </summary>
    public class SqlConfigDto
    {
        public string Db { get; set; } = "";
        public string Email { get; set; } = "";

    }
}
