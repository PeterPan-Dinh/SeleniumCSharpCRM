using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenQA.Selenium.BiDi.Modules.Network.AuthCredentials;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing_BVTN
{
    [TestFixture, Order(4)]
    internal class BookingCuaToiTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// table renderer (used for wait method)
        private string crmRender = "kanban_renderer", bookingRender = "form_sheet_bg", listRenderer = "list_renderer", kanbanRenderer = "kanban_renderer",
        /// title, attribute fields on Form ...
        coHoi = "Cơ hội", moiButton = "/button[.='Mới' or .=' Mới ']", khachHang = "Khách hàng", opportunityId = "opportunity_id", ngayBookingId = "date_order", taoTheLieuTrinh = "Tạo thẻ liệu trình", loaiBooking = "Loại booking", trangThaiBooking = "Trạng thái booking", bacSi = "Bác sĩ",
        dienThoai = "Điện thoại", email = "Email", trangWeb = "Trang web", diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/TP", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã", donvidichvu = "Đơn vị dịch vụ", doiNguKinhDoanh = "Đội ngũ kinh doanh",
        /// Notebookheader
        ghiChuNoiBo = "Ghi chú nội bộ", chiTietDonHang = "Chi tiết đơn hàng", sanPhamTuyChon = "Sản phẩm tuỳ chọn", thongTinKhac = "Thông tin khác", thongTinThem = "Thông tin thêm", xacNhanOnline = "Xác nhận online?", thanhToanId = "require_payment", maThamChieuKH = "Mã tham chiếu khách hàng", the = "Thẻ", chungTuGoc = "Chứng từ gốc?", chienDich = "Chiến dịch?", phuongTien = "Phương tiện?", nguon = "Nguồn?",
        loaiNguon = "Loại nguồn", cbnvGioiThieu = "CBNV Giới thiệu", quanLylieuTrinh = "Quản lý liệu trình", lichSuKham = "Lịch sử khám",
        /// global data
        contactName = "QA auto lh " + dateTimeNow,
        opportunityName = "Cơ hội của QA auto " + dateTimeNow;
        #endregion

        [Test, Category("BVTN - Regression Tests")]
        public void TC001_Create_new_booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            string? dienThoaidata = null, dienThoaidata2 = null, dienThoaidata3 = null;
            /// title, attribute fields on Form
            const string inputSearch = "qa auto";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - BookingCuaToi Test 001 - Create new booking");
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
                    // Go to 'Booking của tôi' >> Chu trình page
                    GeneralAction.Instance.GoToLeftMenu(30, General.bookingCuaToi, "Booking", listRenderer); Thread.Sleep(1500);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    #region Input data to create 'Booking'
                    GeneralAction.Instance.InputFieldLabel(10, khachHang, contactName).PressEnterKeyboard()
                                          .InputByAttributeValue(10, opportunityId, opportunityName, "input").PressDownKeyboard().PressEnterKeyboard()
                                          /// Create 'Cơ hội' in dialog
                                          .WaitForElementVisible(10, General.DialogActiveShow).WaitForElementVisible(10, General.titleButtonInDialog("Lưu & Đóng"));
                    GeneralAction.Instance.InputByAttributeValueDialog(10, "partner_id_1", contactName).PressEnterKeyboard()
                                          .InputByAttributeValueDialog(10, "phone_1", dienThoaidata = "02219999001", "div[@name='phone']//input")
                                          .InputFieldLabelDialog(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabelDialog(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabelDialog(10, email, "qaauto@connext.com")
                                          .InputByAttributeValueDialog(10, "website_2", "https://www.google.com")
                                          .InputFieldLabelDialog(10, diaChi, "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, "TP Hồ Chí Minh", tinhTpPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabelDialog(10, diaChi, "Quận 1", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, diaChi, "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard();
                    GeneralAction.Instance.InputFieldLabelDialog(10, donvidichvu, "BVTN").PressEnterKeyboard();
                    GeneralAction.Instance.InputByAttributeValueDialog(10, "tag_ids", "bao_hiem").PressEnterKeyboard();
                    GeneralAction.Instance.InputFieldLabelDialog(10, doiNguKinhDoanh, "Bán hàng").PressEnterKeyboard()
                                          /// NotebookHeader dialog - Thông tin khác
                                          .CLickNotebookHeader(10, thongTinThem)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "CBNV").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, "Bác sĩ Nam").PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, General.luuVaDong)
                                          .WaitForElementInvisible(10, General.DialogActiveShow)
                                          /// Create Booking
                                          //.InputByAttributeValue(10, ngayBookingId, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")); // --> auto fill
                                          .CLickAndSelectItemInDropdownLabel(10, loaiBooking, "Tái khám")
                                          .CLickAndSelectItemInDropdownLabel(10, trangThaiBooking, "Chưa checkin")
                                          .InputFieldLabel(10, bacSi, "QA Bác Sĩ 01").Sleep(500).PressEnterKeyboard()
                                          /// NotebookHeader - Thông tin khác
                                          .CLickNotebookHeader(10, thongTinKhac)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, loaiNguon, "CBNV").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, cbnvGioiThieu, "Bác sĩ Nam").Sleep(1500).PressEnterKeyboard()
                                          /// NotebookHeader - Quản lý liệu trình
                                          .CLickNotebookHeader(10, quanLylieuTrinh)
                                          /// NotebookHeader - Lịch sử khám
                                          .CLickNotebookHeader(10, lichSuKham)
                                          .CLickButtonTitle(10, General.luu).Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(15);
                    #endregion

                    #region Verify data of the created 'Booking'
                    // Back to man hinh 'Booking' list view by cLicking breadcrumb title
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Booking")
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Remove filter 'Khách VIP' in seachbox
                    GeneralAction.Instance.CLickRemovefilterSearchBox(10, "Khách VIP");

                    // Search the new created 'Booking'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(30, "Đơn hàng", inputSearch);

                    // Verify data of the created 'Booking' is shown correctly
                    string nameAttrVal = "date_order", data;
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày Booking' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "partner_id", data = contactName + " - " + dienThoaidata.Replace("19", "1 9").Replace("90", "9 0"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Khách hàng' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "actual_examination_date", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày khám thực tế' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullnameBvtn);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "doctor_id", data = "QA Bác Sĩ 01");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Bác sĩ' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "booking_status", data = "Chưa checkin");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái Booking' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "activity_ids", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Hoạt động' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "my_activity_date_deadline", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đến hạn' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "service_unit", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đơn vị dịch vụ' is shown after searching the created 'Booking': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created 'Booking'
                    // Delete all these created 'Booking'
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(10).Sleep(1000);

                    // Verify the created 'Booking' is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Booking' is deleted: '" + (data = opportunityName) + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the creeated 'Cơ hội'
                    // Go to 'Data của tôi' >> Chu trình page
                    GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi, "Chu trình", kanbanRenderer); Thread.Sleep(1500);

                    // Click 'List' button at 'switch buttons' (views: List, Kanban, Calendar, Pivot, ...)
                    GeneralAction.Instance.CLickButtonTitle(30, General.list); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1500);

                    // Search the new created 'co hoi'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(30, coHoi, inputSearch);

                    // Verify data of the created 'co hoi' is shown correctly
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = opportunityName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created 'co hoi': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created 'co hoi'
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(10).Sleep(1000);

                    // Verify the created 'co hoi' is deleted 
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
                    verifyPoints.Add(summaryTC = "Verify the Created 'lien he' is deleted: '" + (data = contactName) + "'", verifyPoint);
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
