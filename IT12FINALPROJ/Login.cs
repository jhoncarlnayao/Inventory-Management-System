using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
namespace IT12FINALPROJ
{
    public partial class Login : MaterialForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = usernameTextbox.Text;
            string password = passwordTextbox.Text;


            string connectionString = "server=localhost;database=it12proj;user=root;password=;";

            try
            {

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();


                    string query = "SELECT role FROM Accounts WHERE username=@username AND password=@password";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);


                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string role = result.ToString();

                        if (role == "Admin")
                        {
                            MessageBox.Show("Admin logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //  DashboardForm dashboard = new DashboardForm();
                            //  dashboard.Show();
                            this.Hide();
                        }
                        else if (role == "Staff")
                        {
                            MessageBox.Show("Staff logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            StaffDashboard staffDashboard = new StaffDashboard();
                            staffDashboard.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password.", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}