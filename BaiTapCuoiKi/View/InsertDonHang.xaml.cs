using BaiTapCuoiKi.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Xceed.Words.NET;
using Microsoft.Win32;
using Xceed.Document.NET;
using System.Runtime.InteropServices;

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for InsertDonHang.xaml
    /// </summary>
    public partial class InsertDonHang : Window
    {
        connect db = new connect();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        private CHITIETHOADON selected;
        private int id = -1;
        private int hoadonIDmoi = -1;
        private decimal tongGiaTri = 0;
        private List<CHITIETHOADON> chiTietHoaDonList = new List<CHITIETHOADON>();

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        public InsertDonHang()
        {
            connect db = new connect();
            InitializeComponent();
            hienthitenkhachhang();
            tb_cthdsoluong.PreviewTextInput += chichonhapso;
            hienthitensanpham();
            cbb_sanpham.SelectionChanged += ComboBox_SelectionChanged;
            DateTime currentTime = DateTime.Now;

            // Đặt thời gian vào DatePicker
            tb_thoigiandat.SelectedDate = currentTime;
            cbb_khachhang.SelectionChanged+= ComboBox_SelectionChanged;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbb_sanpham.SelectedIndex >= 0)
            {
                string selectedValue = ((SANPHAM)cbb_sanpham.SelectedItem).Sanpham_gia.ToString();
                tb_dongia.Text = selectedValue;
            }

            if (cbb_khachhang.SelectedIndex >= 0)
            {
                string selectedValue = ((KHACHHANG)cbb_khachhang.SelectedItem).Khachhang_diachi.ToString();
                tb_diachi.Text = selectedValue;
            }
        }
        public InsertDonHang(int idSelected) : this()
        {
            id = idSelected;
            hienthitensanpham();
            hienthitenkhachhang();
            hienThiDuLieuSua();
            var tonggiatrihoadonhientai = db.HOADON.Find(id);
            tongGiaTri = (decimal)tonggiatrihoadonhientai.Donhang_tonggiatri;
        }
        public void hienThiDuLieuSua()
        {
            var hoadonSelected = db.HOADON.Find(id);
            hoadonIDmoi = hoadonSelected.Donhang_ID;
            if (hoadonSelected != null)
            {
                
                KHACHHANG khachhang = db.KHACHHANG.Find(hoadonSelected.Khachhang_ID);
                cbb_khachhang.SelectedItem = khachhang;
                tb_thoigiandat.Text = hoadonSelected.Donhang_ngaydathang.ToString();
                /*tb_mahoadon.Text = hoadonSelected.Donhang_ID.ToString();*/
                getDataChiTietHoaDon(hoadonSelected.Donhang_ID);
            }
            
        }

        public void getDataChiTietHoaDon(int hoaDonID)
        {
            var chiTietHoaDon = db.CHITIETHOADON.Where(ct => ct.Donhang_ID == hoaDonID).ToList();
            dgChiTietHoaDon.ItemsSource = chiTietHoaDon;
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
        public void hienthitenkhachhang()
        {
            var listtenkhachhang = db.KHACHHANG.ToList();
            cbb_khachhang.ItemsSource = listtenkhachhang;
        }
        public void hienthitensanpham()
        {
            var listtensanpham = db.SANPHAM.ToList();
            cbb_sanpham.ItemsSource = listtensanpham;
        }
        private void btn_luuHD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (hoadonIDmoi != -1)
                {
                    int khachhang = int.Parse(cbb_khachhang.SelectedValue.ToString());
                    string thoigiandat = tb_thoigiandat.Text;

                    // Tìm hóa đơn mới tạo theo ID
                    var hoadonmoitao = db.HOADON.FirstOrDefault(x => x.Donhang_ID == hoadonIDmoi);

                    if (hoadonmoitao != null)
                    {
                        hoadonmoitao.Khachhang_ID = khachhang;
                        hoadonmoitao.Donhang_ngaydathang = thoigiandat;
                        hoadonmoitao.Donhang_tonggiatri = tongGiaTri;
                        db.SaveChanges();
                        MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        this.Close();
                     
                    }
                }
                DataChangedEventHandler handler = DataChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs ());    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }


        private void btn_thoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SuaCTHD_Click(object sender, RoutedEventArgs e)
        {
            CHITIETHOADON chitiethoadonSelected = (CHITIETHOADON)dgChiTietHoaDon.SelectedItem;
            tb_cthdsoluong.Text=chitiethoadonSelected.CTHD_soluong.ToString();

            
        }
     

        private void XoaCTHD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn xóa dữ liệu?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CHITIETHOADON hoadonSelected = (CHITIETHOADON)dgChiTietHoaDon.SelectedItem;
                    int idsanphamSelected = hoadonSelected.Sanpham_ID;
                    int idhoadonSelected = hoadonSelected.Donhang_ID;

                    var cthd = db.CHITIETHOADON.Where(ct => ct.Sanpham_ID == idsanphamSelected && ct.Donhang_ID == idhoadonSelected).FirstOrDefault();
                    db.CHITIETHOADON.Remove(cthd);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công");

                    tongGiaTri -= (decimal)hoadonSelected.CTHD_soluong * (decimal)hoadonSelected.SANPHAM.Sanpham_gia;

                    connect db2 = new connect();

                    dgChiTietHoaDon.ItemsSource = db2.CHITIETHOADON.Where(x => x.Donhang_ID == hoadonIDmoi).ToList();
                }
                else if (result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btn_themCTHD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (hoadonIDmoi == -1)
                {
                    var hoadon = new HOADON();
                    db.HOADON.Add(hoadon);
                    db.SaveChanges();
                    hoadonIDmoi = hoadon.Donhang_ID;
                }
               /* tb_mahoadon.Text =hoadonIDmoi.ToString();*/
                int sanpham = int.Parse(cbb_sanpham.SelectedValue.ToString());
                int soluong = int.Parse(tb_cthdsoluong.Text);

                // Kiểm tra xem đã tồn tại đối tượng có cùng khóa chính chưa
                var existingChiTietHoaDon = chiTietHoaDonList.FirstOrDefault(cthd => cthd.Sanpham_ID == sanpham);

                if (existingChiTietHoaDon != null)
                {
                    // Nếu đã tồn tại, cập nhật thông tin
                    existingChiTietHoaDon.CTHD_soluong += soluong;
                    tongGiaTri += soluong * decimal.Parse(tb_dongia.Text);
                }
                else
                {
                    // Nếu chưa tồn tại, thêm mới
                    var chitiethoadon = new CHITIETHOADON();
                    chitiethoadon.Donhang_ID = hoadonIDmoi;
                    chitiethoadon.Sanpham_ID = sanpham;
                    chitiethoadon.CTHD_soluong = soluong;
                    db.CHITIETHOADON.Add(chitiethoadon);
                    chiTietHoaDonList.Add(chitiethoadon);
                    tongGiaTri += (decimal)chitiethoadon.CTHD_soluong * (decimal)chitiethoadon.SANPHAM.Sanpham_gia;
                }
                db.SaveChanges();
                MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                connect db2 = new connect();

                dgChiTietHoaDon.ItemsSource = db2.CHITIETHOADON.Where(x => x.Donhang_ID == hoadonIDmoi).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void xuatFileWord_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".docx";
            saveFileDialog.Filter = "Word Files|*.docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;

                    using (DocX document = DocX.Create(filePath))
                    {
                        // Tạo một tiêu đề
                         document.InsertParagraph("HÓA ĐƠN")
                            .Bold()
                            .FontSize(18)
                            .Alignment = Alignment.center;

                        // Thêm thông tin hóa đơn
                        foreach (CHITIETHOADON hoadon in dgChiTietHoaDon.Items)
                        {
                            var maDonHangParagraph = document.InsertParagraph($"Mã Đơn Hàng: {hoadon.Donhang_ID}")
                                .Bold()
                                .FontSize(12);
                            maDonHangParagraph.SpacingAfter(10); // Khoảng cách sau Mã Đơn Hàng

                            var tenKhachHangParagraph = document.InsertParagraph($"Tên Khách Hàng: {hoadon.HOADON.KHACHHANG.Khachhang_ten}")
                                .FontSize(12);
                            tenKhachHangParagraph.SpacingAfter(20); // Khoảng cách sau Tên Khách Hàng
                            break;
                        }

                        var table = document.AddTable(dgChiTietHoaDon.Items.Count + 1, 3);
                        table.Alignment = Alignment.center;
                        table.Design = TableDesign.TableGrid;

                        // Thiết lập tên các cột trong Word
                        string[] arrColumnHeader =
                        {
                     "Tên Sản Phẩm", "Số Lượng", "Giá Sản Phẩm"
                };

                        for (int i = 0; i < arrColumnHeader.Length; i++)
                        {
                            table.Rows[0].Cells[i].Paragraphs[0].InsertText(arrColumnHeader[i]);
                            table.Rows[0].Cells[i].Paragraphs[0].Bold();
                        }

                        // Đổ dữ liệu vào bảng
                        int row = 1;
                        foreach (CHITIETHOADON hoadon in dgChiTietHoaDon.Items)
                        {
                            table.Rows[row].Cells[0].Paragraphs[0].InsertText(hoadon.SANPHAM.Sanpham_ten);
                            table.Rows[row].Cells[1].Paragraphs[0].InsertText(hoadon.CTHD_soluong.ToString());
                            table.Rows[row].Cells[2].Paragraphs[0].InsertText(hoadon.SANPHAM.Sanpham_gia.ToString());

                            row++;
                        }

                        document.InsertTable(table);

                        foreach (CHITIETHOADON hoadon in dgChiTietHoaDon.Items)
                        {
                            // Ngày Đặt Hàng
                            var ngayDatHangParagraph = document.InsertParagraph($"Ngày Đặt Hàng: {hoadon.HOADON.Donhang_ngaydathang}")
                                                              .Bold()
                                                              .FontSize(12);
                            ngayDatHangParagraph.Alignment = Alignment.right;
                            ngayDatHangParagraph.SpacingAfter(10); 

                            // Tổng Tiền
                            var tongTienParagraph = document.InsertParagraph($"Tổng Tiền: {hoadon.HOADON.Donhang_tonggiatri}")
                                                            .FontSize(12);
                            tongTienParagraph.Alignment = Alignment.right;
                            tongTienParagraph.SpacingAfter(20); 
                            break;
                        }

                        document.Save();

                        MessageBox.Show("Xuất Word thành công!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất file Word: " + ex.Message);
                }
            }
        }

       
    }
}
