using Guna.UI2.WinForms;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IT12FINALPROJ
{
    public partial class StaffDashboard : MaterialForm
    {
        // Declare variables to hold product details
        private string productName;
        private string productDescription;
        private string unit;
        private decimal productPrice;
        private int quantity;
        private string brand;
        private Image productImage;

        public StaffDashboard()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            CountTotalPendingProducts();
            CountTotalAcceptProducts();
            CountTotalReturnProducts();
            LoadProductsOnOrder();


            guna2DataGridView3.Columns.Add("productName", "Product Name");
            guna2DataGridView3.Columns.Add("productDescription", "Product Description");
            guna2DataGridView3.Columns.Add("productPrice", "Product Price");
            guna2DataGridView3.Columns.Add("quantity", "Quantity");
            guna2DataGridView3.Columns.Add("totalPrice", "Total Price");
            guna2DataGridView3.Columns.Add("brand", "Brand");
            guna2DataGridView3.Columns.Add("category", "Category");

        }

        public void CountTotalPendingProducts()
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM pending_products";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            pendingnumber.Text = totalCount.ToString();
        }

        public void CountTotalAcceptProducts()
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM accepted_products";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            AcceptedProduct.Text = totalCount.ToString();
        }

        public void CountTotalReturnProducts()
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM pending_return";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            ReturnProduct.Text = totalCount.ToString();
        }

        private void LoadProductsOnOrder(string category = null)
        {
            string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    string query = @"
            SELECT product_name, product_description, product_price, quantity, brand, image_path, category
            FROM accepted_products
            WHERE status = 'APPROVED'";

                    if (!string.IsNullOrEmpty(category))
                    {
                        query += " AND category = @category";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(category))
                        {
                            command.Parameters.AddWithValue("@category", category);
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            guna2DataGridView2.DataSource = dataTable;


                            guna2DataGridView2.Columns["product_name"].HeaderText = "Product Name";
                            guna2DataGridView2.Columns["product_description"].HeaderText = "Description";
                            guna2DataGridView2.Columns["product_price"].HeaderText = "Price";
                            guna2DataGridView2.Columns["quantity"].HeaderText = "Quantity";
                            guna2DataGridView2.Columns["brand"].HeaderText = "Brand";
                            guna2DataGridView2.Columns["image_path"].HeaderText = "Image Path";
                            guna2DataGridView2.Columns["category"].HeaderText = "Category";

                            guna2DataGridView2.ColumnHeadersVisible = true;
                            guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving data: " + ex.Message);
                }
            }
        }





        private void guna2Button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {

                productImage = Image.FromFile(openFileDialog.FileName);
                pictureBox1.Image = productImage;
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            //!! MAG ADD OG NEW PRODUCT
            productName = Productname.Text;
            productDescription = Productdescription.Text;
            unit = productunit.SelectedItem.ToString();
            productPrice = decimal.Parse(productprice.Text);
            quantity = int.Parse(productquantity.Text);
            brand = productbrand.Text;
            string ccategory = category.SelectedItem.ToString();

            if (productImage != null)
            {
                string directoryPath = "C:\\ProductImages";

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string imageFilePath = Path.Combine(directoryPath, $"{productName}.jpg");

                productImage.Save(imageFilePath);

                string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string query = @"INSERT INTO pending_products (product_name, product_description, unit, product_price, quantity, brand, image_path, status, category)
                                 VALUES (@product_name, @product_description, @unit, @product_price, @quantity, @brand, @image_path, 'PENDING', @category)";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@product_name", productName);
                            command.Parameters.AddWithValue("@product_description", productDescription);
                            command.Parameters.AddWithValue("@unit", unit);
                            command.Parameters.AddWithValue("@product_price", productPrice);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.Parameters.AddWithValue("@brand", brand);
                            command.Parameters.AddWithValue("@image_path", imageFilePath);
                            command.Parameters.AddWithValue("@category", ccategory);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Product added to pending products successfully!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please upload an image before saving the product.");
            }
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";


            string query = "SELECT product_id, product_name, product_description, unit, product_price, quantity, brand,category, image_path, status, created_at FROM pending_products";


            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {

                    connection.Open();


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }


                guna2DataGridView1.DataSource = dataTable;


                guna2DataGridView1.ColumnHeadersVisible = true;


                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AutoResizeColumns();
                guna2DataGridView1.ColumnHeadersHeight = 40;

                guna2DataGridView1.Columns["product_id"].Visible = false;


                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.Width = 150;
                }


                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            actionpanel.Visible = !actionpanel.Visible;
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                string productDescription = selectedRow.Cells["product_description"].Value.ToString();
                string unit = selectedRow.Cells["unit"].Value.ToString();
                decimal productPrice = Convert.ToDecimal(selectedRow.Cells["product_price"].Value);
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                string brand = selectedRow.Cells["brand"].Value.ToString();
                string imagePath = selectedRow.Cells["image_path"].Value.ToString();
                string category = selectedRow.Cells["category"].Value.ToString(); // Retrieve category

                string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            string insertQuery = @"
                        INSERT INTO accepted_products (product_name, product_description, unit, product_price, quantity, brand, image_path, category, status, created_at, accepted_at)
                        VALUES (@product_name, @product_description, @unit, @product_price, @quantity, @brand, @image_path, @category, 'APPROVED', @created_at, @accepted_at)";

                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@product_name", productName);
                                insertCommand.Parameters.AddWithValue("@product_description", productDescription);
                                insertCommand.Parameters.AddWithValue("@unit", unit);
                                insertCommand.Parameters.AddWithValue("@product_price", productPrice);
                                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                                insertCommand.Parameters.AddWithValue("@brand", brand);
                                insertCommand.Parameters.AddWithValue("@image_path", imagePath);
                                insertCommand.Parameters.AddWithValue("@category", category); // Add category
                                insertCommand.Parameters.AddWithValue("@created_at", DateTime.Now);
                                insertCommand.Parameters.AddWithValue("@accepted_at", DateTime.Now);

                                insertCommand.ExecuteNonQuery();
                            }

                            string deleteQuery = "DELETE FROM pending_products WHERE product_id = @product_id";
                            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCommand.Parameters.AddWithValue("@product_id", productId);
                                deleteCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }

                        MessageBox.Show("Product moved to accepted products successfully!");

                        guna2Button4_Click(sender, e); // Refresh pending products table
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to accept.");
            }
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";

            string query = "SELECT product_id, product_name, product_description, unit, product_price, quantity, brand,category , image_path, status, created_at, accepted_at FROM accepted_products";


            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {

                    connection.Open();


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }

                guna2DataGridView1.DataSource = dataTable;


                guna2DataGridView1.ColumnHeadersVisible = true;


                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AutoResizeColumns();
                guna2DataGridView1.ColumnHeadersHeight = 40;

                // Hide the product_id column 
                guna2DataGridView1.Columns["product_id"].Visible = false;


                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.Width = 150; // Adjust column width
                }

                // Optional: Refresh the DataGridView to make sure it's updated properly
                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                string productDescription = selectedRow.Cells["product_description"].Value.ToString();
                string unit = selectedRow.Cells["unit"].Value.ToString();
                decimal productPrice = Convert.ToDecimal(selectedRow.Cells["product_price"].Value);
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                string brand = selectedRow.Cells["brand"].Value.ToString();
                string imagePath = selectedRow.Cells["image_path"].Value.ToString();
                string category = selectedRow.Cells["category"].Value.ToString(); // Retrieve category

                // Show the returndescription panel
                returndescription.Visible = true;

                // Handle Confirm button click in the returndescription panel
                guna2Button13.Click += (s, args) =>
                {
                    string returnReason = returntextbox.Text;

                    if (!string.IsNullOrEmpty(returnReason))
                    {
                        string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                using (MySqlTransaction transaction = connection.BeginTransaction())
                                {
                                    // Insert into pending_return table with category
                                    string insertReturnQuery = @"
                                INSERT INTO pending_return (product_id, product_name, product_description, unit, product_price, quantity, brand, image_path, category, return_reason, created_at)
                                VALUES (@product_id, @product_name, @product_description, @unit, @product_price, @quantity, @brand, @image_path, @category, @return_reason, @created_at)";

                                    using (MySqlCommand insertCommand = new MySqlCommand(insertReturnQuery, connection, transaction))
                                    {
                                        insertCommand.Parameters.AddWithValue("@product_id", productId);
                                        insertCommand.Parameters.AddWithValue("@product_name", productName);
                                        insertCommand.Parameters.AddWithValue("@product_description", productDescription);
                                        insertCommand.Parameters.AddWithValue("@unit", unit);
                                        insertCommand.Parameters.AddWithValue("@product_price", productPrice);
                                        insertCommand.Parameters.AddWithValue("@quantity", quantity);
                                        insertCommand.Parameters.AddWithValue("@brand", brand);
                                        insertCommand.Parameters.AddWithValue("@image_path", imagePath);
                                        insertCommand.Parameters.AddWithValue("@category", category); // Add category
                                        insertCommand.Parameters.AddWithValue("@return_reason", returnReason);
                                        insertCommand.Parameters.AddWithValue("@created_at", DateTime.Now);

                                        insertCommand.ExecuteNonQuery();
                                    }

                                    // Delete from pending_products table
                                    string deleteQuery = "DELETE FROM pending_products WHERE product_id = @product_id";
                                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection, transaction))
                                    {
                                        deleteCommand.Parameters.AddWithValue("@product_id", productId);
                                        deleteCommand.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }

                                MessageBox.Show("Product marked for return successfully!");
                                guna2Button4_Click(sender, e); // Refresh the pending products table
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                            finally
                            {
                                returndescription.Visible = false; // Hide the panel after completion
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a reason for the return.");
                    }
                };

                // Handle Cancel button click to close the returndescription panel without action
                guna2Button12.Click += (s, args) =>
                {
                    returndescription.Visible = false; // Hide the panel
                };
            }
            else
            {
                MessageBox.Show("Please select a product to mark for return.");
            }
        }


        private void guna2Button11_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
            string query = "SELECT product_id, product_name, product_description, unit, product_price, quantity, brand,category,  image_path,return_reason, created_at FROM pending_return";

            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }

                guna2DataGridView1.DataSource = dataTable;
                guna2DataGridView1.ColumnHeadersVisible = true;
                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AutoResizeColumns();
                guna2DataGridView1.ColumnHeadersHeight = 40;

                // Hide the product_id column
                guna2DataGridView1.Columns["product_id"].Visible = false;

                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.Width = 150; // Adjust column width
                }

                // Optional: Refresh the DataGridView to make sure it's updated properly
                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel19_Paint(object sender, PaintEventArgs e)
        {

        }



        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void headsetbutton_Click(object sender, EventArgs e)
        {
            LoadProductsOnOrder("Headset");
        }

        private void mousebutton_Click(object sender, EventArgs e)
        {
            LoadProductsOnOrder("Mouse");
        }

        private void keyboardbutton_Click(object sender, EventArgs e)
        {
            LoadProductsOnOrder("Keyboard");
        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button14_Click_1(object sender, EventArgs e)
        {
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView2.SelectedRows[0];
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                string productDescription = selectedRow.Cells["product_description"].Value.ToString();
                decimal productPrice = Convert.ToDecimal(selectedRow.Cells["product_price"].Value);
                string brand = selectedRow.Cells["brand"].Value.ToString();
                string category = selectedRow.Cells["category"].Value.ToString();

                confirmationpanelorder.Visible = true;

                confirmorder.Click += ConfirmOrderClick;
                cancelorder.Click += CancelOrderClick;

                void ConfirmOrderClick(object s, EventArgs args)
                {
                    int quantity = (int)quantityorder.Value;

                    if (quantity > 0)
                    {
                        decimal totalPrice = productPrice * quantity;

                        guna2DataGridView3.Rows.Add(productName, productDescription, productPrice, quantity, totalPrice, brand, category);

                        MessageBox.Show("Item added to cart successfully!");

                        quantityorder.Value = quantityorder.Minimum;
                        confirmationpanelorder.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid quantity.");
                    }
                }

                void CancelOrderClick(object s, EventArgs args)
                {
                    confirmationpanelorder.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Please select a product to add to the cart.");
            }
        }
    }
    }


