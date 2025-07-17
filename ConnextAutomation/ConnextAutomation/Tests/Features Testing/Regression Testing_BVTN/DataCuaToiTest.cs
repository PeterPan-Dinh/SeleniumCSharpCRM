using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing_BVTN
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
        private string crmRender = "kanban_renderer", bookingRender = "form_sheet_bg", listRenderer = "list_renderer", kanbanRenderer = "kanban_renderer",
        /// title, attribute fields on Form ...
        moiButton = "/button[.='Mới' or .=' Mới ']", opportunityNamePlaceHolder = "vd: Giá sản phẩm", khachHang = "Khách hàng", tenCongTyId = "parent_id", maBenhNhan = "Mã bệnh nhân",
        ngaySinh = "Ngày sinh", namSinhNhapKhiKhongCoNgaySinh = "Năm sinh (nhập khi không có ngày sinh)", diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/TP", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        gioiTinh = "Giới tính", loaiNguoiBenh = "Loại người bệnh", cCCD = "CCCD/CMND", nguoiPhuTrach = "Người phụ trách", nhanVienCSKH = "Nhân viên CSKH", the = "Thẻ", donvidichvu = "Đơn vị dịch vụ", nguonDV = "Nguồn DV", ngayBooking = "Ngày booking", trangThaiBooking = "Trạng thái booking", loaiBooking = "Loại booking",
        /// Notebookheader
        ghiChuNoiBo = "Ghi chú nội bộ", thongTinThem = "Thông tin thêm", thongTinKhac = "Thông tin Khác", ketNoi = "Kết nối", chiSoBETA = "Chỉ số BETA", lichSuKham = "Lịch sử khám", dsNguoiDiCung = "DS người đi cùng",
        nhanSuAds = "Nhân sự Ads", loaiNguon = "Loại nguồn", cbnvGioiThieu = "CBNV Giới thiệu", bacSi = "Bác sĩ", dieuDuong = "Điều dưỡng", cSKH = "CSKH", tickets = "Tickets",
        /// global data
        leadName = "QA auto lead " + dateTimeNow,
        contactName = "QA auto lh " + dateTimeNow,
        opportunityName = "Cơ hội của QA auto " + dateTimeNow;
        #endregion

        [Test, Category("BVTN - Regression Tests")]
        public void TC001_Cohoi_Create_Cohoi_Create_Booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
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
                LoginAction.Instance.LoginSiteBVTN(60, urlInstance);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }

                // Check if url site is not QA-BVTN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvtn"))
                {
                    // Go to 'Data của tôi' >> Chu trình page
                    GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi, "Chu trình", kanbanRenderer); Thread.Sleep(1500);

                    // Click 'List' button at 'switch buttons' (views: List, Kanban, Calendar, Pivot, ...)
                    GeneralAction.Instance.CLickButtonTitle(30, General.list); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1500);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    #region Input data to create 'Co hoi'
                    // Tao co hoi o man hinh detail (list)
                    GeneralAction.Instance.InputByAttributeValue(10, opportunityNamePlaceHolder, opportunityName)
                                          /// tao lien he (inputting data at 'Khach hang')
                                          .InputFieldLabel(10, khachHang, contactName).PressEnterKeyboard()
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']//input")
                                          .InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, email, "qaauto@connext.com")
                                          .InputFieldLabel(10, trangWeb, "https://www.google.com")
                                          .InputFieldLabel(10, diaChi, "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "TP Hồ Chí Minh", tinhTpPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Quận 1", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, donvidichvu, "BVTN").PressEnterKeyboard()
                                          .InputFieldLabel(10, the, "bao_hiem").PressEnterKeyboard()
                                          .InputFieldLabel(10, doanhThuThuc, doanhThuThucData = "1.500.000,00")
                                          .InputFieldLabel(10, dongDuKien, dongDuKienData = DateTime.Now.ToString("dd/MM/yyyy"))
                                          .CLickPriorityStar(10, dongDuKien, "Rất cao")
                                          //.InputFieldLabel(10, doiNguKinhDoanh, "Bán hàng").PressEnterKeyboard(); --> auto fill
                                          /// NotebookHeader (Cơ hội - Ghi chú nội bộ)
                                          .CLickNotebookHeader(10, ghiChuNoiBo)
                                          .InputNotebookHeaderDescriptionContent(10, ghiChuNoiBo, "QA auto " + ghiChuNoiBo)
                                          /// Them thong tin (NotebookHeader)
                                          .CLickNotebookHeader(10, thongTinThem)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "Chưa xác định").PressEnterKeyboard()
                                          //.WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, "Bác sĩ Nam").PressEnterKeyboard()
                                          .CLickNotebookHeader(10, ketNoi)
                                          .CLickNotebookHeader(10, lichSuKham)
                                          .CLickButtonTitle(10, General.luu).Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(15);
                    #endregion

                    #region Verify data of the created 'co hoi'
                    // Back to man hinh 'co hoi' list view by cLicking breadcrumb title
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình");

                    // Search the new created 'co hoi'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(30, coHoi, inputSearch);

                    // Verify data of the created 'co hoi' is shown correctly
                    string nameAttrVal = "date_open", data;
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày phân công' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Quan tâm ads");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giai đoạn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = opportunityName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_name", data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên liên hệ' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "activity_ids", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Hoạt động' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "my_activity_date_deadline", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đến hạn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullnameBvtn);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "campaign_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Chiến dịch' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "source_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nguồn' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Co hoi'
                    // Delete all these created 'co hoi'
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(10).Sleep(1000);

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
                    GeneralAction.Instance.SearchViewInput(30, "Tên", inputSearch);

                    // Delete all these created 'lien he'
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(10).Sleep(1000);

                    // Verify the created 'lien he' is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'lien he' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-bvtn' site !!!");
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
