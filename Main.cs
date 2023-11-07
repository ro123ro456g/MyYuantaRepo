using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; //使用Marsha等類別操作所需的namespace
using System.Threading;
using System.Linq;
using System.Reflection;
using YuantaOneAPI;
using System.Diagnostics;
using System.IO;

namespace YuantaOneAPI_TestAP
{
    public partial class Main : Form
    {
        YuantaOneAPITrader objYuantaOneAPI = new YuantaOneAPITrader();
        enumEnvironmentMode enumEvenMode = enumEnvironmentMode.UAT;
        enumLangType enumLng = enumLangType.NORMAL;
        
        string strLoginAccount = "";

        #region Form元件顯示相關

        /// <summary>
        /// Form初始化
        /// </summary>
        public Main()
        {
            InitializeComponent();
            cboTimeInforce.SelectedIndex = 0;
            cboEnvironment.SelectedIndex = 0;


            // 報價表(指定欄位) 市場代碼下拉選單
            cboWatchlistMarketNo.DataSource = Enum.GetValues(typeof(enumMarketType))
                .Cast<enumMarketType>()
                .Select(v => new ComboEnumItem(v))
                .ToList();

            cboWatchlistMarketNo.DisplayMember = "Text";
            cboWatchlistMarketNo.ValueMember = "Value";
            cboWatchlistMarketNo.SelectedIndex = 0;

            //報價表 市場代碼下拉選單
            cboWatchlistallMarketNo.DataSource = Enum.GetValues(typeof(enumMarketType))
                .Cast<enumMarketType>()
                .Select(v => new ComboEnumItem(v))
                .ToList();

            cboWatchlistallMarketNo.DisplayMember = "Text";
            cboWatchlistallMarketNo.ValueMember = "Value";
            cboWatchlistallMarketNo.SelectedIndex = 0;

            //分時明細 市場代碼下拉選單
            cboStocktickMarketNo.DataSource = Enum.GetValues(typeof(enumMarketType))
            .Cast<enumMarketType>()
            .Select(v => new ComboEnumItem(v))
            .ToList();

            cboStocktickMarketNo.DisplayMember = "Text";
            cboStocktickMarketNo.ValueMember = "Value";
            cboStocktickMarketNo.SelectedIndex = 0;

            //五檔 市場代碼下拉選單
            cboFivetickMarketNo.DataSource = Enum.GetValues(typeof(enumMarketType))
            .Cast<enumMarketType>()
            .Select(v => new ComboEnumItem(v))
            .ToList();
            cboFivetickMarketNo.DisplayMember = "Text";
            cboFivetickMarketNo.ValueMember = "Value";
            cboFivetickMarketNo.SelectedIndex = 0;

            //海外期貨下單 市場別下拉選單
            cboOFMarketNo.DataSource = Enum.GetValues(typeof(enumMarketType))
            .Cast<enumMarketType>()
            .Where(v => v > enumMarketType.TWEMERGING)
            .Select(v => new ComboEnumItem(v))
            .ToList();

            cboOFMarketNo.SelectedIndex = 0;
            cboOFMarketNo.DisplayMember = "Text";
            cboOFMarketNo.ValueMember = "Value";

            this.Text = $"{this.Text} {FileVersionInfo.GetVersionInfo($"{Directory.GetCurrentDirectory()}\\YuantaOneAPI.dll").FileVersion.ToString()}]";
        }

        /// <summary>
        /// 載入Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtTradeDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            this.txtOVFutTradeDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            this.txtFutTradeDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            this.txtSettlementMonth1.Text = DateTime.Now.ToString("yyyyMM");
            this.txtOVFutSettlementMonth.Text = DateTime.Now.ToString("yyyyMM");
            this.txtWatchlist.AppendText("\r\n");
            this.txtWatchListAll.AppendText("\r\n");
            this.txtStocktick.AppendText("\r\n");
            this.txtFiveTick.AppendText("\r\n");

            //設定回應事件
            objYuantaOneAPI.OnResponse += new OnResponseEventHandler(objApi_OnResponse);

            //設定記錄Log的方式
            objYuantaOneAPI.SetLogType(enumLogType.COMMON);

            //設定強制更版/無法取得設定檔/無法取得主機資訊是否跳出彈跳視窗
            objYuantaOneAPI.SetPopUpMsg(true) ;
        }

        /// <summary>
        /// 報價表(指定欄位) 商品資訊新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWathlistAdd_Click(object sender, EventArgs e)
        {
            txtWatchlist.AppendText(String.Format("{0},{1}\r\n", cboWatchlistMarketNo.SelectedValue.ToString(), txtWatchlistStkCode.Text));
        }

        /// <summary>
        /// 報價表 商品資訊新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWathlistallAdd_Click(object sender, EventArgs e)
        {
            txtWatchListAll.AppendText(String.Format("{0},{1}\r\n", cboWatchlistallMarketNo.SelectedValue.ToString(), txtWatchlistallStkCode.Text));
        }

        /// <summary>
        /// 分時明細 商品資訊新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStocktickAdd_Click(object sender, EventArgs e)
        {
            txtStocktick.AppendText(String.Format("{0},{1}\r\n", cboStocktickMarketNo.SelectedValue.ToString(), txtStocktickStkCode.Text));
        }

        /// <summary>
        /// 五檔 商品資訊新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFivetickAdd_Click(object sender, EventArgs e)
        {
            txtFiveTick.AppendText(String.Format("{0},{1}\r\n", cboFivetickMarketNo.SelectedValue.ToString(), txtFivetickStkCode.Text));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            objYuantaOneAPI.Dispose();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            cboAccountList.BeginInvoke(new Action(() =>
            {
                objYuantaOneAPI.LogOut();
                this.cboAccountList.Items.Clear();
                this.cboAccountList.SelectedIndex = -1;
                this.cboAccountList.Text = "";
            }));

            txtStkAccount.BeginInvoke(new Action(() =>
            {
                txtStkAccount.Text = "";
                txtFutAccount.Text = "";
                txtOVFutAccount.Text = "";
            }));
        }

        /// <summary>
        /// 切換帳號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cobAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStkAccount.BeginInvoke(new Action(() =>
            {
                txtStkAccount.Text = this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "";
                txtOVFutAccount.Text = this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "";
                txtFutAccount.Text = this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "";
            }));
        }

        /// <summary>
        /// 關閉連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            btnLogout_Click(null, null);
            objYuantaOneAPI.Close();
            objYuantaOneAPI.Dispose();
            //Close();
        }

        /// <summary>
        /// 開啟連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            switch (cboEnvironment.SelectedIndex)
            {
                case 0: //測試
                    enumEvenMode = enumEnvironmentMode.UAT;
                    break;
                case 1: //正式
                    enumEvenMode = enumEnvironmentMode.PROD;
                    break;
            }
            objYuantaOneAPI.Open(enumEvenMode);
        }


        #endregion

        #region 訂閱及查詢回應事件

        /// <summary>
        /// API回應
        /// </summary>
        /// <param name="intMark">回應資料的種類 0:系統資訊回應 1:查詢資訊回應 2:訂閱資訊回應</param>
        /// <param name="dwIndex">回應資料的狀態，要搭配intMark看</param>
        /// <param name="strIndex">字串型別的功能代碼(EX:20.100.10.10)，若strIndex為空值，代表功能查詢/訂閲有錯誤，請直接用string格式解析objValue</param>
        /// <param name="objHandle">回傳訂閱事件時所傳入的Handle值(不處理)</param>
        /// <param name="objValue">回傳的byte[]資料，使用datasetter解析</param>
        void objApi_OnResponse(int intMark, uint dwIndex, string strIndex, object objHandle, object objValue)
        {
            string strResult = "";
            try
            {
                switch (intMark)
                {
                    case 0: //系統回應
                        strResult = Convert.ToString(objValue);
                        break;
                    case 1: //代表為RQ/RP 所回應的
                        switch (strIndex)
                        {
                            case "Login":       //一般/子帳登入
                                strResult = funAPILogin_Out((byte[])objValue);
                                break;
                            case "GetQuoteList":
                                strResult = funGetQuoteList_Out((byte[])objValue);
                                break;
                            case "GetQuoteListDetail":
                                strResult = funGetQuoteListDetail_Out((byte[])objValue);
                                break;
                            case "10.0.0.16":   //逐筆即時回報彙總
                                strResult = funGetRealReportMerge_Out((byte[])objValue);
                                break;
                            case "10.0.0.20":   //逐筆即時回報
                                strResult = funGetRealReport_Out((byte[])objValue);
                                break;
                            case "20.101.0.18": //委託成交綜合回報-盤中零股
                                strResult = funRptOrderTradeReportGroup_odd_Out((byte[])objValue);
                                break;
                            case "20.103.0.22": //庫存綜合總表--盤中零股
                                strResult = funStoStoreSummaryGroup_odd_Out((byte[])objValue);
                                break;
                            case "30.100.10.31"://現貨下單
                                strResult = funStkOrder_Out((byte[])objValue);
                                break;
                            case "30.100.20.24"://期貨下單(新)
                                strResult = funFutOrder_Out((byte[])objValue);
                                break;
                            case "30.100.40.12"://國際期貨下單(新)
                                strResult = funOVFutOrder_Out((byte[])objValue);
                                break;
                            case "50.0.0.16":   //報價表查詢
                                strResult = funWatchListAll_Out((byte[])objValue);
                                break;
                            default:           //不在表列中的直接呈現訊息
                                {
                                    if (strIndex == "")
                                        strResult = Convert.ToString(objValue);
                                    else
                                        strResult = String.Format("{0},{1}", strIndex, objValue);
                                }
                                break;
                        }
                        break;
                    case 2 : //訂閱所回應
                        switch (strIndex)
                        {
                            case "200.10.10.26":    //逐筆即時回報
                                strResult = funRealReport_Out((byte[])objValue);
                                break;
                            case "200.10.10.27":    //逐筆即時回報彙總
                                strResult = funRealReportMerge_Out((byte[])objValue);
                                break;
                            case "210.10.70.11":    //Watchlist報價表(指定欄位)
                                strResult = funRealWatchlist_Out((byte[])objValue);
                                break;
                            case "98.10.70.10":     //Watchlist報價表
                                strResult = funRealWatchlistAll_Out((byte[])objValue);
                                break;
                            case "210.10.40.10":    //訂閱個股分時明細
                                strResult = funRealStocktick_Out((byte[])objValue);
                                break;
                            case "210.10.60.10":    //訂閱五檔報價
                                strResult = funRealFivetick_Out((byte[])objValue);
                                break;
                            default:
                                {
                                    if (strIndex == "")
                                        strResult = Convert.ToString(objValue);
                                    else
                                        strResult = String.Format("{0},{1}", strIndex, objValue);
                                }
                                break;
                        }
                        break;
                    default :
                        strResult = Convert.ToString(objValue) ;
                        break;
                }
                
                if (strResult != "")
                {
                    txtResult.BeginInvoke(new Action(() => 
                    { 
                        txtResult.AppendText(strResult + "\r\n----------------------" + Environment.NewLine); 
                    }));
                }
                else
                {
                    txtResult.BeginInvoke(new Action(() =>
                    {
                        txtResult.AppendText(Convert.ToString(objValue) + "\r\n---------------------- \r\n" + Environment.NewLine);
                    }));
                }

                txtResult.BeginInvoke(new Action(() =>
                {
                    this.txtResult.Select(this.txtResult.Text.Length, 0);
                }));
               
            }
            catch(Exception exc)
            {
                txtResult.BeginInvoke(new Action(() =>
                {
                    this.txtResult.Text += "Error!! \r\n" + exc.Message;
                }));
            }
        }
        
        #endregion

        #region 即時回報相關
        /// <summary>
        /// 即時回報(查詢結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funGetRealReport_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RealReport.ChildStruct_Out struChildOut = new RealReport.ChildStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                int nRowCount = 0;

                nRowCount = (int)dataGetter.GetUInt();

                strResult += "===============\r\n即時回報(查詢結果) 筆數:" + nRowCount.ToString() + "\r\n===============\r\n";

                for (int i = 0; i < nRowCount; i++)
                {
                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAccount)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyCompanyNo)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkName)) + ",";

                    TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                    strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                    TYuantaTime yuantaTime = dataGetter.GetTYuantaTime();
                    strResult += String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderType)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBS)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyPrice)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTouchPrice)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOpenOffsetKind)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyDayTrade)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderCond)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderErrorNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBasketNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBelongStkCode)) + ",";

                    strResult += dataGetter.GetUInt().ToString() + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyPriceType)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkErrCode)) + ",";

                    //----------
                    strResult += "\r\n";
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }
        /// <summary>
        /// 即時回報彙總(查詢結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funGetRealReportMerge_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RealReportMerge.ChildStruct_Out struChildOut = new RealReportMerge.ChildStruct_Out();
                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);

                //依元件規格文件依序解出內容值
                dataGetter.OutMsgLoad(abyData);

                int nRowCount = 0;

                nRowCount = (int)dataGetter.GetUInt();

                strResult += "===============\r\n即時回報彙總(查詢結果) 筆數:" + nRowCount.ToString() + "\r\n===============\r\n";

                for (int i = 0; i < nRowCount; i++)
                {
                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAccount)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyCompanyNo)) + ",";

                    TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                    strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                    TYuantaTime yuantaTime = dataGetter.GetTYuantaTime();
                    strResult += String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderType)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBS)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderPrice)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTouchPrice)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyLastDealPrice)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAvgDealPrice)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOpenOffsetKind)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyDayTrade)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderCond)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderErrorNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetShort()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkCName)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTradeCode)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBasketNo)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBelongStkCode)) + ",";

                    strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkOrderErrorNo));

                    //----------
                    strResult += "\r\n";
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }
       
        /// <summary>
        /// 即時回報彙總(訂閱結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealReportMerge_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_RealReportMerge.ParentStruct_Out struParentOut = new RR_RealReportMerge.ParentStruct_Out();
                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                strResult += "===============\r\n即時回報彙總(訂閱結果)\r\n===============\r\n";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyAccount)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyCompanyNo)) + ",";

                TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                TYuantaTime yuantaTime = dataGetter.GetTYuantaTime();
                strResult += String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderType)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBS)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderPrice)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyTouchPrice)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyLastDealPrice)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyAvgDealPrice)) + ",";

                strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOpenOffsetKind)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyDayTrade)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderCond)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderErrorNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetShort()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyCompanyName)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyTradeCode)) + ",";

                strResult += String.Format("{0}", dataGetter.GetUInt()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBasketNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBelongStkCode)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyPriceType)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkErrCode));

                //----------
                strResult += "\r\n";

            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }
        /// <summary>
        /// 即時回報(訂閱結果）
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealReport_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_RealReport.ParentStruct_Out struParentOut = new RR_RealReport.ParentStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                strResult += "===============\r\n即時回報(訂閱結果)\r\n===============\r\n";

                strResult += "帳號:" + dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyAccount)) + ",";

                strResult += "回報類別:" + String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += "委託單號" + dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyCompanyNo)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkName)) + ",";

                TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                TYuantaTime yuantaTime = dataGetter.GetTYuantaTime();
                strResult += String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderType)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBS)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyPrice)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyTouchPrice)) + ",";

                strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOpenOffsetKind)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyDayTrade)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderCond)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyOrderErrorNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBasketNo)) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += String.Format("{0}", dataGetter.GetByte()) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyBelongStkCode)) + ",";

                strResult += dataGetter.GetUInt().ToString() + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyPriceType)) + ",";

                strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkErrCode)) + ",";
                //----------
                strResult += "\r\n";
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 即時回報(發送查詢)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRealReport_Click(object sender, EventArgs e)
        {

            RealReport.ParentStruct_In struParentIn = new RealReport.ParentStruct_In();
            RealReport.ChildStruct_In struChildIn = new RealReport.ChildStruct_In();

            cboAccountList.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < this.cboAccountList.Items.Count; i++)
                {
                    YuantaDataHelper dataSetter = new YuantaDataHelper(enumLng);
                    dataSetter.SetFunctionID(10, 0, 0, 20);

                    dataSetter.SetUInt(1);
                    dataSetter.SetTByte(this.cboAccountList.Items[i] != null ? this.cboAccountList.Items[i].ToString().Trim() : "", Marshal.SizeOf(struChildIn.abyAccount)); //填入查詢帳號
                    objYuantaOneAPI.RQ(this.cboAccountList.Items[i] != null ? this.cboAccountList.Items[i].ToString().Trim() : "", dataSetter);
                }
            }));
        }

        /// <summary>
        /// 即時回報彙總(發送查詢)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRealReportMerge_Click(object sender, EventArgs e)
        {

            RealReportMerge.ParentStruct_In struParentIn = new RealReportMerge.ParentStruct_In();
            RealReportMerge.ChildStruct_In struChildIn = new RealReportMerge.ChildStruct_In();

            cboAccountList.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < this.cboAccountList.Items.Count; i++)
                {
                    YuantaDataHelper dataSetter = new YuantaDataHelper(enumLng);
                    dataSetter.SetFunctionID(10, 0, 0, 16);

                    dataSetter.SetByte(Convert.ToByte(0));
                    dataSetter.SetByte(Convert.ToByte(0));
                    dataSetter.SetTByte("", Marshal.SizeOf(struParentIn.abyCompanyNo));
                    dataSetter.SetUInt(1);
                    dataSetter.SetTByte(this.cboAccountList.Items[i] != null ? this.cboAccountList.Items[i].ToString().Trim() : "", Marshal.SizeOf(struChildIn.abyAccount)); //填入查詢帳號

                    objYuantaOneAPI.RQ(this.cboAccountList.Items[i] != null ? this.cboAccountList.Items[i].ToString().Trim() : "", dataSetter);
                }
            }));

        }

        #endregion

        #region 登入

        /// <summary>
        /// 帳號登入
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funAPILogin_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                SgnAPILogin.ParentStruct_Out struParentOut = new SgnAPILogin.ParentStruct_Out();
                SgnAPILogin.ChildStruct_Out struChildOut = new SgnAPILogin.ChildStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);
                
                {
                    string strMsgCode = "";
                    string strMsgContent = "";
                    int intCount = 0;

                    strMsgCode = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyMsgCode));
                    strMsgContent = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyMsgContent));
                    intCount = (int)dataGetter.GetUInt();

                    strResult += FilterBreakChar(strMsgCode) + "," + FilterBreakChar(strMsgContent) + "\r\n";
                    if (strMsgCode == "0001" || strMsgCode == "00001")
                    {
                        strResult += "帳號筆數: " + intCount.ToString() + "\r\n";
                        for (int i = 0; i < intCount; i++)
                        {
                            string strAccount = "", strSubAccount = "", strID = "";
                            short shtSellNo = 0;
                            
                            strAccount = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAccount));
                            strSubAccount = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abySubAccName));
                            strID = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyInvesotrID));
                            
                            strResult += FilterBreakChar(strAccount) + ",";
                            strResult += FilterBreakChar(strSubAccount) + ",";
                            strResult += FilterBreakChar(strID) + ",";
                            
                            shtSellNo = dataGetter.GetShort();

                            strResult += shtSellNo.ToString() + ",";

                            strResult += "\r\n";
                        }

                        cboAccountList.BeginInvoke(new Action(() =>
                        {
                            if (this.cboAccountList.Items.IndexOf((object)strLoginAccount) < 0)
                            {
                                this.cboAccountList.Items.Add((object)strLoginAccount);
                                this.cboAccountList.SelectedItem = this.cboAccountList.Items[this.cboAccountList.Items.Count - 1];
                            }
                        }));
                    }
                    else
                    {
                        strLoginAccount = "";
                    }
                }
            }
            catch (Exception exc)
            {
                strResult = exc.Message;
            }
            return strResult;
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAPILogin_Click(object sender, EventArgs e)
        {

             bool LoginCheck = objYuantaOneAPI.Login(String.Format("{0}{1}", this.txtAccountType.Text.Trim(), this.txtAccount.Text.Trim()).Trim(), this.txtPassword.Text.Trim());
             strLoginAccount = FillSpace(this.txtAccountType.Text.Trim() + this.txtAccount.Text.Trim(), 22).Trim();

        }

        #endregion

        #region 下單

        /// <summary>
        /// 國內期貨下單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFutOrder_Click(object sender, EventArgs e)
        {
            
            List<FutureOrder> lstFutureOrder = new List<FutureOrder>();
            FutureOrder futureOrder = new FutureOrder();

            futureOrder.Identify = Convert.ToInt32(this.txtFutIdentify.Text.Trim());                                //識別碼
            futureOrder.Account =this.txtFutAccount.Text.Trim();                                                    //期貨帳號
            futureOrder.FunctionCode = Convert.ToInt16(this.txtFunctionCode.Text.Trim());                           //功能別                                  
            futureOrder.CommodityID1 = this.txtCommodityID1.Text.Trim();                                            //商品名稱1
            futureOrder.CallPut1 = this.txtCallPut1.Text.Trim();                                                    //買賣權1
            futureOrder.SettlementMonth1 = Convert.ToInt32(this.txtSettlementMonth1.Text.Trim());                   //商品年月1
            futureOrder.StrikePrice1 = Convert.ToInt32(Convert.ToDecimal(this.txtStrikePrice1.Text.Trim()) * 1000); //屐約價1
            futureOrder.Price = Convert.ToInt32(Convert.ToDecimal(this.txtOrderPrice1.Text.Trim()) * 10000);        //委託價格
            futureOrder.OrderQty1 = Convert.ToInt16(this.txtOrderQty1.Text.Trim());                                 //委託口數1
            futureOrder.BuySell1 = this.txtBuySell1.Text.Trim();                                                    //買賣別
            futureOrder.CommodityID2 = this.txtCommodityID2.Text.Trim();                                            //商品名稱2
            futureOrder.CallPut2 = this.txtCallPut2.Text.Trim();                                                    //買賣權2
            futureOrder.SettlementMonth2 = Convert.ToInt32(this.txtSettlementMonth2.Text.Trim());                   //商品年月2
            futureOrder.StrikePrice2 = Convert.ToInt32(Convert.ToDecimal(this.txtStrikePrice2.Text.Trim()) * 1000); //屐約價2                 
            futureOrder.OrderQty2 = Convert.ToInt16(this.txtOrderQty2.Text.Trim());                                 //委託口數2
            futureOrder.BuySell2 =  this.txtBuySell2.Text.Trim();                                                   //買賣別2
            futureOrder.OpenOffsetKind = this.txtOpenOffsetKind.Text.Trim();                                        //新平倉碼                                              
            futureOrder.DayTradeID = this.txtDayTradeID.Text;                                                       //當沖註記
            futureOrder.OrderType = this.txtFutOrderType.Text.Trim();                                               //委託方式    
            futureOrder.OrderCond = this.txtOrderCond.Text;                                                         //委託條件                                           
            futureOrder.SellerNo = Convert.ToInt16(this.txtFutSellerNo.Text.Trim());                                //營業員代碼                                            
            futureOrder.OrderNo =  this.txtFutOrderNo.Text.Trim();                                                  //委託書編號           
            futureOrder.TradeDate = this.txtFutTradeDate.Text;                                                      //交易日期                            
            futureOrder.BasketNo = this.txtFutBasketNo.Text.Trim();                                                 //BasketNo
            futureOrder.Session =  this.txtSession.Text.Trim();                                                     //通路種類                                    

            lstFutureOrder.Add(futureOrder);

            bool bResult = objYuantaOneAPI.SendFutureOrder(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", lstFutureOrder);
        }

        /// <summary>
        /// 現貨下單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStkOrder_Click(object sender, EventArgs e)
        {
            #region 用StockOrder的方式

            List<StockOrder> lstStockOrder = new List<StockOrder>();
            {
                StockOrder stockorder = new StockOrder();
                stockorder.Identify = Convert.ToInt32(this.txtIdentify.Text.Trim());
                stockorder.Account = this.txtStkAccount.Text.Trim();
                stockorder.APCode = Convert.ToInt16(this.txtAPCode.Text.Trim());
                stockorder.TradeKind = Convert.ToInt16(this.txtTradeKind.Text.Trim());
                stockorder.OrderType = this.txtOrderType.Text.Trim();
                stockorder.StkCode = this.txtStkCode.Text.Trim();
                stockorder.PriceFlag = this.txtPriceFlag.Text.Trim();
                stockorder.Price = Convert.ToInt64(Convert.ToSingle(this.txtPrice.Text.Trim()) * 10000);
                stockorder.OrderQty = Convert.ToInt64(this.txtOrderQty.Text.Trim());

                stockorder.BuySell = this.txtBuySell.Text.Trim();
                stockorder.SellerNo = Convert.ToInt16(this.txtSellerNo.Text.Trim());
                stockorder.OrderNo = this.txtOrderNo.Text.Trim();
                stockorder.TradeDate = txtTradeDate.Text.Trim();
                stockorder.BasketNo = this.txtBasketNo.Text.Trim();

                string strTimeInforce = "0";
                switch (this.cboTimeInforce.SelectedIndex)
                {
                    case 0: //ROD
                        strTimeInforce = "0";
                        break;
                    case 1: //IOC
                        strTimeInforce = "3";
                        break;
                    case 2: //FOK
                        strTimeInforce = "4";
                        break;
                    default:
                        strTimeInforce = "0";
                        break;
                }
                stockorder.Time_in_force = strTimeInforce;
                lstStockOrder.Add(stockorder);

            }
            
            //RQRP
            bool bResult = objYuantaOneAPI.SendStockOrder(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", lstStockOrder);
            #endregion
        }

        /// <summary>
        /// 國外期貨下單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOVFutOrder_Click(object sender, EventArgs e)
        {

            #region 用OVFutOrder的方式
            List<OVFutureOrder> lstOVFutureOrder = new List<OVFutureOrder>();
            {
                OVFutureOrder ovFutOrder = new OVFutureOrder();
                ovFutOrder.Identify = Convert.ToInt32(this.txtOVFutIdentify.Text.Trim());
                ovFutOrder.Account = this.txtOVFutAccount.Text.Trim();
                ovFutOrder.FunctionCode = Convert.ToInt16(this.txtOVFutFunctionCode.Text.Trim());
                ovFutOrder.ExhCode = txtExhCode.Text.Trim();
                ovFutOrder.MarketNo = Convert.ToByte(txtOFMarketNo.Text.Trim());
                ovFutOrder.CommodityID = txtOVFutCommodityID.Text.Trim();
                ovFutOrder.SettlementMonth = Convert.ToInt32(txtOVFutSettlementMonth.Text.Trim());
                ovFutOrder.StrikePrice = Convert.ToInt32(Convert.ToSingle(this.txtOVFutStrikePrice.Text.Trim()) * 10000);
                ovFutOrder.UtPrice = Convert.ToInt64(Convert.ToSingle(this.txtOVFutUtPrice.Text.Trim()) * 10000);
                ovFutOrder.BuySell = txtOVFutBS.Text.Trim();
                ovFutOrder.UtPrice2 = Convert.ToInt32(Convert.ToSingle(this.txtOVFutUtPrice2.Text.Trim()) * 10000);
                ovFutOrder.MinPrice2 = Convert.ToInt32(this.txtOVFutMinPrice2.Text.Trim());
                ovFutOrder.UtPrice4 = Convert.ToInt64(Convert.ToSingle(this.txtOVFutUtPrice4.Text.Trim()) * 10000);
                ovFutOrder.UtPrice5 = Convert.ToInt32(Convert.ToSingle(this.txtOVFutUtPrice5.Text.Trim()) * 10000);
                ovFutOrder.UtPrice6 = Convert.ToInt32(this.txtOVFutUtPrice6.Text.Trim());
                ovFutOrder.OrderQty = Convert.ToInt16(this.txtOVFutQty.Text.Trim());
                ovFutOrder.BuySell = this.txtOVFutBS.Text.Trim();
                ovFutOrder.Dtover = this.txtOVFutDayTrade.Text.Trim();
                ovFutOrder.OrderType = this.cboOVFutOrderType.Text.Trim();
                ovFutOrder.OrderNo = this.txtOVFutOrderNo.Text.Trim();
                ovFutOrder.TradeDate = txtOVFutTradeDate.Text.Trim();
                lstOVFutureOrder.Add(ovFutOrder);
            }
            bool bResult = objYuantaOneAPI.SendOVFutureOrder(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", lstOVFutureOrder);
            
            #endregion
        }

        /// <summary>
        /// 期貨下單 回應
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funFutOrder_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                OdrFutOrder.ParentStruct_Out struParentOut = new OdrFutOrder.ParentStruct_Out();
                OdrFutOrder.ChildStruct_Out struChildOut = new OdrFutOrder.ChildStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);
                {

                    strResult += "期貨下單結果: \r\n";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnCode)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnMessage)) + ",";

                    int intCount = 0;

                    intCount = (int)dataGetter.GetUInt();

                    strResult += "筆數:" + intCount.ToString() + "\r\n";

                    for (int i = 0; i < intCount; i++)
                    {
                        strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                        strResult += String.Format("{0}", dataGetter.GetShort()) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNO)) + ",";

                        TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrKind)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrNO)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAdvisory)) + ",";

                        strResult += "\r\n";
                        //
                    }
                }
                dataGetter.ClearOutputData();
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 現貨下單 回應
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funStkOrder_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                OdrStkSOrder.ParentStruct_Out struParentOut = new OdrStkSOrder.ParentStruct_Out();
                OdrStkSOrder.ChildStruct_Out struChildOut = new OdrStkSOrder.ChildStruct_Out();


                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);
                {
                    strResult += "現貨下單結果: \r\n";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnCode)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnMessage)) + ",";

                    int intCount = 0;

                    intCount = (int)dataGetter.GetUInt();

                    strResult += "下單筆數:" + intCount.ToString() + "\r\n";

                    for (int i = 0; i < intCount; i++)
                    {
                        strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                        strResult += String.Format("{0}", dataGetter.GetShort()) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNO)) + ",";

                        TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrKind)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrNO)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAdvisory)) + ",";

                        strResult += "\r\n";
                    }
                }

            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 國外期貨下單 回應
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funOVFutOrder_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                OdrOVFutOrder.ParentStruct_Out struParentOut = new OdrOVFutOrder.ParentStruct_Out();
                OdrOVFutOrder.ChildStruct_Out struChildOut = new OdrOVFutOrder.ChildStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);
                {

                    strResult += "國外期貨下單結果: \r\n";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnCode)) + ",";

                    strResult += dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyRtnMessage)) + ",";

                    int intCount = 0;

                    intCount = (int)dataGetter.GetUInt();

                    strResult += "下單筆數:" + intCount.ToString() + "\r\n";

                    for (int i = 0; i < intCount; i++)
                    {
                        strResult += String.Format("{0}", dataGetter.GetInt()) + ",";

                        strResult += String.Format("{0}", dataGetter.GetShort()) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNO)) + ",";

                        TYuantaDate yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrType)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyErrNO)) + ",";

                        strResult += dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAdvisory)) + ",";

                        strResult += "\r\n";
                        //
                    }
                }
                dataGetter.ClearOutputData();
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }


        #endregion

        #region 報價訂閱/查詢相關

        /// <summary>
        /// 取得目前己訂閱的商品清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckQuote_Click(object sender, EventArgs e)
        {
            objYuantaOneAPI.GetQuoteList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckQuoteDetail_Click(object sender, EventArgs e)
        {
            objYuantaOneAPI.GetQuoteListDetail();
        }


        /// <summary>
        /// 訂閱行情資訊報價
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubWatchlist_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtWatchlist.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<Watchlist> lstWatchlist = new List<Watchlist>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    for (int j = 0; j < this.lstIndexValue.CheckedItems.Count; j++)
                    {
                        int intIndexValue = -1;
                        switch (this.lstIndexValue.CheckedItems[j].ToString().Trim())
                        {
                            case "開盤":
                                intIndexValue = 0;
                                break;
                            case "最高":
                                intIndexValue = 1;
                                break;
                            case "最低":
                                intIndexValue = 2;
                                break;
                            case "買價":
                                intIndexValue = 3;
                                break;
                            case "累計外盤量":
                                intIndexValue = 4;
                                break;
                            case "賣價":
                                intIndexValue = 5;
                                break;
                            case "累計內盤量":
                                intIndexValue = 6;
                                break;
                            case "成交價":
                                intIndexValue = 7;
                                break;
                        }

                        Watchlist watch = new Watchlist();
                        watch.IndexFlag = Convert.ToByte(intIndexValue);    //填入訂閱索引值
                        watch.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                        watch.StockCode = strRealStkNo;                     //填入查詢股票代碼
                        lstWatchlist.Add(watch);
                    }
                }
            }

            objYuantaOneAPI.SubscribeWatchlist(lstWatchlist);

            #endregion
        }

        /// <summary>
        /// 取消訂閱watchlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnsubWatchlist_Click(object sender, EventArgs e)
        {
            #region 使用object方式解除訂閱

            string[] strSubStkList = this.txtWatchlist.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<Watchlist> lstWatchlist = new List<Watchlist>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    for (int j = 0; j < this.lstIndexValue.CheckedItems.Count; j++)
                    {
                        int intIndexValue = -1;
                        switch (this.lstIndexValue.CheckedItems[j].ToString().Trim())
                        {
                            case "開盤":
                                intIndexValue = 0;
                                break;
                            case "最高":
                                intIndexValue = 1;
                                break;
                            case "最低":
                                intIndexValue = 2;
                                break;
                            case "買價":
                                intIndexValue = 3;
                                break;
                            case "累計外盤量":
                                intIndexValue = 4;
                                break;
                            case "賣價":
                                intIndexValue = 5;
                                break;
                            case "累計內盤量":
                                intIndexValue = 6;
                                break;
                            case "成交價":
                                intIndexValue = 7;
                                break;
                        }

                        Watchlist watch = new Watchlist();
                        watch.IndexFlag = Convert.ToByte(intIndexValue);    //填入定閱索引值
                        watch.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                        watch.StockCode = strRealStkNo;                     //填入查詢股票代碼
                        lstWatchlist.Add(watch);
                    }
                }
            }

            objYuantaOneAPI.UnsubscribeWatchlist(lstWatchlist);

            #endregion
        }

        /// <summary>
        /// 訂閱報價表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubWatchlistAll_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtWatchListAll.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<WatchlistAll> lstWatchlistAll = new List<WatchlistAll>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    WatchlistAll watch = new WatchlistAll();
                    watch.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    watch.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstWatchlistAll.Add(watch);

                }
            }

            objYuantaOneAPI.SubscribeWatchlistAll(lstWatchlistAll);

            #endregion
        }

        /// <summary>
        /// 取消訂閱報價表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnsubWatchlistAll_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtWatchListAll.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<WatchlistAll> lstWatchlistAll = new List<WatchlistAll>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    WatchlistAll watch = new WatchlistAll();
                    watch.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    watch.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstWatchlistAll.Add(watch);

                }
            }

            objYuantaOneAPI.UnsubscribeWatchlistAll(lstWatchlistAll);

            #endregion
        }

        /// <summary>
        /// 訂閱個股分時明細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubStocktick_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtStocktick.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<StockTick> lstStocktick = new List<StockTick>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    StockTick stocktick = new StockTick();
                    stocktick.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    stocktick.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstStocktick.Add(stocktick);
                }
            }

            objYuantaOneAPI.SubscribeStockTick(lstStocktick);

            #endregion
        }

        /// <summary>
        /// 取消訂閱個股分時明細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnsubStocktick_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtStocktick.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<StockTick> lstStocktick = new List<StockTick>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    StockTick stocktick = new StockTick();
                    stocktick.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    stocktick.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstStocktick.Add(stocktick);
                }
            }

            objYuantaOneAPI.UnsubscribeStocktick(lstStocktick);

            #endregion
        }


        /// <summary>
        /// 訂閱五檔報價
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubFiveTick_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtFiveTick.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<FiveTickA> lstFiveTick = new List<FiveTickA>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    FiveTickA fiveTickA = new FiveTickA();
                    fiveTickA.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    fiveTickA.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstFiveTick.Add(fiveTickA);
                }
            }

            objYuantaOneAPI.SubscribeFiveTickA(lstFiveTick);

            #endregion
        }

        /// <summary>
        /// 解訂閱五檔報價
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnsubFivetick_Click(object sender, EventArgs e)
        {
            #region 使用object訂閱

            string[] strSubStkList = this.txtFiveTick.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<FiveTickA> lstFiveTick = new List<FiveTickA>();
            for (int i = 0; i < strSubStkList.Length; i++)
            {
                if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                {
                    string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                    enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                    string strRealStkNo = strSubStkInfo[1];
                    FiveTickA fiveTickA = new FiveTickA();
                    fiveTickA.MarketNo = Convert.ToByte(enumMarketNo);      //填入查詢市場代碼
                    fiveTickA.StockCode = strRealStkNo;                     //填入查詢股票代碼
                    lstFiveTick.Add(fiveTickA);
                }
            }

            objYuantaOneAPI.UnsubscribeFivetickA(lstFiveTick);

            #endregion
        }


        /// <summary>
        /// 讀取報價表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetWatchListAllData_Click(object sender, EventArgs e)
        {
            YuantaDataHelper dataSetter = new YuantaDataHelper(enumLng);
            RP_WatchListAll.ChildStruct_In childStructIn = new RP_WatchListAll.ChildStruct_In();
            try
            {
                string[] strSubStkList = this.txtWatchListAll.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                dataSetter.SetFunctionID(50, 0, 0, 16);
                dataSetter.SetUInt((uint)strSubStkList.Length);
                for (int i = 0; i < strSubStkList.Length; i++)
                {
                    if ((strSubStkList[i].ToString().Trim() != "") && (strSubStkList[i].ToString().IndexOf(',') >= 0))
                    {
                        string[] strSubStkInfo = strSubStkList[i].ToString().Split(',');
                        enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), strSubStkInfo[0]);
                        string strStkNo = strSubStkInfo[1];

                        dataSetter.SetByte(Convert.ToByte(enumMarketNo));                                    //填入查詢市場代碼
                        dataSetter.SetTByte(strStkNo, Marshal.SizeOf(childStructIn.abyStkCode));             //填入查詢股票代碼
                    }
                }
            }
            catch (Exception ex)
            {
            }

            objYuantaOneAPI.RQ(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", dataSetter);
        }

        /// <summary>
        /// 取得己訂閱報價商品列表
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funGetQuoteList_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);
                
                uint nRowCount = dataGetter.GetUInt();
                strResult += String.Format("己訂閱報價商品列表 筆數{0}: \r\n", nRowCount);
                for (int i = 0; i < nRowCount; i++)
                {
                    strResult += String.Format("{0} \r\n", dataGetter.GetStr(50));
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 取得己訂閱報價商品明細列表
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funGetQuoteListDetail_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                uint nRowCount = dataGetter.GetUInt();
                strResult += String.Format("己訂閱報價商品明細列表 筆數{0}: \r\n", nRowCount);
                for (int i = 0; i < nRowCount; i++)
                {
                    strResult += String.Format("{0} \r\n", dataGetter.GetStr(50));
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// WatchlistAll (報價表即時訂閱結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealWatchlistAll_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_WatclistAll.ParentStruct_Out struParentOut = new RR_WatclistAll.ParentStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                strResult += "報價表訂閱結果: \r\n";
                string strTemp = "";
                byte byTemp = new byte();
                long longTemp = 0;
                int intTemp = 0;
                uint uintTemp = 0;
                TYuantaTime yuantaTime;

                strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyKey));      //鍵值
                strResult += strTemp.ToString() + ",";
                byTemp = dataGetter.GetByte();                                          //市場代碼
                strResult += byTemp.ToString() + ",";
                strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkCode));  //商品代碼
                strResult += FilterBreakChar(strTemp) + ",";
                longTemp = dataGetter.GetLong();                                        //序號
                strResult += longTemp.ToString() + ",";
                byTemp = dataGetter.GetByte();                                          //索引值
                strResult += byTemp.ToString() + ",";

                switch(byTemp)
                {
                    case 22:
                        {
                            uintTemp = dataGetter.GetUInt();                                        //第一買量
                            strResult += uintTemp.ToString() + ",";
                            uintTemp = dataGetter.GetUInt();                                        //第一賣量
                            strResult += uintTemp.ToString();
                        }
                        break;
                    case 28:
                        {
                            intTemp = dataGetter.GetInt();                                          //買價
                            strResult += intTemp.ToString() + ",";
                            intTemp = dataGetter.GetInt();                                          //賣價
                            strResult += intTemp.ToString();
                        }
                        break;

                    case 29:
                        {
                            yuantaTime = dataGetter.GetTYuantaTime();                               //時間
                            strResult += String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                            uintTemp = dataGetter.GetUInt();                                        //累計外盤量
                            strResult += uintTemp.ToString() + ",";
                            uintTemp = dataGetter.GetUInt();                                        //累計內盤量
                            strResult += uintTemp.ToString() + ",";
                            intTemp = dataGetter.GetInt();                                          //成交價
                            strResult += intTemp.ToString() + ",";

                            uintTemp = dataGetter.GetUInt();                                        //單量
                            strResult += uintTemp.ToString() + ",";
                            uintTemp = dataGetter.GetUInt();                                        //總成交量
                            strResult += uintTemp.ToString() + ",";
                            uintTemp = dataGetter.GetUInt();                                        //總成交金額
                            strResult += uintTemp.ToString() ;

                        }
                        break;
                }

                //----------
                strResult += "\r\n";

            }
            catch(Exception ex)
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 五檔(即時訂閱結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealFivetick_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_WatclistAll.ParentStruct_Out struParentOut = new RR_WatclistAll.ParentStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                int intCheck = 1;
                if (intCheck == 1)
                {
                    strResult += "五檔訂閱結果: \r\n";
                    string strTemp = "";
                    byte byTemp = new byte();
                    int intTemp = 0;

                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyKey));                  //鍵值
                    byTemp = dataGetter.GetByte();                                                      //市場代碼
                    strResult += byTemp.ToString() + ",";                               
                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkCode));              //股票代碼
                    strResult += FilterBreakChar(strTemp) + ",";
                    byTemp = dataGetter.GetByte();                                                      //索引值
 					strResult += byTemp.ToString();

                    //後面的欄位若有需要請再自行解析
                    //----------
                    strResult += "\r\n";
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 分時明細(即時訂閱結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealStocktick_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_WatclistAll.ParentStruct_Out struParentOut = new RR_WatclistAll.ParentStruct_Out();
                TYuantaTime yuantaTime;
                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                {
                    strResult += "分時明細訂閱結果: \r\n";
                    string strTemp = "";
                    byte byTemp = new byte();
                    int intTemp = 0;

                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyKey));          //鍵值
                    byTemp = dataGetter.GetByte();                                              //市場代碼
                    strResult += byTemp.ToString() + ",";                                       
                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkCode));      //股票代碼
                    strResult += FilterBreakChar(strTemp) + ",";
                    intTemp = dataGetter.GetInt();                                              //序號
                    strResult += intTemp.ToString() + ",";
                    yuantaTime = dataGetter.GetTYuantaTime();                                   //時間
                    strTemp = String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec);
                    strResult += strTemp + ",";
                    intTemp = dataGetter.GetInt();                                              //買價
                    strResult += intTemp.ToString() + ",";
                    intTemp = dataGetter.GetInt();                                              //賣價
                    strResult += intTemp.ToString() + ",";
                    intTemp = dataGetter.GetInt();                                              //成交價
                    strResult += intTemp.ToString() + ",";
                    intTemp = dataGetter.GetInt();                                              //成交量
                    strResult += intTemp.ToString() + ",";
                    byTemp = dataGetter.GetByte();                                              //內外盤註記
                    strResult += byTemp.ToString() + ",";
                    byTemp = dataGetter.GetByte();                                              //明細類別
                    strResult += byTemp.ToString() ;             

                    //----------
                    strResult += "\r\n";
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// Watchlist指定欄位 (即時訂閱結果)
        /// </summary>
        /// <param name="abyData"></param>
        /// <returns></returns>
        private string funRealWatchlist_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                RR_WatchList.ParentStruct_Out struParentOut = new RR_WatchList.ParentStruct_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                int intCheck = 1;
                if (intCheck == 1)
                {
                    strResult += "WatchList指定欄位訂閱結果: \r\n";
                    string strTemp = "";
                    byte byTemp = new byte();
                    int intTemp = 0;

                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyKey));          //鍵值
                    byTemp = dataGetter.GetByte();                                              //市場代碼
                    strResult += byTemp.ToString() + ",";
                    strTemp = dataGetter.GetStr(Marshal.SizeOf(struParentOut.abyStkCode));      //股票代碼
                    strResult += FilterBreakChar(strTemp) + ",";
                    byTemp = dataGetter.GetByte();                                              //索引值
                    strResult += byTemp.ToString() + ",";
                    intTemp = dataGetter.GetInt();                                              //資料值
                    strResult += intTemp.ToString() + ",";

                    //----------
                    strResult += "\r\n";
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// WatchListAll
        /// </summary>
        /// <param name="abyData">Server所回傳的資料</param>
        /// <returns>欲輸出的結果字串</returns>
        private string funWatchListAll_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                byte byTemp = new byte();

                RP_WatchListAll.ChildStruct_Out struChildOut = new RP_WatchListAll.ChildStruct_Out();

                strResult = "===============\r\n50.0.0.16 報價表\r\n===============";

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);
                dataGetter.OutMsgLoad(abyData);

                int nRowsCount = (int)dataGetter.GetUInt();

                for (int i = 0; i < nRowsCount; i++)
                {
                    byTemp = dataGetter.GetByte();
                    strResult = String.Format("{0}\r\n市場別:{1} 商品代碼:{2} 商品名稱:{3}\r\n昨收價:{4}\r\n開盤參考價:{5}\r\n漲停價:{6}\r\n跌停價:{7}\r\n昨量:{8}",
                        strResult,
                        (enumMarketType)Enum.Parse(typeof(enumMarketType), byTemp.ToString()),
                        dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkCode)),
                        dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkName)),
                        dataGetter.GetInt(),
                        dataGetter.GetInt(),
                        dataGetter.GetInt(),
                        dataGetter.GetInt(),
                        dataGetter.GetUInt()
                        );

                    dataGetter.GetStr(24); //中間24bytes沒要用不解開

                    strResult = String.Format("{0}\r\n開盤價:{1}\r\n最高價:{2}\r\n最低價:{3}\r\n買價:{4}\r\n累計外盤量:{5}\r\n賣價:{6}\r\n累計內盤量:{7}\r\n成交價:{8}\r\n總成交金額:{9}\r\n單量內外盤標記:{10}\r\n單量:{11}\r\n總成交量:{12}\r\n",
                       strResult,
                       dataGetter.GetInt(),
                       dataGetter.GetInt(),
                       dataGetter.GetInt(),
                       dataGetter.GetInt(),
                       dataGetter.GetUInt(),
                       dataGetter.GetInt(),
                       dataGetter.GetUInt(),
                       dataGetter.GetInt(),
                       dataGetter.GetUInt(),
                       dataGetter.GetByte(),
                       dataGetter.GetUInt(),
                       dataGetter.GetUInt()
                       );

                    dataGetter.GetStr(105); //中間105bytes沒要用不解開

                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }

        #endregion

        #region 委回/帳務查詢相關
        /// <summary>
        /// 查詢委託成交綜合回報--盤中零股
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRptOrderTradeReportGroup_odd_Click(object sender, EventArgs e)
        {
            RptOrderTradeReportGroupIV.ParentStruct_In struParentIn = new RptOrderTradeReportGroupIV.ParentStruct_In();
            RptOrderTradeReportGroupIV.ChildStruct_In struChildIn = new RptOrderTradeReportGroupIV.ChildStruct_In();
            YuantaDataHelper dataSetter = new YuantaDataHelper(enumLng);
            dataSetter.SetFunctionID(20, 101, 0, 18);

            dataSetter.SetTByte(cboNoListChannel.Text, Marshal.SizeOf(struParentIn.abyNoListCancel));
            dataSetter.SetUInt((uint)1);
            dataSetter.SetTByte(this.cboAccountList.Text.Trim(), Marshal.SizeOf(struChildIn.abyAccount));

            objYuantaOneAPI.RQ(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", dataSetter);
        }
        private string funRptOrderTradeReportGroup_odd_Out(byte[] abyData)
        {
            #region 委託成交綜合回報
            string strResult = "";
            try
            {
                RptOrderTradeReportGroupIV.ChildStruct1_Out struChildOut = new RptOrderTradeReportGroupIV.ChildStruct1_Out();
                RptOrderTradeReportGroupIV.ChildStruct2_Out struChildOut2 = new RptOrderTradeReportGroupIV.ChildStruct2_Out();
                RptOrderTradeReportGroupIV.ChildStruct3_Out struChildOut3 = new RptOrderTradeReportGroupIV.ChildStruct3_Out();
                RptOrderTradeReportGroupIV.ChildStruct4_Out struChildOut4 = new RptOrderTradeReportGroupIV.ChildStruct4_Out();
                RptOrderTradeReportGroupIV.ChildStruct5_Out struChildOut5 = new RptOrderTradeReportGroupIV.ChildStruct5_Out();
                RptOrderTradeReportGroupIV.ChildStruct6_Out struChildOut6 = new RptOrderTradeReportGroupIV.ChildStruct6_Out();
                RptOrderTradeReportGroupIV.ChildStruct7_Out struChildOut7 = new RptOrderTradeReportGroupIV.ChildStruct7_Out();
                RptOrderTradeReportGroupIV.ChildStruct8_Out struChildOut8 = new RptOrderTradeReportGroupIV.ChildStruct8_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);

                dataGetter.OutMsgLoad(abyData);
                {
                    string strTemp = "";
                    byte byTemp = new byte();
                    short shtTemp = 0;
                    int intTemp = 0;
                    long longTemp = 0;
                    TYuantaDate yuantaDate;
                    TYuantaTime yuantaTime;
                    TYuantaDateTime yuantaDateTime;

                    strResult += "委託成交回報查詢結果 : \r\n";

                    #region 現貨委託筆數

                    uint uintStkOrderCount = 0;

                    uintStkOrderCount = dataGetter.GetUInt();

                    strResult += "現貨委託筆數: " + uintStkOrderCount.ToString() + "\r\n";

                    for (int i = 0; i < uintStkOrderCount; i++)
                    {
                        string row = string.Empty;
                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAccount));
                        strResult += strTemp + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strTemp = String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay);
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyCompanyNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkName));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBS));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyPriceFlag));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strTemp = String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay);
                        strResult += strTemp + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strTemp = String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec);
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrderErrorNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyEmError));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyChannel));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        //strResult += shtTemp.ToString() + ",";
                        switch(shtTemp)
                        {
                            case 0:
                                strTemp = "現貨";
                                break;
                            case 2:
                                strTemp = "盤後零股";
                                break;
                            case 7:
                                strTemp = "盤後";
                                break;
                            case 4:
                                strTemp = "盤中零股";
                                break;
                            case 99:
                                strTemp = "興櫃";
                                break;

                        }
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyCancelFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyReduceFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTraditionFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyBasketNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTradeCurrency));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTime_in_Force));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyOrder_Success));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyReduce_Flag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyChg_Prz_Flag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTSE_Cancel));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strTemp = String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay);
                        strResult += strTemp + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strTemp = String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec);
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 現貨成交筆數

                    uint uintStkReportCount = 0;

                    uintStkReportCount = dataGetter.GetUInt();

                    strResult += "現貨成交筆數: " + uintStkReportCount.ToString() + "\r\n";

                    for (int i = 0; i < uintStkReportCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyAccount));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyCompanyNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyStkName));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyBS));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        yuantaDateTime = dataGetter.GetTYunataDateTime();
                        strTemp = String.Format("{0}/{1}/{2} {3}:{4}:{5}.{6}", yuantaDateTime.struDate.ushtYear, yuantaDateTime.struDate.bytMon, yuantaDateTime.struDate.bytDay, yuantaDateTime.struTime.bytHour, yuantaDateTime.struTime.bytMin, yuantaDateTime.struTime.bytSec, yuantaDateTime.struTime.ushtMSec) + ",";
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyOrderNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyTradeCurrency));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyPrice_Flag));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        switch(shtTemp)
                        {
                            case 0:
                                strTemp = "一般委託";
                                break;
                            case 1:
                                strTemp = "鉅額";
                                break;
                            case 2:
                                strTemp = "零股";
                                break;
                            case 4:
                                strTemp = "盤後定價";
                                break;
                            case 5:
                                strTemp = "盤中零股";
                                break;
                        }
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 期貨委託筆數

                    uint uintFutOrderCount = 0;
                    uintFutOrderCount = dataGetter.GetUInt();

                    strResult += "期貨委託筆數: " + uintFutOrderCount.ToString() + "\r\n";

                    for (int i = 0; i < uintFutOrderCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyAccount));
                        strResult += strTemp + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyCommodityID1));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyBuySellKind1));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyCommodityID2));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyBuySellKind2));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyOpenOffsetKind));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyOrderCondition));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyOrderPrice));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strResult += String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyErrorNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyErrorMessage));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyOrderNO));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyProductType));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyDayTradeID));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyCancelFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyReduceFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyStkName1));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyStkName2));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyTraditionFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyTRID));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyCurrencyType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyCurrencyType2));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyBasketNo));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyStkCode1));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut3.abyStkCode2));
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 期貨成交筆數

                    uint uintFuTradeCount = 0;
                    uintFuTradeCount = dataGetter.GetUInt();

                    strResult += "期貨成交筆數: " + uintFuTradeCount.ToString() + "\r\n";

                    for (int i = 0; i < uintFuTradeCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyAccount));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyCommodityID1));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyBuySellKind1));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strResult += String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyOrderNo));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyCommodityID2));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyBuySellKind2));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyRecType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyProductType));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyStkName1));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyStkName2));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyDayTradeID));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyTRID));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyCurrencyType));
                        strResult += strTemp + ",";

                        if (enumEvenMode == enumEnvironmentMode.UAT)
                        {
                            strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyCurrencyType2));
                            strResult += strTemp + ",";

                            strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abySub_No));
                            strResult += strTemp + "\r\n";
                        }
                        else
                        {
                            strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut4.abyCurrencyType2));
                            strResult += strTemp + "\r\n";
                        }
                    }

                    #endregion

                    #region 國外股票委託筆數

                    uint uintOVOrderCount = 0;
                    uintOVOrderCount = dataGetter.GetUInt();

                    strResult += "國外股票委託筆數:" + uintOVOrderCount.ToString() + "\r\n";

                    //國外股票股名有可能有逗號，所以用|分隔
                    for (int i = 0; i < uintOVOrderCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyAccount));
                        strResult += strTemp + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyCompanyNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyStkName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyBS));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyCurrencyType));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyPriceType));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += intTemp.ToString() + ",";

                        yuantaDateTime = dataGetter.GetTYunataDateTime();
                        strTemp = String.Format("{0}/{1}/{2} {3}:{4}:{5}", yuantaDateTime.struDate.ushtYear, yuantaDateTime.struDate.bytMon, yuantaDateTime.struDate.bytDay, yuantaDateTime.struTime.bytHour, yuantaDateTime.struTime.bytMin, yuantaDateTime.struTime.bytSec) + "|";
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyOrderType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyOrderNo));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyOrderErrorNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyEmError));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyCurrencyType2));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyCancelFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyReduceFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyTraditionFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abySettleType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut5.abyBasketNo));
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 國外股票成交筆數

                    uint uintOVTradeCount = 0;
                    uintOVTradeCount = dataGetter.GetUInt();

                    strResult += "國外股票成交筆數:" + uintOVTradeCount.ToString() + "\r\n";

                    for (int i = 0; i < uintOVTradeCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyAccount));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyCompanyNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyStkName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyBS));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyCurrencyType));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        yuantaDateTime = dataGetter.GetTYunataDateTime();
                        strTemp = String.Format("{0}/{1}/{2} {3}:{4}:{5}", yuantaDateTime.struDate.ushtYear, yuantaDateTime.struDate.bytMon, yuantaDateTime.struDate.bytDay, yuantaDateTime.struTime.bytHour, yuantaDateTime.struTime.bytMin, yuantaDateTime.struTime.bytSec) + ",";
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyOrderNo));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut6.abyCurrencyType2));
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 國外期貨委託筆數

                    uint uintOFOrderCount = 0;
                    uintOFOrderCount = dataGetter.GetUInt();

                    strResult += "國外期貨委託筆數:" + uintOFOrderCount.ToString() + "\r\n";

                    for (int i = 0; i < uintOFOrderCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyAccount));
                        strResult += strTemp + "|";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyCommodityID));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyStkName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyBuySell));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyOrderType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyOdrPrice));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyTouchPrice));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strResult += String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strResult += String.Format(" {0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyErrorNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyErrorMessage));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyOrderNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyDayTradeID));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyCancelFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyReduceFlag));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyTraditionFlag));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyBasketNo));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyStkCode1));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyCurrencyType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut7.abyCurrencyType2));
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                    #region 國外期貨成交筆數

                    uint uintOFTradeCount = 0;
                    uintOFTradeCount = dataGetter.GetUInt();

                    strResult += "國外期貨成交筆數:" + uintOFTradeCount.ToString() + "\r\n";

                    for (int i = 0; i < uintOFTradeCount; i++)
                    {
                        string row = string.Empty;

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyAccount));
                        strResult += strTemp + "|";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyCommodityID));
                        strResult += strTemp + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyStkName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyBuySell));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyOdrPrice));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyMatchPrice));
                        strResult += strTemp + ",";

                        yuantaDate = dataGetter.GetTYuantaDate();
                        strTemp = String.Format("{0}/{1}/{2}", yuantaDate.ushtYear, yuantaDate.bytMon, yuantaDate.bytDay) + ",";
                        strResult += strTemp + ",";

                        yuantaTime = dataGetter.GetTYuantaTime();
                        strTemp = String.Format("{0}:{1}:{2}.{3}", yuantaTime.bytHour, yuantaTime.bytMin, yuantaTime.bytSec, yuantaTime.ushtMSec) + ",";
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyOrderNo));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyCurrencyType));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut8.abyCurrencyType2));
                        strResult += strTemp + "\r\n";
                    }

                    #endregion

                }
                dataGetter.ClearOutputData();
            }
            catch (Exception ex)
            {
                strResult = "";
            }

            return strResult;
            #endregion 
        }

        /// <summary>
        /// 庫存綜合總表--盤中零股
        /// </summary>
        /// <param name="abyData">Server所回傳的資料</param>
        /// <returns>欲輸出的結果字串</returns>
        private void btnStoStoreSummaryGroup_odd_Click(object sender, EventArgs e)
        {
            StoStoreSummaryGroupIX.ChildStruct_In struChildIn = new StoStoreSummaryGroupIX.ChildStruct_In();

            YuantaDataHelper dataSetter = new YuantaDataHelper(enumLng);
            dataSetter.SetFunctionID(20, 103, 0, 22);
            dataSetter.SetUInt((uint)1);
            dataSetter.SetTByte(this.cboAccountList.Text.Trim(), Marshal.SizeOf(struChildIn.abyAccount));

            objYuantaOneAPI.RQ(this.cboAccountList.SelectedItem != null ? this.cboAccountList.SelectedItem.ToString().Trim() : "", dataSetter);
        }
        private string funStoStoreSummaryGroup_odd_Out(byte[] abyData)
        {
            string strResult = "";
            try
            {
                StoStoreSummaryGroupIX.ParentStruct1_Out struParentOut = new StoStoreSummaryGroupIX.ParentStruct1_Out();
                StoStoreSummaryGroupIX.ChildStruct1_Out struChildOut = new StoStoreSummaryGroupIX.ChildStruct1_Out();
                StoStoreSummaryGroupIX.ChildStruct2_Out struChildOut2 = new StoStoreSummaryGroupIX.ChildStruct2_Out();

                YuantaDataHelper dataGetter = new YuantaDataHelper(enumLng);

                dataGetter.OutMsgLoad(abyData);
                {
                    string strTemp = "";
                    byte byTemp = new byte();
                    short shtTemp = 0;
                    int intTemp = 0;
                    uint uintTemp = 0;
                    long longTemp = 0;

                    #region 股票庫存明細

                    int intStkStoreCount = 0;

                    intStkStoreCount = dataGetter.GetInt();

                    strResult += "庫存綜合總表筆數: " + intStkStoreCount.ToString() + "\r\n";

                    for (int i = 0; i < intStkStoreCount; i++)
                    {

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyAccount));
                        strResult += strTemp + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkCode));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyStkName));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        uintTemp = dataGetter.GetUInt();
                        strResult += uintTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        uintTemp = dataGetter.GetUInt();
                        strResult += uintTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut.abyTradeCurrency));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + "\r\n";
                    }

                    #endregion

                    #region 國外股票庫存明細-API暫不支援

                    int intOVStkStoreCount = 0;

                    intOVStkStoreCount = dataGetter.GetInt();

                    strResult += "國外股票庫存筆數: " + intOVStkStoreCount.ToString() + "\r\n";

                    for (int i = 0; i < intOVStkStoreCount; i++)
                    {

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyAccount));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyCurrencyType));
                        strResult += strTemp + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyMarketName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyStkCode));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyStkName));
                        strResult += strTemp + ",";

                        strTemp = dataGetter.GetStr(Marshal.SizeOf(struChildOut2.abyStkFullName));
                        strResult += strTemp + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        longTemp = dataGetter.GetLong();
                        strResult += longTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        byTemp = dataGetter.GetByte();
                        strResult += byTemp.ToString() + ",";

                        uintTemp = dataGetter.GetUInt();
                        strResult += uintTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        shtTemp = dataGetter.GetShort();
                        strResult += shtTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + ",";

                        intTemp = dataGetter.GetInt();
                        strResult += intTemp.ToString() + "\r\n";
                    }
                    #endregion

                    dataGetter.ClearOutputData();
                }
            }
            catch
            {
                strResult = "";
            }
            return strResult;
        }
    
        /// <summary>
        /// 切換國外期貨下單市場別
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboOVMarketNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                enumMarketType enumMarketNo = (enumMarketType)Enum.Parse(typeof(enumMarketType), cboOFMarketNo.SelectedValue.ToString());
                txtOFMarketNo.Text = Convert.ToByte(enumMarketNo).ToString();
                switch(txtOFMarketNo.Text)
                {
                    case "202":
                        txtExhCode.Text = "SGX";
                        break;
                    case "203":
                        txtExhCode.Text = "CME";
                        break;
                    case "204":
                        txtExhCode.Text = "CBT";
                        break;
                    case "205":
                        txtExhCode.Text = "TCE";
                        break;
                    case "207":
                        txtExhCode.Text = "OSE";
                        break;
                    case "208":
                        txtExhCode.Text = "HKF";
                        break;   
                    case "209":
                        txtExhCode.Text = "NBT";
                        break;
                    case "210":
                        txtExhCode.Text = "LIF";
                        break;
                    case "211":
                        txtExhCode.Text = "EUX";
                        break;
                    case "212":
                        txtExhCode.Text = "ASX";
                        break;
                    case "215":
                        txtExhCode.Text = "CFE";
                        break;
                }
            }
            catch(Exception ex)
            {

            }
        }

        #endregion

        #region 工具函式
        /// <summary>
        /// 取得DataTable
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetDataTable(object obj)
        {
            DataTable table = new DataTable();
            var fieldGroup = obj.GetType().GetFields();
            foreach (var item in fieldGroup)
            {
                DescriptionAttribute[] attributes =
                (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                    table.Columns.Add(new DataColumn(attributes[0].Description, typeof(string)));
            }
            return table;
        }

        /// <summary>
        /// 過濾結束字元\0
        /// </summary>
        /// <param name="strFilterData">欲過濾的字串資料</param>
        /// <returns></returns>
        public string FilterBreakChar(string strFilterData)
        {
            Encoding enc = Encoding.GetEncoding("Big5");//提供Big5的編解碼
            byte[] tmp_bytearyData = enc.GetBytes(strFilterData);
            int intCharLen = tmp_bytearyData.Length;
            int indexCharData = intCharLen;
            for (int i = 0; i < intCharLen; i++)
            {
                if (Convert.ToChar(tmp_bytearyData.GetValue(i)) == 0)
                {
                    indexCharData = i;
                    break;
                }
            }
            return enc.GetString(tmp_bytearyData, 0, indexCharData);
        }

        /// <summary>
        /// 將字串結尾補空白字元
        /// </summary>
        /// <param name="strData">欲處理的字串</param>
        /// <param name="intLen">欲達到的Bytes</param>
        /// <returns></returns>
        public string FillSpace(string strData, int intLen)
        {
            Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
            int intDataLen = enc.GetByteCount(strData);
            if (intDataLen < intLen)
            {
                StringBuilder strRtnData = new StringBuilder(strData);
                strRtnData.Append(' ', intLen - strData.Length);
                return strRtnData.ToString();
            }
            else
            {
                return strData;
            }
        }
        #endregion


    }

    /// <summary>
    /// 設定Combox item顯示及值
    /// </summary>
    public class ComboEnumItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboEnumItem(Enum originalEnum)
        {
            this.Value = originalEnum;
            this.Text = this.ToString();
        }

        /// <summary>
        /// 顯示的文字
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            FieldInfo field = Value.GetType().GetField(Value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? Value.ToString() : String.Format("{0}:{1}", Value.ToString(), attribute.Description);
        }
    }
}
