using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(4)]
    internal class LienHeTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// table renderer (used for wait method)
        private string listRenderer = "list_renderer",
        /// title, attribute fields on Form
        caNhanId = "person", nameId = "name_2", tenCongTyId = "parent_id", maBenhNhan = "Mã bệnh nhân", dienThoai = "Điện thoại", phone = "Phone", phoneId = "phone",
        ngaySinh = "Ngày sinh", namSinhNhapKhiKhongCoNgaySinh = "Năm sinh (nhập khi không có ngày sinh)", diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTp = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        gioiTinh = "Giới tính", loaiNguoiBenh = "Loại người bệnh", cCCD = "CCCD/CMND", nguoiPhuTrach = "Người phụ trách", nhanVienCSKH = "Nhân viên CSKH", the = "Thẻ", maNguoiDungZalo = "",
        /// Notebookheader
        lienHeVaDiaChi = "Liên hệ & Địa chỉ", lichSuKham = "Lịch sử khám", dichVuQuanTam = "Dịch vụ quan tâm", moiQuanHe = "Mối quan hệ", thongTinKhac = "Thông tin Khác", ech = "ECH", lichSuCuocGoi = "Lịch sử cuộc gọi",
        ketNoiZalo = "Kết nối Zalo", ketNoi = "Kết nối", tepDinhKem = "Tệp đính kèm", theBaoHiem = "Thẻ bảo hiểm", ten = "Tên", hoTen = "Họ tên", soThe = "Số thẻ", loai = "Loại", ngayPhatHanh = "Ngày phát hành", ngayBatDauHieuLuc = "Ngày bắt đầu hiệu lực",
        ngayKetThucHieuLuc = "Ngày kết thúc hiệu lực", quocTich = "Quốc tịch", cMND = "CMND", cCCDCMND = "CCCD/CMND",noiPhatHanh = "Nơi phát hành", maHopDong = "Mã hợp đồng", donViDichVu = "Đơn vị dịch vụ",
        themTitle = "Tạo tập dữ liệu", diaChiKhacId = "other", tenLienHe = "Tên Liên hệ", email = "Email", diachiId = "street", tinhTPPlaceHolder = "Tỉnh/Tp",
        maSoThue = "Mã số thuế?", danhXung = "Danh xưng", chucVu = "Chức vụ", phongBan = "Phòng ban", trangWeb = "Trang web", loaiNguon = "Loại nguồn", cbnvGioiThieu = "CBNV Giới thiệu", ngonNgu = "Ngôn ngữ", khachHangGioiThieu = "Khách hàng giới thiệu",
        /// global data
        leadName = "QA auto lead dakhoa " + dateTimeNow,
        contactName = "QA auto lh dakhoa " + dateTimeNow,
        opportunityName = "Cơ hội của QA auto dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_Create_search_delete_KhachHangCaNhan()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            string? tenCongTyData = null, maBenhNhanData = null, dienThoaidata = null, dienThoaidata2 = null, dienThoaidata3 = null, dienThoaidataChong, ngaySinhData = null, diaChiData = null,
            streetData = null, tinhTpData = null, cityData = null, districtData = null, wardData = null, gioiTinhData = null, loaiNguoiBenhData = null, cCCDData = null, nguoiPhuTrachData = null, nhanVienCSKHData = null, tagData = null, maNguoiDungZaloData = null,
            tenLienHeData = null, emailData = null, tinhTPData = null, tinhTP2Data = null, district2Data = null, ward2Data = null, dienThoai2Data = null, maSoThueData = null;
            const string searchboxRole = "searchbox", inputSearch = "qa auto", moiButton= "/button[.='Mới' or .=' Mới ']";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - LienHe Test 001 - Create, delete Lead KHCaNhan");
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

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn")) 
                {
                    // Go to 'lien he' - 'Khach hang ca nhan' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton) //.CLickButtonTitle(10, General.moi.Replace("Mới", " Mới "))
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    #region Input data to create 'Khach hang ca nhan'
                    GeneralAction.Instance.ClickInputByAttributeValue(10, caNhanId)
                                          .InputByAttributeValue(10, nameId, contactName)
                                          //.InputByAttributeValue(10, tenCongTyId, tenCongTyData = "Connext - 098 765 43 21").PressEnterKeyboard()
                                          .InputFieldLabel(10, maBenhNhan, maBenhNhanData = "QAAUTOMBN" + dateTimeNow)
                                          .InputFieldLabel(10, ngaySinh, ngaySinhData = "07/11/1999").PressEnterKeyboard().Sleep(500)
                                          .CLickAndSelectItemInDropdownLabel(10, gioiTinh, gioiTinhData = "Nam")
                                          .InputFieldLabel(10, loaiNguoiBenh, loaiNguoiBenhData = "Siêu Âm").PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, cCCD, cCCDData = "112233445566")
                                          ///.InputFieldLabel(10, donvidichvu, "Đa Khoa") // --> auto fill
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                          .InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, diaChi, streetData = "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, tinhTpData = "TP Hồ Chí Minh", tinhTp).PressEnterKeyboard() // Bug missing this field if inputting data for 'Tên công ty'
                                          .InputFieldLabel(10, diaChi, districtData = "Quận 1", quanHuyenPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, wardData = "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, the, tagData = "VIP").PressEnterKeyboard()
                                          .InputFieldLabel(10, nguoiPhuTrach, nguoiPhuTrachData = "Dương Ngọc Vy").PressEnterKeyboard().Sleep(1000);
                    /// NotebookHeader (Liên hệ & Địa chỉ, Lịch sử khám, ...)
                    GeneralAction.Instance.CLickNotebookHeader(10, lienHeVaDiaChi)
                                          .CLickButtonTitle(10, themTitle)
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .ClickInputByAttributeValueDialog(10, diaChiKhacId)
                                          .InputFieldLabelDialog(10, tenLienHe, tenLienHeData = "QA auto lh 02 " + dateTimeNow).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, email, emailData = "qaauto@connext.com").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, streetData = "232 Cao Thắng", diachiId).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, tinhTP2Data = "TP Hồ Chí Minh", tinhTPPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, district2Data = "Quận 3", quanHuyenPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, ward2Data = "Phường 10", phuongXaPlaceHolder).PressEnterKeyboard()
                                          .InputByAttributeValue(10, phoneId, dienThoai2Data = "02219999992", "div[contains(@class,'modal-content')]//input")
                                          .InputFieldLabelDialog(10, maSoThue, maSoThueData = "QA" + DateTime.Now.ToString("dd/MM/yyyy HH").Replace(" ", "")) // ex: BE0477472701
                                          .InputDescriptionContentInDialog(10, "QA auto Ghi chu tao Lien he 01")
                                          .CLickButtonTitleInDialog(10, "Lưu & Đóng")
                                          .WaitForElementInvisible(10, General.DialogShow);
                    GeneralAction.Instance.CLickNotebookHeader(10, dichVuQuanTam)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng")
                                          .InputRowListTable(10, "1", "1", "[DK-KTQ-T40] Khám tổng quát trên 40 tuổi").PressEnterKeyboard();
                    GeneralAction.Instance.CLickNotebookHeader(10, moiQuanHe)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng")
                                          .InputRowListTable(10, "1", "1", "Conenxt - Toan -").PressEnterKeyboard()
                                          .InputRowListTable(10, "1", "2", "Bạn").PressEnterKeyboard();
                    GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, email, "qaautoNBHeader@connext.com")
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, danhXung, "Ông").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chucVu, "Giám đốc")
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, phongBan, "Kiểm soát chất lượng")
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, trangWeb, "qaautoWebsite.com")
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", cbnvGioiThieu))
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, cbnvGioiThieu, "BS Đỗ Thị Thi").PressEnterKeyboard();
                    GeneralAction.Instance.CLickNotebookHeader(10, ketNoiZalo);
                    GeneralAction.Instance.CLickNotebookHeader(10, ketNoi);
                    GeneralAction.Instance.CLickNotebookHeader(10, lichSuCuocGoi);
                    GeneralAction.Instance.CLickNotebookHeader(10, tepDinhKem);
                    GeneralAction.Instance.CLickNotebookHeader(10, theBaoHiem)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .InputFieldLabelDialog(10, hoTen, "QA auto đa khoa the bao hiem" + dateTimeNow)
                                          .InputFieldLabelDialog(10, soThe, "1987654320")
                                          .InputFieldLabelDialog(10, loai, "Thẻ Bảo hiểm y tế").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayPhatHanh, "15/10/2021").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayBatDauHieuLuc, "15/10/2021").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayKetThucHieuLuc, "15/10/2025").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngaySinh, "15/10/1989").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, "qa auto đa khoa thẻ bh địa chỉ")
                                          .InputFieldLabelDialog(10, gioiTinh, "Nam")
                                          .InputFieldLabelDialog(10, quocTich, "Việt Nam")
                                          .InputFieldLabelDialog(10, cMND, "11223344556677")
                                          .InputFieldLabelDialog(10, noiPhatHanh, "CA.TPHCM")
                                          .InputFieldLabelDialog(10, maHopDong, "mhdthebh01")
                                          .CLickButtonTitleInDialog(10, General.luuVaDong);
                    GeneralAction.Instance.CLickNotebookHeader(10, lichSuKham)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng").WaitForElementVisible(10, General.tableListRowAdd("2", "Thêm một dòng"))
                                          .ClickButtonInRowListTable(10, "1", "14"); // Click 'Checkin' button --> auto Save all data
                    GeneralAction.Instance.WaitForElementInvisible(10, General.formEditable);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Wait for Contact details page load done 
                    GeneralAction.Instance.WaitForElementVisible(10, General.formEditable)
                                          .WaitForElementVisible(10, General.notebookHeader(lichSuKham)).Sleep(2000)
                                          .CLickNotebookHeader(10, lichSuKham).Sleep(1000);

                    //GeneralAction.Instance.CLickButtonTitle(10, General.luuThuCong);
                    //                      //.WaitForElementVisible(10, General.alertShow);
                    //                      //.WaitForElementVisible(10, General.chatterMessage("Tùy chỉnh model Liên hệ trong module connext_voiceIP được tạo")); 
                    //                      Thread.Sleep(3000);
                    #endregion

                    #region Search the created Contact (Lien he)
                    // Back to 'Khách hàng cá nhân' page
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Khách hàng cá nhân")
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search the created Contact (Lien he)
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", contactName).Sleep(3000);
                    #endregion

                    #region Verify data of the created Contact (Lien he) is shown correctly
                    string nameAttrVal = "name", data;
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phone", data = dienThoaidata.Replace("19", "1 9").Replace("90", "9 0"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "birthday_display", data = ngaySinhData);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Năm sinh' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "unread_message_count", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tin nhắn mới' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "address", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Địa chỉ' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "patient_id", data = maBenhNhanData);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Mã bệnh nhân' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "recent_examination_date", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đã khám gần nhất' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "recent_examination_service", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Dịch vụ khám gần nhất' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "booking_date", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày Booking' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "opportunity_latest_update", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "activity_ids", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Công việc cần thực hiện' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "sale_customer_service", data = nhanVienCSKHData = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên CSKH' is shown after searching the created client (contact): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete all of the created clients (Contacts - Lien he)
                    // Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Search the created Contact (Lien he)
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, "Tên", inputSearch);

                    // Click the checkbox 'All' and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    // Verify the created Booking is deleted
                    data = contactName;
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the all of the created clients (contacts) is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' site !!!");
                    test.Log(Status.Info, summaryTC);
                }
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

        [Test, Category("DKHN - Regression Tests")]
        public void TC002_Checkin_Nhanh_Tao_LienHe()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName, data;
            string? tenCongTyData = null, maBenhNhanData = null, dienThoaidata = null, dienThoaidata2 = null, dienThoaidata3 = null, dienThoaidataChong, ngaySinhData = null, diaChiData = null,
            streetData = null, tinhTpData = null, cityData = null, districtData = null, wardData = null, gioiTinhData = null, loaiNguoiBenhData = null, cCCDData = null, nguoiPhuTrachData = null, nhanVienCSKHData = null, tagData = null, maNguoiDungZaloData = null,
            tenLienHeData = null, emailData = null, tinhTPData = null, tinhTP2Data = null, district2Data = null, ward2Data = null, dienThoai2Data = null, maSoThueData = null;
            const string searchboxRole = "searchbox", inputSearch = "qa auto", moiButton = "/button[.='Mới' or .=' Mới ']", timKiemBookingLienhePlaceHolder = "SĐT, CCCD hoặc Họ tên", taoLienHeVaDangKy = "Tạo liên hệ và Đăng ký",
            ngaySinhTheBHXpath = @"/div/div[2]//div[contains(@class,'modal-content')]//input[@id='birthday']", vdTenKhachHangId = "name_2";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - LienHe Test 002 - Checkin Nhanh tao lien he");
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

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Checkin nhanh tim kiem (SDT, CCCD hoac Ho ten) --> tim sdt moi
                    // Click 'Checkin nhanh' icon on the top
                    GeneralAction.Instance.CLickCheckinNhanhIcon(10)
                                          .WaitForElementVisible(10, General.DialogShow);

                    // Input 'SĐT, CCCD hoặc Họ tên' to search booking, liên hệ
                    GeneralAction.Instance.InputByAttributeValueDialog(60, timKiemBookingLienhePlaceHolder, dienThoaidata = "0860000001").PressEnterKeyboard();

                    // Verify message 'Không tìm thấy thông tin với từ khóa' is displayed
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "no-result", data = "Không tìm thấy thông tin với từ khóa\r\n\"" + dienThoaidata + "\"", "/div");
                    verifyPoints.Add(summaryTC = "Verify message (Không tìm thấy...) is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify button 'Tạo liên hệ và Đăng ký' is displayed (when kết quả là Không tìm thấy thông tin với từ khóa)
                    verifyPoint = GeneralAction.Instance.IsButtonTitleShown(10, data = "Tạo liên hệ và Đăng ký");
                    verifyPoints.Add(summaryTC = "Verify button 'Tạo liên hệ và Đăng ký' is displayed (khi kết quả Không tìm thấy thông tin với từ khóa): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Tao lien he va dang ky
                    // Click button 'TẠO LIÊN HỆ VÀ ĐĂNG KÝ'
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, taoLienHeVaDangKy);

                    // Input data to create lien he
                    GeneralAction.Instance.InputFieldLabelDialog(10, ten, contactName);
                    //GeneralAction.Instance.InputFieldLabelDialog(10, loaiNguon, "Offline / Giới thiệu / C2C").PressEnterKeyboard();
                    //GeneralAction.Instance.WaitForElementVisible(10, General.elementHtml("label", khachHangGioiThieu)).Sleep(500)
                    //                      .InputFieldLabelDialog(10, khachHangGioiThieu, "Conenxt - Toan -").Sleep(500).PressEnterKeyboard()

                    GeneralAction.Instance.InputFieldLabelDialog(10, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard();
                    GeneralAction.Instance.WaitForElementVisible(10, General.elementHtml("label", cbnvGioiThieu)).Sleep(500)
                                          .InputFieldLabelDialog(10, cbnvGioiThieu, LoginPage.telesaleIVFnv6).Sleep(500).PressEnterKeyboard()
                                          ///.InputFieldLabelDialog(10, dienThoai, dienThoaidata) // Auto fill
                                          .CLickAndSelectItemInDropdownLabel(10, gioiTinh, gioiTinhData = "Nam")
                                          .InputFieldLabelDialog(10, ngaySinh, ngaySinhData = "07/11/1999").PressEnterKeyboard().Sleep(500);
                    GeneralAction.Instance.WaitForElementInvisible(10, General.elementHtml("label", namSinhNhapKhiKhongCoNgaySinh))
                                          .InputFieldLabelDialog(10, cCCDCMND, cCCDData = "112233445566")
                                          /// Tao the bao hiem
                                          .CLickNotebookHeader(10, theBaoHiem)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng")
                                          .WaitForElementVisible(10, General.DialogIndexShow("2"))
                                          .InputFieldLabelDialog(10, hoTen, "QA auto đa khoa the bao hiem" + dateTimeNow)
                                          .InputFieldLabelDialog(10, soThe, "1987654320")
                                          .InputFieldLabelDialog(10, loai, "Thẻ Bảo hiểm y tế").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayPhatHanh, "15/10/2021").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayBatDauHieuLuc, "15/10/2021").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, ngayKetThucHieuLuc, "15/10/2025").PressEnterKeyboard()
                                          .InputHtmlElement(10, "div", "dialog_container", "15/10/1989", ngaySinhTheBHXpath).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, "qa auto đa khoa thẻ bh địa chỉ")
                                          .InputFieldLabelDialog(10, gioiTinh, gioiTinhData)
                                          .InputFieldLabelDialog(10, quocTich, "Việt Nam")
                                          .InputFieldLabelDialog(10, cMND, "11223344556677")
                                          .InputFieldLabelDialog(10, noiPhatHanh, "CA.TPHCM")
                                          .InputFieldLabelDialog(10, maHopDong, "mhdthebh01")
                                          .CLickButtonTitleInDialog(10, General.luuVaDong).Sleep(500);
                    GeneralAction.Instance.WaitForElementInvisible(10, General.DialogIndexShow("2"));
                    GeneralAction.Instance.WaitForElementVisible(10, General.DialogIndexShow("1"));
                    /// Click button 'TẠO LIÊN HỆ VÀ ĐĂNG KÝ'
                    GeneralAction.Instance.CLickButtonTitleInDialog(60, taoLienHeVaDangKy).Sleep(1500);
                    GeneralAction.Instance.WaitForElementInvisible(60, General.DialogIndexShow("1")).Sleep(1500);
                    GeneralAction.Instance.WaitForElementInvisible(60, General.DialogShow);
                    GeneralAction.Instance.WaitForElementVisible(60, General.formEditable);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }
                    #endregion

                    #region Verify Lien he duoc tao sau khi Checkin nhanh thanh cong
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, vdTenKhachHangId, data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data of 'Tên liên hệ' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, maBenhNhan, data = "");
                    verifyPoints.Add(summaryTC = "Verify data of '" + maBenhNhan + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = dienThoaidata.Replace("600", "60 0").Replace("0001", "0 001"));
                    verifyPoints.Add(summaryTC = "Verify data of '" + dienThoai + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai + " 2", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of '" + dienThoai + "2' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai + " 3", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of '" + dienThoai + "3' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngaySinh, data = ngaySinhData);
                    verifyPoints.Add(summaryTC = "Verify data of '" + ngaySinh + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, gioiTinh, data = gioiTinhData);
                    verifyPoints.Add(summaryTC = "Verify data of '" + gioiTinh + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, loaiNguoiBenh, data = "");
                    verifyPoints.Add(summaryTC = "Verify data of '" + loaiNguoiBenh + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, cCCDCMND, data = cCCDData);
                    verifyPoints.Add(summaryTC = "Verify data of '" + cCCDCMND + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "label", donViDichVu, data = "Đa Khoa", "/ancestor::div[2]/div[2]//span"); // /ancestor::div[contains(@class,'row align-items-start')]/div[2]/div[last()]//span[@title='" + data + "']
                    //verifyPoints.Add(summaryTC = "Verify data of '" + donViDichVu + "' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    // Click tab 'Lịch sử khám' to verify 'lần khám' is created
                    GeneralAction.Instance.CLickNotebookHeader(10, lichSuKham)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Verify 'lần khám' is created
                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "1", data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Ngày khám' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "2", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Ngày tái khám' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "3", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Chuẩn đoán' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "4", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Lời dặn' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "5", data = "Đã checkin");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Trạng thái tư vấn' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "6", data = "");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Booking' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "7", data = "0");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Phí dịch vụ' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "8", data = "0");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Phí đơn thuốc' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputRowListTableGetText(10, "1", "9", data = "", "div");
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Thẻ' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "10", data = "Offline / Giới thiệu / CBNV - BVĐK HN"); // "Offline / Giới thiệu / C2C"
                    verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Loại Nguồn' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "11", data = "");
                    //verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'CBNV Giới thiệu' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "12", data = LoginPage.telesaleIVFnv6); // "Conenxt - Toan -"
                    //verifyPoints.Add(summaryTC = "Verify data of 'Lịch sử khám'-'Khách hàng giới thiệu' title in 'Liên hệ' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created lien he
                    // Go to 'Liên hệ' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer); Thread.Sleep(1500);

                    // Search the created client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(3000);

                    // Verify the created client is shown
                    string nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created clients
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);
                                         
                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' site !!!");
                    test.Log(Status.Info, summaryTC);
                }
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
