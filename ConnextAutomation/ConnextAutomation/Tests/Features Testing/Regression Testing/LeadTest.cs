using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
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
        private string leadRenderer = "list_renderer", leadFormEditableRenderer = "o_lead_opportunity_form o_form_editable", listRenderer = "list_renderer",
        /// title, attribute fields on Form
        sinhNhat = "Sinh nhật", namSinh = "Năm sinh", ngaySinh = "Ngày sinh", email = "Email", dienThoai = "Điện thoại", mucDoUuTien = "Mức độ ưu tiên", the = "Thẻ", trangWeb = "Trang web",
        diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        loaiBenhNhan = "Loại bệnh nhân", ten = "Tên", tenChong = "Tên chồng", sdtChong = "SĐT chồng", donvidichvu = "Đơn vị dịch vụ",
        /// notebook - Ghi chu / Them thong tin
        ghiChuNoiBo = "Ghi chú nội bộ", themMoTaText = "Thêm mô tả...", thongTinThem = "Thông tin thêm", thongTinKhac = "Thông tin Khác", dsNguoiDiCung = "DS người đi cùng", quanHe = "Quan hệ",
        congTy = "Công ty", chienDich = "Chiến dịch", linkDenChienDich = "Link đến chiến dịch", phuongTien = "Phương tiện", nguon = "Nguồn", gioiThieuBoi = "Giới thiệu bởi",
        nhanSuAds = "Nhân sự Ads", loaiNguon = "Loại nguồn", doiTac = "Đối tác", cbnvGioiThieu = "CBNV Giới thiệu", trucChat = "Trực chat", telesale = "Telesale", tuVanVien = "Tư vấn viên", bacSi = "Bác sĩ", dieuDuong = "Điều dưỡng", cSKH = "CSKH",

        /// global data
        leadName = "QA auto lead dakhoa " + dateTimeNow,
        contactName = "QA auto lh dakhoa " + dateTimeNow,
        leadNameChong = "QA auto lead dakhoa  " + dateTimeNow + "_Chồng",
        nguoiDiCungName = "QA auto lead dakhoa di cung 01",
        opportunityName = "Cơ hội của QA auto dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_Create_search_delete_lead()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            /// title, attribute fields on Form
            tieuDePlaceholder = "Tiêu đề", vdGoiNhoTenCoHoiId = "name", xacSuat = "Xác suất", tenCongTy = "Tên công ty", tenLienHe = "Tên liên hệ",
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
                //LoginAction.Instance.LoginSite(60, urlInstance); // default login with dvdv = Da khoa (admin account)
                LoginAction.Instance.LoginSiteXRender(60, null, null, null, General.listRenderer); // default login with dvdv = Da khoa (admin account)

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    
                    // Go to 'Lead' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible (10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGoiNhoTenCoHoiId, leadName)
                                          .InputFieldLabel(10, tenLienHe, contactName)
                                          .InputFieldLabel(10, sinhNhat, "16/08/2004").Sleep(500);
                    GeneralAction.Instance.InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input");
                    GeneralAction.Instance.InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, email, "qaauto@connext.com")
                                          .InputFieldLabel(10, trangWeb, "https://www.google.com")
                                          .InputFieldLabel(10, diaChi, "232 Bùi Viện", diaChiPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "TP Hồ Chí Minh", tinhTpPlaceHolder).Sleep(500).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Quận 1", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          //.InputFieldLabel(10, donvidichvu, "Đa Khoa") // --> auto fill
                                          .InputFieldLabel(10, the, "Đa khoa").PressEnterKeyboard()
                                          //.CLickPriorityStar(10, mucDoUuTien, uuTienRatCaoAriaLabel) // Ticket 464
                                          /// Ghi chu noi bo / Them thong tin
                                          .CLickNotebookHeader(10, ghiChuNoiBo)
                                          .InputNotebookHeaderDescriptionContent(10, ghiChuNoiBo, "QA auto " + ghiChuNoiBo)
                                          .CLickNotebookHeader(10, thongTinThem)
                                          /// Them thong tin -Markketing
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, congTy, "Bệnh Viện Đa Khoa Hà Nội").PressEnterKeyboard() // -> this field was removed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, nhanSuAds, "Nguyễn Huyền Trang").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, LoginPage.telesaleIVFnv6).PressEnterKeyboard()
                                          /// Them thong tin - Nhân sự tại các điểm chạm
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, trucChat, "Bùi Thị Xuyến").PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, telesale, LoginPage.telesaleIVFnv1).PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, tuVanVien, LoginPage.tuVanIVFUsername).PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, bacSi, "Đào Thị Thùy Linh").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, dieuDuong, "Bùi Đức Giỏi").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cSKH, LoginPage.cskhIVFUsername).PressEnterKeyboard()
                                          /// DS nguoi di cung
                                          .CLickNotebookHeader(10, dsNguoiDiCung)
                                          .ClickAddRowListTable(10, "1", "Thêm một dòng")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .InputFieldLabelDialog(10, ten, nguoiDiCungName)
                                          .InputFieldLabelDialog(10, sinhNhat, "16/08/1994")
                                          .InputFieldLabelDialog(10, dienThoai, dienThoaidataNguoiDiCung = dienThoaidata.Replace("01", "04"))
                                          .InputFieldLabelDialog(10, quanHe, "Anh trai").PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, General.luuVaDong);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading)) 
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.CLickButtonTitle(10, General.luu)
                                          .WaitForElementInvisible(10, General.titleButton(General.luu)); Thread.Sleep(1000);
                    #endregion

                    #region Search Lead
                    // Back to Lead Page with list view (to search the created Lead) 
                    GeneralAction.Instance.CLickItemBreadcrumb(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, General.lead, inputSearch);
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

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "phone", "data-tooltip", data = dienThoaidata.Replace("19", "1 9").Replace("90", "9 0")); // "0221 9999 001"
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Điện thoại' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = leadName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    /// Verify cột 'Thẻ' --> Inprogress
                    /// ... Inprogress

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_call", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lần gọi gần nhất' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_trucchat_note", data = ""); // need check content: QA auto Ghi chú nội bộ
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú trực chat' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_id", data = "Offline / Giới thiệu / CBNV");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại nguồn' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_detail_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đường dẫn/chiến dịch' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = LoginPage.fullname);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_telesale_note", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú telesale' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "street2", data = "Phường Phạm Ngũ Lão, Quận 1");
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Quận/Huyện' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "city", data = "TP Hồ Chí Minh");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thành phố' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "my_activity_date_deadline", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đến hạn' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "mkt_chat", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trực chat' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Lead
                    // Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1")
                                          .ThucHienXoaDelete(60);

                    // Verify the created Lead is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + leadName + "'", verifyPoint);
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
        public void TC002_Create_lead_chuyen_thanh_Cohoi()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            /// title, attribute fields on Form
            tieuDePlaceholder = "Tiêu đề", vdGoiNhoTenCoHoiId = "name", xacSuat = "Xác suất", tenCongTy = "Tên công ty", tenLienHe = "Tên liên hệ",
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
                //LoginAction.Instance.LoginSite(60, urlInstance); // default login with dvdv = Da khoa (admin account)
                LoginAction.Instance.LoginSiteXRender(60, null, null, null, General.listRenderer); // default login with dvdv = Da khoa (admin account)

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    // Go to 'Lead' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead and then Chuyển thành Cơ hội
                    // Tao Lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGoiNhoTenCoHoiId, leadName)
                                          .InputFieldLabel(10, tenLienHe, contactName)
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                          .CLickNotebookHeader(10, thongTinThem)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard() // 
                                          .WaitForElementVisible(10, General.elementHtml("label", "CBNV Giới thiệu")) // wait for field displayed
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, LoginPage.telesaleIVFnv6).PressEnterKeyboard(); Thread.Sleep(1500);

                    // Chuyen thanh coi hoi
                    GeneralAction.Instance.CLickButtonTitle(10, "Chuyển thành Cơ hội")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Tạo cơ hội").Sleep(2000)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(2000);

                    //// Check if show error 'Lỗi hệ thống' popup then click 'Đồng ý' button to bypass
                    //if (GeneralAction.Instance.IsElementPresent(General.dialogErrorContentShow("Lỗi hệ thống")))
                    //{
                    //    GeneralAction.Instance.ClickHtmlElement(10, "div", "dialog_error", "//button[.='Đồng ý']").Sleep(500);
                    //    GeneralAction.Instance.CLickButtonTitleInDialog(10, "Tạo cơ hội").Sleep(1000)
                    //                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(2000);
                    //}

                    GeneralAction.Instance.CLickButtonTitleInDialog(10, General.luu.Replace("Lưu", " Lưu "))
                                          .WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.chatterMsgChangedStatusSeparatorDate("Hôm nay", "1", ".='(Đội ngũ kinh doanh)' or .='(Loại)'")) // old: ".='(Đội ngũ kinh doanh)'"
                                          .Sleep(1500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);

                    // Back to 'Lead' page to verify the created lead is not shown
                    GeneralAction.Instance.CLickItemBreadcrumb(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer).Sleep(500)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(1500);
                    #endregion

                    #region Verify the created Lead is deleted after Chuyển thành Cơ hội
                    // Search the created lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, General.lead, inputSearch);

                    // Verify the created Lead is deleted
                    string data = leadName;
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted after chuyển thành Cơ hội: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Verify 'Cơ hội' is created at CRM-Chu trình page
                    // Go to 'Data của tôi' >> Chu trình page
                    GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi); Thread.Sleep(1500);

                    // Search the created 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Cơ hội", inputSearch).Sleep(3000);

                    // Verify 'Cơ hội' is created at 'Data của tôi' >> Chu trình page
                    string nameAttrVal = "create_date";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_open", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày phân công' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Dữ Liệu Mới")
                               || GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Dữ liệu mới");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giai đoạn' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created 'Co hoi'
                    // Way 1 --> not work due to unable to mouse hover on the element
                    // Click '...' button
                    /*
                    GeneralAction.Instance.CLickButtonRecordTitleKanbanInCol(10, "Dữ liệu mới", "QA auto Gia san pham 01")
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickButtonRecordTitleKanbanInCol(10, "Dữ liệu mới", "QA auto Gia san pham 01", "a", "Xoá")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Đồng ý");
                    */

                    // Way 2: Click on the title record in Kanban --> only apply for Kanban view
                    // Click on Record title of a column to verify data
                    /*
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, "Dữ liệu mới", "QA auto Gia san pham " + dateTimeNow)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Back to 'Chu Trinh' page
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Chu trình")
                                          .WaitForElementVisible(10, General.kanbanRenderer);

                    // search the deleted 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the created 'Co hoi' is deleted
                    verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted("Dữ liệu mới", data = "QA auto Gia san pham " + dateTimeNow);
                    verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    */

                    // Way 3: Clikc on Checkbox --> only apply for List view
                    /// Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1").ThucHienXoaDelete(60);

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
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(3000);
                                          
                    // Verify the created client is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created clients
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created (Cơ hội của) 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
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

        //[Test, Category("DKHN - Regression Tests")]
        #region TC003_Create_search_delete_lead_ticket() --> no run
        /*
        public void TC003_Create_search_delete_lead_ticket()
        {
            #region Variables declare
            string urlInstance = LoginPage.url,
            dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            const string inputSearch = "qa auto", searchboxRole = "searchbox",
            /// title, attribute fields on Form
            vdGiaSanPhamId = "name", xacSuat = "Xác suất", tenCongTy = "Tên công ty", tenLienHe = "Tên liên hệ",
            taoTicket = "Tạo ticket", tieuDePlaceHolder = "Tiêu đề...", khackHang = "Khách hàng", boPhanPhuTrach = "Bộ phận phụ trách",
            caNhanPhuTrach = "Cá nhân phụ trách", ngayPhanHoi = "Ngày phản hồi", loaiTicket = "Loại ticket", khCoHoiTiemNang = "Khách hàng tiềm năng/Cơ hội",
            booking = "Booking", loaiVanDe = "Loại vấn đề", quyTrinh = "Quy trình", ngayHetHan = "Ngày hết hạn",
            /// notebook - Thong tin mo ta / Giai phap xu ly ...
            thongTinMoTa = "Thông tin mô tả", giaiPhapXuLy = "Giải pháp xử lý", xuLy = "Xử lý", dienGiai = "Diễn giải", tepDinhKem = "Tệp đính kèm",
            thongTinKhac = "Thông tin khác", ngayDong = "Ngày đóng", dongBoi = "Đóng bởi", ghiChu = "Ghi chú",
            ngayHuyBo = "Ngày hủy bỏ", daHuyBoi = "Đã hủy bởi", lyDoHuy = "Lý do hủy", lichSuThayDoiTrangThai = "Lịch sử thay đổi trạng thái";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Lead Test 003 - Create, delete Lead Ticket");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains("qa-odoo") || urlInstance.Contains("staging-bvdkhn"))
                {
                    // Go to Lead menu
                    GeneralAction.Instance.CLickMenuTitle(10, General.lead)
                                          .WaitForElementVisible(10, General.rendererTable(leadRenderer));

                    // Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead, ticket
                    // Tao Lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGiaSanPhamId, "QA auto Gia san pham " + dateTimeNow)
                                          //.InputFieldLabel(10, tenCongTy, "Connext")
                                          .InputFieldLabel(10, tenLienHe, "QA auto lh " + dateTimeNow);
                    // Tao ticket
                    GeneralAction.Instance.CLickButtonTitle(10, taoTicket)
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .InputByAttributeValue(10, tieuDePlaceHolder, "QA auto Helpdesk ticket tieu de " + dateTimeNow);
                    GeneralAction.Instance.InputFieldLabelDialog(10, khackHang, "QA auto lh " + dateTimeNow) // Dương Ngọc Vy - 0869 812 427 - 27/02/1999
                                          .ClickInputFieldLabelDialog(10, boPhanPhuTrach); Thread.Sleep(1000);
                    /// Check If the popup/dialog 'Mới: Khách hàng' is shown then click 'Tạo' to create a new one
                    if (GeneralAction.Instance.IsElementPresent(General.DialogActiveShow) && GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Tạo")))
                    {
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Tạo")
                                              .WaitForElementInvisible(10, General.DialogIndexShow("2")); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.InputFieldLabelDialog(10, boPhanPhuTrach, "Khối Kinh Doanh / Đơn Vị Vô Sinh Hiếm Muộn / Nhóm Tư Vấn / Bộ Phận Chăm Sóc Khách Hàng")
                                          .InputFieldLabelDialog(10, email, "qaauto@connext.com")
                                          .InputFieldLabelDialog(10, caNhanPhuTrach, "QA auto Ca nhan phu trach")
                                          .InputFieldLabelDialog(10, dienThoai, "02219999991")
                                          .InputFieldLabelDialog(10, ngayPhanHoi, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          .InputFieldLabelDialog(10, loaiTicket, "Phàn nàn") // Khám mới
                                                                                             //.InputFieldLabelDialog(10, khCoHoiTiemNang, "QA auto Gia san pham 01") --> No need input (already have default)
                                          .InputFieldLabelDialog(10, mucDoUuTien, "Rất cao")
                                          //.InputFieldLabelDialog(10, booking, "QA auto Booking"); // if input -> will open 'Tao Booking' popup (maybe link to books)
                                          .InputFieldLabelDialog(10, loaiVanDe, "Thái độ nhân viên")
                                          .InputFieldLabelDialog(10, quyTrinh, "Tiếp đón")
                                          .InputFieldLabelDialog(10, the, "YHTT")
                                          .InputFieldLabelDialog(10, ngayHetHan, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          /// Thong tin mo ta / Giai phap xu ly ...
                                          .CLickNotebookHeader(10, thongTinMoTa)
                                          .InputNotebookHeaderDescriptionContent(10, thongTinMoTa, "QA auto " + thongTinMoTa)
                                          .CLickNotebookHeader(10, giaiPhapXuLy)
                                          .InputNotebookHeaderAndFieldNameContent(10, giaiPhapXuLy, xuLy, "Đã nhắc nhở cá nhân phụ trách")
                                          .InputNotebookHeaderDescriptionContent(10, giaiPhapXuLy, "QA auto " + dienGiai)
                                          .CLickNotebookHeader(10, tepDinhKem) // --> No test this Notebook Header
                                          .CLickNotebookHeader(10, thongTinKhac)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, ngayDong, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, dongBoi, LoginPage.fullname) // old: Bác sĩ Điệp
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, ghiChu, "QA auto " + ghiChu, "textarea") // textarea --> (tag in html)
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, ngayHuyBo, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, daHuyBoi, LoginPage.fullname) // old: Châu Văn Liêm
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinKhac, lyDoHuy, "QA auto " + lyDoHuy)
                                          .CLickNotebookHeader(10, lichSuThayDoiTrangThai).CLickNotebookHeader(10, lichSuThayDoiTrangThai) // Workaround if spinner icon not shown
                                          .CLickButtonTitleInDialog(10, General.luu); Thread.Sleep(3000);

                    // Check if the spinner loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading)) 
                    {
                        GeneralAction.Instance.WaitForLoadingIconToDisappear(10, General.spinnerLoading);
                    }

                    //GeneralAction.Instance.WaitForElementInvisible(10, General.DialogShow); 
                    Thread.Sleep(2000);
                    #endregion

                    #region Verify data on 'ticket' page
                    // Click on 'xTickets' button
                    GeneralAction.Instance.CLickButtonTitle(10, "1Tickets")
                                          .WaitForElementVisible(10, General.rendererTable("form_sheet position")); Thread.Sleep(2000);

                    // Verify data in Ticket page
                    string data = "QA auto Helpdesk ticket tieu de " + dateTimeNow;
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, tieuDePlaceHolder, data);
                    verifyPoints.Add(summaryTC = "Verify data of 'Helpdesk' title in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khackHang, data= "QA auto lh " + dateTimeNow); //  Dương Ngọc Vy - 0869 812 427 - 27/02/1999
                    verifyPoints.Add(summaryTC = "Verify data of '" + khackHang + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, email, data = "qaauto@connext.com");
                    verifyPoints.Add(summaryTC = "Verify data of '" + email + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, boPhanPhuTrach, "1", data = "Khối Kinh Doanh / Đơn Vị Vô Sinh Hiếm Muộn / Nhóm Tư Vấn / Bộ Phận Chăm Sóc Khách Hàng");
                    verifyPoints.Add(summaryTC = "Verify data of '" + boPhanPhuTrach + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, caNhanPhuTrach, "1", data = "QA auto Ca nhan phu trach");
                    verifyPoints.Add(summaryTC = "Verify data of '" + caNhanPhuTrach + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = "0221 9999 991");
                    verifyPoints.Add(summaryTC = "Verify data of '" + dienThoai + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayPhanHoi, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data of '" + ngayPhanHoi + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, loaiTicket, data = "Phàn nàn"); // Khám mới
                    verifyPoints.Add(summaryTC = "Verify data of '" + loaiTicket + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khCoHoiTiemNang, data = "QA auto Gia san pham " + dateTimeNow);
                    verifyPoints.Add(summaryTC = "Verify data of '" + khCoHoiTiemNang + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, mucDoUuTien, data = "Rất cao");
                    verifyPoints.Add(summaryTC = "Verify data of '" + mucDoUuTien + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, loaiVanDe, data = "Thái độ nhân viên");
                    verifyPoints.Add(summaryTC = "Verify data of '" + loaiVanDe + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, quyTrinh, "1", data = "Tiếp đón");
                    verifyPoints.Add(summaryTC = "Verify data of '" + quyTrinh + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayHetHan, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data of '" + ngayHetHan + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, the, "1", data = "YHTT");
                    verifyPoints.Add(summaryTC = "Verify data of '" + the + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    /// Verify data on Notebook 'Thông tin mô tả', 'Giải pháp xử lý', ...
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderDescriptionContentGetText(10, thongTinMoTa, data = "QA auto Thông tin mô tả");
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinMoTa + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Click on tab 'Giải pháp xử lý' and then verify data in the field
                    GeneralAction.Instance.CLickNotebookHeader(10, giaiPhapXuLy);
                    verifyPoint = GeneralAction.Instance.DataItemNotebookHeaderAndFieldNameContentGetText(10, giaiPhapXuLy, xuLy, "1", data = "Đã nhắc nhở cá nhân phụ trách")
                               || GeneralAction.Instance.DataItemNotebookHeaderAndFieldNameContentGetText(10, giaiPhapXuLy, xuLy, "1", data = "đã nhắc nhở cá nhân phụ trách");
                    verifyPoints.Add(summaryTC = "Verify data of '" + giaiPhapXuLy + "'-'" + xuLy + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderDescriptionContentGetText(10, giaiPhapXuLy, data = "QA auto Diễn giải");
                    verifyPoints.Add(summaryTC = "Verify data of '" + giaiPhapXuLy + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Click on tab 'Thông tin khác' and then verify data in the field
                    GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, ngayDong, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + ngayDong + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, ngayHuyBo, data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + ngayHuyBo + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, dongBoi, data = LoginPage.fullname); // old: "Bác sĩ Điệp"
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + dongBoi + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, daHuyBoi, data = LoginPage.fullname); // old: "Châu Văn Liêm"
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + daHuyBoi + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, ghiChu, data = "QA auto Ghi chú", "textarea");
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + ghiChu + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinKhac, lyDoHuy, data = "QA auto Lý do hủy");
                    verifyPoints.Add(summaryTC = "Verify data of '" + thongTinKhac + "'-'" + lyDoHuy + "' field in Ticket is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete Ticket
                    // Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Verify ticket is deleted
                    verifyPoint = GeneralAction.Instance.IsButtonTitleDeleted("1Tickets");
                    verifyPoints.Add(summaryTC = "Verify the created ticket is deleted", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete Lead
                    // Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, General.lead, inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);
                  
                    // Verify the created Lead is deleted 
                    data = "QA auto Gia san pham " + dateTimeNow; // QA auto lh 01
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Verify 'Khách hàng cá nhân' is created at 'Liên hệ' page, and then delete it
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
                    string nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data= "QA auto lh " + dateTimeNow);
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

                    #region Verify 'Cá nhân phụ trách' is created at 'Nhân viên' (Employee) page, and then delete it
                    // Go to 'Nhân viên' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.nhanVien)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Click 'list' button at 'switch buttons'
                    GeneralAction.Instance.CLickButtonTitle(10, General.list)
                                          .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                    // Search the created 'Cá nhân phụ trách' (Employee)
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Nhân viên", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the created 'Cá nhân phụ trách' (Employee) is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = "QA auto Ca nhan phu trach");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên Nhân viên' is shown after searching the created 'Cá nhân phụ trách' (Nhân viên): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete this created client
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);

                    // Verify the created 'Cá nhân phụ trách' (Employee) is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Cá nhân phụ trách' at 'Nhân viên' (Employee) is deleted: '" + data + "'", verifyPoint);
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
        */
        #endregion
    }
}
