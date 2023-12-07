using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ASP.NET_Core_Web_Application_With_SQL_Server_Database_Connection.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientsInfo = new ClientInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myStore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients where id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientsInfo.id = "" + reader.GetInt32(0);
                                clientsInfo.name = reader.GetString(1);
                                clientsInfo.email = reader.GetString(2);
                                clientsInfo.phone = reader.GetString(3);
                                clientsInfo.address = reader.GetString(4);


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            clientsInfo.id = Request.Form["id"];
            clientsInfo.name = Request.Form["name"];
            clientsInfo.email = Request.Form["email"];
            clientsInfo.phone = Request.Form["phone"];
            clientsInfo.address = Request.Form["address"];

            if (clientsInfo.id.Length == 0 || clientsInfo.name.Length == 0 || clientsInfo.email.Length == 0 || clientsInfo.phone.Length == 0 || clientsInfo.address.Length == 0)
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
                    string sql = "UPDATE clients " + " SET name=@name,email=@email,phone=@phone,address=@address " + " WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientsInfo.name);
                        command.Parameters.AddWithValue("@email", clientsInfo.email);
                        command.Parameters.AddWithValue("@phone", clientsInfo.phone);
                        command.Parameters.AddWithValue("@address", clientsInfo.address);
                        command.Parameters.AddWithValue("@id", clientsInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            SuccessMessage = "Client Details Updated Successfully.";

            Response.Redirect("/Clients/Index");
        }
    }
}
