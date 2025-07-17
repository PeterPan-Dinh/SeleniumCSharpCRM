using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(5)]
    internal class PhanBoDuLieuTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");

        /// table renderer (used for wait method)
        private string listRenderer = "list_renderer", leadFormEditableRenderer = "o_lead_opportunity_form o_form_editable",
        /// title, attribute fields on Form
        sinhNhat = "Sinh nhật", namSinh = "Năm sinh", ngaySinh = "Ngày sinh", email = "Email", dienThoai = "Điện thoại", mucDoUuTien = "Mức độ ưu tiên", the = "Thẻ", trangWeb = "Trang web",
        diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        loaiBenhNhan = "Loại bệnh nhân", tenChong = "Tên chồng", sdtChong = "SĐT chồng", donvidichvu = "Đơn vị dịch vụ",
        /// notebook - Ghi chu / Them thong tin
        ghiChuNoiBo = "Ghi chú nội bộ", themMoTaText = "Thêm mô tả...", thongTinThem = "Thông tin thêm", thongTinKhac = "Thông tin Khác",
        congTy = "Công ty", chienDich = "Chiến dịch", linkDenChienDich = "Link đến chiến dịch", phuongTien = "Phương tiện", nguon = "Nguồn", gioiThieuBoi = "Giới thiệu bởi",
        nhanSuAds = "Nhân sự Ads", loaiNguon = "Loại nguồn", doiTac = "Đối tác", cbnvGioiThieu = "CBNV Giới thiệu", trucChat = "Trực chat", telesale = "Telesale", tuVanVien = "Tư vấn viên", bacSi = "Bác sĩ", dieuDuong = "Điều dưỡng", cSKH = "CSKH",

        /// global data
        leadName = "QA auto lead ivf " + dateTimeNow,
        contactName = "QA auto lh ivf " + dateTimeNow,
        leadNameChong = "QA auto lead ivf  " + dateTimeNow + "_Chồng",
        opportunityName = "Cơ hội của QA auto ivf " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_PhanBoDuLieu_lead()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            adminIVFUsername = LoginPage.adminIVFUsername,
            adminIVFPass = LoginPage.adminIVFPass,
            trucChatLeadIVFEmail = LoginPage.trucChatLeadIVFEmail,
            trucChatLeadIVFUsername = LoginPage.trucChatLeadIVFUsername,
            telesaleLeadIVFEmail = LoginPage.telesaleLeadIVFEmail,
            telesaleLeadIVFUsername = LoginPage.telesaleLeadIVFUsername + " ",
            telesaleIVFUsername = LoginPage.telesaleIVFUsername,
            telesaleIVFTeam = LoginPage.telesaleIVFTeam,
            /// title, attribute fields on Form
            vdGoiNhoTenCoHoiId = "name", tenLienHe = "Tên liên hệ",
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong, loaiNguonData, doiTacData, cbnvGioiThieuData;
            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Phan bo du lieu Lead");
            try
            {
                // Log into the application
                //LoginAction.Instance.LoginSite(60, urlInstance, adminIVFUsername, adminIVFPass);
                LoginAction.Instance.LoginSiteXRender(60, urlInstance, adminIVFUsername, adminIVFPass, General.listRenderer);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    // Switch to truc chat lead account to create a Lead (khach hang tiem nang)
                    GeneralAction.Instance.SwitchUser(10, trucChatLeadIVFEmail, trucChatLeadIVFUsername);

                    // Go to 'Lead' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer);

                    // Create a Lead (khach hang tiem nang)
                    /// Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.rendererTable(leadFormEditableRenderer));

                    #region Input data to create lead
                    GeneralAction.Instance.InputByAttributeValue(10, vdGoiNhoTenCoHoiId, leadName)
                                          .InputFieldLabel(10, tenLienHe, contactName)
                                          .InputFieldLabel(10, sinhNhat, "16/08/2004");
                    GeneralAction.Instance.InputByAttributeValue(10, "phone", dienThoaidata = "02219999001", "div[@name='phone']/input")
                                          .InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, email, "qaauto@connext.com")
                                          .InputFieldLabel(10, trangWeb, "https://www.google.com").Sleep(500)
                                          .InputFieldLabel(10, diaChi, "232 Bùi Viện", diaChiPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "TP Hồ Chí Minh", tinhTpPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Quận 1", quanHuyenPlaceHolder).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Phường Phạm Ngũ Lão", phuongXaPlaceHolder).Sleep(500).PressEnterKeyboard();
                    GeneralAction.Instance.CLickAndSelectItemInDropdownLabel(10, loaiBenhNhan, "Bệnh lý");
                    /// Input thông tin chồng
                    GeneralAction.Instance.InputFieldLabel(10, tenChong, leadNameChong);
                    GeneralAction.Instance.InputByAttributeValue(10, "phone", dienThoaidataChong = dienThoaidata.Replace("001", "201"), "div[@name='husband_phone']/input")
                                          .InputFieldLabel(10, ngaySinh, "16/01/1994")
                                          //.InputFieldLabel(10, donvidichvu, "IVF") // --> auto fill
                                          .InputFieldLabel(10, the, "ICSI/IVF").PressEnterKeyboard()
                                          /// Ghi chu noi bo / Them thong tin
                                          .CLickNotebookHeader(10, ghiChuNoiBo)
                                          .InputNotebookHeaderDescriptionContent(10, ghiChuNoiBo, "QA auto " + ghiChuNoiBo)
                                          .CLickNotebookHeader(10, thongTinThem)
                                          /// Them thong tin -Markketing
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, congTy, "Bệnh Viện Đa Khoa Hà Nội")
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, nhanSuAds, "Nguyễn Huyền Trang").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, loaiNguon, loaiNguonData = "Offline / Giới thiệu / CBNV - BVĐK HN").PressEnterKeyboard()
                                          .WaitForElementVisible(10, General.elementHtml("label", cbnvGioiThieu))
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cbnvGioiThieu, cbnvGioiThieuData = "Nguyễn Khánh Tâm").PressEnterKeyboard()
                                          /// Them thong tin - Nhân sự tại các điểm chạm
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, trucChat, "QA tn IVF trực chat 01").PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, telesale, "QA tn IVF telesale 01").PressEnterKeyboard()
                                          //.InputNotebookHeaderAndFieldNameContent(10, thongTinThem, tuVanVien, "QA tn tư vấn 01").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, bacSi, "Đỗ Thị Thi").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, dieuDuong, "Nguyễn Văn Minh").PressEnterKeyboard()
                                          .InputNotebookHeaderAndFieldNameContent(10, thongTinThem, cSKH, "Dương Ngọc Vy").PressEnterKeyboard();

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.CLickButtonTitle(10, General.luuThuCong);
                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }
                    Thread.Sleep(3000);
                    #endregion

                    #region Search Lead
                    // Click 'Lead' menu to back to Lead menu with 'List view'
                    GeneralAction.Instance.CLickMainMenuTitle(10, General.lead).Sleep(5000)
                                          .WaitForElementVisible(10, General.rendererTable(listRenderer));

                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

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

                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "street2", data = "Phường Phạm Ngũ Lão, Quận 1");
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Quận/Huyện' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "city", data = "TP Hồ Chí Minh");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thành phố' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phone", data = "Gọi"); // old: 0221 9999 991 Gọi
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Điện thoại' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = leadName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_telesale_note", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú telesale' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "tag_ids", data = "ICSI/IVF");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Thẻ' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = trucChatLeadIVFUsername); // "QA tn IVF trực chat 01"
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "product_ids", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nguồn DV' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_detail_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đường dẫn/chiến dịch' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_trucchat_note", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ghi chú của Trực chat' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "my_activity_date_deadline", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày đến hạn' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "mkt_chat", data = trucChatLeadIVFUsername); // "QA tn IVF trực chat 01"
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trực chat' is shown after searching the created lead: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu ('lead' cho nhom telesale)
                    // Click the checkbox of the 1st row
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, "QA auto lead ivf " + dateTimeNow, "name", "name");

                    // Click 'Thực hiện' button to phân bổ lead cho nhóm telesale
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Chia dữ liệu cho nhóm: Telesale IVF - Hiệu suất").WaitForElementVisible(10, General.DialogShow);

                    // Click checkbox 'Yêu cầu can thiệp bằng tay'
                    GeneralAction.Instance.ClickCheckboxAtFieldLabel(10, "Yêu cầu can thiệp bằng tay")
                                          .InputFieldLabelDialog(10, "Nhóm", "TEAM THUỶ").PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, "Phân bổ dữ liệu").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify 'Đội ngũ kinh doanh' was distributed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = telesaleIVFTeam);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after after distributing lead for team: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify 'Nhân viên kinh doanh' was distributed
                    // Define data
                    string nv1 = LoginPage.telesaleIVFnv1;
                    string nv2 = LoginPage.telesaleIVFnv2;
                    string nv3 = LoginPage.telesaleIVFnv3;
                    string nv4 = LoginPage.telesaleIVFnv4;
                    string nv5 = LoginPage.telesaleIVFnv5;
                    string nv6 = LoginPage.telesaleIVFnv6;
                    string nv7 = LoginPage.telesaleIVFnv7;
                    string nv8 = LoginPage.telesaleIVFnv8;
                    string nv9 = LoginPage.telesaleIVFnv9;
                    string nv10 = LoginPage.telesaleIVFnv10;
                    string nv11 = LoginPage.telesaleIVFnv11;
                    // Create list
                    List<string>? list_nv = new List<string> { nv1, nv2, nv3, nv4, nv5, nv6, nv7, nv8, nv9, nv10, nv11 };
                    string nvGetText = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetTextOnly(10, "1", nameAttrVal = "user_id");
                    string matchedNv = string.Empty;
                    foreach (string? nv in list_nv)
                    {
                        if (nvGetText.Contains(nv))
                        {
                            matchedNv = nv;
                            break;
                        }
                    }

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = matchedNv);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after after distributing lead for user: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu ('lead' cho thanh vien)
                    // Switch to account lead telesale to distribute data for member telesale
                    GeneralAction.Instance.SwitchUser(10, telesaleLeadIVFEmail, telesaleLeadIVFUsername);

                    // Go to 'Lead' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lead, "Lead", listRenderer);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, General.lead, inputSearch);

                    // Click the checkbox of the 1st row
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, "QA auto lead ivf " + dateTimeNow, "name", "name");

                    // Click 'Thực hiện' button to phân bổ lead thành viên telesale
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Chia dữ liệu cho thành viên: Telesale IVF 2 - Chia đều").WaitForElementVisible(10, General.DialogShow);

                    // Click checkbox 'Yêu cầu can thiệp bằng tay'
                    GeneralAction.Instance.ClickCheckboxAtFieldLabel(10, "Yêu cầu can thiệp bằng tay").Sleep(1000)
                                          .InputFieldLabelDialog(10, "Nhân viên", data = telesaleIVFUsername).Sleep(1000).PressEnterKeyboard().Sleep(1000)
                                          .CLickButtonTitleInDialog(10, "Phân bổ dữ liệu").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify lead was distributed at 'Nhân viên kinh doanh' 
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after distributing lead for member: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Lead
                    // Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1").ThucHienXoaDelete(60);

                    // Verify the created Lead is deleted 
                    data = leadName;
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu hang loat
                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, General.lead, "zalo test lead").Sleep(3000);

                    // Check all checkboxes
                    GeneralAction.Instance.ClickCheckboxAlltable(10).Sleep(1500)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Chia dữ liệu cho thành viên: Telesale IVF 2 - Chia đều").WaitForElementVisible(10, General.DialogShow);

                    // Click checkbox 'Yêu cầu can thiệp bằng tay'
                    GeneralAction.Instance.ClickCheckboxAtFieldLabel(10, "Yêu cầu can thiệp bằng tay")
                                          .InputFieldLabelDialog(10, "Nhân viên", data = telesaleIVFUsername).PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, "Phân bổ dữ liệu").Sleep(1000).WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).Sleep(1000).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify lead was distributed at 'Nhân viên kinh doanh' 
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after distributing (multiple) lead for member: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "user_id", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 2nd row, column 'Nhân viên kinh doanh' is shown after distributing (multiple) lead for member: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "user_id", data);
                    //verifyPoints.Add(summaryTC = "Verify data at the 3rd row, column 'Nhân viên kinh doanh' is shown after distributing (multiple) lead for member: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "4", nameAttrVal = "user_id", data);
                    //verifyPoints.Add(summaryTC = "Verify data at the 4th row, column 'Nhân viên kinh doanh' is shown after distributing (multiple) lead for member: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "5", nameAttrVal = "user_id", data);
                    //verifyPoints.Add(summaryTC = "Verify data at the 5th row, column 'Nhân viên kinh doanh' is shown after distributing (multiple) lead for member: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
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
        public void TC002_PhanBoDuLieu_Booking()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            adminIVFUsername = LoginPage.adminIVFUsername,
            adminIVFFullname = LoginPage.adminIVFFullname,
            adminIVFPass = LoginPage.adminIVFPass,
            tuVanLeadIVFEmail = LoginPage.tuVanLeadIVFEmail,
            tuVanLeadIVFUsername = LoginPage.tuVanLeadIVFUsername,
            tuVanIVFUsername = LoginPage.tuVanIVFUsername,
            tuVanIVFTeam = LoginPage.tuVanIVFTeam,
            /// title, attribute fields on Form
            vdGoiNhoTenCoHoiId = "name", tenLienHe = "Tên liên hệ", khachHang = "Khách hàng", ngayBookingId = "date_order", loaiBooking = "Loại booking", gioiTinh = "Giới tính", opportunityId = "opportunity_id",
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong;
            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Phan bo du lieu Booking");
            try
            {
                // Log into the application
                //LoginAction.Instance.LoginSite(60, urlInstance, adminIVFUsername, adminIVFPass);
                LoginAction.Instance.LoginSiteXRender(60, urlInstance, adminIVFUsername, adminIVFPass, General.listRenderer);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn")) 
                {
                    // Switch to 'Tu van' lead account to create a Booking
                    GeneralAction.Instance.SwitchUser(10, tuVanLeadIVFEmail, tuVanLeadIVFUsername);

                    #region Create new Booking
                    // Go to 'Tư vấn' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.tuVan, "Booking", listRenderer);

                    // Create Booking
                    /// Click on 'MỚI' button
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Input data to create Booking
                    ///string theLieutrinhDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                    GeneralAction.Instance.ClickInputFieldLabel(10, khachHang);
                    GeneralAction.Instance.InputFieldLabel(10, khachHang, contactName); Thread.Sleep(500);
                    GeneralAction.Instance.PressDownKeyboard();
                    GeneralAction.Instance.PressEnterKeyboard(); Thread.Sleep(3000);

                    /// Check If the popup/dialog 'Mới: Khách hàng' is shown then click 'Tạo' to create a new one
                    if (GeneralAction.Instance.IsElementPresent(General.DialogActiveShow) && GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Lưu & Đóng")))
                    {
                        GeneralAction.Instance.InputFieldLabelDialog(10, dienThoai, "02219999001");
                        GeneralAction.Instance.InputFieldLabelDialog(10, ngaySinh, "20/09/2001");
                        GeneralAction.Instance.CLickAndSelectItemInDropdownLabel(10, gioiTinh, "Nữ");
                        GeneralAction.Instance.ClickHtmlElement(10, "div[contains(@class,'modal-content')]//*", "notebook_header", "//li[.='" + thongTinKhac + "' or .='" + thongTinKhac.Replace("Khác", "khác") + "']");
                        GeneralAction.Instance.InputFieldLabelDialog(10, loaiNguon, "Offline / Tự đến").PressEnterKeyboard();
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu & Đóng");
                    }

                    /// input data at field 'Cơ hội'
                    GeneralAction.Instance.InputByAttributeValue(10, opportunityId, opportunityName, "input").PressEnterKeyboard();
                    GeneralAction.Instance.ClickHtmlElement(10, "*", "notebook_header", "//li[.='" + thongTinKhac + "' or .='" + thongTinKhac.Replace("Khác", "khác") + "']");
                    GeneralAction.Instance.InputHtmlElement(10, "*", "notebook_header", "Offline / Tự đến", "//li[.='" + thongTinKhac + "' or .='" + thongTinKhac.Replace("Khác", "khác") + "']/ancestor::div[contains(@class,'o_notebook d')]//label[contains(.,'" + loaiNguon + "')]/parent::div/following-sibling::div//input").PressEnterKeyboard();
                    GeneralAction.Instance.CLickButtonTitle(10, General.luu.Replace(" ", "")).Sleep(1000);
                    GeneralAction.Instance.CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
                    #endregion

                    #region Verify the new booking is created
                    // Back to 'Booking' list screen by clickinng breadcrumb title
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Booking").Sleep(1000);

                    // Check if dialog alert show then wait for it to disappear, and then re-click button 'booking'
                    if (GeneralAction.Instance.IsElementPresent(General.DialogShow))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.DialogShow);
                    }

                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Booking")
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Search the new created booking
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, "Họ tên vợ - năm sinh", contactName);

                    // Verify the new Booking is created
                    string nameAttrVal = "create_date", data = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_order", data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày giờ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_id", data = "Chưa xác định"); // old: "Offline / Tự đến"
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại nguồn' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "booking_type", data = "Khám");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại Booking' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "wife_infor", data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên vợ - năm sinh' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "wife_phone", "data-tooltip", data = "0221 9999 001");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT Vợ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "husband_infor", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên chồng - năm sinh' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "husband_phone", "data-tooltip", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT Chồng' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "description", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tình trạng bệnh lý' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "booking_status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "last_consulting_status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái tư vấn' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "telesale_employee", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'NV Phụ trách' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "doctor_id", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Bác sĩ' is shown after searching the created Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu ('Booking') cho nhom Tu van
                    // Click the checkbox of the 1st row
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, contactName + " - " + "20/09/2001", "name", "wife_infor");

                    // Click 'Thực hiện' button to phân bổ lead cho nhóm telesale
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Chia dữ liệu cho nhóm: Nhóm Tư Vấn IVF - Chia đều")
                                          .WaitForElementInvisible(10, General.titleButton(General.thucHien)); Thread.Sleep(1000);

                    // Verify 'Đội ngũ kinh doanh' was distributed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = tuVanIVFTeam);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after after distributing Booking (auto): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu ('Booking') cho thanh vien Tu van
                    // Click the checkbox of the 1st row
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, contactName + " - " + "20/09/2001", "name", "wife_infor");

                    // Click 'Thực hiện' button to phân bổ lead thành viên tư vấn
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Chia dữ liệu cho thành viên: Tư vấn IVF - Chia đều").WaitForElementVisible(10, General.DialogShow);

                    // Click checkbox 'Yêu cầu can thiệp bằng tay'
                    GeneralAction.Instance.ClickCheckboxAtFieldLabel(10, "Yêu cầu can thiệp bằng tay")
                                          .InputFieldLabelDialog(10, "Nhân viên", data = tuVanIVFUsername).PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, "Phân bổ dữ liệu").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify 'NV Phụ trách' was distributed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "consulting_employee", data = tuVanIVFUsername);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'NV tư vấn' is shown after after distributing Booking: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Booking
                    // Click the checkbox of the 1st row
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, contactName + " - " + "20/09/2001", "name", "wife_infor");

                    // Click 'Thực hiện' menu/button and then click 'Xóa'
                    GeneralAction.Instance.ThucHienXoaDelete(60);

                    // Verify the created Booking is deleted 
                    data = contactName + " - " + "20/09/2001";
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created Booking is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created 'lien he'
                    // Switch back to the original account (admin)  to have permission to delete
                    GeneralAction.Instance.SwitchUserBack(15).Sleep(2000).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);

                    // Go to 'Liên hệ' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer); Thread.Sleep(1500);

                    // Search the created client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(5000);

                    // Verify the created client is shown
                    nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = contactName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created client
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Lead
                    //// Go to Lead page
                    //GeneralAction.Instance.GoToLeftMenu(10, General.lead); Thread.Sleep(1000);

                    //// Check if the spinner Loading icon is shown then wait for it to load done
                    //if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    //{
                    //    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    //}
                    //Thread.Sleep(1000);

                    //// Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    //GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    //// Search Lead
                    ///// Input a value in seachbox to Search
                    //GeneralAction.Instance.SearchViewInput(60, General.lead, inputSearch);

                    //// Verify the created Lead is shown
                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = opportunityName);
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the created 'Lead': '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    //// Delete all these created lead
                    //GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    //// Verify the created Lead is deleted 
                    //verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    //verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the created Co hoi
                    // Go to 'Data của tôi' >> 'Chu trình' page (list view)
                    GeneralAction.Instance.GoToLeftMenu(10, General.dataCuaToi, "Chu trình", listRenderer); Thread.Sleep(1000);

                    // Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }
                    Thread.Sleep(1000);

                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Search Cơ hội
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Cơ hội", inputSearch);

                    // Verify the created Lead is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = opportunityName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Cơ hội' is shown after searching the created 'Cơ hội': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created lead
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    // Verify the created Lead is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
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
        public void TC003_PhanBoDuLieu_LanKham()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            adminIVFUsername = LoginPage.adminIVFUsername,
            adminIVFFullname = LoginPage.adminIVFFullname,
            adminIVFPass = LoginPage.adminIVFPass,
            cskhLeadIVFEmail = LoginPage.cskhLeadIVFEmail,
            cskhLeadIVFUsername = LoginPage.cskhLeadIVFUsername,
            cskhIVFUsername = LoginPage.cskhIVFUsername,
            cskhIVFTeam = LoginPage.cskhIVFTeam;
            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Phan bo du lieu Lan kham");
            try
            {
                // Log into the application
                //LoginAction.Instance.LoginSite(60, urlInstance, adminIVFUsername, adminIVFPass);
                LoginAction.Instance.LoginSiteXRender(60, urlInstance, adminIVFUsername, adminIVFPass, General.listRenderer);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    // Switch to 'CSKH' lead account to phan bo data
                    GeneralAction.Instance.SwitchUser(10, cskhLeadIVFEmail, cskhLeadIVFUsername);

                    #region Search lan kham theo ten co hoi
                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Search (lần khám) theo cơ hội
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(10, "Cơ hội", "zalo hkd ivf test");

                    // Verify 'lần khám' is shown after searching Cơ hội at 'CSKH' screen (list)
                    string nameAttrVal, data;
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "res_partner_id", data = "zalo hkd ivf lh test 02 - 1111 111112");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Người khám' is shown after searching 'Cơ hội at CSKH screen (list): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu hang loat ('Lan kham') cho nhom CSKH (auto)
                    // Check all checkboxes
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Phân bổ cho đội")
                                          .InputFieldLabelDialog(10, "Đội ngũ CSKH", data = cskhIVFTeam).PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, "Gán").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify 'Đội ngũ CSKH' was distributed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "cskh_team_id", data = cskhIVFTeam);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ CSKH' is shown after after distributing Booking (auto): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "cskh_team_id", data = cskhIVFTeam);
                    verifyPoints.Add(summaryTC = "Verify data at the 4th row, column 'Đội ngũ CSKH' is shown after after distributing Booking (auto): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Phan bo du lieu hang loat ('Lan kham') cho thanh vien CSKH (auto)
                    // Check all checkboxes
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Phân bổ cho thành viên").Sleep(1500)
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .InputFieldLabelDialog(10, "Nhân viên CSKH", data = cskhIVFUsername).PressEnterKeyboard()
                                          .CLickButtonTitleInDialog(10, "Gán").WaitForElementInvisible(10, General.DialogShow).Sleep(1500)
                                          .WaitForElementVisible(10, General.alertContentShow("Đã phân bổ thành công")).WaitForElementInvisible(10, General.alertContentShow("Đã phân bổ thành công"));

                    // Verify 'Nhân viên CSKH' was distributed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "cskh_user_id", data = cskhIVFUsername)
                               || GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "cskh_user_id", data = cskhLeadIVFUsername); 
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên CSKH' is shown after after distributing Booking (auto): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "cskh_user_id", data = cskhIVFUsername)
                               || GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "cskh_user_id", data = cskhLeadIVFUsername);
                    verifyPoints.Add(summaryTC = "Verify data at the 4tht row, column 'Nhân viên CSKH' is shown after after distributing Booking (auto): '" + data + "'", verifyPoint);
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
