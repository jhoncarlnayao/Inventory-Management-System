﻿using Guna.UI2.WinForms;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
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


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderRadius = 30;


            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                path.AddArc(this.Width - borderRadius - 1, 0, borderRadius, borderRadius, 270, 90);
                path.AddArc(this.Width - borderRadius - 1, this.Height - borderRadius - 1, borderRadius, borderRadius, 0, 90);
                path.AddArc(0, this.Height - borderRadius - 1, borderRadius, borderRadius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);
            }
        }


        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            CountTotalPendingProducts();
            CountTotalAcceptProducts();
            CountTotalReturnProducts();
            CountTotalPendingReturnProducts();
            LoadProductsOnOrder();

            update_confirmupdate.Click += update_confirmupdate_Click;


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

        public void CountTotalPendingReturnProducts()
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

            pendingreturnproduct.Text = totalCount.ToString();
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

                            // Debug: Print column names to verify
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                Console.WriteLine("Column: " + column.ColumnName);
                            }


                            guna2DataGridView2.AutoGenerateColumns = true;
                            guna2DataGridView2.DataSource = dataTable;


                            if (dataTable.Columns.Contains("product_name"))
                                guna2DataGridView2.Columns["product_name"].HeaderText = "Product Name";
                            if (dataTable.Columns.Contains("product_description"))
                                guna2DataGridView2.Columns["product_description"].HeaderText = "Description";
                            if (dataTable.Columns.Contains("product_price"))
                                guna2DataGridView2.Columns["product_price"].HeaderText = "Price";
                            if (dataTable.Columns.Contains("quantity"))
                                guna2DataGridView2.Columns["quantity"].HeaderText = "Quantity";
                            if (dataTable.Columns.Contains("brand"))
                                guna2DataGridView2.Columns["brand"].HeaderText = "Brand";
                            if (dataTable.Columns.Contains("image_path"))
                                guna2DataGridView2.Columns["image_path"].HeaderText = "Image Path";
                            if (dataTable.Columns.Contains("category"))
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


        private void LoadInventoryProducts()
        {
            string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT product_id, product_name, product_description, unit, quantity, brand, product_price, image_path, status, category, accepted_at FROM accepted_products";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32("product_id");
                            string productName = reader.GetString("product_name");
                            string productDescription = reader.IsDBNull(reader.GetOrdinal("product_description")) ? string.Empty : reader.GetString("product_description");
                            string unit = reader.GetString("unit");
                            int quantity = reader.GetInt32("quantity");
                            string category = reader.GetString("category");
                            string brand = reader.IsDBNull(reader.GetOrdinal("brand")) ? string.Empty : reader.GetString("brand");
                            decimal productPrice = reader.GetDecimal("product_price");
                            string imagePath = reader.IsDBNull(reader.GetOrdinal("image_path")) ? string.Empty : reader.GetString("image_path");
                            string status = reader.GetString("status");
                            DateTime acceptedAt = reader.GetDateTime("accepted_at");

                            // Load the product image if the path is not empty
                            Image productImage = string.IsNullOrEmpty(imagePath) ? null : Image.FromFile(imagePath);

                            // Add row to the DataGridView
                            guna2DataGridView4.Rows.Add(
                                false,                            // Checkbox column (default value false)
                                productImage,                     // Image column
                                productName,                      // Product name
                                productDescription,               // Product description
                                unit,                             // Unit
                                quantity,                         // Quantity
                                category,                         // Category
                                brand,                            // Brand
                                productPrice.ToString("C2"),      // Product price (formatted)
                                status,                           // Status
                                acceptedAt.ToString("yyyy-MM-dd"), // Accepted at
                                productId                          // Product ID (hidden column)
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            // Ensure product_id column exists and hide it
            if (guna2DataGridView4.Columns.Contains("productid"))
            {
                guna2DataGridView4.Columns["productid"].Visible = false;  // Hide the product_id column
            }
            else
            {
                // If the column doesn't exist, you can manually add it if needed
                DataGridViewTextBoxColumn productIdColumn = new DataGridViewTextBoxColumn();
                productIdColumn.Name = "productid";
                productIdColumn.HeaderText = "Product ID"; // Optionally, set header text
                productIdColumn.Visible = false; // Hide it
                guna2DataGridView4.Columns.Add(productIdColumn);
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
            string query = "SELECT return_id, product_id, product_name, product_description, unit, product_price, quantity, brand, category, image_path, return_reason, created_at FROM pending_return";

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

                guna2DataGridView1.Columns["return_id"].Visible = false; // Hide return_id if you don’t want to display it

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


                // Unsubscribe before adding handlers to prevent multiple subscriptions
                confirmorder.Click -= ConfirmOrderClick;
                cancelorder.Click -= CancelOrderClick;

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


        private void guna2HtmlLabel53_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button16_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";
            string query = "SELECT product_id, product_name, product_description, unit, product_price, quantity, brand,category,  image_path,return_reason, accepted_at FROM accepted_returns";

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

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                int returnId = Convert.ToInt32(selectedRow.Cells["return_id"].Value);  // Use return_id as primary key for deletion
                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                string productDescription = selectedRow.Cells["product_description"].Value.ToString();
                string unit = selectedRow.Cells["unit"].Value.ToString();
                decimal productPrice = Convert.ToDecimal(selectedRow.Cells["product_price"].Value);
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                string brand = selectedRow.Cells["brand"].Value.ToString();
                string imagePath = selectedRow.Cells["image_path"].Value.ToString();
                string category = selectedRow.Cells["category"].Value.ToString();
                string returnReason = selectedRow.Cells["return_reason"].Value.ToString();

                string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            // Insert into accepted_returns
                            string insertQuery = @"
                    INSERT INTO accepted_returns (product_id, product_name, product_description, unit, product_price, quantity, brand, category, image_path, return_reason, accepted_at)
                    VALUES (@product_id, @product_name, @product_description, @unit, @product_price, @quantity, @brand, @category, @image_path, @return_reason, @accepted_at)";

                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@product_id", productId);
                                insertCommand.Parameters.AddWithValue("@product_name", productName);
                                insertCommand.Parameters.AddWithValue("@product_description", productDescription);
                                insertCommand.Parameters.AddWithValue("@unit", unit);
                                insertCommand.Parameters.AddWithValue("@product_price", productPrice);
                                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                                insertCommand.Parameters.AddWithValue("@brand", brand);
                                insertCommand.Parameters.AddWithValue("@category", category);
                                insertCommand.Parameters.AddWithValue("@image_path", imagePath);
                                insertCommand.Parameters.AddWithValue("@return_reason", returnReason);
                                insertCommand.Parameters.AddWithValue("@accepted_at", DateTime.Now);

                                insertCommand.ExecuteNonQuery();
                            }

                            // Delete from pending_return using return_id
                            string deleteQuery = "DELETE FROM pending_return WHERE return_id = @return_id";
                            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCommand.Parameters.AddWithValue("@return_id", returnId); // Use return_id as primary key in pending_return
                                deleteCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }

                        MessageBox.Show("Return confirmed and moved to accepted returns!");
                        guna2Button11_Click(sender, e); // Refresh pending return table
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to confirm return.");
            }
        }

        private void guna2DataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e) // !! KAUBAN NI SA INVENTORY PRODUCTS TABLE
        {

        }

        private void guna2Button19_Click(object sender, EventArgs e) //!! THIS IS BUTTON FOR MY INVENTORY PRODUCT LIST
        {
            LoadInventoryProducts();
        }

        private void guna2DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // CART TABLE
        }

        private void confirmationpanelorder_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            // CHECK OUT BUTTON
        }

        private void guna2HtmlLabel51_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel23_Paint(object sender, PaintEventArgs e)
        {

        }

        private void inventory_updatebutton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in guna2DataGridView4.Rows)
            {
                // Check if the checkbox in the row is selected
                bool isSelected = Convert.ToBoolean(row.Cells["Select"].Value); // The checkbox column name is "Select"

                if (isSelected)
                {
                    // Use the correct column names that match your DataGridView
                    string productName = row.Cells["product_name"].Value.ToString(); // Column name is "product_name"
                    string productDescription = row.Cells["product_description"].Value.ToString(); // Column name is "product_description"
                    string unit = row.Cells["unitt"].Value.ToString(); // Column name is "unitt"
                    string category = "N/A"; // There is no "category" column, assuming you need to handle it differently

                    // Get the product price, remove the currency symbol, and convert to decimal
                    string priceString = row.Cells["product_price"].Value.ToString(); // Column name is "product_price"
                    decimal productPrice = 0;

                    // Remove currency symbol and any commas
                    if (!string.IsNullOrEmpty(priceString))
                    {
                        priceString = priceString.Replace("₱", "").Replace(",", "").Trim();
                        if (decimal.TryParse(priceString, out productPrice))
                        {
                            // Successfully converted to decimal
                        }
                        else
                        {
                            // Handle the error if conversion fails
                            MessageBox.Show("Invalid price format.");
                        }
                    }

                    // Populate the fields in the update panel
                    update_productname.Text = productName;
                    update_productdescription.Text = productDescription;
                    update_unit.SelectedItem = unit;
                    update_category.SelectedItem = category; // Update how the category is set if applicable
                    update_price.Text = productPrice.ToString("F2");

                    // Display the update panel
                    inventory_product_upatepanel.Visible = true;
                    break; // Show for the first selected item
                }
            }
        }






        private void update_confirmupdate_Click(object sender, EventArgs e)
        {
          
            string newProductName = update_productname.Text;
            string newProductDescription = update_productdescription.Text;
            string newUnit = update_unit.SelectedItem.ToString();
            string newCategory = update_category.SelectedItem.ToString();
            decimal newPrice = decimal.Parse(update_price.Text);


            foreach (DataGridViewRow row in guna2DataGridView4.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells[0].Value); 

                if (isSelected)
                {
                    int productId = Convert.ToInt32(row.Cells["productid"].Value); 

                    // Update the database
                    string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE accepted_products SET product_name = @name, product_description = @description, unit = @unit, " +
                                       "category = @category, product_price = @price WHERE product_id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@name", newProductName);
                        cmd.Parameters.AddWithValue("@description", newProductDescription);
                        cmd.Parameters.AddWithValue("@unit", newUnit);
                        cmd.Parameters.AddWithValue("@category", newCategory);
                        cmd.Parameters.AddWithValue("@price", newPrice);
                        cmd.Parameters.AddWithValue("@id", productId); // Use the product_id

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please check the product ID.");
                        }
                    }

                    // Format the product price as currency when updating the DataGridView
                    row.Cells["product_name"].Value = newProductName;
                    row.Cells["product_description"].Value = newProductDescription;
                    row.Cells["unitt"].Value = newUnit;
                    row.Cells["categoryy"].Value = newCategory;
                    row.Cells["product_price"].Value = newPrice.ToString("C2"); // Formatting price as currency

                    inventory_product_upatepanel.Visible = false; // Hide the panel after updating
                    break;
                }
            }
        }





    }
}




