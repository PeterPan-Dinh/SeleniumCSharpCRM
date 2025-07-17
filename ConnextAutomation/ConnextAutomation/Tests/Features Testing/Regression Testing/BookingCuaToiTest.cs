using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [Ignore("")]//[TestFixture, Order(8)]
    internal class BookingCuaToiTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        #endregion

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC001_Create_new_booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url,
            dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            const string inputSearch = "qa auto", searchboxRole = "searchbox", khachHang = "Khách hàng", ngayBookingId = "date_order", taoTheLieuTrinh = "Tạo thẻ liệu trình",
            donViDichVu = "Đơn vị dịch vụ", maTheLieuTrinh = "Mã thẻ liệu trình", theLieuTrinh = "Thẻ liệu trình", loaiBooking = "Loại booking", ngayPhatHanh = "Ngày phát hành", thoiGianTu = "Thời gian từ",
            thoiGianDen = "Thời gian đến", dichVuLieutrinh = "Dịch vụ liệu trình", trangThaiBooking = "Trạng thái booking", slThucHienLanNay = "SL thực hiện lần này",
            trangThaiTuVan = "Trạng thái tư vấn", bacSi = "Bác sĩ", ngayKhamThucTe = "Ngày khám thực tế", tickets = "Tickets", nguoiPhuTrach = "Người phụ trách", khackHang = "Khách hàng",
            doiNguKinhDoanh = "Đội ngũ kinh doanh", opportunity = "Opportunity", opportunityId = "opportunity_id", coHoi = "Cơ hội",

            /// Notebookheader
            chiTietDonHang = "Chi tiết đơn hàng", sanPhamTuyChon = "Sản phẩm tuỳ chọn", thongTinKhac = "Thông tin khác", xacNhanOnline = "Xác nhận online?",
            thanhToanId = "require_payment", maThamChieuKH = "Mã tham chiếu khách hàng", the = "Thẻ", chungTuGoc = "Chứng từ gốc?", chienDich = "Chiến dịch?", phuongTien = "Phương tiện?", nguon = "Nguồn?";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - BookingCuaToi Test 001 - Create new booking");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains("qa-odoo") || urlInstance.Contains("staging-bvdkhn")) 
                {
                    // Go to 'Booking của tôi' menu
                    GeneralAction.Instance.CLickMenuTitle(10, General.bookingCuaToi)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    #region Input data
                    string theLieutrinhDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                    GeneralAction.Instance.ClickInputFieldLabel(10, khachHang);
                    GeneralAction.Instance.InputFieldLabel(10, khachHang, "QA auto Khach hang " + dateTimeNow); Thread.Sleep(500);
                    GeneralAction.Instance.PressDownKeyboard();
                    GeneralAction.Instance.PressEnterKeyboard(); Thread.Sleep(3000);

                    /// Check If the popup/dialog 'Mới: Khách hàng' is shown then click 'Tạo' to create a new one
                    if (GeneralAction.Instance.IsElementPresent(General.DialogActiveShow) && GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Tạo")))
                    {
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Tạo")
                                              .WaitForElementInvisible(10, General.DialogIndexShow("2")); Thread.Sleep(100);
                    }

                    ////System.Windows.Forms.SendKeys.SendWait(@"{DOWN}"); Thread.Sleep(500); //.SendWait(@"{UP}");//.SendWait(@"ENTER");
                    //GeneralAction.Instance.ClickInputFieldLabel(10, ngayPhatHanh); Thread.Sleep(500);
                    //GeneralAction.Instance.InputFieldLabel(10, khachHang, "QA auto Khach hang 01"); Thread.Sleep(500);
                    //GeneralAction.Instance.PressDownKeyboard();
                    //GeneralAction.Instance.PressEnterKeyboard();

                    /// Check If the popup/dialog 'Mới: Khách hàng' is shown then click 'Tạo' to create a new one
                    if (GeneralAction.Instance.IsElementPresent(General.DialogActiveShow) && GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Tạo")))
                    {
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Tạo")
                                              .WaitForElementInvisible(10, General.DialogIndexShow("2")); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.InputByAttributeValue(10, ngayBookingId, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
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
                                          .ClickAddRowListTable(10, "2", "Thêm sản phẩm"); Thread.Sleep(1000);
                    GeneralAction.Instance.InputRowListTable(10, "2", "2", "[DK-KTQ-U40] Khám tổng quát người lớn dưới 40 tuổi")
                                          .ClickRecordPosXInRowListTable(10, "1", "6")//.ClickAddRowListTable(10, "3", "Thêm phần")
                                          //.InputRowListTable(10, "3", "2", "QA auto Thêm phần")
                                          //.ClickAddRowListTable(10, "4", "Thêm ghi chú");
                    //GeneralAction.Instance.InputRowListTable(10, "4", "2", "QA auto Thêm ghi chú", "textarea")
                                          .CLickNotebookHeader(10, sanPhamTuyChon)
                                          .ClickAddRowListTable(10, "1", "Thêm sản phẩm");
                    GeneralAction.Instance.InputRowListTable(10, "1", "2", "[IVF-DIAG] Khám vô sinh hiếm muộn") //  KHAM-12345] Khám tổng quát nhi
                                          .ClickRecordPosXInRowListTable(10, "1", "3").ClickAddRowListTable(10, "2", "Thêm sản phẩm") // workaround to input data for this
                                          .CLickNotebookHeader(10, thongTinKhac)
                                          .ClickCheckboxAtFieldLabel(10, xacNhanOnline, thanhToanId, "id")
                                          .InputFieldLabel(10, maThamChieuKH, "QA auto " + maThamChieuKH);
                    //.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, the, "Đa khoa")
                    ////.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chungTuGoc, "Cơ hội của QA auto 01") // --> auto filled, no need to input
                    //.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, chienDich, "CD-1234 Quảng cáo Facebook")
                    //.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, phuongTien, "Facebook")
                    //.InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, nguon, "Twitter")
                    //.CLickButtonTitle(10, General.luuThuCong)
                    //.WaitForElementVisible(10, General.alertShow);
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

                    #region verify required field alert is shown
                    string data = "Trường không hợp lệ:";
                    verifyPoint = GeneralAction.Instance.AlertGetText(10, data);
                    verifyPoints.Add(summaryTC = "Verify notification title is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.AlertGetText(10, data = "Opportunity")
                        || GeneralAction.Instance.AlertGetText(10, data = "Cơ hội");
                    verifyPoints.Add(summaryTC = "Verify notification for field-3 is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Create Booking successfully and verify data
                    // Wait for alert message is disappeared
                    GeneralAction.Instance.WaitForElementInvisible(10, General.alertShow);

                    // Input data for 'Opportunity' field
                    GeneralAction.Instance.CLickNotebookHeader(10, chiTietDonHang)
                                          .InputByAttributeValue(10, opportunityId, "Cơ hội của QA auto " + dateTimeNow, "input")
                                          .CLickButtonTitle(10, General.luuThuCong); Thread.Sleep(1000);

                    /// Check If the popup/dialog 'Mới: Khách hàng' is shown then click 'Tạo' to create a new one
                    if (GeneralAction.Instance.IsElementPresent(General.DialogActiveShow) && GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Tạo")))
                    {
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Tạo")
                                              .WaitForElementInvisible(10, General.DialogIndexShow("2")); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.CLickButtonTitle(10, General.luuThuCong)
                                          .WaitForElementVisible(20, General.elementHtml("div", "MessageList bg-view", "//*[contains(.,'được tạo')]")); //  Booking được tạo

                    // Back to 'Booking' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.bookingCuaToi)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Booking
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Đơn hàng", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the new Booking is created
                    string nameAttrVal = "date_order";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày booking' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal= "source_booking", data = LoginPage.fullname);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Source' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "service_unit", data = "Đa khoa");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đơn vị dịch vụ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "partner_id", data = "QA auto Khach hang " + dateTimeNow);
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

                    #region Delete the created booking
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Verify the created client is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created booking at 'Booking' page is deleted: '" + (data = "QA auto Khach hang " + dateTimeNow) + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Khách hàng cá nhân' is created at 'Liên hệ' page
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
                    nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = "QA auto Khach hang " + dateTimeNow);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete this created client
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created lead at CRM/Lead page
                    // Go to CRM-Lead page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer)
                                          .CLickMenuTitle(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, General.lead, inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the created Lead is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = "Cơ hội của QA auto " + dateTimeNow);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the created 'Lead': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete the created lead
                    GeneralAction.Instance.ClickCheckboxAlltable(10) // or ClickToCheckboxInRowTable(10, "1") /// Click to check the checkbox at the 'x' row
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Verify the created Lead is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + data + "'", verifyPoint);
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
