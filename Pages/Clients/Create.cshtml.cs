using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ASP.NET_Core_Web_Application_With_SQL_Server_Database_Connection.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientsInfo = new ClientInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {

            clientsInfo.name = Request.Form["name"];
            clientsInfo.email = Request.Form["email"];
            clientsInfo.phone = Request.Form["phone"];
            clientsInfo.address = Request.Form["address"];

            if (clientsInfo.name.Length == 0 || clientsInfo.email.Length == 0 || clientsInfo.phone.Length == 0 || clientsInfo.address.Length == 0)
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients" + "(name,email,phone,address) values" + "(@name,@email,@phone,@address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientsInfo.name);
                        command.Parameters.AddWithValue("@email", clientsInfo.email);
                        command.Parameters.AddWithValue("@phone", clientsInfo.phone);
                        command.Parameters.AddWithValue("@address", clientsInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            clientsInfo.name = ""; clientsInfo.email = ""; clientsInfo.phone = ""; clientsInfo.address = "";

            SuccessMessage = "New Client Added Correctly.";

            Response.Redirect("/Clients/Index");
        }
    }
}
