using Connext.UITest.Core.BaseClass;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Pages;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;
using System.Reflection.Emit;
using System.Security.Policy;
using AventStack.ExtentReports.Model;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Connext.UITest.Core.BaseTestCase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static OpenQA.Selenium.BiDi.Modules.Script.RemoteValue;

namespace Connext.UITest.Generals
{
    internal class General : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static readonly string
        /// menu (button)
        homeMenu = "Home Menu", switchUser = "Switch User", hoiThoai = "Hội thoại", cacHoatDong = "Các hoạt động",
        /// title (button)
        luuThuCong = "Lưu thủ công", luuVaDong = "Lưu & Đóng", luuVaTaoMoi = "Lưu & Tạo mới", loaiBoNhungThayDoi = "Loại bỏ những thay đổi",
        searchBoxRemoveDataIcon = "Gỡ", moi = "Mới", luu = "Lưu", huyBo = "Huỷ bỏ", addToSpreadesheet = "Add to spreadesheet",
        thucHien = "Thực hiện", timKiem = "Tìm kiếm", boLoc = "Bộ lọc", nhomTheo = "Nhóm theo", yeuThich = "Yêu thích",
        /// types of view (button)
        kanban = "Kanban", list = "List", calendar = "Calendar", pivot = "Pivot", doThi = "Đồ thị", hoatDong = "Hoạt động",
        /// title CRM (page)
        crm = "CRM", banHang = "Bán hàng", dataCuaToi = "Data của tôi", bookingCuaToi = "Booking của tôi", lead = "Lead", tuVan = "Tư vấn", baoCao = "Báo cáo", importData = "Import Data", cauHinh = "Cấu hình", khachHangDoanhNghiep = "Khách hàng doanh nghiệp",
        /// title Quan ly chat luong (page)
        quanlychatluong = "Quản lý chất lượng", tickets = "Tickets", danhGiaCuaKH = "Đánh giá của khách hàng", thietLap = "Thiết lập",
        /// title ECH (page)
        ech = "ECH", ketNoiZalo = "Kết nối Zalo", znsTemplates = "ZNS Templates",
        /// title Lien he (page)
        lienHe = "Liên hệ", lichSuCuocGoi = "Lịch sử cuộc gọi", nguoiDungCuoi = "Người dùng cuối",
        /// title Activity Dashboard (page)
        activityDashboard = "Activity Dashboard", dashboard = "Dashboard", configuration = "Configuration",
        /// title Du an (page)
        duAn = "Dự án", nhiemVuCuaToi = "Nhiệm vụ của tôi",
        /// title Nhan vien (page)
        nhanVien = "Nhân viên", phongBan = "Phòng/Ban", importDuLieu = "Import dữ liệu",
        /// title Ung dung (page)
        ungDung = "Ứng dụng",
        /// title Thiet lap (page)
        thietLapChung = "Thiết lập chung", nguoiDungVaCongTy = "Người dùng & Công ty",
        /// table
        sortUpIcon = "up", sortDownIcon = "down", noSortIcon = "opacity",
        /// title in Dialog
        userQuestionMark = "User?";

        // Initiate the By objects for elements
        internal static By elementHtml(string tagHtml, string attrValue, string? nextXPath = null) => By.XPath(@"//" + tagHtml + "[contains(@id,'" + attrValue + "') or contains(@class,'" + attrValue + "') or contains(@name,'" + attrValue + "') or contains(@title,'" + attrValue + "') or .='" + attrValue + "']" + nextXPath + "");

        /// By objects for wait to display
        internal static By spinnerLoading = By.XPath(@"//div[contains(@class,'spinner')]");
        internal static By rendererTable(string tableRenderer) => By.XPath(@"//*[contains(@class,'" + tableRenderer + "')]");
        internal static By DialogShow = By.XPath(@"//div[contains(@class,'modal-content')]");
        internal static By DialogHeaderContentShow => By.XPath(@"//div[contains(@class,'modal-content')]//*[contains(@class,'modal-header')]");
        internal static By DialogBodyContentShow => By.XPath(@"//div[contains(@class,'modal-content')]//*[contains(@class,'modal-body')]");
        internal static By DialogActiveShow = By.XPath(@"//div[contains(@class,'dialog_container')]/div/div[not(contains(@class,'inactive_modal'))]//div[contains(@class,'modal-content')]");
        internal static By DialogIndexShow(string index) => By.XPath(@"//div[contains(@class,'dialog_container')]/div/div[" + index + "]//div[contains(@class,'modal-content')]");
        internal static By dropDownShow = By.XPath(@"//div[contains(@class,'show')]");
        internal static By alertShow = By.XPath(@"//div[@role='alert']"); //*[contains(@class,'notification')]
        internal static By alertContentShow(string content) => By.XPath(@"//div[contains(@class,'o_notification')]/div[contains(.,'" + content + "')]");
        internal static By dialogErrorContentShow(string content) => By.XPath(@"//div[contains(@class,'dialog_error')]/*[contains(.,'" + content + "')]");
        internal static By kanbanQuickCreate = By.XPath(@"//div[contains(@class,'kanban_quick_create')]");
        internal static By kanbanRenderer = By.XPath(@"//div[contains(@class,'kanban_renderer')]");
        internal static By listRenderer = By.XPath(@"//div[contains(@class,'list_renderer')]");
        internal static By listView = By.XPath(@"//div[contains(@class,'list_view')]");
        internal static By formEditable = By.XPath(@"//div[contains(@class,'form_editable')]");
        internal static By formReadonly = By.XPath(@"//div[contains(@class,'form_readonly')]");
        internal static By allRenderers = By.XPath(@"//div[contains(@class,'kanban_renderer') or contains(@class,'kanban_quick_create') or contains(@class,'list_renderer') or contains(@class,'list_view') or contains(@class,'form_editable') or contains(@class,'form_readonly') or contains(@class,'ks_dashboard_ninja')]");

        /// By objects for table
        internal static By tableDataRow(string rowNumber) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + rowNumber + "]");
        internal static By tableDataRowWithValueColumn(string fieldValue) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr/td[.='" + fieldValue + "']");
        internal static By tableDataColByTxtOrAttrValInRow(string rowNumber, string textOrAttrVal) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + rowNumber + "]/td[@name='" + textOrAttrVal + "' or contains(@data-tooltip,'" + textOrAttrVal + "') or contains(.,'" + textOrAttrVal + "')]");
        internal static By tableCheckBoxAll = By.XPath(@"//th/div[contains(@class,'form-check')]//input"); //div[@class='o-checkbox form-check d-flex']//input
        internal static By tableCheckboxInRow(string rowNumber) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + rowNumber + "]//input");
        internal static By tableCheckboxInRowWithValueColumn(string fieldValue, string? attribute = null, string? attrValue = null) => By.XPath("//td[@" + attribute + "='" + attrValue + "' and .='" + fieldValue + "']/parent::tr/td[1]");
        internal static By tableListRowAdd(string index, string colName) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + index + "]//a[@name='" + colName + "' or contains(@data-tooltip,'" + colName + "') or contains(.,'" + colName + "')]"); // ex: Click at Create Booking -> Add Thêm sản phẩm / Thêm phần / ...
        internal static By inputTableListRowAdd(string row, string column, string? tagHtml = null) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + row + "]/td[" + column + "]//" + tagHtml + "");
        internal static By inputTableListRowAddWithNameId(string row, string nameId, string? nextXPath = null) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + row + "]/td[contains(@name,'" + nameId + "')]" + nextXPath + "");
        internal static By lichSuKhamCheckinButton(string row, string? tagHtml = null) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + row + "]/td[contains(@class,'checkin')]//" + tagHtml + "");
        internal static By recordInTableListRowAdd(string row, string column) => By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + row + "]/td[" + column + "]");
        internal static By sortTableByColumnName(string columnName) => By.XPath(@"//span[.='" + columnName + "']/ancestor::th");
        internal static By sortTableByColumnNameIcon(string columnName) => By.XPath(sortTableByColumnName(columnName).ToString().Remove(0, 10) + "//i");
        internal static By sortTableByColumnNameIconStatus(string columnName, string sortStatus) => By.XPath(sortTableByColumnName(columnName).ToString().Remove(0, 10) + "//i[contains(@class,'" + sortStatus + "')]");

        /// By objects for Menu & Button, Icon ...
        internal static By titleMainMenu(string title) => By.XPath(@"//a[@role='menuitem' and .='" + title + "']");
        internal static By titleMenu(string title) => By.XPath(@"//*[@title='" + title + "']");
        internal static By titleButton(string title) => By.XPath(@"//button[contains(.,'" + title + "') or contains(@class,'" + title + "') or contains(@aria-label,'" + title + "') or contains(@title,'" + title + "') or contains(@data-tooltip,'" + title + "')]");
        internal static By titleButton(string title, int pos) => By.XPath(@"//button[contains(.,'" + title + "') or contains(@class,'" + title + "') or contains(@aria-label,'" + title + "') or contains(@title,'" + title + "') or contains(@data-tooltip,'" + title + "')][position()=" + pos + "]");
        internal static By switchUserIconStatusOrg = By.XPath(@"//*[@id='switch_user' and not(@style)]");
        internal static By checkinNhanhIcon = By.XPath(@"//*[@class='toolbar-checkin-container']//i");
        internal static By breadcrumbItem(string title) => By.XPath(@"//*[@class='breadcrumb']/*[.='" + title + "']"); // --> element at Chu trình / Cơ hội của KH ...
        internal static By inputByAttrValue(string attrValue, string? tagHtml = null) => By.XPath(@"//" + tagHtml + "[contains(@class,'" + attrValue + "') or contains(@id,'" + attrValue + "') or contains(@type,'" + attrValue + "') or contains(@role,'" + attrValue + "') or contains(@placeholder,'" + attrValue + "')]");
        internal static By inputFieldTitle(string title, string? placeHolderValue = null, string? attribute = null) => By.XPath(@"//label[contains(.,'" + title + "')]/ancestor::div[contains(@class,'o_wrap_label')]/following-sibling::div//input[contains(@" + attribute + ",'" + placeHolderValue + "') or not(@" + attribute + ")]");
        internal static By itemFieldTitle(string title, string index, string? placeHolderValue = null, string? attribute = null) => By.XPath(inputFieldTitle(title, placeHolderValue, attribute).ToString().Remove(0, 10) + "/ancestor::div[contains(@class,'o_field_tags')]/span[" + index + "]");
        internal static By uploadFile = By.XPath(@"//input[@name='upload_file']");
        internal static By uploadFileInDialog(string title) => By.XPath(@"//div[contains(@class,'modal-content')]//td[.='" + title + "']/following-sibling::td//input[@type='file']");
        internal static By labelDropdown(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/ancestor::div[contains(@class,'o_wrap_label')]/following-sibling::div//select");
        internal static By itemDropdown(string item) => By.XPath(@"//div//*[.='" + item + "']");
        internal static By itemSubDropdown(string id, string item) => By.XPath("//*[@name='" + id + "']//ul/li[contains(.,'" + item + "')]");
        internal static By searchBoxDropdown = By.XPath(@"//ul[@role='menu']");
        internal static By searchBoxItemInDropdown(string menuLabel, string searchInputted) => By.XPath(@"//ul[@role='menu']/li[contains(.,'" + menuLabel + " for: " + searchInputted + "')]");
        internal static By searchBoxSubItemInDropdown(string menuLabel, string searchInputted, string name) => By.XPath(@"//ul[@role='menu']/li[contains(.,'" + menuLabel + " for: " + searchInputted + "')]/following-sibling::li/a[.='" + name + "']");
        internal static By searchBoxRemovefilter(string filter, string? removeIcon = null) => By.XPath(@"//*[.='" + filter + "']//*[@title='" + removeIcon + "']");
        internal static By priorityStar(string label, string attrValue) => By.XPath(@"//label[contains(.,'" + label + "')]/parent::div/following-sibling::div//a[@data-tooltip='" + attrValue + "' or @aria-label='" + attrValue + "']");
        internal static By inputFieldLabelGroupGrid(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/parent::div//input");
        internal static By inputByAttrValueInDialog(string attrValue, string? tagHtml = null) => By.XPath(@"//div[contains(@class,'modal-content')]//" + tagHtml + "[contains(@class,'" + attrValue + "') or contains(@id,'" + attrValue + "') or contains(@type,'" + attrValue + "') or contains(@role,'" + attrValue + "') or contains(@placeholder,'" + attrValue + "') or .='" + attrValue + "' or not(.)]");
        internal static By inputFieldLabelInDialog(string label, string? attrValue = null, string? attribute = null) => By.XPath(@"//div[contains(@class,'modal-content')]//label[.='" + label + "' or @for='" + label + "']/parent::div/following-sibling::div//" + attribute + "[contains(@class,'" + attrValue + "') or contains(@id,'" + attrValue + "') or contains(@placeholder,'" + attrValue + "')]");
        internal static By inputDescriptionContentInDialog = By.XPath(@"//div[contains(@class,'modal-content')]//div[contains(@class,'o_wrap_field')]//p");
        internal static By titleButtonInDialog(string title) => By.XPath(@"//div[contains(@class,'modal-content')]//button[.='" + title + "' or contains(@class,'" + title + "') or contains(@aria-label,'" + title + "') or contains(@title,'" + title + "') or contains(@data-tooltip,'" + title + "')]");
        internal static By titleButtonInKabanDialog(string label, string? classValueHtml = null) => By.XPath(@"//div[contains(@class,'" + classValueHtml + "')]//button[contains(.,'" + label + "') or contains(@class,'" + label + "') or contains(@aria-label,'" + label + "') or contains(@title,'" + label + "') or contains(@data-tooltip,'" + label + "')]");
        internal static By titleRecordKanbanInCol(string colName, string titleRecord) => By.XPath(@"//span[.='" + colName + "']/ancestor::div[contains(@class,'kanban_group flex')]//span[.='" + titleRecord + "']");
        internal static By textInTitleRecordKanbanInCol(string colName) => By.XPath(@"//span[.='" + colName + "']/ancestor::div[contains(@class,'kanban_group flex')]//div[contains(@class,'kanban_content')]//span");
        internal static By textSubTitleRecordKanbanInCol(string colName, string row) => By.XPath(@"//span[.='" + colName + "']/ancestor::div[contains(@class,'kanban_group flex')]//div[contains(@class,'kanban_content')]/div[" + row + "]//span");
        internal static By buttonTitleRecordKanbanInCol(string colName, string titleRecord, string? tagHtml = null, string? attrValue = null) => By.XPath(@"//span[.='" + colName + "']/ancestor::div[contains(@class,'kanban_group flex')]//span[.='" + titleRecord + "']/ancestor::div[contains(@class,'kanban_record d')]//" + tagHtml + "[.='" + attrValue + "' or contains(@class,'" + attrValue + "') or contains(@role,'" + attrValue + "') or contains(@data-tooltip,'" + attrValue + "') or contains(@aria-label,'" + attrValue + "')]");
        internal static By titleSettingsLeftMenu(string label) => By.XPath(@"//span[.='" + label + "']"); //div[@role='tab' and contains(@data-key,'general_settings')]

        /// By objects for notebook (Ghi chu)
        internal static By notebookHeader(string header) => By.XPath(@"//*[contains(@class,'notebook_header')]//li[.='" + header + "']");
        internal static By notebookHeaderDescriptionContent(string header) => By.XPath(@"//*[contains(@class,'notebook_header')]//li[.='" + header + "']/ancestor::div[contains(@class,'o_notebook d')]//p");
        internal static By notebookHeaderAndFieldNameContent(string header, string fieldName, string? tagHtml = null) => By.XPath(@"//*[contains(@class,'notebook_header')]//li[.='" + header + "']/ancestor::div[contains(@class,'o_notebook d')]//label[contains(.,'" + fieldName + "')]/parent::div/following-sibling::div//" + tagHtml + "");
        internal static By itemNotebookHeaderAndFieldNameContent(string header, string fieldName, string index, string? tagHtml = null) => By.XPath(notebookHeaderAndFieldNameContent(header, fieldName, tagHtml).ToString().Remove(0, 10) + "/ancestor::div[contains(@class,'o_field_tags')]/span[" + index + "]");
        internal static By notebookTableGetFieldValueWithName(string fieldValue, string attrStringValue, string? attribute = null, string? attrValue = null) => By.XPath(@"//*[contains(@class,'notebook_content')]//table//td[@" + attribute + "='" + attrValue + "' and .='" + fieldValue + "']/parent::tr/td[" + attrStringValue + "]");

        /// By objects for Chatter (message right pane)
        internal static By chatterMessage(string msg) => By.XPath(@"//div[contains(@class,'MessageList bg-view')]//*[.='" + msg + "']");
        internal static By chatterMsgChangedStatusSeparatorDate(string separatorDate, string areaNumber, string msgLine) => By.XPath(@"//div[contains(@class,'MessageList bg-view')]//div[.='" + separatorDate + "']/following-sibling::div[" + areaNumber + "]//i[@title='Đã thay đổi']/following-sibling::span[" + msgLine + "]");
        internal static By rewardFaceGroupStatus = By.XPath(@"//div[contains(@class,'reward_face_group')]");
        internal static By textFollowerList(string index) => By.XPath(@"//em[contains(@class,'follower-list')]/a[" + index + "]");
        internal static By recipientAndFollower(string nameOrEmail) => By.XPath(@"//div[contains(@title,'Add as recipient and follower')]//label[contains(.,'" + nameOrEmail + "')]");
        internal static By recipientAndFollowerCheckbox = By.XPath(@"//div[contains(@title,'Add as recipient and follower')]//input");
        internal static By chatterCommunicationPortDropdown = By.XPath(@"//div[contains(@class,'Chatter position-relative')]//select");
        internal static By chatterCommunicationPortItemInDropdown(string itemDropdown) => By.XPath(chatterCommunicationPortDropdown.ToString().Remove(0, 10) + "/option[.='" + itemDropdown + "']");

        // Initiate the elements
        public IWebElement htmlElement(int timeoutInSeconds, string tagHtml, string attrValue, string? nextXPath = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(elementHtml(tagHtml, attrValue, nextXPath)));
        }
        public IWebElement htmlElementInput(int timeoutInSeconds, string tagHtml, string attrValue, string? nextXPath = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(elementHtml(tagHtml, attrValue, nextXPath)));
        }
        public IWebElement contentDialogHeaderShow(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(DialogHeaderContentShow));
        }
        public IWebElement contentDialogBodyShow(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(DialogBodyContentShow));
        }
        public IWebElement showAlert(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(alertShow));
        }
        /// Highlight & Un-Highlight Element
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr = null)
        {
            // Check if give color/setOrRemoveAttr with a specific color/setOrRemoveAttr, if no then will get blue color (by default)
            color ??= "blue"; setOrRemoveAttr ??= "removeAttribute";

            IJavaScriptExecutor? js = Driver.Browser as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, " border: 3px solid " + color + "; "); Thread.Sleep(150);
            js.ExecuteScript("arguments[0]." + setOrRemoveAttr + "('style', arguments[1]);", element, " border: 3px solid " + color + ";"); // un-highlight
            return element;
        }
        /// Table Element
        public IWebElement dataRowTable(int timeoutInSeconds, string rowNumber)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableDataRow(rowNumber)));
        }
        public IWebElement dataRowTableWithValueColumn(int timeoutInSeconds, string fieldValue)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableDataRowWithValueColumn(fieldValue)));
        }
        public IWebElement dataColByTxtOrAttrValInRowTable(int timeoutInSeconds, string rowNumber, string textOrAttrVal)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableDataColByTxtOrAttrValInRow(rowNumber, textOrAttrVal)));
        }
        public IWebElement checkboxAlltable(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableCheckBoxAll));
        }
        public IWebElement checkboxInRowTable(int timeoutInSeconds, string rowNumber)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableCheckboxInRow(rowNumber)));
        }
        public IWebElement checkboxInRowTableWithValueColumn(int timeoutInSeconds, string fieldValue, string? attribute = null, string? attrValue = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableCheckboxInRowWithValueColumn(fieldValue, attribute, attrValue)));
        }
        public IWebElement listRowAddTable(int timeoutInSeconds, string index, string colName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(tableListRowAdd(index, colName)));
        }
        public IWebElement tableListRowAddInput(int timeoutInSeconds, string row, string column, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(inputTableListRowAdd(row, column, tagHtml)));
        }
        public IWebElement tableListRowAddWithNameIdInput(int timeoutInSeconds, string row, string nameId, string? nextXPath = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(inputTableListRowAddWithNameId(row, nameId, nextXPath)));
        }

        public IWebElement buttonCheckinLichSuKham(int timeoutInSeconds, string row, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(lichSuKhamCheckinButton(row, tagHtml)));
        }
        public IWebElement tableListRowAddRecord(int timeoutInSeconds, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(recordInTableListRowAdd(row, column)));
        }
        public IWebElement columnNameSortTable(int timeoutInSeconds, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(sortTableByColumnName(columnName)));
        }
        public IWebElement iconSortColumnNameTable(int timeoutInSeconds, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(sortTableByColumnNameIcon(columnName)));
        }
        /// Menu & Button, Icon ... Element
        public IWebElement mainMenuTitle(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleMainMenu(title)));
        }
        public IWebElement menuTitle(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleMenu(title)));
        }
        public IWebElement buttonTitle(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleButton(title)));
        }
        public IWebElement buttonTitle(int timeoutInSeconds, string title, int pos)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleButton(title, pos)));
        }
        public IWebElement iconCheckinNhanh(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(checkinNhanhIcon));
        }
        public IWebElement itemBreadcrumb(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(breadcrumbItem(title)));
        }
        public IWebElement dropdownLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdown(label)));
        }
        public IWebElement itemInDropdown(int timeoutInSeconds, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(itemDropdown(item)));
        }
        public IWebElement itemInSubDropdown(int timeoutInSeconds, string id, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(itemSubDropdown(id, item)));
        }
        public IWebElement fileUpload(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(uploadFile));
        }
        public IWebElement fileUploadInDialog(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(uploadFileInDialog(label)));
        }
        public IWebElement attrValueInput(int timeoutInSeconds, string attrValue, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputByAttrValue(attrValue, tagHtml)));
        }
        public IWebElement fieldTitleInput(int timeoutInSeconds, string title, string ? placeHolderValue = null, string? attribute = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputFieldTitle(title, placeHolderValue, attribute)));
        }
        public IWebElement fieldTitleItem(int timeoutInSeconds, string title, string index, string? placeHolderValue = null, string? attribute = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(itemFieldTitle(title, index, placeHolderValue, attribute)));
        }
        public IWebElement itemInDropdownSearchBox(int timeoutInSeconds, string menuLabel, string searchInputted)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(searchBoxItemInDropdown(menuLabel, searchInputted)));
        }
        public IWebElement subItemInDropdownSearchBox(int timeoutInSeconds, string menuLabel, string searchInputted, string name)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(searchBoxSubItemInDropdown(menuLabel, searchInputted, name)));
        }
        public IWebElement removefilterSearchBox(int timeoutInSeconds, string filter, string? removeIcon=null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(searchBoxRemovefilter(filter, removeIcon)));
        }
        public IWebElement starPriority(int timeoutInSeconds, string label, string attrValue)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(priorityStar(label, attrValue)));
        }
        public IWebElement fieldLabelGroupGridInput(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputFieldLabelGroupGrid(label)));
        }
        public IWebElement attrValueDialogInput(int timeoutInSeconds, string attrValue, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputByAttrValueInDialog(attrValue, tagHtml)));
        }
        public IWebElement fieldLabelDialogInput(int timeoutInSeconds, string label, string? attrValue = null, string? attribute = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputFieldLabelInDialog(label, attrValue, attribute)));
        }
        public IWebElement descriptionContentInDialogInput(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(inputDescriptionContentInDialog));
        }
        public IWebElement buttonTitleInDialog(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleButtonInDialog(title)));
        }
        public IWebElement buttonTitleInKanbanDialog(int timeoutInSeconds, string title, string? classValHtml=null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleButtonInKabanDialog(title, classValHtml)));
        }
        public IWebElement recordTitleKanbanInCol(int timeoutInSeconds, string colName, string titleRecord)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleRecordKanbanInCol(colName, titleRecord)));
        }
        public IWebElement recordTextInTitleKanbanInCol(int timeoutInSeconds, string colName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(textInTitleRecordKanbanInCol(colName)));
        }
        public IWebElement recordTextSubTitleKanbanInCol(int timeoutInSeconds, string colName, string row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(textSubTitleRecordKanbanInCol(colName, row)));
        }
        public IWebElement recordButtonInTitleKanbanInCol(int timeoutInSeconds, string colName, string titleRecord, string? tagHtml = null, string? attrValue = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(buttonTitleRecordKanbanInCol(colName, titleRecord, tagHtml, attrValue)));
        }
        public IWebElement settingsLeftMenuTitle(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(titleSettingsLeftMenu(label)));
        }
        /// Notebook (Ghi chu) Element
        public IWebElement headerNotebook(int timeoutInSeconds, string header)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(notebookHeader(header)));
        }
        public IWebElement notebookHeaderDescriptionContentInput(int timeoutInSeconds, string header)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(notebookHeaderDescriptionContent(header)));
        }
        public IWebElement notebookHeaderAndFieldNameContentInput(int timeoutInSeconds, string header, string fieldName, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(notebookHeaderAndFieldNameContent(header, fieldName, tagHtml)));
        }
        public IWebElement notebookHeaderAndFieldNameContentItem(int timeoutInSeconds, string header, string fieldName, string index, string? tagHtml = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(itemNotebookHeaderAndFieldNameContent(header, fieldName, index, tagHtml)));
        }
        public IWebElement notebookDataTableGetFieldValueWithName(int timeoutInSeconds, string fieldValue, string attrStringValue, string? attribute = null, string? attrValue = null)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(notebookTableGetFieldValueWithName(fieldValue, attrStringValue, attribute, attrValue)));
        }
        /// Chatter (message right pane) Element
        public IWebElement messageChatter(int timeoutInSeconds, string msg)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(chatterMessage(msg)));
        }
        public IWebElement messageChatterChangedStatusSeparatorDate(int timeoutInSeconds, string separatorDate, string areaNumber, string msgLine)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(chatterMsgChangedStatusSeparatorDate(separatorDate, areaNumber, msgLine)));
        }
        public IWebElement statusRewardFaceGroup(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rewardFaceGroupStatus));
        }
        public IWebElement followerListText(int timeoutInSeconds, string index)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(textFollowerList(index)));
        }
        public IWebElement checkboxRecipientAndFollower(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(recipientAndFollowerCheckbox));
        }
        public IWebElement communicationPortChatterDropdown(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(chatterCommunicationPortDropdown));
        }
        public IWebElement communicationPortChatterItemInDropdown(int timeoutInSeconds, string itemDropdown)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(chatterCommunicationPortItemInDropdown(itemDropdown)));
        }
    }

    internal sealed class GeneralAction : BasePage<GeneralAction, General>
    {
        #region Constructor
        private GeneralAction() { }
        #endregion

        // Items Action & Built-in Actions

        #region Wait action
        // Wait for element visible
        public GeneralAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }
        // Wait for element has value/text visible
        public GeneralAction WaitForElementVisible(int timeoutInSeconds, By by, string text)
        {
            if (timeoutInSeconds > 0) 
            {
                var wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));

                wait.Until(d => d.FindElement(by).GetAttribute("value") == text
                             || d.FindElement(by).Text == text);
            }
            return this;
        }

        // Wait for element Invisible
        public GeneralAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // Wait for loading Spinner icon to disappear
        public GeneralAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // Wait for CRM page load done
        public GeneralAction WaitForPageLoadDone(int timeoutInSeconds)
        {
            int timne = 0;
            while (timne <= timeoutInSeconds)
            {
                if (IsElementPresent(LoginPage.kanbanRendererTable) ||
                    IsElementPresent(LoginPage.listRenderer))
                { break; }

                if (timne == timeoutInSeconds)
                {
                    Console.WriteLine("Timeout on web page load in " + timeoutInSeconds + "s");
                    BaseTestCase.ExtReportResult(false, "Timeout on web page load in " + timeoutInSeconds + "s");
                }

                timne++;
                Thread.Sleep(1000);
            }
            return this;
        }

        // Checking element exists or not
        public bool IsElementPresent(By by)
        {
            try
            {
                Driver.Browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        // Check if the spinner Loading icon is shown then wait for it to load done
        public GeneralAction CheckIfSpinnerLoadingIconShownThenWaitLoadDone(int timeoutInSeconds)
        {
            if (IsElementPresent(General.spinnerLoading))
            {
                WaitForElementInvisible(timeoutInSeconds, General.spinnerLoading); Thread.Sleep(100);
            }
            return this;
        }
        #endregion

        #region verify elements
        public bool IsHtmlElementShown(int timeoutInSeconds, string tagHtml, string attrValue, string? nextXPath=null)
        {
            // Check nextXPath with a specific nextXPath, if no then will get nextXPath by default = null
            if (nextXPath == null) nextXPath = null;

            var iweb = Map.htmlElement(timeoutInSeconds, tagHtml, attrValue, nextXPath);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ElementShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool HtmlElementGetText(int timeoutInSeconds, string tagHtml, string attrValue, string textParam, string? nextXPath = null)
        {
            // Check nextXPath with a specific nextXPath, if no then will get nextXPath by default = null
            if (nextXPath == null) nextXPath = null;

            var iweb = Map.htmlElement(timeoutInSeconds, tagHtml, attrValue, nextXPath);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ElementShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool HtmlElementGetTextValue(int timeoutInSeconds, string tagHtml, string attrValue, string textParam, string? nextXPath = null)
        {
            // Check nextXPath with a specific nextXPath, if no then will get nextXPath by default = null
            if (nextXPath == null) nextXPath = null;

            var iweb = Map.htmlElement(timeoutInSeconds, tagHtml, attrValue, nextXPath);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ElementShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DialogHeaderContentGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.contentDialogHeaderShow(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DialogContent" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DialogBodyContentGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.contentDialogBodyShow(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DialogContent" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool AlertGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.showAlert(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ToastContent" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsMainTitleShown(int timeoutInSeconds, string title)
        {
            var iweb = Map.mainMenuTitle(timeoutInSeconds, title);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_MenuTitleShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsMenuTitleShown(int timeoutInSeconds, string title)
        {
            var iweb = Map.menuTitle(timeoutInSeconds, title);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element==false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_MenuTitleShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsButtonTitleShown(int timeoutInSeconds, string title)
        {
            var iweb = Map.buttonTitle(timeoutInSeconds, title);
            bool element =  Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_buttonTitleShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DropdownLabelGetText(int timeoutInSeconds, string title, string textParam)
        {
            var iweb = Map.dropdownLabel(timeoutInSeconds, title);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_SelectedDropdown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool UploadFileGetValue(int timeoutInSeconds, string title, string textParam)
        {
            var iweb = Map.fileUpload(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_fileUploadText" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public string UploadFileInDialogGetValue(int timeoutInSeconds, string title)
        {
            var iweb = Map.fileUploadInDialog(timeoutInSeconds, title);
            string? element = Map.HighlightElement(iweb, "blue").GetAttribute("value");
            return element;
        }
        public string DataColByTxtOrAttrValInRowTableGetTextOnly(int timeoutInSeconds, string rowNumber, string textOrAttrVal)
        {
            ScrollIntoView(Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal));
            var iweb = Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal);
            string? element = Map.HighlightElement(iweb, "green").Text;
            return element;
        }
        public bool DataColByTxtOrAttrValInRowTableGetText(int timeoutInSeconds, string rowNumber, string textOrAttrVal, string textParam)
        {
            ScrollIntoView(Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal));
            var iweb = Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                ScrollIntoView(Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal));
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_dataInTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataColByTxtOrAttrValInRowTableGetAttributeVal(int timeoutInSeconds, string rowNumber, string textOrAttrVal, string attributeParam, string textParam)
        {
            ScrollIntoView(Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal));
            var iweb = Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute(attributeParam).Contains(textParam);
            if (element == false)
            {
                ScrollIntoView(Map.dataColByTxtOrAttrValInRowTable(timeoutInSeconds, rowNumber, textOrAttrVal));
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_dataInTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsCheckboxInRowTableDeleted(string rowNumber)
        {
            Thread.Sleep(1000);
            if (IsElementPresent(General.tableCheckboxInRow(rowNumber))) return false;
            return true;
        }
        public bool IsButtonTitleDeleted(string title)
        {
            Thread.Sleep(1000);
            if (IsElementPresent(General.titleButton(title))) return false;
            return true;
        }
        public bool TitleRecordKanbanInColumnGetText(int timeoutInSeconds, string columnName, string textParam)
        {
            Thread.Sleep(1000);
            var iweb = Map.recordTextInTitleKanbanInCol(timeoutInSeconds, columnName);
            ScrollIntoView(iweb);

            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_dataInTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return true;
        }
        public bool TitleSubOfRecordKanbanInColumnGetText(int timeoutInSeconds, string columnName, string row, string textParam)
        {
            Thread.Sleep(1000);
            var iweb = Map.recordTextSubTitleKanbanInCol(timeoutInSeconds, columnName, row);
            ScrollIntoView(iweb);

            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_dataInTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return true;
        }
        public bool IsTitleRecordKanbanInColumnDeleted(string columnName, string titleRecord)
        {
            Thread.Sleep(1000);
            if (IsElementPresent(General.titleRecordKanbanInCol(columnName, titleRecord))) return false;
            return true;
        }
        public bool IsAttribtuePresent(IWebElement element, string attribute) // use for Priority Star, checkbox ....
        {
            bool result = false;
            try
            {
                string value = Map.HighlightElement(element, "green", "setAttribute").GetAttribute(attribute);
                if (value != null)
                {
                    result = true;
                }
            }
            catch (Exception e) {}

            if (result == false) 
            {
                Map.HighlightElement(element, "red", "setAttribute");
                Driver.TakeScreenShot("ss_isAttribtuePresent" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(element, "red", "removeAttribute");
            }

            return result;
        }
        public bool IsPriorityStarShown(int timeoutInSeconds, string label, string attrValue, string attrNameShown)
        {
            var iweb = Map.starPriority(timeoutInSeconds, label, attrValue);
            return IsAttribtuePresent(iweb, attrNameShown);
        }
        public bool DataInputByAttrValueGetText(int timeoutInSeconds, string attrValue, string textParam, string? tagHtml=null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            var iweb = Map.attrValueInput(timeoutInSeconds, attrValue, tagHtml);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInTheField" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputFieldTitleGetText(int timeoutInSeconds, string title, string textParam, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            var iweb = Map.fieldTitleInput(timeoutInSeconds, title, placeHolderValue, attribute);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInTheField" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataItemFieldTitleGetText(int timeoutInSeconds, string title, string index, string textParam, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            var iweb = Map.fieldTitleItem(timeoutInSeconds, title, index, placeHolderValue, attribute);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataItemInTheField" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public string SortIconColumnNameTableGetStatus(int timeoutInSeconds, string columName)
        {
            return Map.iconSortColumnNameTable(timeoutInSeconds, columName).GetAttribute("class");
        }
        /// verify elements for Notebook (Ghi chu) 
        public bool DataInputNotebookHeaderDescriptionContentGetText(int timeoutInSeconds, string header, string textParam)
        {
            var iweb = Map.notebookHeaderDescriptionContentInput(timeoutInSeconds, header);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInTheFieldNoteBook" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputNotebookHeaderAndFieldNameContentGetText(int timeoutInSeconds, string header, string fieldName, string textParam, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            var iweb = Map.notebookHeaderAndFieldNameContentInput(timeoutInSeconds, header, fieldName, tagHtml);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInTheFieldNoteBook" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputNotebookHeaderAndFieldNameContentGetTextValue(int timeoutInSeconds, string header, string fieldName, string textParam, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            var iweb = Map.notebookHeaderAndFieldNameContentInput(timeoutInSeconds, header, fieldName, tagHtml);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInTheFieldNoteBook" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataItemNotebookHeaderAndFieldNameContentGetText(int timeoutInSeconds, string header, string fieldName, string index, string textParam, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            var iweb = Map.notebookHeaderAndFieldNameContentItem(timeoutInSeconds, header, fieldName, index, tagHtml);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataItemInTheFieldNoteBook" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputRowListTableGetText(int timeoutInSeconds, string row, string column, string textParam, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            var iweb = Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInRowListTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataRecordRowListTableGetText(int timeoutInSeconds, string row, string column, string textParam)
        {
            var iweb = Map.tableListRowAddRecord(timeoutInSeconds, row, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInRowListTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputRowListTableWithNameIdGetText(int timeoutInSeconds, string row, string nameId, string textParam, string? nextXPath = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (nextXPath == null) nextXPath = "";

            var iweb = Map.tableListRowAddWithNameIdInput(timeoutInSeconds, row, nameId, nextXPath);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInRowListTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool DataInputRowListTableWithNameIdGetText(int timeoutInSeconds, string row, string nameId, string textParam)
        {
            var iweb = Map.tableListRowAddWithNameIdInput(timeoutInSeconds, row, nameId);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInRowListTable" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        /// verify elements for Chatter (Message right pane) <summary>
        public bool ChatterMessageRightPaneGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.messageChatter(timeoutInSeconds, textParam);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ChatterMessageRightPane" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool ChatterMessageChangedStatusSeparatorDateGetText(int timeoutInSeconds, string separatorDate, string areaNumber, string msgLine, string textParam)
        {
            var iweb = Map.messageChatterChangedStatusSeparatorDate(timeoutInSeconds, separatorDate, areaNumber, msgLine);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ChatterMessageRightPane" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsRewardFaceGroupStatusShown(int timeoutInSeconds)
        {
            var iweb = Map.statusRewardFaceGroup(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_RewardFaceGroupStatusShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool FollowerListGetText(int timeoutInSeconds, string index, string textParam)
        {
            var iweb = Map.followerListText(timeoutInSeconds, index);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ChatterFollowerListRightPane" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsCheckboxRecipientAndFollowerShown(int timeoutInSeconds, string trueOrFalse)
        {
            var iweb = Map.checkboxRecipientAndFollower(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Equals(trueOrFalse);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_RewardFaceGroupStatusShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        #endregion

        #region Actions
        /// thread sleep
        public GeneralAction Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
            return this;
        }
        /// Scroll Into View IwebElement
        public GeneralAction ScrollIntoView(IWebElement iwebE)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);
            return this;
        }
        /// PageDown To scroll down page
        public GeneralAction PageDownToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Build().Perform();
            return this;
        }
        /// Zoom in and out
        public GeneralAction Zoom(int level)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Browser;
            js.ExecuteScript(string.Format("document.body.style.zoom='{0}%'", level));
            //js.ExecuteScript(string.Format("document.body.style['-webkit-transform'] = 'scale(.8)';"));
            return this;
        }
        /// Press Keyboard
        public GeneralAction PressInputTextKeyboard(string text)
        {
            System.Windows.Forms.SendKeys.SendWait(text);
            return this;
        }
        public GeneralAction PressEnterKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
            //System.Windows.Forms.SendKeys.SendWait(@"{ENTER}");
            return this;
        }
        public GeneralAction PressTabKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Tab).Build().Perform();
            //System.Windows.Forms.SendKeys.SendWait(@"{TAB}");
            return this;
        }
        public GeneralAction PressUpKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.ArrowUp).Build().Perform();
            //System.Windows.Forms.SendKeys.SendWait(@"{UP}");
            return this;
        }
        public GeneralAction PressDownKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
            //System.Windows.Forms.SendKeys.SendWait(@"{DOWN}");
            return this;
        }
        public GeneralAction PressRightKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.ArrowRight).Build().Perform();
            //System.Windows.Forms.SendKeys.SendWait(@"{DOWN}");
            return this;
        }
        /// Click html element
        public GeneralAction ClickHtmlElement(int timeoutInSeconds, string tagHtml, string attrValue, string? nextXPath = null)
        {
            // Check nextXPath with a specific nextXPath, if no then will get nextXPath by default = null
            if (nextXPath == null) nextXPath = null;

            Map.HighlightElement(Map.htmlElement(timeoutInSeconds, tagHtml, attrValue, nextXPath)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction InputHtmlElement(int timeoutInSeconds, string tagHtml, string attrValue, string textParam, string? nextXPath = null)
        {
            // Check nextXPath with a specific nextXPath, if no then will get nextXPath by default = null
            if (nextXPath == null) nextXPath = null;

            Map.HighlightElement(Map.htmlElementInput(timeoutInSeconds, tagHtml, attrValue, nextXPath)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.htmlElementInput(timeoutInSeconds, tagHtml, attrValue, nextXPath)).SendKeys(textParam); Thread.Sleep(350);
            return this;
        }

        /// Action for table
        public GeneralAction ClickDataRowTable(int timeoutInSeconds, string rowNumber)
        {
            Map.HighlightElement(Map.dataRowTable(timeoutInSeconds, rowNumber)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickDataRowTableWithValueColumn(int timeoutInSeconds, string fieldValue)
        {
            Map.HighlightElement(Map.dataRowTableWithValueColumn(timeoutInSeconds, fieldValue)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickCheckboxAlltable(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.checkboxAlltable(timeoutInSeconds)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickCheckboxInRowTable(int timeoutInSeconds, string rowNumber)
        {
            Map.HighlightElement(Map.checkboxInRowTable(timeoutInSeconds, rowNumber)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickCheckboxInRowTableWithValueColumn(int timeoutInSeconds, string fieldValue, string? attribute = null, string? attrValue = null)
        {
            // Check attrValue/attribute with a specific attrValue/attribute, if no then will get attribute by default: attribute=name, attrValue=full_name
            if (attribute == null) attribute = "name";
            if (attrValue == null) attrValue = "full_name";
            
            Map.HighlightElement(Map.checkboxInRowTableWithValueColumn(timeoutInSeconds, fieldValue, attribute, attrValue)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickAddRowListTable(int timeoutInSeconds, string index, string colName)
        {
            PageDownToScrollDownPage();
            ScrollIntoView(Map.listRowAddTable(timeoutInSeconds, index, colName));

            // Way1: Using OpenQA build Click 
            //Actions actions = new Actions(Driver.Browser);
            //actions.Click(Map.HighlightElement(Map.listRowAddTable(timeoutInSeconds, index, colName))).Build().Perform(); Thread.Sleep(500);
            //Actions actions = new Actions(Driver.Browser);
            //actions.MoveToElement(this.Map.HighlightElement(Map.listRowAddTable(timeoutInSeconds, index, colName)));
            //actions.Perform();

            // Way2: Using Javascript click if there issue 'element click intercepted'
            //IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            //je.ExecuteScript("arguments[0].click();", Map.HighlightElement(Map.listRowAddTable(timeoutInSeconds, index, colName))); Thread.Sleep(500);

            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.listRowAddTable(timeoutInSeconds, index, colName))).Build().Perform(); Thread.Sleep(500); ;

            //Map.HighlightElement(Map.listRowAddTable(timeoutInSeconds, index, colName)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction InputRowListTable(int timeoutInSeconds, string row, string column, string text,string? tagHtml=null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml))).Build().Perform(); Thread.Sleep(250);

            //Map.HighlightElement(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml)).Click(); Thread.Sleep(250);
            Map.HighlightElement(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public GeneralAction ClickButtonInRowListTable(int timeoutInSeconds, string row, string column, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = button
            if (tagHtml == null) tagHtml = "button";

            ScrollIntoView(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml));
            Map.HighlightElement(Map.tableListRowAddInput(timeoutInSeconds, row, column, tagHtml)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickButtonCheckinLichSuKham(int timeoutInSeconds, string row, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = button
            if (tagHtml == null) tagHtml = "button";

            ScrollIntoView(Map.buttonCheckinLichSuKham(timeoutInSeconds, row, tagHtml));
            Map.HighlightElement(Map.buttonCheckinLichSuKham(timeoutInSeconds, row, tagHtml)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction ClickRecordPosXInRowListTable(int timeoutInSeconds, string row, string column)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.tableListRowAddRecord(timeoutInSeconds, row, column))).Build().Perform(); Thread.Sleep(500);

            //Map.HighlightElement(Map.tableListRowAddRecord(timeoutInSeconds, row, column)).Click(); Thread.Sleep(250);
            return this;
        }
        public GeneralAction ClickColumnNameToSortTable(int timeoutInSeconds, string columnName)
        {
            Map.HighlightElement(Map.columnNameSortTable(timeoutInSeconds, columnName)).Click(); Thread.Sleep(250);
            return this;
        }
        /// Action for Menu/button/input ...
        public GeneralAction CLickMainMenuTitle(int timeoutInSeconds, string title)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.mainMenuTitle(timeoutInSeconds, title))).Build().Perform(); Thread.Sleep(500);

            //Map.HighlightElement(Map.mainMenuTitle(timeoutInSeconds, title)).Click();
            return this;
        }
        public GeneralAction CLickMenuTitle(int timeoutInSeconds, string title)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.menuTitle(timeoutInSeconds, title))).Build().Perform(); Thread.Sleep(500);

            //Map.HighlightElement(Map.menuTitle(timeoutInSeconds, title)).Click();
            return this;
        }
        public GeneralAction CLickButtonTitle(int timeoutInSeconds, string title)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.buttonTitle(timeoutInSeconds, title))).Build().Perform(); Thread.Sleep(500);

            //Map.HighlightElement(Map.buttonTitle(timeoutInSeconds, title)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickButtonTitle(int timeoutInSeconds, string title, int pos)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.buttonTitle(timeoutInSeconds, title, pos))).Build().Perform(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickCheckinNhanhIcon(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.iconCheckinNhanh(timeoutInSeconds)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickItemBreadcrumb(int timeoutInSeconds, string title)
        {
            Map.HighlightElement(Map.itemBreadcrumb(timeoutInSeconds, title)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction UploadFileInput(int timeoutInSeconds, string filepath)
        {
            this.Map.fileUpload(timeoutInSeconds).SendKeys(filepath);
            return this;
        }
        public GeneralAction UploadFileInDialogInput(int timeoutInSeconds, string title, string filepath)
        {
            // Hide 'select file' button
            var clickChonTapTinBtn = this.Map.buttonTitleInDialog(timeoutInSeconds, "Click chọn tập tin");
            ((IJavaScriptExecutor)Driver.Browser).ExecuteScript("arguments[0].setAttribute('class', 'btn btn-primary o_select_file_button o_hidden')", clickChonTapTinBtn);
            
            // Upload file in dialog
            var element = this.Map.fileUploadInDialog(timeoutInSeconds, title);
            //((IJavaScriptExecutor)Driver.Browser).ExecuteScript("arguments[0].removeAttribute('readonly');arguments[0].style.display = 'block'", element);
            //((IJavaScriptExecutor)Driver.Browser).ExecuteScript("arguments[0].setAttribute('class', 'o_input')", element);
            element.SendKeys(filepath);
            return this;
        }
        public GeneralAction CLickDropdownLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label)).Click();
            return this;
        }
        public GeneralAction CLickItemInDropdown(int timeoutInSeconds, string item)
        {
            Map.HighlightElement(Map.itemInDropdown(timeoutInSeconds, item)).Click();
            return this;
        }
        public GeneralAction CLickItemInSubDropdown(int timeoutInSeconds, string id, string item)
        {
            Map.HighlightElement(Map.itemInSubDropdown(timeoutInSeconds, id, item)).Click();
            return this;
        }
        public GeneralAction InputByAttributeValue(int timeoutInSeconds, string attrValue, string text, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            Map.HighlightElement(Map.attrValueInput(timeoutInSeconds, attrValue, tagHtml)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.attrValueInput(timeoutInSeconds, attrValue, tagHtml)).SendKeys(text); Thread.Sleep(350);
            return this;
        }
        public GeneralAction InputFieldLabel(int timeoutInSeconds, string label, string inputText, string ? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(150);
            Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).SendKeys(inputText); Thread.Sleep(200);
            return this;
        }
        public GeneralAction ClickInputFieldLabel(int timeoutInSeconds, string label, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute))).Build().Perform(); Thread.Sleep(100);
            return this;
        }
        public GeneralAction ClickInputByAttributeValue(int timeoutInSeconds, string attrValue, string? tagHtml = null)
        {
            // Check tagHtml with a specific tagHtml, if no then will get tagHtml by default = input
            if (tagHtml == null) tagHtml = "input";

            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.attrValueInput(timeoutInSeconds, attrValue, tagHtml))).Build().Perform(); Thread.Sleep(100);
            return this;
        }
        public GeneralAction ClickInputFieldLabelDialog(int timeoutInSeconds, string label, string? attrValue = null, string? attribute = null)
        {
            // Check attrValue/attribute with a specific attrValue/attribute, if no then will get attribute by default: attrValue=null; attribute=input
            if (attrValue == null) attrValue = "";
            if (attribute == null) attribute = "input";

            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.fieldLabelDialogInput(timeoutInSeconds, label, attrValue, attribute))).Build().Perform(); Thread.Sleep(100);
            return this;
        }
        public GeneralAction ClickCheckboxAtFieldLabel(int timeoutInSeconds, string label, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickItemInDropdownSearchBox(int timeoutInSeconds, string menuLabel, string searchInputted)
        {
            Map.HighlightElement(Map.itemInDropdownSearchBox(timeoutInSeconds, menuLabel, searchInputted)).Click();
            return this;
        }
        public GeneralAction CLickItemInDropdownSearchBox(int timeoutInSeconds, string menuLabel, string searchInputted, string name) // this use for click on Sub item in searchbox-dropdown 
        {
            Map.HighlightElement(Map.subItemInDropdownSearchBox(timeoutInSeconds, menuLabel, searchInputted, name)).Click();
            return this;
        }
        public GeneralAction CLickRemovefilterSearchBox(int timeoutInSeconds, string filter, string? removeIcon=null)
        {
            // Check removeIcon with a specific removeIcon, if no then will get removeIcon by default = Gỡ
            if (removeIcon == null) removeIcon = "Gỡ";

            Map.HighlightElement(Map.removefilterSearchBox(timeoutInSeconds, filter, removeIcon)).Click();
            return this;
        }
        public GeneralAction CLickPriorityStar(int timeoutInSeconds, string label, string attrValue)
        {
            Map.HighlightElement(Map.starPriority(timeoutInSeconds, label, attrValue)).Click();
            return this;
        }
        public GeneralAction InputFieldLabelGroupGrid(int timeoutInSeconds, string label, string text)
        {
            Map.HighlightElement(Map.fieldLabelGroupGridInput(timeoutInSeconds, label)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.fieldLabelGroupGridInput(timeoutInSeconds, label)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public GeneralAction InputByAttributeValueDialog(int timeoutInSeconds, string attrValue, string text, string? tagHtml = null)
        {
            // Check tagName with a specific tagName, if no then will get tagName by default = input
            if (tagHtml == null) tagHtml = "input";

            Map.HighlightElement(Map.attrValueDialogInput(timeoutInSeconds, attrValue, tagHtml)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.attrValueDialogInput(timeoutInSeconds, attrValue, tagHtml)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public GeneralAction InputFieldLabelDialog(int timeoutInSeconds, string label, string inputText, string? attrValue = null, string? attribute = null)
        {
            // Check attrValue/attribute with a specific attrValue/attribute, if no then will get attribute by default: attrValue=null; attribute=input
            if (attrValue == null) attrValue = "";
            if (attribute == null) attribute = "input";

            Map.HighlightElement(Map.fieldLabelDialogInput(timeoutInSeconds, label, attrValue, attribute)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(150);
            Map.HighlightElement(Map.fieldLabelDialogInput(timeoutInSeconds, label, attrValue, attribute)).SendKeys(inputText); Thread.Sleep(150);
            return this;
        }
        public GeneralAction InputDescriptionContentInDialog(int timeoutInSeconds, string inputText)
        {
            Map.HighlightElement(Map.descriptionContentInDialogInput(timeoutInSeconds)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.descriptionContentInDialogInput(timeoutInSeconds)).SendKeys(inputText); Thread.Sleep(250);
            return this;
        }
        public GeneralAction ClickInputByAttributeValueDialog(int timeoutInSeconds, string attrValue, string? tagHtml = null)
        {
            // Check tagName with a specific tagName, if no then will get tagName by default = input
            if (tagHtml == null) tagHtml = "input";

            Map.HighlightElement(Map.attrValueDialogInput(timeoutInSeconds, attrValue, tagHtml)).Click(); Thread.Sleep(100);
            return this;
        }
        public GeneralAction CLickButtonTitleInDialog(int timeoutInSeconds, string title)
        {
            Map.HighlightElement(Map.buttonTitleInDialog(timeoutInSeconds, title)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickButtonTitleInKabanDialog(int timeoutInSeconds, string title, string? classValHtml=null)
        {
            // Check classValHtml with a specific classValHtml, if no then will get classValHtml by default = quick_create
            if (classValHtml == null) classValHtml = "quick_create";
            Map.HighlightElement(Map.buttonTitleInKanbanDialog(timeoutInSeconds, title, classValHtml)).Click(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickRecordTitleKanbanInCol(int timeoutInSeconds, string colName, string titleRecord)
        {
            var iweb = Map.recordTitleKanbanInCol(timeoutInSeconds, colName, titleRecord);

            // Mouse hover and click
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(iweb).Build().Perform();

            Map.HighlightElement(Map.recordTitleKanbanInCol(timeoutInSeconds, colName, titleRecord)).Click();
            return this;
        }
        public GeneralAction CLickButtonRecordTitleKanbanInCol(int timeoutInSeconds, string colName, string titleRecord, string? tagHtml = null, string? attrValue = null)
        {
            // Check tagName/attrValue with a specific tagName/attrValue, if no then will get tagName by default = button, attrValue = ""
            if (tagHtml == null) tagHtml = "button";
            if (attrValue == null) attrValue = "";

            var iweb = Map.recordButtonInTitleKanbanInCol(timeoutInSeconds, colName, titleRecord, tagHtml, attrValue);

            // Mouse hover and click
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(iweb).Click().Build().Perform();

            //IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            //je.ExecuteScript("arguments[0].click();", Map.HighlightElement(iweb)); Thread.Sleep(500);
            //((IJavaScriptExecutor)Driver.Browser).ExecuteScript("arguments[0].onmouseover;", iweb);

            //// Click
            //Map.HighlightElement(iweb).Click();
            return this;
        }
        public GeneralAction CLickSettingsLeftMenuTitle(int timeoutInSeconds, string title)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.settingsLeftMenuTitle(timeoutInSeconds, title))).Build().Perform(); Thread.Sleep(500);
            return this;
        }

        /// Action for Notebook (Ghi chu)
        public GeneralAction CLickNotebookHeader(int timeoutInSeconds, string header)
        {
            Map.HighlightElement(Map.headerNotebook(timeoutInSeconds, header)).Click(); Thread.Sleep(1000);
            return this;
        }
        public GeneralAction InputNotebookHeaderDescriptionContent(int timeoutInSeconds, string header, string text)
        {
            Map.HighlightElement(Map.notebookHeaderDescriptionContentInput(timeoutInSeconds, header)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.notebookHeaderDescriptionContentInput(timeoutInSeconds, header)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public GeneralAction InputNotebookHeaderAndFieldNameContent(int timeoutInSeconds, string header, string fieldName, string text, string? tagHtml=null)
        {
            // Check tagName with a specific tagName, if no then will get tagName by default = input
            if (tagHtml == null) tagHtml = "input";

            Map.HighlightElement(Map.notebookHeaderAndFieldNameContentInput(timeoutInSeconds, header, fieldName, tagHtml)).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            Map.HighlightElement(Map.notebookHeaderAndFieldNameContentInput(timeoutInSeconds, header, fieldName, tagHtml)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public GeneralAction ClickInputNotebookHeaderAndFieldNameContent(int timeoutInSeconds, string header, string fieldName, string? tagHtml = null)
        {
            // Check tagName with a specific tagName, if no then will get tagName by default = input
            if (tagHtml == null) tagHtml = "input";
            Map.HighlightElement(Map.notebookHeaderAndFieldNameContentInput(timeoutInSeconds, header, fieldName, tagHtml)).Click(); Thread.Sleep(250);
            return this;
        }
        public string DataRecordRowListTableNotebookGetText(int timeoutInSeconds, string fieldValue, string attrStringValue, string? attribute = null, string? attrValue = null)
        {
            // Check attrValue/attribute with a specific attrValue/attribute, if no then will get attribute by default: attrValue=display_name; attribute=name
            if (attribute == null) attribute = "name";
            if (attrValue == null) attrValue = "display_name";
            return Map.HighlightElement(Map.notebookDataTableGetFieldValueWithName(timeoutInSeconds, fieldValue, attrStringValue, attribute, attrValue)).Text;
        }

        /// Action for Chatter (Message right pane)
        public GeneralAction CLickCheckboxRecipientAndFollower(int timeoutInSeconds)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.checkboxRecipientAndFollower(timeoutInSeconds))).Build().Perform(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickCommunicationPortChatterDropdown(int timeoutInSeconds)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.communicationPortChatterDropdown(timeoutInSeconds))).Build().Perform(); Thread.Sleep(500);
            return this;
        }
        public GeneralAction CLickCommunicationPortChatterItemInDropdown(int timeoutInSeconds, string itemDropdown)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.communicationPortChatterItemInDropdown(timeoutInSeconds, itemDropdown))).Build().Perform(); Thread.Sleep(500);
            return this;
        }

        #endregion

        #region Built-in Actions
        public GeneralAction GoToLeftMenu(int timeoutInSeconds, string title, string? breadcrumbTitle = null, string? renderersLoadDone = null)
        {
            // Go to 'lien he' - 'Khach hang ca nhan' page
            CLickMenuTitle(10, General.homeMenu);
            WaitForElementVisible(10, General.dropDownShow).Sleep(150);
            ScrollIntoView(Map.mainMenuTitle(timeoutInSeconds, title));
            CLickMainMenuTitle(10, title).Sleep(1000).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
            if (renderersLoadDone == null) { WaitForElementVisible(10, General.allRenderers).Sleep(1000); }
            else { WaitForElementVisible(10, General.rendererTable(renderersLoadDone)).Sleep(1000); }
            CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
            // Click left menu until breadcrumb title is displayed
            if (breadcrumbTitle == null) { breadcrumbTitle = null; }
            else
            {
                int time = 0;
                while (IsElementPresent(General.breadcrumbItem(breadcrumbTitle))==false && time < timeoutInSeconds)
                {
                    CLickMenuTitle(10, General.homeMenu);
                    WaitForElementVisible(10, General.dropDownShow).Sleep(150);
                    ScrollIntoView(Map.mainMenuTitle(timeoutInSeconds, title));
                    CLickMainMenuTitle(10, title).Sleep(150);
                    if (renderersLoadDone == null) { WaitForElementVisible(10, General.allRenderers).Sleep(1000); }
                    else { WaitForElementVisible(10, General.rendererTable(renderersLoadDone)).Sleep(1000); }
                    CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);
                    if (IsElementPresent(General.breadcrumbItem(breadcrumbTitle))==true) { break; }
                    if (time == timeoutInSeconds) { Console.WriteLine("Click left menu '" + title + "' timeout in '" + timeoutInSeconds + "'!"); break;}
                    time++;
                    Thread.Sleep(1000);
                }
            }
            return this;
        }

        public GeneralAction SearchViewInput(int timeoutInSeconds, string fieldName, string inputSearch)
        {
            // Variables declare
            string searchboxRoleAttr = "searchbox";

            // Input a value in seachbox to Search
            InputByAttributeValue(timeoutInSeconds, searchboxRoleAttr, inputSearch);
            WaitForElementVisible(timeoutInSeconds, General.searchBoxDropdown); // wait for sb dropdown displays
            CLickItemInDropdownSearchBox(timeoutInSeconds, fieldName, inputSearch); // search data with filter 'Cơ hội'
            WaitForElementInvisible(timeoutInSeconds, General.searchBoxDropdown).Sleep(1000);
            CheckIfSpinnerLoadingIconShownThenWaitLoadDone(timeoutInSeconds).Sleep(1000);
            return this;
        }

        public GeneralAction ThucHienXoaDelete(int timeoutInSeconds)
        {
            CLickButtonTitle(timeoutInSeconds, General.thucHien).WaitForElementVisible(timeoutInSeconds, General.dropDownShow);
            CLickItemInDropdown(timeoutInSeconds, "Xoá").WaitForElementVisible(timeoutInSeconds, General.DialogShow);
            if (IsElementPresent(General.itemDropdown("Đồng ý"))==true)
            {
                CLickButtonTitle(timeoutInSeconds, "Đồng ý").WaitForElementInvisible(timeoutInSeconds, General.DialogShow);
            }
            if (IsElementPresent(General.itemDropdown("Áp dụng")) == true)
            {
                CLickButtonTitle(timeoutInSeconds, "Áp dụng").WaitForElementInvisible(timeoutInSeconds, General.DialogShow);
            }
            //CLickButtonTitle(10, "Đồng ý").WaitForElementInvisible(10, General.DialogShow);
            return this;
        }

        public GeneralAction CLickAndSelectItemInDropdownLabel(int timeoutInSeconds, string label, string item)
        {
            CLickDropdownLabel(timeoutInSeconds, label); Thread.Sleep(250);
            CLickItemInDropdown(timeoutInSeconds, item); Thread.Sleep(250);
            return this;
        }

        public GeneralAction ClickToCheckboxLabel(int timeoutInSeconds, string label, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            bool isChecked = Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).Selected;
            if (isChecked == false) 
            {
                Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).Click();
            }
            return this;
        }
        public GeneralAction ClickToUncheckboxLabel(int timeoutInSeconds, string label, string? placeHolderValue = null, string? attribute = null)
        {
            // Check attribute with a specific attribute, if no then will get attribute by default = placeholder
            if (attribute == null) attribute = "placeholder";

            bool isChecked = Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).Selected;
            if (isChecked == true)
            {
                Map.HighlightElement(Map.fieldTitleInput(timeoutInSeconds, label, placeHolderValue, attribute)).Click();
            }
            return this;
        }

        public GeneralAction ClickToCheckboxInRowTable(int timeoutInSeconds, string rowNmuber)
        {
            bool isChecked = Driver.Browser.FindElement(By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + rowNmuber + "]//input/ancestor::tr")).GetAttribute("class").Contains("selected");
            if (isChecked)
            {
                return this;
            }
            else { ClickCheckboxInRowTable(timeoutInSeconds, rowNmuber); Thread.Sleep(250); }
            return this;
        }
        public GeneralAction ClickToUncheckboxInRowTable(int timeoutInSeconds, string rowNmuber)
        {
            bool isChecked = Driver.Browser.FindElement(By.XPath(@"//tbody[contains(@class,'ui-sortable')]/tr[" + rowNmuber + "]//input/ancestor::tr")).GetAttribute("class").Contains("selected");
            if (isChecked)
            {
                ClickCheckboxInRowTable(timeoutInSeconds, rowNmuber); Thread.Sleep(250);
            }
            return this;
        }

        public GeneralAction SwitchUser(int timeoutInSeconds, string email, string username)
        {
            // Check if the 'switch user' icon reverts back to the original user if so click on it
            if (IsElementPresent(General.switchUserIconStatusOrg) == true)
            {
                SwitchUserAction(timeoutInSeconds, email, username);
            }

            // Check if the 'switch user' icon has already changed then click on it to back to the original user
            else
            {
                CLickMenuTitle(timeoutInSeconds, General.switchUser);
                // Check if the spinner Loading icon is shown then wait for it to load done
                if (IsElementPresent(General.spinnerLoading))
                {
                    WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }
                WaitForElementVisible(10, General.switchUserIconStatusOrg);
                WaitForElementVisible(10, General.allRenderers); Thread.Sleep(1000);

                // Click 'Switch User' button again to switch user
                SwitchUserAction(timeoutInSeconds, email, username);
            }
            return this;
        }
        private GeneralAction SwitchUserAction(int timeoutInSeconds, string email, string username)
        {
            CLickMenuTitle(timeoutInSeconds, General.switchUser);
            // Check if the spinner Loading icon is shown then wait for it to load done
            if (IsElementPresent(General.spinnerLoading))
            {
                WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
            }
            WaitForElementVisible(timeoutInSeconds, General.DialogShow);
            InputFieldLabelDialog(timeoutInSeconds, General.userQuestionMark, email);
            WaitForElementVisible(timeoutInSeconds, By.XPath("//*[@name='user_id']//ul")); Thread.Sleep(1000);
            CLickItemInSubDropdown(timeoutInSeconds, "user_id", username); Thread.Sleep(1000);
            CLickButtonTitleInDialog(timeoutInSeconds, General.switchUser.ToLower());
            WaitForElementInvisible(timeoutInSeconds, General.DialogShow);
            WaitForElementVisible(10, General.allRenderers); Thread.Sleep(1000);
            return this;
        }
        public GeneralAction SwitchUserBack(int timeoutInSeconds)
        {
            CLickMenuTitle(timeoutInSeconds, General.switchUser);
            WaitForElementVisible(10, General.switchUserIconStatusOrg);
            WaitForElementVisible(10, General.allRenderers); Thread.Sleep(1000);
            // Check if the spinner Loading icon is shown then wait for it to load done
            if (IsElementPresent(General.spinnerLoading))
            {
                WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
            }
            return this;
        }        
        #endregion
    }
}
