using BaiTapCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Document.NET;
using Xceed.Words.NET;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.Win32;

namespace BaiTapCuoiKi.View
{
    /// <summary>
    /// Interaction logic for ChiTietDonHangWindow.xaml
    /// </summary>
    public partial class ChiTietDonHangWindow : Window
    {
        connect db = new connect();
        private CHITIETHOADON selected;
        private int id = -1;
        public ChiTietDonHangWindow()
        {

            connect db = new connect();
            InitializeComponent();
        }
        public ChiTietDonHangWindow(int idSelected) : this()
        {
            id = idSelected;
            hienthidulieuhoadon();

        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public void hienthidulieuhoadon()
        {
            var hoadonSelected = db.HOADON.Find(id);
            tbl_mahoadon.Text= hoadonSelected.Donhang_ID.ToString();
            tbl_tenkhachhang.Text= hoadonSelected.KHACHHANG.Khachhang_ten.ToString();
            tbl_ngaydathang.Text = hoadonSelected.Donhang_ngaydathang.ToString();
            string formattedText = TinhTongTien(hoadonSelected).ToString("#,0.##");
            tbl_trigia.Text = formattedText;
            if (hoadonSelected != null)
            {
                var chiTietHoaDon = db.CHITIETHOADON.Where(ct => ct.Donhang_ID == hoadonSelected.Donhang_ID).ToList();
                dgChiTietHoaDon.ItemsSource = chiTietHoaDon;
            }

        }
        public decimal TinhTongTien(HOADON hoadonSelected)
        {
            decimal tongGiaTri = 0;

           
            if (hoadonSelected != null && hoadonSelected.CHITIETHOADON != null)
            {
                
                foreach (var chiTietHoaDon in hoadonSelected.CHITIETHOADON)
                {
                    decimal giaTriChiTiet = (decimal)(chiTietHoaDon.CTHD_soluong * chiTietHoaDon.SANPHAM.Sanpham_gia);
                    tongGiaTri += giaTriChiTiet;
                }
            }

            return tongGiaTri;
        }

        private void btn_thoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_inhoadon_Click(object sender, RoutedEventArgs e)
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
