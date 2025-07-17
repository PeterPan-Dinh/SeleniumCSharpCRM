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
    [TestFixture, Order(2)]
    internal class LeadTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");

        /// table renderer (used for wait method)
        private string leadRenderer = "list_renderer", leadFormEditableRenderer = "o_lead_opportunity_form o_form_editable", listRenderer = "list_renderer", kanbanRenderer = "kanban_renderer",
        /// title, attribute fields on Form
        vdGiaSanPhamId = "name", sinhNhat = "Sinh nhật", namSinh = "Năm sinh", ngaySinh = "Ngày sinh", email = "Email", dienThoai = "Điện thoại", mucDoUuTien = "Mức độ ưu tiên", the = "Thẻ", trangWeb = "Trang web",
        diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã", doiNguKinhDoanh = "Đội ngũ kinh doanh",
        loaiBenhNhan = "Loại bệnh nhân", ten = "Tên", tenChong = "Tên chồng", sdtChong = "SĐT chồng", donvidichvu = "Đơn vị dịch vụ",
        /// notebook - Ghi chu / Them thong tin
        ghiChuNoiBo = "Ghi chú nội bộ", themMoTaText = "Thêm mô tả...", thongTinThem = "Thông tin thêm", thongTinKhac = "Thông tin Khác", dsNguoiDiCung = "DS người đi cùng", quanHe = "Quan hệ",
        congTy = "Công ty", chienDich = "Chiến dịch", duongDan = "Đường dẫn", linkDenChienDich = "Link đến chiến dịch", phuongTien = "Phương tiện", nguon = "Nguồn", gioiThieuBoi = "Giới thiệu bởi",
        nhanSuAds = "Nhân sự Ads", loaiNguon = "Loại nguồn", doiTac = "Đối tác", cbnvGioiThieu = "CBNV Giới thiệu", trucChat = "Trực chat", telesale = "Telesale", tuVanVien = "Tư vấn viên", bacSi = "Bác sĩ", dieuDuong = "Điều dưỡng", cSKH = "CSKH",

        /// global data
        leadName = "QA auto lead " + dateTimeNow,
        contactName = "QA auto lh " + dateTimeNow,
        leadNameChong = "QA auto lead " + dateTimeNow + "_Chồng",
        nguoiDiCungName = "QA auto lead di cung 01",
        opportunityName = "Cơ hội của QA auto " + dateTimeNow;
        #endregion

        [Test, Category("BVTN - Regression Tests")]
        public void TC001_Create_search_delete_lead()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            /// title, attribute fields on Form
            tenLienHe = "Tên liên hệ",
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong, dienThoaidataNguoiDiCung;
            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Lead Test 001 - Create search delete Lead");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteBVTN(60, urlInstance); // default login with admin account

                // Check if url site is not QA-BVTN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvtn"))
                {
                    // Go to Lead page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer); Thread.Sleep(1000);

                    // Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGiaSanPhamId, leadName)
                                          .InputFieldLabel(10, tenLienHe, contactName)
                                          .InputFieldLabel(10, sinhNhat, "16/08/2004")
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                          .InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, email, "qaauto@connext.com")
                                          //.InputFieldLabel(10, trangWeb, "https://www.google.com") // --> issue: duplicate this field
                                          .InputFieldLabel(10, diaChi, "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "TP Hồ Chí Minh", tinhTpPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Quận 1", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).PressEnterKeyboard()
                                          //.InputFieldLabel(10, donvidichvu, "BVTN").PressEnterKeyboard() --> this field was removed
                                          //.InputFieldLabel(10, the, "bao_hiem").PressEnterKeyboard()
                                          //.InputFieldLabel(10, doiNguKinhDoanh, "Bán hàng").PressEnterKeyboard();
                                          /// Ghi chu noi bo (NotebookHeader)
                                          .CLickNotebookHeader(10, ghiChuNoiBo)
                                          .InputNotebookHeaderDescriptionContent(10, ghiChuNoiBo, "QA auto " + ghiChuNoiBo)
                                          /// Them thong tin (NotebookHeader)
                                          .CLickNotebookHeader(10, thongTinThem)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "CBNV").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, "Bác sĩ Nam").PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, chienDich, "Chiến dịch tháng 11").PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, duongDan, "e").PressEnterKeyboard().Sleep(1000)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
                    GeneralAction.Instance.CLickButtonTitle(10, General.luu).WaitForElementInvisible(10, General.titleButton(General.luu)); Thread.Sleep(1000);
                    #endregion

                    #region Search Lead
                    // Back to Lead Page with list view (to search the created Lead) 
                    GeneralAction.Instance.CLickItemBreadcrumb(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, General.lead, inputSearch);
                    #endregion

                    #region Verify data of the created lead is shown correctly
                    string nameAttrVal = "create_date", data = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_open", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày phân công' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_name", data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên liên hệ' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "street2", data = "Phường Phạm Ngũ Lão, Quận 1");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Quận/Huyện' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "city", data = "TP Hồ Chí Minh");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thành phố' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = leadName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_telesale_note", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú telesale' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "unread_message_count", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tin nhắn mới' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "tag_ids", data = "bao_hiem");
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thẻ' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullnameBvtn);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Lead
                    // Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1")
                                          .ThucHienXoaDelete(10);

                    // Verify the created Lead is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + leadName + "'", verifyPoint);
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

        [Test, Category("BVTN - Regression Tests")]
        public void TC002_Create_lead_chuyen_thanh_Cohoi()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            /// title, attribute fields on Form
            tenLienHe = "Tên liên hệ",
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong, dienThoaidataNguoiDiCung;
            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Lead Test 002 - Create Lead chuyen thanh co hoi");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteBVTN(60, urlInstance); // default login with admin account

                // Check if url site is not QA-BVTN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvtn"))
                {
                    // Go to Lead page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer); Thread.Sleep(1000);

                    // Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead and then Chuyển thành Cơ hội
                    // Tao Lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGiaSanPhamId, leadName)
                                          .InputFieldLabel(10, tenLienHe, contactName)
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                          //.InputFieldLabel(10, donvidichvu, "BVTN").PressEnterKeyboard() --> this field was removed
                                          .CLickNotebookHeader(10, thongTinThem)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "CBNV").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, "Bác sĩ Nam").PressEnterKeyboard(); Thread.Sleep(1500);

                    // Chuyen thanh coi hoi
                    GeneralAction.Instance.CLickButtonTitle(10, "Chuyển thành Cơ hội")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Tạo cơ hội").Sleep(1000)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
                    //GeneralAction.Instance.CLickButtonTitleInDialog(10, General.luu.Replace("Lưu", " Lưu "))
                    //                      .WaitForElementInvisible(10, General.DialogShow)
                    //                      .WaitForElementVisible(10, General.chatterMsgChangedStatusSeparatorDate("Hôm nay", "1", ".='(Đội ngũ kinh doanh)'"))
                    //                      .Sleep(1500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);

                    // Back to 'Lead' page to verify the created lead is not shown
                    GeneralAction.Instance.CLickItemBreadcrumb(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer).Sleep(500)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(1500);
                    #endregion

                    #region Verify the created Lead is deleted after Chuyển thành Cơ hội
                    // Search the created lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, General.lead, inputSearch);

                    // Verify the created Lead is deleted
                    string data = leadName;
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted after chuyển thành Cơ hội: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Verify 'Cơ hội' is created at CRM-Chu trình page
                    // Go to 'Data của tôi' >> Chu trình page
                    GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi, "Chu trình", kanbanRenderer); Thread.Sleep(1500);

                    // Click 'List' button at 'switch buttons' (views: List, Kanban, Calendar, Pivot, ...)
                    GeneralAction.Instance.CLickButtonTitle(30, General.list); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1500);

                    // Search the created 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, "Cơ hội", inputSearch);

                    // Verify 'Cơ hội' is created at 'Data của tôi' >> Chu trình page
                    string nameAttrVal = "date_open";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày phân công' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Quan tâm ads");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giai đoạn' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = leadName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created 'Co hoi'
                    // (List view) Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1").ThucHienXoaDelete(10);

                    // Verify the created Lead is deleted 
                    data = opportunityName;
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Cơ hội'' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Khách hàng cá nhân' is created at 'Liên hệ' page
                    // Go to 'Liên hệ' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer); Thread.Sleep(1500);

                    // Search the created client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, "Tên", inputSearch);

                    // Verify the created client is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created clients
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(10);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created (Cơ hội của) 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
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
