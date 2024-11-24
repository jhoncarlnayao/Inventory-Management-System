using Guna.UI2.WinForms;
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
            Productserialnumber.TextChanged += Productserialnumber_TextChanged;

            guna2DataGridView3.CellValueChanged += guna2DataGridView3_CellValueChanged;
            guna2DataGridView3.RowsAdded += guna2DataGridView3_RowsAdded;
            guna2DataGridView3.RowsRemoved += guna2DataGridView3_RowsRemoved;

           
            UpdateTotalAmount();
          


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
   
            GenerateOrderNumber();

            update_confirmupdate.Click += update_confirmupdate_Click;
            Productserialnumber.TextChanged += Productserialnumber_TextChanged;



            guna2DataGridView3.Columns.Add("productName", "Product Name");
            guna2DataGridView3.Columns.Add("productDescription", "Product Description");
            guna2DataGridView3.Columns.Add("productPrice", "Product Price");
            guna2DataGridView3.Columns.Add("quantity", "Quantity");
            guna2DataGridView3.Columns.Add("totalPrice", "Total Price");
            guna2DataGridView3.Columns.Add("brand", "Brand");
            guna2DataGridView3.Columns.Add("category", "Category");
            guna2DataGridView3.Columns.Add("serial_number", "Serial Number");
            guna2DataGridView3.Columns.Add("batch_number", "Batch Number");
            guna2DataGridView3.Columns.Add("supplier", "Supplier");


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
            SELECT 
                product_name, 
                product_description, 
                product_price, 
                quantity, 
                brand, 
                serial_number, 
                batch_number, 
                supplier, 
                image_path, 
                category 
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

                            guna2DataGridView2.AutoGenerateColumns = true;
                            guna2DataGridView2.DataSource = dataTable;

                            // Set headers for the relevant columns
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
                            if (dataTable.Columns.Contains("serial_number"))
                                guna2DataGridView2.Columns["serial_number"].HeaderText = "Serial Number";
                            if (dataTable.Columns.Contains("batch_number"))
                                guna2DataGridView2.Columns["batch_number"].HeaderText = "Batch Number";
                            if (dataTable.Columns.Contains("supplier"))
                                guna2DataGridView2.Columns["supplier"].HeaderText = "Supplier";
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
                    string query = @"
            SELECT 
                product_id, product_name, product_description, unit, quantity, 
                brand, product_price, image_path, status, category, 
                accepted_at, supplier, batch_number, serial_number 
            FROM accepted_products";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
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
                                string acceptedAt = reader.GetDateTime("accepted_at").ToString("yyyy-MM-dd");
                                string supplier = reader.IsDBNull(reader.GetOrdinal("supplier")) ? string.Empty : reader.GetString("supplier");
                                string batchNumber = reader.IsDBNull(reader.GetOrdinal("batch_number")) ? string.Empty : reader.GetString("batch_number");
                                string serialNumber = reader.IsDBNull(reader.GetOrdinal("serial_number")) ? string.Empty : reader.GetString("serial_number");

                                Image productImage = null;
                                if (!string.IsNullOrEmpty(imagePath))
                                {
                                    try { productImage = Image.FromFile(imagePath); }
                                    catch { productImage = null; }
                                }

                                int rowIndex = guna2DataGridView4.Rows.Add(
                                    false, productImage, productName, productDescription, unit,
                                    quantity, category, brand, productPrice.ToString("C2"),
                                    status, supplier, batchNumber, serialNumber, acceptedAt, productId
                                );

                                // Apply light color coding based on quantity
                                DataGridViewRow row = guna2DataGridView4.Rows[rowIndex];
                                if (quantity <= 5) // Low stock
                                {
                                    row.DefaultCellStyle.BackColor = Color.LightCoral; // Light red for low stock
                                    row.DefaultCellStyle.ForeColor = Color.Black;      // Optional: Black text for readability
                                }
                                else if (quantity <= 20) // Medium stock
                                {
                                    row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow; // Light yellow for medium stock
                                    row.DefaultCellStyle.ForeColor = Color.Black;
                                }
                                else // Sufficient stock
                                {
                                    row.DefaultCellStyle.BackColor = Color.LightGreen; // Light green for sufficient stock
                                    row.DefaultCellStyle.ForeColor = Color.Black;
                                }
                            }
                            catch (Exception rowEx)
                            {
                                Console.WriteLine($"Error processing row: {rowEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            if (!guna2DataGridView4.Columns.Contains("productid"))
            {
                DataGridViewTextBoxColumn productIdColumn = new DataGridViewTextBoxColumn
                {
                    Name = "productid",
                    HeaderText = "Product ID",
                    Visible = false
                };
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
            // PAG MAG ADD KA OG NEW PRODUCTS
            productName = Productname.Text;
            productDescription = Productdescription.Text;
            unit = productunit.SelectedItem.ToString();
            productPrice = decimal.Parse(productprice.Text);
            brand = productbrand.Text;
            string ccategory = category.SelectedItem.ToString();
            string productSupplier = Productsupplier.Text;
            string productBatchNumber = Productbatchnumber.Text;
            string productSerialNumber = Productserialnumber.Text;

            // Validate Serial Number and Batch Number to ensure they are numeric
            if (!int.TryParse(productSerialNumber, out _) && !string.IsNullOrEmpty(productSerialNumber))
            {
                MessageBox.Show("Serial Number must be numeric.");
                return;
            }

            if (!int.TryParse(productBatchNumber, out _) && !string.IsNullOrEmpty(productBatchNumber))
            {
                MessageBox.Show("Batch Number must be numeric.");
                return;
            }

            // Disable or enable quantity field based on serial number input
            if (!string.IsNullOrEmpty(productSerialNumber))
            {
                productquantity.Enabled = false;
                quantity = 1; // Default to 1 for serial number-specific products
            }
            else
            {
                productquantity.Enabled = true;
                quantity = int.Parse(productquantity.Text); // Allow manual input for non-serial products
            }

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

                        string query = @"INSERT INTO pending_products (product_name, product_description, unit, product_price, quantity, brand, image_path, status, category, supplier, batch_number, serial_number)
                                 VALUES (@product_name, @product_description, @unit, @product_price, @quantity, @brand, @image_path, 'PENDING', @category, @supplier, @batch_number, @serial_number)";

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
                            command.Parameters.AddWithValue("@supplier", productSupplier);
                            command.Parameters.AddWithValue("@batch_number", productBatchNumber);
                            command.Parameters.AddWithValue("@serial_number", productSerialNumber);

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

        private void Productserialnumber_TextChanged(object sender, EventArgs e)
        {
            productquantity.Enabled = string.IsNullOrEmpty(Productserialnumber.Text);
        }





        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";

            // Updated SQL query to include 'supplier', 'batch_number', and 'serial_number'
            string query = "SELECT product_id, product_name, product_description, unit, product_price, quantity, brand, image_path, category, supplier, batch_number, serial_number, status, created_at FROM pending_products";

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

                // Set the DataSource to the updated DataTable
                guna2DataGridView1.DataSource = dataTable;

                // Configure the DataGridView appearance
                guna2DataGridView1.ColumnHeadersVisible = true;
                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AutoResizeColumns();
                guna2DataGridView1.ColumnHeadersHeight = 40;

                // Hide 'product_id' column as it is not needed for display
                guna2DataGridView1.Columns["product_id"].Visible = false;

                // Set the width for each column for better visibility
                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.Width = 150;
                }

                // Refresh the DataGridView to reflect changes
                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                // Handle errors
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
                string category = selectedRow.Cells["category"].Value.ToString();
                string productSupplier = selectedRow.Cells["supplier"].Value.ToString();
                string productBatchNumber = selectedRow.Cells["batch_number"].Value?.ToString() ?? "";
                string productSerialNumber = selectedRow.Cells["serial_number"].Value?.ToString() ?? "";

                // Validate that batch and serial numbers are numeric if they exist
                if (!string.IsNullOrEmpty(productBatchNumber) && !int.TryParse(productBatchNumber, out _))
                {
                    MessageBox.Show("Batch Number must be numeric.");
                    return;
                }

                if (!string.IsNullOrEmpty(productSerialNumber) && !int.TryParse(productSerialNumber, out _))
                {
                    MessageBox.Show("Serial Number must be numeric.");
                    return;
                }

                string connectionString = "Server=localhost;Database=it12proj;User=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            // Insert into accepted_products table
                            string insertQuery = @"
                        INSERT INTO accepted_products (product_name, product_description, unit, product_price, quantity, brand, image_path, category, status, supplier, batch_number, serial_number, created_at, accepted_at)
                        VALUES (@product_name, @product_description, @unit, @product_price, @quantity, @brand, @image_path, @category, 'APPROVED', @supplier, @batch_number, @serial_number, @created_at, @accepted_at)";

                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@product_name", productName);
                                insertCommand.Parameters.AddWithValue("@product_description", productDescription);
                                insertCommand.Parameters.AddWithValue("@unit", unit);
                                insertCommand.Parameters.AddWithValue("@product_price", productPrice);
                                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                                insertCommand.Parameters.AddWithValue("@brand", brand);
                                insertCommand.Parameters.AddWithValue("@image_path", imagePath);
                                insertCommand.Parameters.AddWithValue("@category", category);
                                insertCommand.Parameters.AddWithValue("@supplier", productSupplier);
                                insertCommand.Parameters.AddWithValue("@batch_number", productBatchNumber);
                                insertCommand.Parameters.AddWithValue("@serial_number", productSerialNumber);
                                insertCommand.Parameters.AddWithValue("@created_at", DateTime.Now);
                                insertCommand.Parameters.AddWithValue("@accepted_at", DateTime.Now);

                                insertCommand.ExecuteNonQuery();
                            }

                            // Record the stock-in
                            string stockInQuery = @"
                        INSERT INTO stock_in (product_name, product_description, quantity, reason, supplier, batch_number, serial_number, created_at, remarks)
                        VALUES (@product_name, @product_description, @quantity, @reason, @supplier, @batch_number, @serial_number, @created_at, @remarks)";

                            using (MySqlCommand stockInCommand = new MySqlCommand(stockInQuery, connection, transaction))
                            {
                                stockInCommand.Parameters.AddWithValue("@product_name", productName);
                                stockInCommand.Parameters.AddWithValue("@product_description", productDescription);
                                stockInCommand.Parameters.AddWithValue("@quantity", quantity);
                                stockInCommand.Parameters.AddWithValue("@reason", "Accepted from Pending");
                                stockInCommand.Parameters.AddWithValue("@supplier", productSupplier);
                                stockInCommand.Parameters.AddWithValue("@batch_number", productBatchNumber);
                                stockInCommand.Parameters.AddWithValue("@serial_number", productSerialNumber);
                                stockInCommand.Parameters.AddWithValue("@created_at", DateTime.Now);
                                stockInCommand.Parameters.AddWithValue("@remarks", "Product accepted into stock from pending.");

                                stockInCommand.ExecuteNonQuery();
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

                        MessageBox.Show("Product moved to accepted products and stock-in recorded successfully!");

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

            // Updated query to include 'supplier', 'batch_number', and 'serial_number' columns
            string query = @"SELECT product_id, product_name, product_description, unit, product_price, 
                     quantity, brand, category, image_path, status, created_at, accepted_at, 
                     supplier, batch_number, serial_number 
                     FROM accepted_products";

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
                            dataAdapter.Fill(dataTable); // Fill the DataTable with the result set
                        }
                    }
                }

                // Bind the DataTable to the DataGridView
                guna2DataGridView1.DataSource = dataTable;

                // Make headers visible and set the column auto-sizing properties
                guna2DataGridView1.ColumnHeadersVisible = true;
                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.ColumnHeadersHeight = 40;

                // Hide the 'product_id' column if it's not necessary to display
                if (guna2DataGridView1.Columns.Contains("product_id"))
                {
                    guna2DataGridView1.Columns["product_id"].Visible = false;
                }

                // Optional: Set specific widths for columns
                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.Width = 150; // Adjust column width
                }

                // Optional: Refresh the DataGridView to make sure it's updated properly
                guna2DataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display an error message
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
            // Ensure at least one row is selected
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                // Show confirmation panel
                confirmationpanelorder.Visible = true;

                // Remove existing handlers to avoid stacking
                confirmorder.Click -= ConfirmOrderClick;
                cancelorder.Click -= CancelOrderClick;

                // Attach fresh event handlers
                confirmorder.Click += ConfirmOrderClick;
                cancelorder.Click += CancelOrderClick;

                // Define the ConfirmOrderClick logic
                void ConfirmOrderClick(object s, EventArgs args)
                {
                    // Re-fetch the currently selected row (fresh reference)
                    if (guna2DataGridView2.SelectedRows.Count > 0)
                    {
                        DataGridViewRow selectedRow = guna2DataGridView2.SelectedRows[0];

                        // Extract data from the selected row
                        string productName = selectedRow.Cells["product_name"].Value.ToString();
                        string productDescription = selectedRow.Cells["product_description"].Value.ToString();
                        decimal productPrice = Convert.ToDecimal(selectedRow.Cells["product_price"].Value);
                        string brand = selectedRow.Cells["brand"].Value.ToString();
                        string category = selectedRow.Cells["category"].Value.ToString();
                        string serialNumber = selectedRow.Cells["serial_number"].Value.ToString();
                        string batchNumber = selectedRow.Cells["batch_number"].Value.ToString();
                        string supplier = selectedRow.Cells["supplier"].Value.ToString();

                        // Get the quantity entered
                        int quantity = (int)quantityorder.Value;

                        if (quantity > 0)
                        {
                            decimal totalPrice = productPrice * quantity;

                            // Add the item to the cart
                            guna2DataGridView3.Rows.Add(
                                productName,
                                productDescription,
                                productPrice,
                                quantity,
                                totalPrice,
                                brand,
                                category,
                                serialNumber,
                                batchNumber,
                                supplier
                            );

                            MessageBox.Show("Item added to cart successfully!");

                            // Reset quantity and hide the confirmation panel
                            quantityorder.Value = quantityorder.Minimum;
                            confirmationpanelorder.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid quantity.");
                        }
                    }
                }

                // Define the CancelOrderClick logic
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
            // Get the currently selected row
            if (guna2DataGridView4.CurrentRow != null)
            {
                DataGridViewRow row = guna2DataGridView4.CurrentRow;

                try
                {
                    string productName = row.Cells["product_name"].Value.ToString();
                    string productDescription = row.Cells["product_description"].Value.ToString();
                    string unit = row.Cells["unitt"].Value.ToString();
                    string category = row.Cells["categoryy"].Value.ToString(); // Use the correct column name "categoryy"

                    string priceString = row.Cells["product_price"].Value.ToString();
                    decimal productPrice = 0;

                    if (!string.IsNullOrEmpty(priceString))
                    {
                        // Clean and parse the price string
                        priceString = priceString.Replace("₱", "").Replace(",", "").Trim();
                        if (!decimal.TryParse(priceString, out productPrice))
                        {
                            MessageBox.Show("Invalid price format.");
                            return;
                        }
                    }

                    // Populate the fields in the update panel
                    update_productname.Text = productName;
                    update_productdescription.Text = productDescription;
                    update_unit.SelectedItem = unit;
                    update_category.SelectedItem = category;
                    update_price.Text = productPrice.ToString("F2");

                    // Show the update panel
                    inventory_product_upatepanel.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                // Handle case where no row is selected
                MessageBox.Show("Please select a row to update.");
            }
        }








        private void update_confirmupdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the currently highlighted row
                if (guna2DataGridView4.CurrentRow != null)
                {
                    DataGridViewRow row = guna2DataGridView4.CurrentRow;

                    // Retrieve updated values from the input fields
                    string newProductName = update_productname.Text;
                    string newProductDescription = update_productdescription.Text;
                    string newUnit = update_unit.SelectedItem?.ToString() ?? string.Empty; // Safe null check
                    string newCategory = update_category.SelectedItem?.ToString() ?? string.Empty;
                    decimal newPrice = decimal.Parse(update_price.Text);

                    // Get the product ID from the DataGridView
                    int productId = Convert.ToInt32(row.Cells["productid"].Value);

                    // Update the database
                    string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                    UPDATE accepted_products 
                    SET product_name = @name, 
                        product_description = @description, 
                        unit = @unit, 
                        category = @category, 
                        product_price = @price 
                    WHERE product_id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@name", newProductName);
                        cmd.Parameters.AddWithValue("@description", newProductDescription);
                        cmd.Parameters.AddWithValue("@unit", newUnit);
                        cmd.Parameters.AddWithValue("@category", newCategory);
                        cmd.Parameters.AddWithValue("@price", newPrice);
                        cmd.Parameters.AddWithValue("@id", productId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully.");

                            // Update the DataGridView row with the new values
                            row.Cells["product_name"].Value = newProductName;
                            row.Cells["product_description"].Value = newProductDescription;
                            row.Cells["unitt"].Value = newUnit;
                            row.Cells["categoryy"].Value = newCategory;
                            row.Cells["product_price"].Value = newPrice.ToString("C2"); // Format as currency
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please check the product ID.");
                        }
                    }

                    // Hide the update panel
                    inventory_product_upatepanel.Visible = false;
                }
                else
                {
                    MessageBox.Show("Please highlight a row to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }




        private void guna2HtmlLabel10_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openbtn_sumarrycart_Click(object sender, EventArgs e)
        {
            panel_summaryorder.Visible = true;



        }

        private void guna2Button15_Click_1(object sender, EventArgs e)
        {
            panel_summaryorder.Visible = false;
        }



        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! WHEN CHECKING OUT ORDERS FUNCTION START
        private void confirmorderbtn_summaryorder_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView3.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty. Add items before checking out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string orderNumber = Guid.NewGuid().ToString().Substring(0, 8);
            customer_ordernumber.Text = orderNumber;

            string fullname = customer_fullname.Text.Trim();
            string phoneNumber = customer_phonenumber.Text.Trim();
            string address = customer_address.Text.Trim();
            string paymentMethod = "Cash";
            decimal totalAmount;

            if (string.IsNullOrWhiteSpace(fullname) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(address) ||
                !decimal.TryParse(customer_totalamount.Text.Trim(), out totalAmount) ||
                totalAmount <= 0)
            {
                MessageBox.Show("Please fill in all required customer details and ensure total amount is valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Server=localhost;Database=it12proj;User ID=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        string insertQuery = @"
                INSERT INTO checkout_orders (
                    order_number, product_name, product_description, product_price, quantity, 
                    brand, category, serial_number, batch_number, supplier, 
                    fullname, phone_number, address, payment_method, total_amount
                ) VALUES (
                    @orderNumber, @productName, @productDescription, @productPrice, @quantity, 
                    @brand, @category, @serialNumber, @batchNumber, @supplier, 
                    @fullname, @phoneNumber, @address, @paymentMethod, @totalAmount
                )";

                        string updateQuery = @"
                UPDATE accepted_products 
                SET quantity = quantity - @orderedQuantity 
                WHERE product_name = @productName AND quantity >= @orderedQuantity";

                        string deleteQuery = @"
                DELETE FROM accepted_products 
                WHERE product_name = @productName AND quantity = 0";

                        string stockOutQuery = @"
                INSERT INTO stock_out (
                    product_name, product_description, quantity, reason, customer_name, order_number, remarks
                ) VALUES (
                    @productName, @productDescription, @quantity, @reason, @customerName, @orderNumber, @remarks
                )";

                        foreach (DataGridViewRow row in guna2DataGridView3.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string productName = row.Cells["productName"].Value.ToString();
                            string productDescription = row.Cells["productDescription"].Value?.ToString() ?? string.Empty;
                            decimal productPrice = Convert.ToDecimal(row.Cells["productPrice"].Value);
                            int orderedQuantity = Convert.ToInt32(row.Cells["quantity"].Value);
                            string brand = row.Cells["brand"].Value?.ToString() ?? string.Empty;
                            string category = row.Cells["category"].Value?.ToString() ?? string.Empty;
                            string serialNumber = row.Cells["serial_number"].Value?.ToString() ?? string.Empty;
                            string batchNumber = row.Cells["batch_number"].Value?.ToString() ?? string.Empty;
                            string supplier = row.Cells["supplier"].Value?.ToString() ?? string.Empty;

                            // Insert into checkout_orders
                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                                insertCommand.Parameters.AddWithValue("@productName", productName);
                                insertCommand.Parameters.AddWithValue("@productDescription", productDescription);
                                insertCommand.Parameters.AddWithValue("@productPrice", productPrice);
                                insertCommand.Parameters.AddWithValue("@quantity", orderedQuantity);
                                insertCommand.Parameters.AddWithValue("@brand", brand);
                                insertCommand.Parameters.AddWithValue("@category", category);
                                insertCommand.Parameters.AddWithValue("@serialNumber", serialNumber);
                                insertCommand.Parameters.AddWithValue("@batchNumber", batchNumber);
                                insertCommand.Parameters.AddWithValue("@supplier", supplier);
                                insertCommand.Parameters.AddWithValue("@fullname", fullname);
                                insertCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                                insertCommand.Parameters.AddWithValue("@address", address);
                                insertCommand.Parameters.AddWithValue("@paymentMethod", paymentMethod);
                                insertCommand.Parameters.AddWithValue("@totalAmount", totalAmount);
                                insertCommand.ExecuteNonQuery();
                            }

                            // Update accepted_products
                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection, transaction))
                            {
                                updateCommand.Parameters.AddWithValue("@productName", productName);
                                updateCommand.Parameters.AddWithValue("@orderedQuantity", orderedQuantity);
                                updateCommand.ExecuteNonQuery();
                            }

                            // Delete zero-quantity items from accepted_products
                            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCommand.Parameters.AddWithValue("@productName", productName);
                                deleteCommand.ExecuteNonQuery();
                            }

                            // Insert into stock_out
                            using (MySqlCommand stockOutCommand = new MySqlCommand(stockOutQuery, connection, transaction))
                            {
                                stockOutCommand.Parameters.AddWithValue("@productName", productName);
                                stockOutCommand.Parameters.AddWithValue("@productDescription", productDescription);
                                stockOutCommand.Parameters.AddWithValue("@quantity", orderedQuantity);
                                stockOutCommand.Parameters.AddWithValue("@reason", "Sold to Customer");
                                stockOutCommand.Parameters.AddWithValue("@customerName", fullname);
                                stockOutCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                                stockOutCommand.Parameters.AddWithValue("@remarks", "Order completed");
                                stockOutCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        // Clear cart and customer details
                        guna2DataGridView3.Rows.Clear();
                        customer_fullname.Clear();
                        customer_phonenumber.Clear();
                        customer_address.Clear();
                        customer_totalamount.Clear();
                        customer_ordernumber.Clear();

                        MessageBox.Show("Order successfully checked out and saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in guna2DataGridView3.Rows)
            {
                if (row.IsNewRow) continue;

                // Retrieve productPrice and quantity values
                decimal productPrice = Convert.ToDecimal(row.Cells["productPrice"].Value ?? 0);
                int quantity = Convert.ToInt32(row.Cells["quantity"].Value ?? 0);

                // Calculate total for this row and add to totalAmount
                totalAmount += productPrice * quantity;
            }

            // Update the total amount TextBox
            customer_totalamount.Text = totalAmount.ToString("0.00");
        }

        // Event handler: Update total when a cell value changes
        private void guna2DataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == guna2DataGridView3.Columns["productPrice"].Index ||
                                    e.ColumnIndex == guna2DataGridView3.Columns["quantity"].Index))
            {
                UpdateTotalAmount();
            }
        }

        // Event handler: Update total when rows are added
        private void guna2DataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotalAmount();
        }

        // Event handler: Update total when rows are removed
        private void guna2DataGridView3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalAmount();
        }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! WHEN CHECKING OUT ORDERS FUNCTION END





        // Method to generate a random order number
        private string GenerateOrderNumber()
        {
            return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999).ToString();
        }

        private void guna2Button20_Click(object sender, EventArgs e) //! RESET TEXT INPUTS FOR ADDING NEW PRODUCTS
        {
            Productname.Clear();
            Productdescription.Clear();
            //  productunit.SelectedItem.ToString();
            productprice.Clear();
            productbrand.Clear();
            //  category.SelectedItem();
            Productsupplier.Clear();
            Productbatchnumber.Clear();
            Productserialnumber.Clear();
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            LoadProductsOnOrder("Monitor");
        }
    }
}




