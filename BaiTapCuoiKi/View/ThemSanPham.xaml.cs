using BaiTapCuoiKi.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for ThemSanPham.xaml
    /// </summary>
    public partial class ThemSanPham : Window
    {
        connect db = new connect();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        private int id = -1;
        public ThemSanPham()
        {
            InitializeComponent();
            tb_soluongsp.PreviewTextInput += chichonhapso;
            tb_giasp.PreviewTextInput += chichonhapso;
         
        }
        public ThemSanPham(int idSanPhamSelected) : this()
        {
            id = idSanPhamSelected;
            HienThiDuLieuSua();
            tb_tieude.Text = "THÔNG TIN SẢN PHẨM";
            btn_them.Content = "Lưu";
            btn_xoa.Visibility = Visibility.Visible;
        }
       
        private void chichonhapso(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumber(e.Text))
            {
                e.Handled = true; // dùng để từ chối kí tự nhập
                MessageBox.Show("Vui lòng chỉ nhập số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool IsNumber(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public void HienThiDuLieuSua()
        {
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Image/anhchuanhap.jpg";
            var sanphamSelected = db.SANPHAM.Find(id);
            if (sanphamSelected != null)
            {
                string pathImageSach = (sanphamSelected.Sanpham_anh == "" || sanphamSelected.Sanpham_anh == null) ? pathNoImage : sanphamSelected.Sanpham_anh;
                imgsanpham.Source = new BitmapImage(new Uri(pathImageSach));
                pathImage.Text = sanphamSelected.Sanpham_anh;
                tb_tensp.Text=sanphamSelected.Sanpham_ten;
                tb_motasp.Text=sanphamSelected.Sanpham_mota;
                tb_giasp.Text=sanphamSelected.Sanpham_gia.ToString();
                tb_soluongsp.Text=sanphamSelected.Sanpham_soluong.ToString();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_them_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tensanpham = tb_tensp.Text;
                string mota = tb_motasp.Text;
                decimal gia = decimal.Parse(tb_giasp.Text);
                int soluong = int.Parse(tb_soluongsp.Text);
                if (id == -1)
                {
                    string pathAnhBia = "";
                    pathAnhBia = HandeUploadImage();
                    var sanpham = new SANPHAM();
                    sanpham.Sanpham_ten = tensanpham;
                    sanpham.Sanpham_mota = mota;
                    sanpham.Sanpham_gia = gia;
                    sanpham.Sanpham_soluong = soluong;
                    sanpham.Sanpham_anh = pathAnhBia;
                    db.SANPHAM.Add(sanpham);
                    db.SaveChanges();
                    MessageBox.Show("Thêm Sản Phẩm Thành Công", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    string pathAnhBia = pathImage.Text;
                    var sanphamedit = db.SANPHAM.Find(id);
                    if (pathAnhBia != sanphamedit.Sanpham_anh)
                    {
                        pathAnhBia = HandeUploadImage();
                    }
                    sanphamedit.Sanpham_ten = tensanpham;
                    sanphamedit.Sanpham_mota= mota;
                    sanphamedit.Sanpham_gia = gia;
                    sanphamedit.Sanpham_soluong = soluong;
                    sanphamedit.Sanpham_anh = pathAnhBia;
                    db.SaveChanges();
                    MessageBox.Show("Sửa dữ liệu thành công!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                DataChangedEventHandler handler = DataChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files |*.jpg;*.png;*.bmp;*.gif;*.jpeg";
            if (dlg.ShowDialog() == true)
            {
                string selectedImagePath = dlg.FileName;
                pathImage.Text = selectedImagePath;
                imgsanpham.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }
        public string HandeUploadImage()
        {
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Image/anhchuanhap.jpg";
            // xử lý upload ảnh 
            // lấy thời gian hiện tại dưới dạng timestamp
            string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            // lấy đường dẫn đế tệp ảnh từ nguồn( được giả định là bạn đã cung cấp pathImage.Text)
            string imageUpload = pathImage.Text;
            // nếu không có ảnh upload khi mới thêm return ảnh noImage
            if (imageUpload == null || imageUpload == "") return pathNoImage;
            // phân tách tên file và định dạng file
            string[] nameArray = imageUpload.Split('.');
            string imgTempName = nameArray[0];
            string extension = nameArray[1];
            // tạo tên tệp mới với thười gian đính kèm;
            string pathFileNew = imgTempName + "_" + timeStamp + "." + extension;
            // xác định thư mục lưu trữ ảnh đã upload
            string uploadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Image";
            // kiểm tra xem thư mục tồn tại chưa, nếu chưa thì tạo mới
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            // tạo đường dẫn đến tệp ảnh trong thư mục Uploads
            string destinationPath = System.IO.Path.Combine(uploadDirectory, System.IO.Path.GetFileName(pathFileNew));
            // xử lý lưu tệp ảnh từ nguồn đến đích
            File.Copy(imageUpload, destinationPath);
            // trả về đường dẫn đến tệp ảnh đã lưu
            return destinationPath;

        }

        private void btn_upload_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_xoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn xóa dữ liệu?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SANPHAM sanpham = db.SANPHAM.Find(id);
                    int idsanpham = sanpham.Sanpham_ID;
                    var chitiethoadons = db.CHITIETHOADON.Where(ct => ct.Sanpham_ID == idsanpham).ToList();
                    db.CHITIETHOADON.RemoveRange(chitiethoadons);
                    db.SANPHAM.Remove(sanpham);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công");
                    this.Close();    

                }
                else if (result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
                DataChangedEventHandler handler = DataChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
