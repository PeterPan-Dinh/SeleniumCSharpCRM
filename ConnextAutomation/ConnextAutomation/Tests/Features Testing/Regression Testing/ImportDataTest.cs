using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(8)]
    internal class ImportDataTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// title, attribute fields on Form ...
        private string moiButton = "/button[.='Mới' or .=' Mới ']",
        /// global data
        leadName = "QA auto lead dakhoa " + dateTimeNow,
        contactName = "QA auto lh dakhoa " + dateTimeNow,
        opportunityName = "Cơ hội của QA auto dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_ImportData_ImportDaKhoa()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            const string inputSearch = "qa auto", searchboxRole = "searchbox", importBtn = "btn btn-primary ms", clientName = "QA auto import đa khoa 01",
            yearOfBirth = "1991", phoneNumbers = "02218999001", opportunityNamePlaceHolder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", vdNguyenVanAPlaceHolder = "Hãy nhập tên khách hàng, ví dụ: Nguyễn Văn An",
            doanhThuDuKienId = "expected_revenue", maBenhNhan = "Mã bệnh nhân", khachHang = "Khách hàng", nhanVienkinhDoang = "Nhân viên kinh doanh", dienThoai = "Điện thoại",
            phoneId = "phone", namSinh = "Năm sinh (nhập khi không có ngày sinh)", gioiTinh = "Giới tính", nvCSKH = "Nhân viên CSKH", nguoiPhuTrach = "Người phụ trách", ngayKham = "Ngày khám", the = "Thẻ",
            /// Notebook header
            lichSuKham = "Lịch sử khám",
            menuFilter1 = "/select[1]", menuFilter2 = "/select[2]", apDungButton = "/following-sibling::div/button[.=' Áp dụng ']";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Import Data Test 001 - Import Da khoa");
            try
            {
                // Log into the application
                //LoginAction.Instance.LoginSite(60, urlInstance);
                LoginAction.Instance.LoginSiteXRender(60, null, null, null, General.listRenderer);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Go to 'Import đa khoa' page and delete all imported data if already exists
                    // Go to 'Import Data' - 'Import đa khoa' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer)
                                          .CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import đa khoa")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Check if all imported data already exists then delete all
                    if (GeneralAction.Instance.IsElementPresent(General.tableCheckboxInRow("1")))
                    {
                        // Delete all imported data
                        GeneralAction.Instance.ClickCheckboxAlltable(10)
                                              .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                              .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                              .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                        // Verify all imported data is deleted 
                        verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                        verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import đa khoa' is deleted: '" + inputSearch + "'", verifyPoint);
                        ExtReportResult(verifyPoint, summaryTC);
                    }
                    #endregion

                    #region Upload file to table (list)
                    // Click 'TẢI DỮ LIỆU' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Tải dữ liệu")
                                          .WaitForElementVisible(10, General.DialogShow);
                    //.CLickButtonTitleInDialog(10, "Click chọn tập tin"); Thread.Sleep(2000); // window dialog

                    // Upload file (Excel file)
                    string fileName = "Đa khoa_Input.xlsx";
                    string filePath = Path.Combine(AppContext.BaseDirectory, @"Tests\Documents\Excel files\BVDKHN\Import Data\");
                    GeneralAction.Instance.UploadFileInDialogInput(10, "Tệp đính kèm", filePath + fileName);
                    //SendKeys.SendWait(Path.Combine(filePath + fileName, "{ENTER}")); Thread.Sleep(2000); // window dialog

                    // wait for file name is shown at upload field 'Tệp đính kèm'
                    //GeneralAction.Instance.WaitForElementVisible(10, General.uploadFileInDialog("Tệp đính kèm"), fileName);

                    // Click 'LƯU' button (import data to table)
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu"); Thread.Sleep(1000);

                    // Check if dialog 'Tệp không đúng định dạng' shown then click 'Đồng ý' / 'Áp dụng'
                    if (GeneralAction.Instance.IsElementPresent(General.DialogShow))
                    {
                        if (GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Đồng ý")))
                        { 
                            // Click 'Đồng ý' button
                            GeneralAction.Instance.CLickButtonTitleInDialog(10, "Đồng ý");
                        }
                        if (GeneralAction.Instance.IsElementPresent(General.titleButtonInDialog("Áp dụng")))
                        {
                            // Click 'Áp dụng' button
                            GeneralAction.Instance.CLickButtonTitleInDialog(10, "Áp dụng");
                        }

                        // Click 'LƯU' button (import data to table)
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu"); Thread.Sleep(1000);
                    }
                    #endregion

                    #region Verify data (excel file) is shown in table correctly
                    string nameAttrVal = "patient_code", data = "11111101";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Mã BN' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "full_name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "year_of_birth", data = yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Năm sinh' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "gender", data = "Nam");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giới tính' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phonenumbers", data = phoneNumbers.Replace("02", "2"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "examination_package", data = "TSUT-NC");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gói khám' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "expected_revenue", data = "8.925.000 ₫");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Bill' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "medical_examination_day", data = "22/01/2024");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày khám' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_name", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_suggest", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Import data into application
                    // Click checkbox at a specific row (field value)
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, clientName)
                                          .CLickButtonTitle(10, importBtn); Thread.Sleep(2000);
                                          //.WaitForElementInvisible(10, General.tableCheckboxInRowWithValueColumn(clientName, "name", "full_name"));
                    #endregion

                    #region Search the imported 'Cơ hội' & Verify data
                    // Go 'Data cua toi' page (Kanban view)
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.dataCuaToi)
                                          .WaitForElementVisible(10, General.breadcrumbItem("Chu trình"))
                                          .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                    // Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Input a value in seachbox to search
                    GeneralAction.Instance.SearchViewInput(60, "Cơ hội", inputSearch);

                    //// Verify 'Cơ hội' was imported at Chu trình-cơ hội (kanban view)
                    //verifyPoint = GeneralAction.Instance.TitleSubOfRecordKanbanInColumnGetText(10, "Chưa chăm sóc sau khám", "3",data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth);
                    //verifyPoints.Add(summaryTC = "Verify the 'Cơ hội' was imported at CRM-Chu trình page: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    //// Click on record title of a column to verify data (kanban view)
                    //GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, "Chưa chăm sóc sau khám", data)
                    //                      .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Click on the searched co hội 
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, "QA auto import đa khoa 01")
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Verify 'Cơ hội' was imported at 'Chu trình-Cơ hội' detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityNamePlaceHolder, data= "TSUT-NC");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khachHang, data = clientName + " - " + phoneNumbers.Replace("18", "1 8").Replace("90", "9 0") + " - " + yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + khachHang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = phoneNumbers.Replace("18", "1 8").Replace("90", "9 0"), "phone_1", "id");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + dienThoai + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayKham, data = "22/01/2024 12:30:00");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + ngayKham + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, the, "1", data = "Đa khoa");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + the + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "user_id", data = "Dương Ngọc Vy", "//div"); // //span[position()=2]; DataInputFieldTitleGetText(10, nhanVienkinhDoang, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + nhanVienkinhDoang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete 'Co hoi' KH
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.ThucHienXoaDelete(60); Thread.Sleep(1500);

                    // Verify the created 'co hoi' is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'co hoi' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //// Verify the created 'Co hoi' is deleted (Kanban view)
                    //verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted("Chưa chăm sóc sau khám", data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth);
                    //verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Search the imported 'Liên hệ' & Verify data
                    // Go to 'lien he' - 'Khach hang ca nhan' page
                    GeneralAction.Instance.CLickMenuTitle(20, General.homeMenu)
                                          .WaitForElementVisible(20, General.dropDownShow).Sleep(3000)
                                          .CLickItemInDropdown(20, General.lienHe)
                                          .WaitForElementVisible(10, General.listRenderer)
                                          .WaitForElementVisible(10, General.breadcrumbItem("Khách hàng cá nhân"));

                    // Search the imported client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(3000);

                    // Verify the imported client is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click the imported client to go to the detail contact page
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, clientName)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Verify 'Liên hệ' was imported at 'Liên hệ'-'Khách hàng cá nhân' detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, vdNguyenVanAPlaceHolder, data = clientName);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' title is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, maBenhNhan, data = "11111101");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + maBenhNhan + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Điện thoại (Phone)
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, phoneId, data = phoneNumbers.Replace("18", "1 8").Replace("90", "9 0"), "div[contains(@class,'o_content')]//input");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + phoneId + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, namSinh, data = yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + namSinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, gioiTinh, data = "Nam");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + gioiTinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Thẻ (Tag)
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "o_field_tags", data = "Đa khoa", "/span[1]");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + the + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, nguoiPhuTrach, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + nguoiPhuTrach + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    ///// Verify NotebookHeader (Lịch sử khám) --> build 20240402
                    //GeneralAction.Instance.CLickNotebookHeader(10, lichSuKham).WaitForElementVisible(10, General.listRenderer);
                    //verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "1", data = "22/01/2024 12:30:00"); //DataRecordRowListTableNotebookGetText
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Lịch sử khám' at field 'Ngày khám' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataRecordRowListTableGetText(10, "1", "3", data = "TSUT-NC");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Lịch sử khám' at field 'Cơ hội' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    // Delete the imported 'Liên hệ'
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.ThucHienXoaDelete(60); Thread.Sleep(1500);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the imported 'Liên hệ'-'Khách hàng cá nhân' is deleted: '" + clientName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the imported data table at 'Import Data' >> 'Import đa khoa'
                    // Go to CRM page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu).Sleep(3000)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm).Sleep(3000)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Go to 'Import Data' - 'Import đa khoa' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import đa khoa")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Delete all imported data table
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .ThucHienXoaDelete(60); Thread.Sleep(1000);

                    // Verify all imported data is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import đa khoa' is deleted (after successful import): '" + inputSearch + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' (qa-bvdkhn) site !!!");
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

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC002_ImportData_ImportYHTT()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName; ;
            const string inputSearch = "qa auto", searchboxRole = "searchbox", importBtn = "btn btn-primary ms", clientName = "QA AUTO IMPORT YHTT 01",
            yearOfBirth = "13/10/1990", phoneNumbers = "02218999001", opportunityNamePlaceHolder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", vdGiaSanPhamPlaceHolder = "vd: Giá sản phẩm", doanhThuDuKienId = "expected_revenue",
            khachHang = "Khách hàng", nhanVienkinhDoang = "Nhân viên kinh doanh", dienThoai = "Điện thoại", ngayKham = "Ngày khám", the = "Thẻ",
            vdNguyenVanAPlaceHolder = "ví dụ: Nguyễn Văn An", maBenhNhan = "Mã bệnh nhân", phoneId = "phone", namSinh = "sinh", gioiTinh = "Giới tính", nvCSKH = "Nhân viên CSKH",
            menuFilter1 = "/select[1]", menuFilter2 = "/select[2]", apDungButton = "/following-sibling::div/button[.=' Áp dụng ']";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Import Data Test 002 - Import YHTT");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Go to 'Import YHTT' page and delete all imported data if already exists
                    // Go to 'Import Data' - 'Import YHTT' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import YHTT")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ và tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Check if all imported data already exists then delete all
                    if (GeneralAction.Instance.IsElementPresent(General.tableCheckboxInRow("1")))
                    {
                        // Delete all imported data
                        GeneralAction.Instance.ClickCheckboxAlltable(10)
                                              .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                              .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                              .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                        // Verify all imported data is deleted 
                        verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                        verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import YHTT' is deleted: '" + inputSearch + "'", verifyPoint);
                        ExtReportResult(verifyPoint, summaryTC);
                    }
                    #endregion

                    #region Upload file to table (list)
                    // Click 'TẢI DỮ LIỆU' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Tải dữ liệu")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Click chọn tập tin");

                    // Upload file (Excel file)
                    string fileName = "YHTT_Input.xlsx";
                    string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\Excel files\BVDKHN\Import Data\");
                    //GeneralAction.Instance.UploadFileInput(filePath + fileName);
                    SendKeys.SendWait(filePath + fileName);
                    SendKeys.SendWait("{ENTER}");

                    // Click 'LƯU' button (import data to table)
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu");
                    #endregion

                    #region Verify data (excel file) is shown in table correctly
                    string nameAttrVal = "medical_examination_day", data = "13/10/1990";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày ĐK' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "full_name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ và tên' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "patient_code", data = "QAAUTO01");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Mã KH' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "year_of_birth", data = "13/10/1990");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày sinh' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "address", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Địa chỉ' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phonenumbers", data = phoneNumbers.Replace("02", "2"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "examination_package", data = "1B OZ");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Dịch vụ' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "expected_revenue", data = "3.000.000 ₫");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Số tiền' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_name", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_suggest", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "employee_name", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên nhân sự' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "employee_suggest", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý tên nhân sự' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "care_content", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nội dung chăm sóc' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "import_result", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Kết quả' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "import_status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Kết quả chi tiết' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Import data into application
                    // Click checkbox at a specific row (field value)
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, clientName)
                                          .CLickButtonTitle(10, importBtn); Thread.Sleep(2000);
                                          //.WaitForElementInvisible(10, General.tableCheckboxInRowWithValueColumn(clientName, "name", "full_name"));
                    #endregion

                    #region Search the imported 'Cơ hội' & Verify data
                    // Go CRM - Cơ hội page (Kanban view)
                    GeneralAction.Instance.CLickMainMenuTitle(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Input a value in seachbox to search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(30, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Verify 'Cơ hội' was imported at CRM-Chu trình page
                    verifyPoint = GeneralAction.Instance.TitleSubOfRecordKanbanInColumnGetText(10, "Chưa chăm sóc sau khám", "3", data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the 'Cơ hội' was imported at CRM-Chu trình page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click on record title of a column to verify data
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, "Chưa chăm sóc sau khám", data)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Verify 'Cơ hội' was imported at CRM-Chu trình detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityNamePlaceHolder, data = "1B OZ");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, doanhThuDuKienId, data = "3.000.000");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'Doanh thu dự kiến' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 465
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khachHang, data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + khachHang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001"));
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + dienThoai + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayKham, data = "13/10/1990 12:30:00");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + ngayKham + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, the, "1", data = "YHTT");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + the + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.HtmlElementGetTextValue(10, "div", "user_id", data = "Dương Ngọc Vy", "//input"); //span[position()=2] ;DataInputFieldTitleGetText(10, nhanVienkinhDoang, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + nhanVienkinhDoang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete 'Co hoi' KH
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Verify the created 'Co hoi' is deleted
                    verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted("Chưa chăm sóc sau khám", data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Search the imported 'Liên hệ' & Verify data
                    // Go to 'lien he' - 'Khach hang ca nhan' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.lienHe)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search the imported client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Tên", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the imported client is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click the imported client to go to the detail contact page
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, clientName)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Verify 'Liên hệ' was imported at 'Liên hệ'-'Khách hàng cá nhân' detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, vdNguyenVanAPlaceHolder, data = clientName);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' title is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, maBenhNhan, data = "QAAUTO01");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + maBenhNhan + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Điện thoại (Phone)
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, phoneId, data = phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001"), "div[contains(@class,'o_content')]//input");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + phoneId + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, namSinh, data = yearOfBirth);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + namSinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, gioiTinh, data = "Nam");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + gioiTinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Thẻ (Tag)
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "o_field_tags", data = "YHTT", "/span[1]");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + the + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, nvCSKH, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + nvCSKH + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete the imported 'Liên hệ'
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the imported 'Liên hệ'-'Khách hàng cá nhân' is deleted: '" + clientName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the imported data table at 'Import Data' >> 'Import YHTT'
                    // Go to CRM page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Go to 'Import Data' - 'Import YHTT' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import YHTT")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ và tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Delete the imported data table
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                    // Verify all imported data is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import YHTT' is deleted (after successful import): '" + inputSearch + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' (qa-bvdkhn) site !!!");
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

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC003_ImportData_ImportIVF()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            const string inputSearch = "qa auto", searchboxRole = "searchbox", importBtn = "btn btn-primary ms", hoVaTen = "\"QA auto import A 1989\r\nQA auto import B 1985\"\r\nHải Dương",
            clientName = "QA auto import A", clientName2 = "QA auto import B", phoneNumbers = "02218999001", yearOfBirth1 = "1989", yearOfBirth2 = "1985", opportunityNamePlaceHolder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", vdGiaSanPhamPlaceHolder = "vd: Giá sản phẩm", doanhThuDuKienId = "expected_revenue",
            khachHang = "Khách hàng", nhanVienkinhDoang = "Nhân viên kinh doanh", dienThoai = "Điện thoại", ngayKham = "Ngày khám", the = "Thẻ",
            vdNguyenVanAPlaceHolder = "ví dụ: Nguyễn Văn An", maBenhNhan = "Mã bệnh nhân", phoneId = "phone", namSinh = "Năm sinh", gioiTinh = "Giới tính", nvCSKH = "Nhân viên CSKH",
            menuFilter1 = "/select[1]", menuFilter2 = "/select[2]", apDungButton = "/following-sibling::div/button[.=' Áp dụng ']";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Import Data Test 003 - Import IVF");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Go to 'Import IVF' page and delete all imported data if already exists
                    // Go to 'Import Data' - 'Import IVF' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import IVF")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ và tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Check if all imported data already exists then delete all
                    if (GeneralAction.Instance.IsElementPresent(General.tableCheckboxInRow("1")))
                    {
                        // Delete all imported data
                        GeneralAction.Instance.ClickCheckboxAlltable(10)
                                              .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                              .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                              .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                        // Verify all imported data is deleted 
                        verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                        verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import IVF' is deleted: '" + inputSearch + "'", verifyPoint);
                        ExtReportResult(verifyPoint, summaryTC);
                    }
                    #endregion

                    #region Upload file to table (list)
                    // Click 'TẢI DỮ LIỆU' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Tải dữ liệu")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Click chọn tập tin");

                    // Upload file (Excel file)
                    string fileName = "IVF_Input.xlsx";
                    string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\Excel files\BVDKHN\Import Data\");
                    //GeneralAction.Instance.UploadFileInput(filePath + fileName);
                    SendKeys.SendWait(filePath + fileName);
                    SendKeys.SendWait("{ENTER}");

                    // Click 'LƯU' button (import data to table)
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu");
                    #endregion

                    #region Verify data (excel file) is shown in table correctly
                    string nameAttrVal = "date", data = "24/10/2023";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal= "full_name", data= hoVaTen);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ và tên' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "full_name1", data = "QA auto import A");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên Liên hệ 1' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "year_of_birth_1", data = yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày sinh Liên hệ 1' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "gender1", data = "Nữ");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giới tính Liên hệ 1' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phonenumbers", data = phoneNumbers.Replace("02", "2"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT Liên hệ 1' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "full_name2", data = "QA auto import B");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên Liên hệ 2' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "year_of_birth_2", data = yearOfBirth2);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày sinh Liên hệ 2' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "gender2", data = "Nam");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giới tính Liên hệ 2' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phonenumbers2", data = "0800000001");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT Liên hệ 2' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "home_town", data = "Hải Dương");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Địa chỉ' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "condition", data = "có 1 bé hơn 10 tuổi rồi nhưng trước đó thì bỏ 1 bé sau khi sinh bé này thì lại bị lưu mời qua khám kiểm tra");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tình trạng' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_name", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_suggest", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý tên tư vấn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "care_staff", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'NV chăm sóc' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "care_staff_suggest", data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý NV chăm sóc' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "note", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Note của CSKH' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "import_result", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Kết quả' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "import_status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Kết quả chi tiết' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Import data into application
                    // Click checkbox at a specific row (field value)
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, clientName + " ", "name", "full_name1")
                                          .CLickButtonTitle(10, importBtn); Thread.Sleep(2000);
                                          //.WaitForElementInvisible(10, General.tableCheckboxInRowWithValueColumn(hoVaTen, "name", "full_name"));
                    #endregion

                    #region Search the imported 'Cơ hội' & Verify data
                    // Go CRM - Cơ hội page (Kanban view)
                    GeneralAction.Instance.CLickMainMenuTitle(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Input a value in seachbox to search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Cơ hội", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(3000);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(60, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Verify 'Cơ hội' was imported at CRM-Chu trình page
                    verifyPoint = GeneralAction.Instance.TitleSubOfRecordKanbanInColumnGetText(10, "Chưa chăm sóc sau khám", "3", data = clientName + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify the 'Cơ hội' was imported at CRM-Chu trình page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click on record title of a column to verify data
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, "Chưa chăm sóc sau khám", data = clientName + "  - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth1)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Verify 'Cơ hội' was imported at CRM-Chu trình detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityNamePlaceHolder, data = "QA auto import A . QA auto import B");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, doanhThuDuKienId, data = "0");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'Doanh thu dự kiến' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 465
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khachHang, data = clientName + " " + " - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + khachHang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001"));
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + dienThoai + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, ngayKham, data = "24/10/2023 12:30:00");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + ngayKham + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataItemFieldTitleGetText(10, the, "1", data = "IVF");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + the + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.HtmlElementGetTextValue(10, "div", "user_id", data = "Dương Ngọc Vy", "//input"); //span[position()=2]; DataInputFieldTitleGetText(10, nhanVienkinhDoang, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Cơ hội'-'" + nhanVienkinhDoang + "' title in 'Chu trình' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete 'Co hoi' KH
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Verify the created 'Co hoi' is deleted
                    verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted("Chưa chăm sóc sau khám", data = clientName + "  - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Search the imported 'Liên hệ' & Verify data
                    // Go to 'lien he' - 'Khach hang ca nhan' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.lienHe)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search the imported client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, "Tên", inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Click on a Column name to sort the table
                    GeneralAction.Instance.ClickColumnNameToSortTable(10, "Tên") // Sort A->Z
                                          .WaitForElementVisible(10, General.sortTableByColumnNameIconStatus("Tên", General.sortUpIcon)); Thread.Sleep(1000);

                    // Verify the imported client is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "birthday_display", data = yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Năm sinh' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "name", data = clientName2);
                    verifyPoints.Add(summaryTC = "Verify data at the 2nd row, column 'Tên' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "birthday_display", data = yearOfBirth2);
                    verifyPoints.Add(summaryTC = "Verify data at the 2nd row, column 'Năm sinh' is shown after searching the imported 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click the imported client to go to the detail contact page
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, clientName + " ")
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Verify 'Liên hệ' was imported at 'Liên hệ'-'Khách hàng cá nhân' detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, vdNguyenVanAPlaceHolder, data = clientName);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' title is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, maBenhNhan, data = "");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + maBenhNhan + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Điện thoại (Phone)
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, phoneId, data = phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001"), "div[contains(@class,'o_content')]//input");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + phoneId + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, namSinh, data = yearOfBirth1);
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + namSinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, gioiTinh, data = "Nữ");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + gioiTinh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Thẻ (Tag)
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "o_field_tags", data = "IVF", "/span[1]");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + the + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, nvCSKH, data = "Dương Ngọc Vy");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Liên hệ'-'Khách hàng cá nhân' at field '" + nvCSKH + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete the imported 'Liên hệ'
                    /// Click on 'Thuc Hien' --> 'Xoa' (the 1st contact)
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          //.WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);
                                          .WaitForElementVisible(10, General.elementHtml("span", clientName2 + "  - " + phoneNumbers.Replace("0800", "0800 ").Replace("001", " 001") + " - " + yearOfBirth2)); 
                                          Thread.Sleep(1500);

                    /// Click on 'Thuc Hien' --> 'Xoa' (the 2nd contact)
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the imported 'Liên hệ'-'Khách hàng cá nhân' is deleted: '" + clientName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the imported data table at 'Import Data' >> 'Import IVF'
                    // Go to CRM page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Go to 'Import Data' - 'Import IVF' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import IVF")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Họ và tên']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Delete the imported data table
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                    // Verify all imported data is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import IVF' is deleted (after successful import): '" + inputSearch + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' (qa-bvdkhn) site !!!");
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

        [Ignore("")]//[Test, Category("DKHN - Regression Tests")]
        public void TC004_ImportData_ImportLeadData()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            const string inputSearch = "qa auto", searchboxRole = "searchbox", importBtn = "btn btn-primary ms",
            menuFilter1 = "/select[1]", menuFilter2 = "/select[2]", apDungButton = "/following-sibling::div/button[.=' Áp dụng ']",
            clientName = "QA auto import Lead 01", phoneNumbers = "02218999001", opportunityNamePlaceHolder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", tenCongTy = "Tên công ty",
            tenLienHe = "Tên liên hệ", tieuDePlaceHolder = "Tiêu đề", diaChiPlaceHolder = "Địa chỉ...", dienThoai = "Điện thoại", trangThai = "Trạng thái",
            nvKinhDoanh = "Nhân viên kinh doanh", ghiChuNoiBo = "Ghi chú nội bộ", thongTinThem = "Thông tin thêm", chienDich = "Chiến dịch",
            ngayPhanCong = "Ngày phân công", nguon = "Nguồn", nhanSuAds = "Nhân sự Ads", trucChat = "Trực chat", telesale = "Telesale";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Import Data Test 004 - Import Lead Data");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Go to 'Import Lead Data' page and delete all imported data if already exists
                    // Go to 'Import Data' - 'Import Lead Data' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import Lead Data")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Tên đăng ký']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Check if all imported data already exists then delete all
                    if (GeneralAction.Instance.IsElementPresent(General.tableCheckboxInRow("1")))
                    {
                        // Delete all imported data
                        GeneralAction.Instance.ClickCheckboxAlltable(10)
                                              .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                              .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                              .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                        // Verify all imported data is deleted 
                        verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                        verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import Lead Data' is deleted: '" + inputSearch + "'", verifyPoint);
                        ExtReportResult(verifyPoint, summaryTC);
                    }
                    #endregion

                    #region Upload file to table (list)
                    // Click 'TẢI DỮ LIỆU' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Tải dữ liệu")
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitleInDialog(10, "Click chọn tập tin");

                    // Upload file (Excel file)
                    string fileName = "Import Lead Data.xlsx";
                    string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\Excel files\BVDKHN\Import Data\");
                    //GeneralAction.Instance.UploadFileInput(filePath + fileName);
                    SendKeys.SendWait(filePath + fileName);
                    SendKeys.SendWait("{ENTER}");

                    // Click 'LƯU' button (import data to table)
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, "Lưu");
                    #endregion

                    #region Verify data (excel file) is shown in table correctly
                    string nameAttrVal = "datetime", data = "05/05/2023";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "time", data = "23:29:54");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giờ' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = clientName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên đăng ký' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "phone", data = phoneNumbers.Replace("02", "2"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'SĐT' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "service", data = "Bệnh lý");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Dịch vụ' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "source", data = "FACEBOOK");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nguồn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "source_suggest", data = "Facebook");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý Nguồn' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "campaign", data = "IVF2");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Chiến dịch' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "campaign_suggest", data = "IVF2");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý Chiến dịch' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_name", data = "Lưu Thị Nhung");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Phụ trách chiến dịch' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "counselor_suggest", data = "Lưu Thị Nhung");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý tên phụ trách chiến dịch' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "note", data = "Ck 31 vk 39 mình sinh 1 cháu trai 10 tuổi r, sk bình thường, muốn sinh thêm sớm nhất có thể, cần xét nghiệm nhg gì nhỉ");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Note' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team", data = "QC TELESALE DEPARTMENT");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Team' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_suggest", data = "QC Telesale Department");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý Team' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "telesale_name", data = "QA Telesale 01");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Telesale' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "telesale_suggest", data = "QA Telesale 01");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Gợi ý Telesale' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "status", data = "Đã tư vấn");
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Trạng thái' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 441 Remove Trang thai
                    #endregion

                    #region Import data into application
                    // Click checkbox at a specific row (field value)
                    GeneralAction.Instance.ClickCheckboxInRowTableWithValueColumn(10, clientName, "name", "name")
                                          .CLickButtonTitle(10, importBtn)
                                          .WaitForElementInvisible(10, General.tableCheckboxInRowWithValueColumn(clientName, "name", "full_name"));
                    #endregion

                    #region Search the imported 'Lead' & Verify data
                    // Go to Lead menu
                    GeneralAction.Instance.CLickMenuTitle(10, General.lead)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Lead
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.InputByAttributeValue(10, searchboxRole, inputSearch)
                                          .WaitForElementVisible(10, General.searchBoxDropdown) // wait for sb dropdown displays
                                          .CLickItemInDropdownSearchBox(10, General.lead, inputSearch)
                                          .WaitForElementInvisible(10, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the imported Lead is shown
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = clientName + " - Dịch vụ");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Lead' is shown after searching the imported 'Lead: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click the imported Lead to go to the detail contact page
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, clientName + " - Dịch vụ")
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Verify 'Lead' was imported at 'Lead' detail page
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityNamePlaceHolder, data = clientName + " - Dịch vụ");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' title is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, tenCongTy, data = "");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Lead' at field '" + tenCongTy + "' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 465
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, tenLienHe, data = "");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Lead' at field '" + tenLienHe + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, tieuDePlaceHolder, data = "");
                    //verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + tieuDePlaceHolder + "-Ông/Bà...' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 465
                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = phoneNumbers);
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + dienThoai + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, diaChiPlaceHolder, data = "Hà Nội");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + diaChiPlaceHolder + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DropdownLabelGetText(10, trangThai, data = "Đã tư vấn");
                    //verifyPoints.Add(summaryTC = "Verify the imported data of 'Lead' at field '" + trangThai + "' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC); // Ticket 441 Remove Trang thai
                    verifyPoint = GeneralAction.Instance.HtmlElementGetTextValue(10, "div", "user_id", data = "Van Anh CSKH Manager", "//input"); //span[position()=2]; DataInputFieldTitleGetText(10, nvKinhDoanh, data = "Van Anh CSKH Manager");
                    verifyPoints.Add(summaryTC = "Verify the imported data of 'Lead' at field '" + nvKinhDoanh + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderDescriptionContentGetText(10, ghiChuNoiBo, data = "Note: Ck 31 vk 39 mình sinh 1 cháu trai 10 tuổi r, sk bình thường, muốn sinh thêm sớm nhất có thể, cần xét nghiệm nhg gì nhỉ, Dịch vụ: Bệnh lý, Nội dung tư vấn: 1/8: 8h35: thả 3 năm chưa có, trc chỉ khám phụ khoa, siêu âm bơm nước,... 2 vợ chồng đều có con riêng, chưa làm ĐKKH. chị trao đổi với anh để đi khám xem tnao");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + ghiChuNoiBo + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // CLick NotebookHeader (Thông tin thêm)
                    GeneralAction.Instance.CLickNotebookHeader(10, thongTinThem);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetTextValue(10, thongTinThem, chienDich, data = "IVF2");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + chienDich + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetText(10, thongTinThem, ngayPhanCong, data = "02/02/2024 16:47:12", "span");
                    //verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + ngayPhanCong + "' is shown: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetTextValue(10, thongTinThem, nguon, data = "Facebook");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + nguon + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetTextValue(10, thongTinThem, nhanSuAds, data = "Lưu Thị Nhung");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + nhanSuAds + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetTextValue(10, thongTinThem, trucChat, data = LoginPage.fullname);
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + trucChat + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = GeneralAction.Instance.DataInputNotebookHeaderAndFieldNameContentGetTextValue(10, thongTinThem, telesale, data = "Van Anh CSKH Manager");
                    verifyPoints.Add(summaryTC = "Verify the imported dara of 'Lead' at field '" + thongTinThem + " - " + telesale + "' is shown: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete the imported Lead
                    /// Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow)
                                          .WaitForElementVisible(10, General.listRenderer); Thread.Sleep(1500);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the imported Lead is deleted: '" + clientName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete the imported data table at 'Import Data' >> 'Import Lead Data'
                    // Go to CRM page
                    GeneralAction.Instance.CLickMenuTitle(10, General.homeMenu)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, General.crm)
                                          .WaitForElementVisible(10, General.kanbanRenderer); Thread.Sleep(1500);

                    // Go to 'Import Data' - 'Import  Lead Data' page
                    GeneralAction.Instance.CLickMenuTitle(10, General.importData)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Import Lead Data")
                                          .WaitForElementInvisible(10, General.dropDownShow)
                                          .WaitForElementVisible(10, General.listView); Thread.Sleep(1000);

                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Tên đăng ký']")
                                          .InputHtmlElement(10, "div", "filter_condition", inputSearch, "//input")
                                          .ClickHtmlElement(10, "div", "filter_condition", apDungButton); Thread.Sleep(2000);

                    // Delete all imported data
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                          .CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);

                    // Verify all imported data is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all imported data table at 'Import Data' >> 'Import Lead Data' is deleted (after successful import): '" + inputSearch + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' (qa-bvdkhn) site !!!");
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
