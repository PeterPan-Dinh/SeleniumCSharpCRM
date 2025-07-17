using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(3)]
    internal class DataCuaToiTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// table renderer (used for wait method)
        private string crmRender = "kanban_renderer", bookingRender = "form_sheet_bg", listRenderer = "list_renderer",
        /// title, attribute fields on Form ...
        moiButton = "/button[.='Mới' or .=' Mới ']", opportunityNamePlaceHolder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", khachHang = "Khách hàng", tenCongTyId = "parent_id", maBenhNhan = "Mã bệnh nhân",
        ngaySinh = "Ngày sinh", namSinhNhapKhiKhongCoNgaySinh = "Năm sinh (nhập khi không có ngày sinh)", diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTp = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        gioiTinh = "Giới tính", loaiNguoiBenh = "Loại người bệnh", cCCD = "CCCD/CMND", nguoiPhuTrach = "Người phụ trách", nhanVienCSKH = "Nhân viên CSKH", the = "Thẻ", donvidichvu = "Đơn vị dịch vụ", nguonDV = "Nguồn DV", ngayBooking = "Ngày booking", trangThaiBooking = "Trạng thái booking", loaiBooking = "Loại booking",
        /// Notebookheader
        ghiChuNoiBo = "Ghi chú nội bộ", thongTinThem = "Thông tin thêm", thongTinKhac = "Thông tin Khác", ketNoi = "Kết nối", chiSoBETA = "Chỉ số BETA", lichSuKham = "Lịch sử khám", dsNguoiDiCung = "DS người đi cùng",
        nhanSuAds = "Nhân sự Ads", loaiNguon = "Loại nguồn", cbnvGioiThieu = "CBNV Giới thiệu", bacSi = "Bác sĩ", dieuDuong = "Điều dưỡng", cSKH = "CSKH", tickets = "Tickets",
        /// global data
        leadName = "QA auto lead dakhoa " + dateTimeNow,
        contactName = "QA auto lh dakhoa " + dateTimeNow,
        opportunityName = "Cơ hội của QA auto dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_Cohoi_Create_Cohoi_Create_Booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? tenCongTyData = null, maBenhNhanData = null, ngaySinhData = null, gioiTinhData = null, loaiNguoiBenhData = null, cCCDData = null,
            dienThoaidata = null, dienThoaidata2 = null, dienThoaidata3 = null, streetData = null, tinhTpData = null, districtData = null, wardData = null, tagData = null, tagCohoiData = null, nguoiPhuTrachData = null,
            doanhThuThucData = null, dongDuKienData = null, emailData = null, trangWebData = null, nguonDVData = null;
            /// title, attribute fields on Form
            const string inputSearch = "qa auto", searchboxRole = "searchbox", toChucLienHe = "Tổ chức/Liên hệ", coHoi = "Cơ hội", email = "Email", trangWeb = "Trang web",
            dienThoai = "Điện thoại", doanhThuDuKien = "Doanh thu dự kiến",
            them = "Thêm", sua = "Sửa", duLieuMoi = "Dữ liệu mới", doanhThuDuKienId = "expected_revenue", nhanVienkinhDoang = "Nhân viên kinh doanh",
            doanhThuThuc = "Doanh thu thực", dongDuKien = "Đóng dự kiến", threeStars = "Rất cao", mat = "Mất", lyDoMat = "Lý do mất", vdGiDaXayRaPlaceHolder = "Vấn đề gì đã xảy ra?";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Cohoi Test 001 - Create Co hoi, create booking from Co hoi");
            try
            {
                // Log into the application
                //LoginAction.Instance.LoginSite(60, urlInstance);
                LoginAction.Instance.LoginSiteXRender(60, null, null, null, General.listRenderer);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }

                //// Switch User
                //GeneralAction.Instance.SwitchUser(10, "Minh Đăng", "Minh Đăng");

                // Go to 'Data của tôi' >> 'Chu trình' page (list view)
                GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi)
                                      .WaitForElementVisible(10, General.breadcrumbItem("Chu trình"))
                                      .WaitForElementVisible(10, General.listRenderer).Sleep(1000)
                                      .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(1500); 
                
                // Click on 'MỚI' button
                GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton) //.CLickButtonTitle(10, General.moi.Replace("Mới", " Mới "))
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                #region Input data to create 'Co hoi'
                /* Tao co hoi o man hinh kanban
                GeneralAction.Instance.InputFieldLabel(10, toChucLienHe, "QA auto tclh " + dateTimeNow)
                                      .InputFieldLabel(10, coHoi, "Cơ hội của QA auto " + dateTimeNow, vdGiaSanPhamPlaceHolder) // Ex: KH quan tam gia san pham
                                      .InputFieldLabel(10, email, "qaauto@connext.com")
                                      .InputFieldLabel(10, dienThoai, "02219999991")
                                      .InputFieldLabel(10, doanhThuDuKien, "9.000.000")
                                      .CLickPriorityStar(10, doanhThuDuKien, "Rất cao")
                                      .CLickButtonTitleInKabanDialog(10, them); Thread.Sleep(1000);
                */

                // Tao co hoi o man hinh detail (list)
                GeneralAction.Instance.InputByAttributeValue(10, opportunityNamePlaceHolder, opportunityName)
                                      /// tao lien he (dialog popup is shown if inputting data at 'Khach hang')
                                      .InputFieldLabel(10, khachHang, contactName).PressEnterKeyboard()
                                      .WaitForElementVisible(10, General.DialogShow)
                                      //.InputByAttributeValueDialog(10, tenCongTyId, tenCongTyData = "Connext - 098 765 43 21").PressEnterKeyboard(); // Bug 797 missing this field if inputting data for 'Tên công ty'
                                      .InputFieldLabelDialog(10, maBenhNhan, maBenhNhanData = "QAAUTOMBN" + dateTimeNow)
                                      .InputFieldLabelDialog(10, ngaySinh, ngaySinhData = "07/11/1999").PressEnterKeyboard().Sleep(500)
                                      .CLickAndSelectItemInDropdownLabel(10, gioiTinh, gioiTinhData = "Nam")
                                      .InputFieldLabelDialog(10, loaiNguoiBenh, loaiNguoiBenhData = "Siêu Âm").PressEnterKeyboard().Sleep(500)
                                      .InputFieldLabelDialog(10, cCCD, cCCDData = "112233445566")
                                      ///.InputFieldLabelDialog(10, donvidichvu, "Đa Khoa") // --> auto fill
                                      .InputByAttributeValueDialog(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                      .InputFieldLabelDialog(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                      .InputFieldLabelDialog(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                      .InputFieldLabelDialog(10, diaChi, streetData = "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard()
                                      .InputFieldLabelDialog(10, diaChi, tinhTpData = "TP Hồ Chí Minh", tinhTp).PressEnterKeyboard() // Bug 797 missing this field if inputting data for 'Tên công ty'
                                      .InputFieldLabelDialog(10, diaChi, districtData = "Quận 1", quanHuyenPlaceHolder).PressEnterKeyboard()
                                      .InputFieldLabelDialog(10, diaChi, wardData = "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard()
                                      .InputFieldLabelDialog(10, the, tagData = "VIP").PressEnterKeyboard()
                                      .InputFieldLabelDialog(10, nguoiPhuTrach, nguoiPhuTrachData = "Dương Ngọc Vy").PressEnterKeyboard().Sleep(1000)
                                      /// NotebookHeader (Liên hệ dialog - Thông tin thêm)
                                      .CLickNotebookHeader(10, thongTinKhac)
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard()
                                      .WaitForElementVisible(10, General.elementHtml("label", cbnvGioiThieu))
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, cbnvGioiThieu, "BS Đỗ Thị Thi").PressEnterKeyboard()
                                      .CLickButtonTitleInDialog(10, General.luuVaDong).Sleep(1000);
                // Back to Co hoi detail to continue to input data
                GeneralAction.Instance.WaitForElementInvisible(30, General.DialogShow);
                ///.InputFieldLabel(10, donvidichvu, "Đa Khoa") // --> auto fill
                ///.InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input") // --> auto fill
                //GeneralAction.Instance.InputFieldLabel(10, nguonDV, nguonDVData = "[BHYT-CN-001] Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân)").PressEnterKeyboard(); // Bug: missing data after saving
                GeneralAction.Instance.InputFieldLabel(10, the, tagCohoiData = "Khám lẻ").PressEnterKeyboard()
                                      //.InputFieldLabel(10, doanhThuThuc, doanhThuThucData = "1.500.000,00") // remove this field base on new spec
                                      //.InputFieldLabel(10, dongDuKien, dongDuKienData = DateTime.Now.ToString("dd/MM/yyyy")) // remove this field base on new spec
                                      .InputFieldLabel(10, email, emailData = "qaauto@connext.com")
                                      .InputFieldLabel(10, trangWeb, trangWebData = "qaautoWebsite.com")
                                      ///.InputFieldLabel(10, diaChi, streetData = "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard() // --> auto fill
                                      ///.InputFieldLabel(10, diaChi, tinhTpData = "TP Hồ Chí Minh", tinhTp).PressEnterKeyboard() // --> auto fill
                                      .InputFieldLabel(10, diaChi, districtData = "Quận 1", quanHuyenPlaceHolder).PressEnterKeyboard()
                                      .InputFieldLabel(10, diaChi, wardData = "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard()
                                      /// NotebookHeader (Cơ hội - Ghi chú nội bộ)
                                      .CLickNotebookHeader(10, ghiChuNoiBo)
                                      .InputNotebookHeaderDescriptionContent(10, ghiChuNoiBo, "QA auto " + ghiChuNoiBo)
                                      /// NotebookHeader (Cơ hội - Thông tin thêm - Marketing)
                                      .CLickNotebookHeader(10, thongTinThem)
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, nhanSuAds, "Hoàng Tùng Lâm").PressEnterKeyboard()
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard()
                                      .WaitForElementVisible(10, General.elementHtml("label", cbnvGioiThieu))
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, "BS Đỗ Thị Thi").PressEnterKeyboard()
                                      /// NotebookHeader (Cơ hội - Thông tin thêm - Nhân sự tại điểm chạm)
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, bacSi, "Đỗ Thị Thi").PressEnterKeyboard()
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, dieuDuong, "Bùi Thị Hồng").PressEnterKeyboard()
                                      .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cSKH, "Dương Ngọc Vy").PressEnterKeyboard()
                                      .CLickNotebookHeader(10, ketNoi)
                                      .CLickNotebookHeader(10, chiSoBETA)
                                      .CLickNotebookHeader(10, lichSuKham)
                                      .CLickNotebookHeader(10, dsNguoiDiCung)
                                      //.CLickPriorityStar(10, dongDuKien, "Rất cao") // remove this field base on new spec (Phương tiện / Cao / Rất cao)
                                      .CLickButtonTitle(10, General.luu).Sleep(500);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }

                /// Check if the msg ' Không thể lưu lại ' is shown then Click 'Lưu' button to save
                Thread.Sleep(2000);
                if (GeneralAction.Instance.IsElementPresent(General.elementHtml("div", "", "//*[contains(text(),'Không thể lưu lại')]")))
                {
                    GeneralAction.Instance.CLickButtonTitle(10, General.luu).Sleep(500);
                }
                #endregion

                #region Verify data of the created 'co hoi'
                // Back to man hinh 'co hoi' list view by cLicking breadcrumb title
                GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình");

                // Search the new created 'co hoi'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.SearchViewInput(60, coHoi, inputSearch);

                // Verify data of the created 'co hoi' is shown correctly
                string nameAttrVal = "create_date", data;
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_open", data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày phân công' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Quan tâm ads");
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Dữ Liệu Mới") // old: "Quan tâm ads
                           || GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Dữ liệu mới");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giai đoạn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = opportunityName);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_telesale_note", data = "");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú telesale' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "tag_ids", data = "Khám lẻ");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thẻ' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "patient_id", data = maBenhNhanData);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Mã bệnh nhân' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_name", data = contactName);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên liên hệ' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "phone", "data-tooltip", data = dienThoaidata.Replace("19", "1 9").Replace("90", "9 0")); //  0221 9999 001
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Điện thoại' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "street2", data = "Phường Phạm Ngũ Lão, Quận 1");
                //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Quận/Huyện' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "city", data = "TP Hồ Chí Minh");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thành phố' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "activity_ids", "data-tooltip", data = "Không có dữ liệu");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Hoạt động' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "my_activity_date_deadline", data = "");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đến hạn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullname);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "TEAM NHI");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_id", data = "Offline / Giới thiệu / CBNV - BVĐK HN");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại nguồn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "product_ids", data = nguonDVData);
                //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nguồn DV' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_detail_id", data = "");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Chiến dịch' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /* Verify data at man hinh chi tiat 'co hoi' 
                // Click on Record title of a column to verify data
                GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                // Verify data of the created 'co hoi'
                string data = "Cơ hội của QA auto " + dateTimeNow;
                verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityNamePlaceHolder, data);
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, doanhThuDuKienId, data= "9.000.000");
                //verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + doanhThuDuKien + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC); // Ticket 465

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khachHang, data="");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + khachHang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = "0221 9999 991");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + dienThoai + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, email, data = "qaauto@connext.com");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + email + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, doanhThuThuc, data = "0,00");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + doanhThuThuc + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.IsPriorityStarShown(10, dongDuKien, threeStars, data = "aria-checked");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + dongDuKien + "'-'" + threeStars + "' title in 'Chu trình' is shown: '" + data.Replace("aria-","") + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.HtmlElementGetTextValue(10, "div", "user_id", data = LoginPage.fullname, "//input"); // //span[position()=1]; DataInputFieldTitleGetText(10, nhanVienkinhDoang, data = LoginPage.fullname)
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội'-'" + nhanVienkinhDoang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion

                #region Create booking from 'co hoi'
                // Click the created 'co hoi' to go to details page
                GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, opportunityName)
                                      .WaitForElementVisible(10, General.formEditable)
                                      .CLickButtonTitle(10, "Booking mới")
                                      .WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitleInDialog(10, General.luu.Replace("Lưu", " Lưu "))
                                      .WaitForElementInvisible(10, General.DialogShow);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }
                #endregion

                #region Verify the new created booking (via co hoi)
                // Click on 'xBooking' button
                GeneralAction.Instance.CLickButtonTitle(10, "1Booking")
                                      .WaitForElementVisible(10, General.rendererTable("form_sheet position"))
                                      .Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(60).Sleep(2000); 

                // Verify the new created booking
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khachHang, data = contactName + " - " + dienThoaidata.Replace("19", "1 9").Replace("90", "9 0") + " - " + ngaySinhData);
                verifyPoints.Add(summaryTC = "Verify data of '" + khachHang + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "service_unit_id", data = "Đa Khoa", "//*[text()]"); // Dơn vị dịch vụ
                verifyPoints.Add(summaryTC = "Verify data of '" + donvidichvu + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.HtmlElementGetTextValue(10, "input", "opportunity_id", data = opportunityName, "[not(@id = 'opportunity_id_1')]"); // Cơ hội
                verifyPoints.Add(summaryTC = "Verify data of '" + coHoi + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, nguonDV, "1", data = nguonDVData);
                //verifyPoints.Add(summaryTC = "Verify data of '" + nguonDV + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC); // Bug --> ko nhập dc data Nguon DV do bị read-only

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayBooking, data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data of '" + ngayBooking + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, trangThaiBooking, data = "");
                verifyPoints.Add(summaryTC = "Verify data of '" + trangThaiBooking + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, loaiBooking, data = "Khám");
                verifyPoints.Add(summaryTC = "Verify data of '" + loaiBooking + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, bacSi, data = "");
                verifyPoints.Add(summaryTC = "Verify data of '" + bacSi + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, tickets, data = "");
                verifyPoints.Add(summaryTC = "Verify data of '" + tickets + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'Thông tin khác' tab (Notebookheader) to verify 'Loại nguồn'
                GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac.Replace("Khác", "khác"));

                // Verify data of 'Loại nguồn' is get from 'Co hoi'
                verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac.Replace("Khác", "khác"), loaiNguon, data = "Offline / Giới thiệu / CBNV - BVĐK HN", "a");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac.Replace("Khác", "khác") + "' - '" + loaiNguon + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac.Replace("Khác", "khác"), cbnvGioiThieu, data = "BS Đỗ Thị Thi", "a");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac.Replace("Khác", "khác") + "' - '" + cbnvGioiThieu + "' title in 'Booking' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete the created booking
                GeneralAction.Instance.ThucHienXoaDelete(60).Sleep(3000);
                #endregion

                #region Delete the created 'co hoi'
                // Back to man hinh 'co hoi' list view by cLicking breadcrumb title
                GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình");

                // Delete all these created 'co hoi'
                GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60).Sleep(1000);

                // Verify the created client is deleted 
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created 'co hoi' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete the created 'lien he'
                // Go to 'Liên hệ' page (list view)
                GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer); Thread.Sleep(1500);

                // Search the created 'lien he'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(3000);

                // Delete all these created 'lien he'
                GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60).Sleep(1000);

                // Verify the created 'lien he' is deleted 
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created 'lien he' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Mark 'Lost' ==> NO RUN
                /*
                GeneralAction.Instance.CLickButtonTitle(10, mat)
                                      .WaitForElementVisible(10, General.DialogShow)
                                      .InputFieldLabel(10, lyDoMat, "Thời gian chờ quá lâu")
                                      .InputByAttributeValueDialog(10, "", "QA auto " + vdGiDaXayRaPlaceHolder, "p") // command-temporary-hint
                                      .CLickButtonTitleInDialog(10, "Gửi")
                                      .WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                // Verify ribbon top right is displayed
                verifyPoint = GeneralAction.Instance.IsHtmlElementShown(10, "span", data= "Mất");
                verifyPoints.Add(summaryTC = "Verify data of 'Cơ hội' đánh dấu 'MẤT' in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion

                #region Restore 'Co hoi' ==> NO RUN
                /*
                // Back to 'Chu Trinh' page
                GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, General.crm)
                                      .WaitForElementVisible(10, General.rendererTable(crmRender)); Thread.Sleep(1500);

                // Click 'Filter' (Bộ lọc) and select 'lost' (Mất)
                GeneralAction.Instance.CLickButtonTitle(10, General.boLoc)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Mất")
                                      .WaitForElementVisible(10, General.titleRecordKanbanInCol(duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)); Thread.Sleep(1000);

                // Click on Record title of a column
                GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                // Click 'Khôi phục' (Restore) button
                GeneralAction.Instance.CLickButtonTitle(10, "Khôi phục"); Thread.Sleep(3000);

                // Verify 'Cơ hội' is restored (Khôi phục)
                verifyPoint = GeneralAction.Instance.ChatterMessageRightPaneGetText(10, data = "Cơ hội được khôi phục");
                verifyPoints.Add(summaryTC = "Verify 'Cơ hội được khôi phục' in 'Chu trình' is shown on Chatter: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion

                #region Delete 'Co hoi' KH ==> NO RUN
                /*
                // Click on 'Thuc Hien' --> 'Xoa'
                GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1500);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }

                GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình")
                                      .WaitForElementVisible(10, General.rendererTable(crmRender)); Thread.Sleep(1500);

                // search the deleted 'Cơ hội'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.CLickRemovefilterSearchBox(10, "Mất") // Remove filter 'Mất' in seachbox
                                      .InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Verify the created 'Co hoi' is deleted
                verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted(duLieuMoi, data = "Cơ hội của QA auto " + dateTimeNow);
                verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC002_Cohoi_Create_ChangeStatus_Cohoi()
        {
            #region Variables declare
            string urlInstance = LoginPage.url,
            dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            /// title, attribute fields on Form
            const string inputSearch = "qa auto", searchboxRole = "searchbox", toChucLienHe = "Tổ chức/Liên hệ", coHoi = "Cơ hội", email = "Email",
            dienThoai = "Điện thoại", doanhThuDuKien = "Doanh thu dự kiến", vdGiaSanPhamPlaceHolder = "vd: Giá sản phẩm",
            them = "Thêm", sua = "Sửa", duLieuMoi = "Dữ liệu mới", daDatHen = "Đã đặt hẹn";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - CRM Test 002 - Create, Change status Co hoi");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Go to CRM page
                GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, General.crm)
                                      .WaitForElementVisible(10, General.rendererTable(crmRender)); Thread.Sleep(1500);

                // Click "MOI" button
                GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                      .WaitForElementVisible(30, General.kanbanQuickCreate);

                #region Input data to create 'Co hoi'
                GeneralAction.Instance.InputFieldLabel(10, toChucLienHe, "QA auto tclh " + dateTimeNow)
                                      .InputFieldLabel(10, coHoi, "Cơ hội của QA auto " + dateTimeNow, vdGiaSanPhamPlaceHolder) // Ex: KH quan tam gia san pham
                                      .InputFieldLabel(10, email, "qaauto@connext.com")
                                      .InputFieldLabel(10, dienThoai, "02219999991")
                                      .InputFieldLabel(10, doanhThuDuKien, "9.000.000")
                                      .CLickPriorityStar(10, doanhThuDuKien, "Rất cao")
                                      .CLickButtonTitleInKabanDialog(10, them); Thread.Sleep(1000);
                #endregion

                #region Change status 'Co hoi'
                // Click on Record title of a column to verify data
                GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                // Click 'X' status button
                string data = "Đã tiếp cận";
                GeneralAction.Instance.CLickButtonTitle(10, data); Thread.Sleep(2000);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-2 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'X' status button
                GeneralAction.Instance.CLickButtonTitle(10, data = "Rất tiềm năng"); Thread.Sleep(2500);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-3 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'X' status button
                GeneralAction.Instance.CLickButtonTitle(10, data = "Cần đặt hẹn lại"); Thread.Sleep(2500);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-4 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); 

                // Click 'X' status button
                GeneralAction.Instance.CLickButtonTitle(10, data = "Đã chăm sóc sau khám");
                verifyPoint = GeneralAction.Instance.IsRewardFaceGroupStatusShown(10);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-5 (Reward face group) of 'Cơ hội' in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                GeneralAction.Instance.WaitForElementInvisible(10, General.rewardFaceGroupStatus);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-5 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'X' status button
                GeneralAction.Instance.CLickButtonTitle(10, data = "Chưa chăm sóc sau khám");
                verifyPoint = GeneralAction.Instance.IsRewardFaceGroupStatusShown(10);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-6 (Reward face group) of 'Cơ hội' in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                GeneralAction.Instance.WaitForElementInvisible(10, General.rewardFaceGroupStatus);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-6 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'X' status button
                GeneralAction.Instance.CLickButtonTitle(10, data = "Đã đặt hẹn"); // Work-around for issue duplucate stages for Staging CLickButtonTitle(10, data = "Đã đặt hẹn", 1)
                verifyPoint = GeneralAction.Instance.IsRewardFaceGroupStatusShown(10);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-7 (Reward face group) of 'Cơ hội' in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                GeneralAction.Instance.WaitForElementInvisible(10, General.rewardFaceGroupStatus);
                verifyPoint = GeneralAction.Instance.ChatterMessageChangedStatusSeparatorDateGetText(10, "Hôm nay", "1", "1", data);
                verifyPoints.Add(summaryTC = "Verify (Chatter) status-7 of 'Cơ hội' in 'Chu trình' is changed to: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete 'Co hoi'
                // Click on 'Thuc Hien' --> 'Xoa'
                GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);
                                      //.WaitForElementVisible(10, General.rendererTable(crmRender)); Thread.Sleep(1500);

                // Back to 'Chu Trinh' page
                GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình")
                                      .WaitForElementVisible(10, General.rendererTable(crmRender));

                // search the deleted 'Cơ hội'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Verify the created 'Co hoi' is deleted
                verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted(duLieuMoi, data = "Cơ hội của QA auto " + dateTimeNow);
                verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC003_Cohoi_ChuTrinh_Create_Cohoi_Booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url,
            dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            /// title, attribute fields on Form
            const string inputSearch = "qa auto", searchboxRole = "searchbox", toChucLienHe = "Tổ chức/Liên hệ", coHoi = "Cơ hội", email = "Email",
            dienThoai = "Điện thoại", doanhThuDuKien = "Doanh thu dự kiến", vdGiaSanPhamPlaceHolder = "vd: Giá sản phẩm", them = "Thêm", sua = "Sửa",
            baoGiaKhachHang = "Báo giá khách hàng", taoKhachHangmoiId = "create",
            duLieuMoi = "Dữ liệu mới", daDatHen = "Đã đặt hẹn", khachHang = "Khách hàng", ngayBookingId = "date_order", taoTheLieuTrinh = "Tạo thẻ liệu trình",
            donViDichVu = "Đơn vị dịch vụ", maTheLieuTrinh = "Mã thẻ liệu trình", theLieuTrinh = "Thẻ liệu trình", loaiBooking = "Loại booking", ngayPhatHanh = "Ngày phát hành", thoiGianTu = "Thời gian từ",
            thoiGianDen = "Thời gian đến", dichVuLieutrinh = "Dịch vụ liệu trình", trangThaiBooking = "Trạng thái booking", slThucHienLanNay = "SL thực hiện lần này",
            trangThaiTuVan = "Trạng thái tư vấn", bacSi = "Bác sĩ", ngayKhamThucTe = "Ngày khám thực tế", tickets = "Tickets",
            nguoiPhuTrach = "Người phụ trách", khackHang = "Khách hàng", doiNguKinhDoanh = "Đội ngũ kinh doanh",
            /// Notebookheader
            chiTietDonHang = "Chi tiết đơn hàng", sanPhamTuyChon = "Sản phẩm tuỳ chọn", thongTinKhac = "Thông tin khác", xacNhanOnline = "Xác nhận online?",
            thanhToanId = "require_payment", maThamChieuKH = "Mã tham chiếu khách hàng", the = "Thẻ", chungTuGoc = "Chứng từ gốc?", chienDich = "Chiến dịch?", phuongTien = "Phương tiện?", nguon = "Nguồn?";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - CRM Test 003 - Create Co hoi-Booking");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Go to CRM page
                GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, General.crm)
                                      .WaitForElementVisible(10, General.rendererTable(crmRender)); Thread.Sleep(1500);

                // Click "MOI" button
                GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                      .WaitForElementVisible(10, General.kanbanQuickCreate);

                #region Input data to create 'Co hoi'
                GeneralAction.Instance.InputFieldLabel(10, toChucLienHe, "QA auto tclh " + dateTimeNow)
                                      .InputFieldLabel(10, coHoi, "Cơ hội của QA auto " + dateTimeNow, vdGiaSanPhamPlaceHolder) // Ex: KH quan tam gia san pham
                                      .InputFieldLabel(10, email, "qaauto@connext.com")
                                      .InputFieldLabel(10, dienThoai, "02219999991")
                                      .InputFieldLabel(10, doanhThuDuKien, "9.000.000")
                                      .CLickPriorityStar(10, doanhThuDuKien, "Rất cao")
                                      .CLickButtonTitleInKabanDialog(10, them); Thread.Sleep(1000);
                #endregion

                #region Create Booking
                // Click on Record title of a column
                GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                // Click 'Booking mới' button
                GeneralAction.Instance.CLickButtonTitle(10, "Booking mới")
                                      .WaitForElementVisible(10, General.DialogShow)
                                      .ClickCheckboxAtFieldLabel(10, baoGiaKhachHang, taoKhachHangmoiId, "id")
                                      .CLickButtonTitleInDialog(10, "Xác nhận")
                                      .WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1500);

                // Input data to create Booking
                string theLieutrinhDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                GeneralAction.Instance.InputFieldLabel(10, khachHang, "Đặng Lê Vy - 0799 309 789")
                                      .InputByAttributeValue(10, ngayBookingId, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                      .ClickCheckboxAtFieldLabel(10, taoTheLieuTrinh)
                                      .CLickAndSelectItemInDropdownLabel(10, donViDichVu, "Đa khoa")
                                      .InputFieldLabel(10, maTheLieuTrinh, "QA auto " + maTheLieuTrinh + " " + theLieutrinhDate)
                                      //.InputFieldLabel(10, theLieuTrinh, "QA auto " + theLieuTrinh)
                                      .CLickAndSelectItemInDropdownLabel(10, loaiBooking, "Tái khám")
                                      //.WaitForElementVisible(10, General.DialogShow)
                                      //.CLickButtonTitleInDialog(10, "Tạo")
                                      //.WaitForElementInvisible(10, General.DialogShow)
                                      .InputFieldLabel(10, ngayPhatHanh, DateTime.Now.ToString("dd/MM/yyyy"))
                                      .InputFieldLabel(10, thoiGianTu, "01:00")
                                      .InputFieldLabel(10, thoiGianDen, "02:30")
                                      .InputFieldLabel(10, dichVuLieutrinh, "[BHYT-CN-001] Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân)")
                                      .CLickAndSelectItemInDropdownLabel(10, trangThaiBooking, "Chưa checkin")
                                      .InputFieldLabel(10, slThucHienLanNay, "0")
                                      .CLickAndSelectItemInDropdownLabel(10, trangThaiTuVan, "Ra bill")
                                      .InputFieldLabel(10, bacSi, "Nguyễn Lương Y")
                                      .InputFieldLabel(10, ngayKhamThucTe, DateTime.Now.ToString("dd/MM/yyyy"))
                                      //.InputFieldLabel(10, tickets, "QA auto " + tickets); // If input this will open a popup 'create Ticket' 
                                      /// NotebookHeader (Chi tiết đơn hàng, Sản phẩm tuỳ chọn, ...)
                                      .CLickNotebookHeader(10, chiTietDonHang)
                                      .ClickAddRowListTable(10, "2", "Thêm sản phẩm")
                                      .InputRowListTable(10, "2", "2", "[DK-KTQ-U40] Khám tổng quát người lớn dưới 40 tuổi")
                                      .ClickRecordPosXInRowListTable(10, "1", "6").ClickRecordPosXInRowListTable(10, "1", "6")
                                      //.ClickAddRowListTable(10, "3", "Thêm phần")
                                      //.InputRowListTable(10, "3", "2", "QA auto Thêm phần")
                                      //.ClickAddRowListTable(10, "4", "Thêm ghi chú")
                                      //.InputRowListTable(10, "4", "2", "QA auto Thêm ghi chú", "textarea")
                                      .CLickNotebookHeader(10, sanPhamTuyChon)
                                      .ClickAddRowListTable(10, "1", "Thêm sản phẩm")
                                      .InputRowListTable(10, "1", "2", "[IVF-DIAG] Khám vô sinh hiếm muộn") //  KHAM-12345] Khám tổng quát nhi
                                      .ClickRecordPosXInRowListTable(10, "1", "3").ClickAddRowListTable(10, "2", "Thêm sản phẩm") // workaround to input data for this
                                      .CLickNotebookHeader(10, thongTinKhac)
                                      .ClickCheckboxAtFieldLabel(10, xacNhanOnline, thanhToanId, "id")
                                      .InputFieldLabel(10, maThamChieuKH, "QA auto " + maThamChieuKH);

                GeneralAction.Instance.ClickInputNotebookHeaderAndFieldNameContent(10, thongTinKhac, the);
                //GeneralAction.Instance.PressInputTextKeyboard("Đa khoa"); Thread.Sleep(1000);
                GeneralAction.Instance.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, the, "Đa khoa"); Thread.Sleep(1000);

                //.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chungTuGoc, "Cơ hội của QA auto 01") // --> auto filled, no need to input
                GeneralAction.Instance.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chienDich, "CD-1234 Quảng cáo Facebook"); Thread.Sleep(500);
                GeneralAction.Instance.ClickInputNotebookHeaderAndFieldNameContent(10, thongTinKhac, the);
                GeneralAction.Instance.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, phuongTien, "Facebook"); Thread.Sleep(500);
                GeneralAction.Instance.ClickInputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chienDich);
                GeneralAction.Instance.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, nguon, "Twitter"); Thread.Sleep(500);
                GeneralAction.Instance.ClickInputNotebookHeaderAndFieldNameContent(10, thongTinKhac, phuongTien);
                GeneralAction.Instance.CLickButtonTitle(10, General.luuThuCong); Thread.Sleep(1500);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }
                #endregion

                #region Verify 'Co Hoi' >> 'Booking'
                // Back to 'Chu trinh / Co hoi cua Kh...' page
                GeneralAction.Instance.CLickItemBreadcrumb(10, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.rendererTable(bookingRender)); Thread.Sleep(3000);

                // Issue: Lỗi hệ thống RPC_ERROR Odoo Server Error due to input fields so fast --> Work around by Click 'Đồng ý' button
                /// Check if this error is shown then click 'Đồng ý' button
                if (GeneralAction.Instance.IsElementPresent(General.DialogShow)) // General.elementHtml("*", "Odoo Server Error")
                {
                    GeneralAction.Instance.CLickButtonTitle(10, "Đồng ý")
                                          .WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(2000);
                }

                // Click 'xBooking' button to verify data
                GeneralAction.Instance.CLickButtonTitle(10, "1Booking")
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(2000);

                // Verify data in Booking (Edit) page
                string data = "Cơ hội của QA auto " + dateTimeNow + " - 0221 9999 991";
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khackHang, data);
                verifyPoints.Add(summaryTC = "Verify data of '" + khackHang + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, ngayBookingId, data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data of 'Ngày booking' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, donViDichVu, data= "Đa khoa");
                verifyPoints.Add(summaryTC = "Verify data of '" + donViDichVu + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, theLieuTrinh, data = "[QA auto Mã thẻ liệu trình " + theLieutrinhDate + "] Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân) "); // [1 - Mới phát hành]
                verifyPoints.Add(summaryTC = "Verify data of '" + theLieuTrinh + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, loaiBooking, data = "Tái khám");
                verifyPoints.Add(summaryTC = "Verify data of '" + loaiBooking + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, thoiGianTu, data= "01:00");
                verifyPoints.Add(summaryTC = "Verify data of '" + thoiGianTu + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, thoiGianDen, data = "02:30");
                verifyPoints.Add(summaryTC = "Verify data of '" + thoiGianDen + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayPhatHanh, data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data of '" + ngayPhatHanh + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, trangThaiBooking, data = "Chưa checkin");
                verifyPoints.Add(summaryTC = "Verify data of '" + trangThaiBooking + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, trangThaiTuVan, data = "Ra bill");
                verifyPoints.Add(summaryTC = "Verify data of '" + trangThaiTuVan + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, slThucHienLanNay, data = "0");
                verifyPoints.Add(summaryTC = "Verify data of '" + slThucHienLanNay + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, bacSi, data = "Nguyễn Lương Y");
                verifyPoints.Add(summaryTC = "Verify data of '" + bacSi + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayKhamThucTe, data = DateTime.Now.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data of '" + ngayKhamThucTe + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, tickets, data = "");
                verifyPoints.Add(summaryTC = "Verify data of '" + tickets + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Notebook header (Chi tiết đơn hàng, Sản phẩm tuỳ chọn, Thông tin khác)
                GeneralAction.Instance.CLickNotebookHeader(10, chiTietDonHang);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "1", "2", data= "[BHYT-CN-001] Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân)", "span");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Sản phẩm' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "1", "3", data = "Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân)", "span");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Mô tả' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "4", data = "1");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Số lượng' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "6", data = "mm");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Đơn vị tính' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "7", data = "10.000.000");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Đơn giá' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "1", "8", data = "Thuế GTGT phải nộp 5%", "span");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Thuế' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "1", "9", data = "10.000.000 ₫", "span");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + chiTietDonHang + "' - 'Thành tiền' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "2", "2", data = "[DK-KTQ-U40] Khám tổng quát người lớn dưới 40 tuổi", "span");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Sản phẩm' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "2", "3", data = "[DK-KTQ-U40] Khám tổng quát người lớn dưới 40 tuổi", "span");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Mô tả' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "2", "4", data = "1");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Số lượng' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "2", "6", data = "Đơn vị");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Đơn vị tính' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "2", "7", data = "800.000");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Đơn giá' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "2", "8", data = "Thuế GTGT phải nộp 10%", "span");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Thuế' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "2", "9", data = "800.000 ₫", "span");
                verifyPoints.Add(summaryTC = "Verify the 2nd data of '" + chiTietDonHang + "' - 'Thành tiền' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "3", "2", data = "QA auto Thêm phần", "span");
                //verifyPoints.Add(summaryTC = "Verify data of '" + chiTietDonHang + "' - 'Thêm phần' in Booking is shown: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "4", "2", data = "QA auto Thêm ghi chú", "span");
                //verifyPoints.Add(summaryTC = "Verify data of '" + chiTietDonHang + "' - 'Thêm ghi chú' in Booking is shown: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                GeneralAction.Instance.CLickNotebookHeader(10, sanPhamTuyChon);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "2", data = "[IVF-DIAG] Khám vô sinh hiếm muộn");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + sanPhamTuyChon + "' - 'Sản phẩm' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "3", data = "[IVF-DIAG] Khám vô sinh hiếm muộn");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + sanPhamTuyChon + "' - 'Mô tả' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "4", data = "1,00");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + sanPhamTuyChon + "' - 'Số lượng' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "5", data = "Đơn vị");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + sanPhamTuyChon + "' - 'Đơn vị tính' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "6", data = "2.000.000,00");
                verifyPoints.Add(summaryTC = "Verify the 1st data of '" + sanPhamTuyChon + "' - 'Đơn giá' in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, nguoiPhuTrach, data=LoginPage.fullname);
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + nguoiPhuTrach + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, doiNguKinhDoanh, data = "Bán hàng");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + doiNguKinhDoanh + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, maThamChieuKH, data = "QA auto Mã tham chiếu khách hàng");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + maThamChieuKH + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataItemNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, the, "1", data = "Đa khoa");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + the + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, chungTuGoc, data = "Cơ hội của QA auto " + dateTimeNow);
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + chungTuGoc + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, chienDich, data = "CD-1234 Quảng cáo Facebook");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + chienDich + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, phuongTien, data = "Facebook");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + phuongTien + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, nguon, data = "Twitter");
                verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "' - '" + nguon + "' field in Booking is shown: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Go to 'Booking của tôi' page to verify the created Booking is shown
                GeneralAction.Instance.CLickMenuTitle(10, General.bookingCuaToi)
                                      .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                // Search the created Booking
                /// Input a value in seachbox to Search
                GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Đơn hàng", inputSearch) // search data with filter 'Cơ hội'
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Verify the created Booking is shown at the 'Booking của tôi' page
                string nameAttrVal = "date_order";
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày booking' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "source_booking", data = LoginPage.fullname);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Source' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "service_unit", data = "Đa khoa");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đơn vị dịch vụ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "partner_id", data = "Cơ hội của QA auto " + dateTimeNow + " - 0221 9999 991");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Khách hàng' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "service_booking", data = "Dịch vụ khám Bảo Hiểm Y Tế (Cá nhân)"); // (Tổng số buổi: 1, Số buổi đang book: 0)
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Dịch vụ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "booking_status", data = "Chưa checkin");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái booking' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "consulting_status", data = "Ra bill");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái tư vấn' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "actual_examination_date", data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày khám thực tế' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullname);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Người phụ trách' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "doctor_id", data = "Nguyễn Lương Y");
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Bác sĩ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete Booking at 'Booking của tôi' page
                GeneralAction.Instance.ClickCheckboxAlltable(10)
                                      .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                // Verify the created Booking is deleted
                data = "Cơ hội của QA auto " + dateTimeNow + " - 0221 9999 991";
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created Booking is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete 'Co Hoi'
                // Back to CRM-Chu trinh page
                GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, General.crm)
                                      .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                // Search the created 'Cơ hội'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch)
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Click on Record title of a column
                GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, duLieuMoi, "Cơ hội của QA auto " + dateTimeNow)
                                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                // Delete 'Co Hoi'
                // Click on 'Thuc Hien' --> 'Xoa'
                GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                // Back to CRM - Chu trinh page
                GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình")
                                      .WaitForElementVisible(10, General.rendererTable(crmRender));

                // search the deleted 'Cơ hội'
                /// Input a value in seachbox to Search
                GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Verify the created 'Co hoi' is deleted
                verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted(duLieuMoi, data = "Cơ hội của QA auto " + dateTimeNow);
                verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' at 'CRM-Chu trình' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify (Cơ hội của) 'Khách hàng cá nhân' is created at 'Liên hệ' page, and then delete it
                // Go to 'Liên hệ' page
                GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                      .WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, General.lienHe)
                                      .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                // Search the created client
                /// Input a value in seachbox to Search
                GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                      .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                      .CLickItemInDropdownSearchBox(10, "Tên", inputSearch)
                                      .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                // Verify the created client is shown
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = "Cơ hội của QA auto " + dateTimeNow);
                verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Delete this created client
                GeneralAction.Instance.ClickCheckboxAlltable(10)
                                      .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                      .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                      .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                // Verify the created client is deleted 
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created (Cơ hội của) 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }
    }
}
