using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;
namespace IT12FINALPROJ
{
    public partial class Login : MaterialForm
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Define the border radius
            int borderRadius = 30;  // Change this value for more/less rounding

            // Set the form's region (rounded corners)
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90); // Top-left corner
                path.AddArc(this.Width - borderRadius - 1, 0, borderRadius, borderRadius, 270, 90); // Top-right corner
                path.AddArc(this.Width - borderRadius - 1, this.Height - borderRadius - 1, borderRadius, borderRadius, 0, 90); // Bottom-right corner
                path.AddArc(0, this.Height - borderRadius - 1, borderRadius, borderRadius, 90, 90); // Bottom-left corner
                path.CloseAllFigures();

                this.Region = new Region(path);
            }
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

                            StaffDashboard staffDashboard = new StaffDashboard();
                            staffDashboard.Show();
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