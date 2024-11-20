using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace NguyenMinhPhuc_CNPM_BaiKT2
{
    /// <summary>
    /// Interaction logic for frmShippers_2121050560_NguyenMinhPhuc.xaml
    /// </summary>
    public partial class frmShippers_2121050560_NguyenMinhPhuc : Window
    {
        private string connectionString = "Server=DESKTOP-U7LD9P5;Database=QLShippers;Trusted_Connection=True;";

        public frmShippers_2121050560_NguyenMinhPhuc()
        {
            InitializeComponent();
            LoadShippersData();
        }

        // Load dữ liệu Shippers vào DataGrid
        private void LoadShippersData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM [QLShippers].[dbo].[Shippers]";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvShippers.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý khi người dùng nhấn nút Tìm kiếm
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM [QLShippers].[dbo].[Shippers] WHERE 1=1";

                    if (!string.IsNullOrEmpty(txtShipperID.Text))
                        query += $" AND ShipperID = {txtShipperID.Text}";
                    if (!string.IsNullOrEmpty(txtCompanyName.Text))
                        query += $" AND CompanyName LIKE N'%{txtCompanyName.Text}%'";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvShippers.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý khi chọn một dòng trong DataGrid
        //private void dgvShippers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    if (dgvShippers.SelectedItem is DataRowView selectedRow)
        //    {
        //        txtShipperIDDetail.Text = selectedRow["ShipperID"].ToString();
        //        txtCompanyNameDetail.Text = selectedRow["CompanyName"].ToString();
        //        txtPhone.Text = selectedRow["Phone"].ToString();
        //    }
        //}

        // Cập nhật thông tin chi tiết khi chọn một Shipper
        private void dgvShippers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn hay không
                if (dgvShippers.SelectedItem is DataRowView selectedRow)

            {

                txtShipperIDDetail.Text = selectedRow["ShipperID"].ToString();
                 txtCompanyNameDetail.Text = selectedRow["CompanyName"].ToString();
                 txtPhone.Text = selectedRow["Phone"].ToString();

                // Ẩn các label khi có dữ liệu
                tbId.Visibility = string.IsNullOrEmpty(txtShipperIDDetail.Text) ? Visibility.Visible : Visibility.Collapsed;
                tbName.Visibility = string.IsNullOrEmpty(txtCompanyNameDetail.Text) ? Visibility.Visible : Visibility.Collapsed;
                tbPhone.Visibility = string.IsNullOrEmpty(txtPhone.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // Nếu không có dòng nào được chọn, làm sạch các trường và hiển thị lại các label
                txtShipperIDDetail.Clear();
                txtCompanyNameDetail.Clear();
                txtPhone.Clear();

                tbId.Visibility = Visibility.Visible;
                tbName.Visibility = Visibility.Visible;
                tbPhone.Visibility = Visibility.Visible;
            }
        }


        // Xử lý lưu thông tin Shipper
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE [QLShippers].[dbo].[Shippers]
                        SET CompanyName = @CompanyName, Phone = @Phone
                        WHERE ShipperID = @ShipperID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShipperID", txtShipperIDDetail.Text);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyNameDetail.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadShippersData();
                }
                txtShipperIDDetail.Clear();
                txtCompanyNameDetail.Clear();
                txtPhone.Clear();

                tbId.Visibility = Visibility.Visible;
                tbName.Visibility = Visibility.Visible;
                tbPhone.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu thông tin: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // Xử lý hủy chỉnh sửa
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtShipperIDDetail.Text = string.Empty;
            txtCompanyNameDetail.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        // Xử lý thêm mới Shipper
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO [QLShippers].[dbo].[Shippers] (CompanyName, Phone)
                        VALUES (@CompanyName, @Phone)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyNameDetail.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadShippersData();
                }
                txtShipperIDDetail.Clear();
                txtCompanyNameDetail.Clear();
                txtPhone.Clear();

                tbId.Visibility = Visibility.Visible;
                tbName.Visibility = Visibility.Visible;
                tbPhone.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm mới: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sửa thông tin Shipper
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtShipperIDDetail.Text))
            {
                MessageBox.Show("Vui lòng chọn một Shipper để chỉnh sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            btnSave.IsEnabled = true;
            txtCompanyNameDetail.IsReadOnly = false;
            txtPhone.IsReadOnly = false;
        }

        // Xử lý xóa Shipper
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtShipperIDDetail.Text))
            {
                MessageBox.Show("Vui lòng chọn một Shipper để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM [QLShippers].[dbo].[Shippers] WHERE ShipperID = @ShipperID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShipperID", txtShipperIDDetail.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadShippersData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
