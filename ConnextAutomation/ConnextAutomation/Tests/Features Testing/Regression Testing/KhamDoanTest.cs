using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter.Config;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using Microsoft.Testing.Platform.Extensions.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(9)]
    internal class KhamDoanTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// table renderer (used for wait method)
        private string listRenderer = "list_renderer", kanRenderer = "kanban_renderer",
        /// title, attribute fields on Form ...
        moiButton = "/button[.='Mới' or .=' Mới ']", importBtn = "btn btn-primary ms", companyNamePlaceholder = "ví dụ: Công ty TNHH Số 1", nguoiKhamPlaceholder = "Hãy nhập thông tin gợi nhớ cơ hội. VD: Nguyễn Thị Bé - Lê Văn Nam - IVF", dienThoai = "Điện thoại", khackHang = "Khách hàng",
        diaChi = "Địa chỉ", diaChiPlaceHolder = "Địa chỉ...", tinhTpPlaceHolder = "Tỉnh/Tp", quanHuyenPlaceHolder = "Quận/Huyện", phuongXaPlaceHolder = "Phường/Xã",
        the = "Thẻ", nguoiPhuTrach = "Người phụ trách", hopDong = "Hợp đồng", soHopDong = "Số hợp đồng", toChucLienHe = "Tổ chức/Liên hệ", coHoi = "Cơ hội", vdGiaSanPhamPlaceHolder = "Ví dụ: Khách hàng quan tâm sản phẩm", maHopDong = "Mã hợp đồng", doanhThuDuKien = "Doanh thu dự kiến", ngayKham = "Ngày khám", medicalExaminationDayId = "medical_examination_day", medicalExaminationDateClosedId = "medical_examination_date_closed",
        moi = "Mới", them = "Thêm", sua = "Sửa", xetNghiem = "Xét nghiệm", kham = "Khám", opportunityId = "name", hanKhamVetId = "medical_examination_due_date", tenCongTyPlaceHolder = "Tên Công ty...", maBenhNhan = "Mã bệnh nhân", ngaySinh = "Ngày sinh", gioiTinh = "Giới tính", cccd_CMND = "CCCD/CMND",

        /// Notebookheader
        lienHeVaDiaChi = "Liên hệ & Địa chỉ", lichSuKham = "Lịch sử khám", dichVuQuanTam = "Dịch vụ quan tâm", moiQuanHe = "Mối quan hệ", thongTinKhac = "Thông tin Khác", tepDinhKem = "Tệp đính kèm", ketNoiZalo = "Kết nối Zalo", ech = "ECH", lichSuCuocGoi = "Lịch sử cuộc gọi",
        themTitle = "Tạo tập dữ liệu", tenLienHe = "Tên Liên hệ", email = "Email", danhXung = "Danh xưng", chucVu = "Chức vụ", phoneId = "phone", maSoThue = "Mã số thuế", loaiNguon = "Loại nguồn",
        menuFilter1 = "/select[1]", menuFilter2 = "/select[2]", apDungButton = "/following-sibling::div/button[.=' Áp dụng ']",

        /// global data
        companyName = "Công ty QA auto " + dateTimeNow,
        examinerName = "QA auto khám " + dateTimeNow;
        //contactName = "QA auto lh dakhoa " + dateTimeNow,
        //opportunityName = "Cơ hội của QA auto dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_KhamDoan_KhamDoanDakhoa()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            adminIVFEmail = LoginPage.adminIVFUsername,
            adminIVFFullname = LoginPage.adminIVFFullname,
            khachDoanLeadEmail = LoginPage.khachDoanLeadEmail,
            khachDoanLeadPass = LoginPage.khachDoanLeadPass,
            khachDoanLeadUsername = LoginPage.khachDoanLeadUsername,
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong, dienThoaidataNguoiDiCung, tagData, nguoiPhuTrachData, soHopDongData,
            tenLienHeData, emailData, danhXungData, chucVuData, dienThoai2Data, examinerPhone, examinerPhone2, examinerPhone3, maSoThueData, maHopDongData, ngayKhamData, ngayKhamKetThucData;

            const string inputSearch = "qa auto", searchboxRole = "searchbox";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Kham doan Test 001 - Kham doan Da khoa");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteXRender(60, urlInstance, khachDoanLeadEmail, khachDoanLeadPass, LoginPage.allRenderers);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    // Verify if the user is on the correct page
                    string data = "Khách hàng doanh nghiệp";
                    verifyPoint = GeneralAction.Instance.IsMainTitleShown(10, data);
                    verifyPoints.Add(summaryTC = "Verify if the user (khach doan) is on the correct page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    #region Create new 'Khách hàng doanh nghiệp'
                    // Click on 'MỚI' button
                    GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Input data to create 'Khách hàng doanh nghiệp'
                    GeneralAction.Instance.InputByAttributeValue(10, companyNamePlaceholder, companyName)
                                          .InputByAttributeValue(10, "phone", dienThoaidata = "02219899001", "div[@name='phone']/input");
                    GeneralAction.Instance.InputFieldLabel(10, dienThoai + " 2", dienThoaidata2 = dienThoaidata.Replace("01", "02"))
                                          .InputFieldLabel(10, dienThoai + " 3", dienThoaidata3 = dienThoaidata.Replace("01", "03"))
                                          .InputFieldLabel(10, diaChi, "Số 199 Bùi Viện", diaChiPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "Hà Nội", tinhTpPlaceHolder).Sleep(500).PressEnterKeyboard().Sleep(500)
                                          .InputFieldLabel(10, diaChi, "Quận Ba Đình", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          .InputFieldLabel(10, diaChi, "Phường Cống Vị", phuongXaPlaceHolder).Sleep(500).PressEnterKeyboard()
                                          //.InputFieldLabel(10, the, tagData = "VIP").PressEnterKeyboard();
                                          //.InputFieldLabel(10, nguoiPhuTrach, nguoiPhuTrachData = "Đỗ Thị Hương Giang").PressEnterKeyboard().Sleep(1000)
                                          .InputFieldLabel(10, soHopDong, soHopDongData = "25061015/QAATHD01").PressEnterKeyboard().Sleep(1000);
                    /// NotebookHeader (Liên hệ & Địa chỉ, Lịch sử khám, ...)
                    GeneralAction.Instance.CLickNotebookHeader(10, lienHeVaDiaChi)
                                          .CLickButtonTitle(10, themTitle)
                                          .WaitForElementVisible(10, General.DialogShow)
                                          .InputFieldLabelDialog(10, tenLienHe, tenLienHeData = "Nguyễn Thị Dung").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, email, emailData = "").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, danhXung, danhXungData = "Chi").PressEnterKeyboard()
                                          .InputFieldLabelDialog(10, chucVu, chucVuData = "Nhân viên kinh doanh").PressEnterKeyboard()
                                          .InputByAttributeValue(10, phoneId, dienThoai2Data = "02219889001", "div[contains(@class,'modal-content')]//input")
                                          //.InputFieldLabelDialog(10, maSoThue, maSoThueData = "" + DateTime.Now.ToString("dd/MM/yyyy HH").Replace(" ", "")) // ex: BE0477472701
                                          .InputDescriptionContentInDialog(10, "QA auto Ghi chu tao Lien he 01")
                                          .CLickButtonTitleInDialog(10, "Lưu & Đóng")
                                          .WaitForElementInvisible(10, General.DialogShow);
                    GeneralAction.Instance.CLickNotebookHeader(10, lichSuCuocGoi);
                    GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac);
                    GeneralAction.Instance.CLickNotebookHeader(10, tepDinhKem);
                    GeneralAction.Instance.CLickNotebookHeader(10, ketNoiZalo);

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.CLickButtonTitle(10, General.luu)
                                          .WaitForElementInvisible(10, General.titleButton(General.luu)); Thread.Sleep(1000);
                    #endregion

                    #region Search 'Khách hàng doanh nghiệp'
                    // Back to 'Khách hàng doanh nghiệp' Page with list view (to search the created Company) 
                    GeneralAction.Instance.CLickItemBreadcrumb(10, General.khachHangDoanhNghiep)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Search Company Name
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch);
                    #endregion

                    #region Verify that data of the created Company is displayed correctly
                    string nameAttrVal = "create_date";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "name", data = companyName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên doanh nghiệp' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contract_id", data = soHopDongData);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Số hợp đồng' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_child_name", data = tenLienHeData);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên liên hệ' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "contact_child_phone", "data-tooltip", data = dienThoai2Data.Replace("19", "1 9").Replace("90", "9 0"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Điện thoại' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_child_email", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Email liên hệ' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_child_function", data = "Nhân viên kinh doanh");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Chức vụ' is shown after searching the created company: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Create Hợp đồng
                    // Click on the created Company Name to open the detail view
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(10, companyName)
                                          .WaitForElementVisible(10, General.formEditable);

                    // Click on the 'Hợp đồng' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Hợp đồng")
                                      .WaitForElementVisible(10, General.rendererTable("kanban_renderer"))
                                      .Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(60).Sleep(2000);

                    // Click on 'Mới' button to create a new 'Hợp đồng'
                    GeneralAction.Instance.CLickButtonTitle(10, General.moi)
                                          .WaitForElementVisible(30, General.kanbanQuickCreate);

                    // Input data to create 'Hợp đồng'
                    GeneralAction.Instance//.InputFieldLabel(10, toChucLienHe, "") // --> auto fill in with the created Company Name
                                          //.InputFieldLabel(10, coHoi, "Cơ hội của " + dateTimeNow, vdGiaSanPhamPlaceHolder) // automatically fill in with the created chance of Company
                                          .InputFieldLabel(10, email, "qaauto@connext.com")
                                          //.InputFieldLabel(10, dienThoai, dienThoaidata) // --> auto fill in with the created phone number
                                          .InputFieldLabel(10, maHopDong, maHopDongData = "mhdqaauto01")
                                          /// Ngày khám
                                          .InputByAttributeValue(10, medicalExaminationDayId, ngayKhamData = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          .InputByAttributeValue(10, medicalExaminationDateClosedId, ngayKhamKetThucData = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                          .CLickPriorityStar(10, doanhThuDuKien, "Rất cao")
                                          .CLickButtonTitleInKabanDialog(10, them); Thread.Sleep(3000);
                    #endregion

                    #region Verify that the data of the created contract is displayed correctly
                    // Click on Record title of a column to verify data
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, moi, "Cơ hội của " + companyName)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // Verify data of the created 'Hợp đồng' is shown correctly
                    verifyPoint = GeneralAction.Instance.DataInputByAttrValueGetText(10, opportunityId, data = "Cơ hội của " + companyName);
                    verifyPoints.Add(summaryTC = "Verify that the data of 'Cơ hội' field in 'Hợp đồng' detail is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "div", "contract_code", data = maHopDongData);
                    verifyPoints.Add(summaryTC = "Verify that the data of '" + maHopDong + "' field in 'Hợp đồng' detail is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, khackHang, data = companyName + " - " + dienThoaidata.Replace("19", "1 9").Replace("90", "9 0"));
                    verifyPoints.Add(summaryTC = "Verify that the data of '" + khackHang + "' field in 'Hợp đồng' detail is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, dienThoai, data = dienThoaidata.Replace("19", "1 9").Replace("90", "9 0"));
                    verifyPoints.Add(summaryTC = "Verify that the data of '" + dienThoai + "' field in 'Hợp đồng' detail is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataInputFieldTitleGetText(10, email, data = "qaauto@connext.com");
                    verifyPoints.Add(summaryTC = "Verify that the data of '" + email + "' field in 'Hợp đồng' detail is displayed: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Create 'DS Khám'
                    // Click on the 'DS Khám' button
                    GeneralAction.Instance.ClickHtmlElement(10, "span", "DS Khám")
                                          .WaitForElementVisible(10, General.rendererTable("list_renderer"))
                                          .Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(60).Sleep(2000);

                    // Click on 'MỚI' button
                    GeneralAction.Instance.ClickHtmlElement(10, "div", "o_list_buttons", moiButton)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1500);

                    // Input data to create 'DS Khám'
                    GeneralAction.Instance.InputByAttributeValue(10, nguoiKhamPlaceholder, "01- " + examinerName);
                    GeneralAction.Instance.InputFieldLabel(10, khackHang, examinerName).PressEnterKeyboard().Sleep(1000);
                    /// Create new 'Liên hệ' for 'DS Khám' in popup dialog
                    GeneralAction.Instance.WaitForElementVisible(10, General.DialogShow);
                    GeneralAction.Instance.InputByAttributeValueDialog(10, tenCongTyPlaceHolder, companyName + " - " + dienThoaidata.Replace("19", "1 9").Replace("90", "9 0")).PressEnterKeyboard();
                    GeneralAction.Instance.InputFieldLabelDialog(10, maBenhNhan, "mbnqaauto01").PressEnterKeyboard();
                    GeneralAction.Instance.InputByAttributeValueDialog(10, "phone", examinerPhone = "02219889002", "div[@name='phone']/input").PressEnterKeyboard();
                    GeneralAction.Instance.InputFieldLabelDialog(10, ngaySinh, "09/07/2000").PressEnterKeyboard().Sleep(1500);
                    GeneralAction.Instance.CLickAndSelectItemInDropdownLabel(10, gioiTinh, "Nam").Sleep(1000);
                    //GeneralAction.Instance.InputFieldLabelDialog(10, loaiNguoiBenh, "").PressEnterKeyboard();
                    GeneralAction.Instance.InputFieldLabelDialog(10, cccd_CMND, "12345678901").PressEnterKeyboard();
                    GeneralAction.Instance.CLickNotebookHeader(10, thongTinKhac).Sleep(1000);
                    GeneralAction.Instance.InputFieldLabelDialog(10, loaiNguon, "Offline / P.Khách đoàn / P.Khách đoàn").PressEnterKeyboard().Sleep(1000);
                    GeneralAction.Instance.CLickButtonTitleInDialog(10, General.luuVaDong).Sleep(1000)
                                          .WaitForElementInvisible(10, General.titleButton(General.luuVaDong)); Thread.Sleep(1000);

                    //GeneralAction.Instance.InputByAttributeValue(10, "phone", examinerPhone = "02219889002", "div[@name='phone']/input")
                    //                      .InputFieldLabel(10, dienThoai + " 2", examinerPhone2 = examinerPhone.Replace("002", "003"))
                    //                      .InputFieldLabel(10, dienThoai + " 3", examinerPhone3 = examinerPhone.Replace("002", "004"))
                    //                      .InputFieldLabel(10, email, emailData = "qaautoExaminer@abc.com").PressEnterKeyboard()
                    //                      .InputFieldLabel(10, diaChi, "CÔNG TY TNHH QA Auto " + dateTimeNow, diaChiPlaceHolder).Sleep(500).PressEnterKeyboard()
                    //                      .InputFieldLabel(10, diaChi, "Hà Nội", tinhTpPlaceHolder).Sleep(500).PressEnterKeyboard().Sleep(500)
                    //                      .InputFieldLabel(10, diaChi, "Quận Ba Đình", quanHuyenPlaceHolder).Sleep(500).PressEnterKeyboard()
                    //                      .InputFieldLabel(10, diaChi, "Phường Cống Vị", phuongXaPlaceHolder).Sleep(500).PressEnterKeyboard()
                    //                      .InputFieldLabel(10, hopDong, "Cơ hội của " + companyName).Sleep(500).PressEnterKeyboard()
                    //                      .InputByAttributeValue(10, hanKhamVetId, ngayKhamData = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                    }

                    GeneralAction.Instance.CLickButtonTitle(10, General.luu)
                                          .WaitForElementInvisible(10, General.titleButton(General.luu)); Thread.Sleep(1000);
                    #endregion

                    #region Verify that data of the created 'DS Khám' is displayed correctly
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Cơ hội của " + companyName)
                                          .WaitForElementVisible(10, General.formEditable);

                    // verify Number of examinations displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "1", "//div[@name='active_opportunity_count']"); // 1/x DS Khám
                    verifyPoints.Add(summaryTC = "verify Number of examinations displayed on the 'DS Khám' button: '" + data + "'/x DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // verify total examinations count displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "1", "//div[@name='total_opportunity_count']"); // x/1 DS Khám
                    verifyPoints.Add(summaryTC = "verify total examinations count displayed on the 'DS Khám' button: x/'" + data + "' DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete DS Khám
                    // Click on the 'DS Khám' button
                    GeneralAction.Instance.ClickHtmlElement(10, "button", "action_view_opportunity_contract")
                                          .WaitForElementVisible(10, General.rendererTable("list_renderer")).Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(60).Sleep(2000);

                    // verify that the created 'examinations' are displayed in the list
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal= "contract_id", data = "Cơ hội của " + companyName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên đoàn' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_name", data = examinerName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên người khám' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Mới");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Giai đoạn' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "place_of_registration", data = "Nội viện");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nơi đăng ký khám' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "exam_appointment_date", data = DateTime.Now.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày hẹn khám' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = khachDoanLeadUsername);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nhân viên kinh doanh' is shown after searching the created examinations: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete the created 'examinations'
                    // Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickToCheckboxInRowTable(10, "1")
                                          .ThucHienXoaDelete(60);

                    // Verify the created examinations is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created examinations is deleted: '" + examinerName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Hợp đồng' Khách đoàn
                    GeneralAction.Instance.CLickItemBreadcrumb(10, "Cơ hội của " + companyName)
                                          .WaitForElementVisible(10, General.formEditable)
                                          .ThucHienXoaDelete(60);
                    #endregion

                    #region Delete 'Khách hàng doanh nghiệp'
                    GeneralAction.Instance.CLickItemBreadcrumb(10, companyName + " - " + dienThoaidata.Replace("19", "1 9").Replace("90", "9 0"))
                                          .WaitForElementVisible(10, General.formEditable);

                    // verify Number of contracts displayed on the 'Hợp đồng' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "0", "//div[@name='contract_count']/span[contains(@class,'value')]"); // 0
                    verifyPoints.Add(summaryTC = "verify Number of contracts displayed on the 'Hợp đồng' button: '" + data + "'/x Hợp đồng", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Detlete the created 'Khách hàng doanh nghiệp'
                    GeneralAction.Instance.ThucHienXoaDelete(60)
                                          .WaitForElementVisible(10, General.listRenderer);

                    // Verify the created 'Khách hàng doanh nghiệp' is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Khách hàng doanh nghiệp' is deleted: '" + companyName + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Liên hệ' Khách đoàn (switch to admin IVF)
                    // Switch to admin IVF account to delete the created 'Liên hệ' Khách đoàn
                    GeneralAction.Instance.SwitchUser(10, adminIVFEmail, adminIVFFullname);

                    // Go to 'Liên hệ' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer); Thread.Sleep(1500);

                    // Search the created client
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch).Sleep(5000);

                    // Verify the created client is shown
                    nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = examinerName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Tên' is shown after searching the created 'Khách hàng': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Delete all these created client
                    GeneralAction.Instance.ClickCheckboxAlltable(10).ThucHienXoaDelete(60);

                    // Verify the created client is deleted 
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the Created 'Khách hàng cá nhân' at 'Liên hệ' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on qa-dkhn / staging-dkhn site !!!");
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
        public void TC002_KhamDoan_ImportDSKham()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName,
            adminIVFEmail = LoginPage.adminIVFUsername,
            adminIVFFullname = LoginPage.adminIVFFullname,
            khachDoanLeadEmail = LoginPage.khachDoanLeadEmail,
            khachDoanLeadPass = LoginPage.khachDoanLeadPass,
            khachDoanLeadUsername = LoginPage.khachDoanLeadUsername,
            /// data
            dienThoaidata, dienThoaidata2, dienThoaidata3, dienThoaidataChong, dienThoaidataNguoiDiCung, tagData, nguoiPhuTrachData, soHopDongData,
            tenLienHeData, emailData, danhXungData, chucVuData, dienThoai2Data, examinerPhone, examinerPhone2, examinerPhone3, maSoThueData, maHopDongData, ngayKhamData, ngayKhamKetThucData;

            const string inputSearch = "qa auto", searchboxRole = "searchbox", opportunityContractName = "Cơ hội của Cty QA test 01";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Kham doan Test 002 - Import DS kham");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteXRender(60, urlInstance, khachDoanLeadEmail, khachDoanLeadPass, LoginPage.allRenderers);

                // Check if url site is not QA-DKHN then no run
                if (urlInstance.Contains(instanceName) || urlInstance.Contains("staging-bvdkhn"))
                {
                    #region Go to 'CRM' menu
                    GeneralAction.Instance.GoToLeftMenu(10, General.crm, "Hợp đồng Khách Đoàn", kanRenderer);

                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Cơ hội", "cty qa");

                    // verify that the searched 'Hợp đồng' is displayed
                    string stage = "Mới", row = "1", data = opportunityContractName;
                    verifyPoint = GeneralAction.Instance.TitleSubOfRecordKanbanInColumnGetText(10, stage, row, data);
                    verifyPoints.Add(summaryTC = "Verify that the searched 'Hợp đồng' is displayed in the '" + stage + "' column: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Go to 'Import đa khoa' page and delete all imported data if already exists
                    // Click on Record title of a column to verify data
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(10, moi, data)
                                          .WaitForElementVisible(10, General.formEditable); Thread.Sleep(1000);

                    // verify Number of examinations displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "0", "//div[@name='active_opportunity_count']"); // 0/x DS Khám
                    verifyPoints.Add(summaryTC = "verify Number of examinations displayed on the 'DS Khám' button when no import dsk: '" + data + "'/x DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click 'IMPORT DS KHÁM' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Import DS Khám")
                                          .WaitForElementVisible(10, General.listView);

                    // Delete all imported data if already exists
                    // Filter rows which contain a certain string (to delete the imported data before importing)
                    /// Click on 'Gỡ' icon to remove the value inpputted (search box)
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon)
                                          .CLickButtonTitle(10, General.boLoc)
                                          .WaitForElementVisible(10, General.dropDownShow)
                                          .CLickItemInDropdown(10, " Thêm bộ lọc tùy chỉnh ")
                                          .WaitForElementVisible(10, General.elementHtml("div", "filter_condition"))
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1)
                                          .ClickHtmlElement(10, "div", "filter_condition", menuFilter1 + "/option[.='Tên nhân viên']")
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

                    #region Import 'DS Khám' from file
                    // Click 'TẢI DỮ LIỆU' button
                    GeneralAction.Instance.CLickButtonTitle(10, "Tải dữ liệu")
                                          .WaitForElementVisible(10, General.DialogShow);
                    //.CLickButtonTitleInDialog(10, "Click chọn tập tin"); Thread.Sleep(2000); window dialog 'Chọn tệp' will be shown

                    // Upload file (Excel file)
                    string fileName = "Import_DS_Kham.xlsx";
                    string filePath = Path.Combine(AppContext.BaseDirectory, @"Tests\Documents\Excel files\BVDKHN\Import Data\");
                    
                    GeneralAction.Instance.UploadFileInDialogInput(10, "Tệp đính kèm", filePath + fileName);

                    /// ORG: not stable window dialog 'Chọn tệp'
                    //SendKeys.SendWait(Path.Combine(filePath + fileName, "{ENTER}")); Thread.Sleep(2000); 

                    // wait for file name is shown at upload field 'Tệp đính kèm'
                    //GeneralAction.Instance.WaitForElementVisible(10, General.uploadFileInDialog("Tệp đính kèm"), fileName); window dialog -> if use 

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
                    string nameAttrVal = "ksk_code";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = "1")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal, data = "2")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal, data = "3");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st -> 3rd row, column 'Mã KSK' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    nameAttrVal = "employee_code";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = "CODEQA01")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal, data = "CODEQA02")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal, data = "CODEQA03");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st -> 3rd row, column 'Mã NV' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    nameAttrVal = "name";
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal, data = "QA auto import DSK nv 01")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal, data = "QA auto import DSK nv 02")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal, data = "QA auto import DSK nv 03");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st -> 3rd row, column 'Tên nhân viên' is shown in table (after uploading excel): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Import data into application
                    // Click checkbox at a specific row (field value)
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .CLickButtonTitle(10, importBtn)
                                          .WaitForElementInvisible(10, General.titleButton(importBtn)).Sleep(3000);
                    #endregion

                    #region Verify that the imported 'DS Kham' was imported at 'Cơ hội của công ty...'
                    // Click breadcrumb 'Cơ hội của công ty...'
                    GeneralAction.Instance.CLickItemBreadcrumb(10, opportunityContractName)
                                          .WaitForElementVisible(10, General.formEditable);

                    // verify Number of examinations displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "3", "//div[@name='active_opportunity_count']");
                    verifyPoints.Add(summaryTC = "verify Number of examinations displayed on the 'DS Khám' button after importing: '" + data + "'/x DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // verify total examinations count displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "3", "//div[@name='total_opportunity_count']");
                    verifyPoints.Add(summaryTC = "verify total examinations count displayed on the 'DS Khám' button after importing: x/'" + data + "' DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Go to 'DS Khám' page and verify that the imported data is shown correctly
                    // Click on the 'DS Khám' button
                    GeneralAction.Instance.ClickHtmlElement(10, "button", "action_view_opportunity_contract")
                                          .WaitForElementVisible(10, General.rendererTable("list_renderer")).Sleep(500).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(60).Sleep(2000);

                    // Click on a Column name (Tên người khám) to sort the table
                    GeneralAction.Instance.ClickColumnNameToSortTable(10, "Tên người khám").Sleep(1500); // Sort A->Z
                    //.WaitForElementVisible(10, General.sortTableByColumnNameIconStatus("Tên người khám", General.sortUpIcon)); Thread.Sleep(1000);

                    // click column until the table to be sorted by 'Tên người khám' column (A->Z)
                    int time = 0;
                    while (GeneralAction.Instance.IsElementPresent(General.sortTableByColumnNameIconStatus("Tên người khám", General.sortUpIcon))==false && time<5)
                    {
                        GeneralAction.Instance.ClickColumnNameToSortTable(10, "Tên người khám"); // Sort A->Z
                        time += 1;
                        Thread.Sleep(1000);

                        if (GeneralAction.Instance.IsElementPresent(General.sortTableByColumnNameIconStatus("Tên người khám", General.sortUpIcon))==true)
                        {
                            break;
                        }

                        if (time == 5)
                        {
                            Console.WriteLine("Time out: The table is not sorted by 'Tên người khám' column (A->Z) after clicking 5 times.");
                            break;
                        }
                    }

                    // verify that the imported 'DS Khám' are displayed in the list
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contract_id", data = opportunityContractName)
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "contract_id", data = opportunityContractName)
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "contract_id", data = opportunityContractName);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Tên đoàn' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "contact_name", data = "QA auto import DSK nv 01")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "contact_name", data = "QA auto import DSK nv 02")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "contact_name", data = "QA auto import DSK nv 03");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Tên người khám' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetAttributeVal(10, "1", nameAttrVal = "phone", "data-tooltip", data = "0819 000 001");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Điện thoại' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "stage_id", data = "Mới")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "stage_id", data = "Mới")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "stage_id", data = "Mới");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Giai đoạn' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "test_appointment_date", data = "")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "test_appointment_date", data = "")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "test_appointment_date", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Ngày hẹn XN' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "test_status", data = "Chưa lấy mẫu")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "test_status", data = "")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "test_status", data = "Chưa lấy mẫu");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Trạng thái XN' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "place_of_registration", data = "Nội viện")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "place_of_registration", data = "Nội viện")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "place_of_registration", data = "Nội viện");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Nơi đăng ký khám' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "first_time_checkin", data = "27/08/2024")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "first_time_checkin", data = "")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "first_time_checkin", data = "27/08/2024");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Checkin lần đầu' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "exam_appointment_date", data = "03/07/2025 10:35:00")
                    //    && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "exam_appointment_date", data = "03/07/2025 10:35:00")
                    //    && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "exam_appointment_date", data = "03/07/2025 10:35:00");
                    //verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Ngày hẹn khám' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "consulting_status", data = "Cọc tại chỗ")
                       && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "consulting_status", data = "")
                       && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "consulting_status", data = "");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Trạng thái tư vấn' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "user_id", data = khachDoanLeadUsername)
                       && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "user_id", data = khachDoanLeadUsername)
                       && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "user_id", data = khachDoanLeadUsername);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Nhân viên kinh doanh' are displayed on 'DS Khám' page: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete DS Khám
                    // Delete the created 'examinations'
                    // Click to check all the checkbox and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .ThucHienXoaDelete(60);

                    // Verify the created examinations is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify the created 'DS Khám' is deleted: '" + "QA auto import DSK nv 01/02/03" + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click breadcrumb 'Cơ hội của công ty...'
                    GeneralAction.Instance.CLickItemBreadcrumb(10, opportunityContractName)
                                          .WaitForElementVisible(10, General.formEditable);

                    // verify Number of examinations displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "0", "//div[@name='active_opportunity_count']");
                    verifyPoints.Add(summaryTC = "verify Number of examinations displayed on the 'DS Khám' button after deleting: '" + data + "'/x DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // verify total examinations count displayed on the 'DS Khám' button
                    verifyPoint = GeneralAction.Instance.HtmlElementGetText(10, "button", "action_view_opportunity_contract", data = "0", "//div[@name='total_opportunity_count']");
                    verifyPoints.Add(summaryTC = "verify total examinations count displayed on the 'DS Khám' button after deleting: x/'" + data + "' DS Khám ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Search the created 'Booking' & Verify, delete data (Admin IVF)
                    // Switch to admin IVF account to search the imported 'Booking'
                    GeneralAction.Instance.SwitchUser(10, adminIVFEmail, adminIVFFullname);

                    // Go to 'Booking' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.bookingCuaToi, "Booking", listRenderer);

                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                    // Input a value in seachbox to Search (Booking of Nữ)
                    GeneralAction.Instance.SearchViewInput(60, "Họ tên vợ - năm sinh", inputSearch);

                    // verify that the searched 'Booking' Nữ (of DS Khám) is displayed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "create_date", data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_order", data = "26/08/2024");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày giờ' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_id", data = "Chưa xác định");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại nguồn' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "wife_infor", data = "QA auto import DSK nv");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên vợ - năm sinh' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "team_id", data = "Phòng Khách Đoàn");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ kinh doanh' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "create_team_id", data = "Phòng Khách Đoàn");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Đội ngũ tạo booking' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "source_booking", data = khachDoanLeadUsername);
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Nguồn booking' is shown after searching the created Booking (nữ): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click to check all the checkbox and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .ThucHienXoaDelete(60);

                    // Verify the created Booking is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all the created 'Booking' (Nữ) are deleted: '" + "QA auto import DSK nv..." + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);


                    // Input a value in seachbox to Search (Booking of Nam)
                    // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                    GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);
                    GeneralAction.Instance.SearchViewInput(60, "Họ tên chồng - năm sinh", inputSearch);

                    // verify that the searched 'Booking' Nam (of DS Khám) is displayed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "create_date", data = DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày tạo' is shown after searching the created Booking (nam): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "date_order", data = "26/08/2024");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Ngày giờ' is shown after searching the created Booking (nam): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "connext_source_type_id", data = "Chưa xác định");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Loại nguồn' is shown after searching the created Booking (nam): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal = "husband_infor", data = "QA auto import DSK nv 01");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st row, column 'Họ tên chồng - năm sinh' is shown after searching the created Booking (nam): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click to check all the checkbox and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .ThucHienXoaDelete(60);

                    // Verify the created Booking is deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all the created 'Booking' (Nam) are deleted: '" + "QA auto import DSK nv..." + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Search the created 'Liên hệ' & Verify, delete data (Admin IVF)
                    // Go to 'lien he' - 'Khach hang ca nhan' page
                    GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer);

                    // Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(60, "Tên", inputSearch);

                    // verify that the searched 'Liên hệ' is displayed
                    verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "1", nameAttrVal= "name", data= "QA auto import DSK nv 01")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "2", nameAttrVal = "name", data = "QA auto import DSK nv 02")
                        && GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(10, "3", nameAttrVal = "name", data = "QA auto import DSK nv 03");
                    verifyPoints.Add(summaryTC = "Verify data at the 1st->3rd row, column 'Tên' is shown after searching the created 'Liên hệ': '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Click to check all the checkbox and then click 'Thực hiện' >> Xóa
                    GeneralAction.Instance.ClickCheckboxAlltable(10)
                                          .ThucHienXoaDelete(60);

                    // Verify all the created 'Liên hệ' are deleted
                    verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                    verifyPoints.Add(summaryTC = "Verify all the created 'Liên hệ' are deleted: '" + "QA auto import DSK nv 01/02/03" + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on qa-dkhn / staging-dkhn site !!!");
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
