namespace YuantaOneAPI_TestAP.Model
{
    public class GetWatchListAllDataModel
    {
        /// <summary>
        /// 市場代碼
        /// </summary>
        public string byMarketNo { get; set; }

        /// <summary>
        /// 股票代碼
        /// </summary>
        public string abyStkCode { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string abyStkName { get; set; }

        /// <summary>
        /// 昨收價
        /// </summary>
        public string intYstPrice { get; set; }

        /// <summary>
        /// 開盤參考價
        /// </summary>
        public string intOpenRefPrice { get; set; }

        /// <summary>
        /// 漲停價
        /// </summary>
        public string intUpStopPrice { get; set; }

        /// <summary>
        /// 跌停價
        /// </summary>
        public string intDownStopPrice { get; set; }

        /// <summary>
        /// 昨量
        /// </summary>
        public string uintYstVol { get; set; }

        /// <summary>
        /// 擴充名
        /// </summary>
        public string abyExtName { get; set; }

        /// <summary>
        /// 小數位數
        /// </summary>
        public string shtDecimal { get; set; }

        /// <summary>
        /// 融資成數
        /// </summary>
        public string byCreditPercent { get; set; }

        /// <summary>
        /// 融券成數
        /// </summary>
        public string byLenBondPercent { get; set; }

        /// <summary>
        /// 開盤
        /// </summary>
        public string intOpenPrice { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        public string intHighPrice { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        public string intLowPrice { get; set; }

        /// <summary>
        /// 買價
        /// </summary>
        public string intBuyPrice { get; set; }

        /// <summary>
        /// 累計外盤量
        /// </summary>
        public string uintTotalOutVol { get; set; }

        /// <summary>
        /// 賣價
        /// </summary>
        public string intSellPrice { get; set; }

        /// <summary>
        /// 累計內盤量
        /// </summary>
        public string uintTotalInVol { get; set; }

        /// <summary>
        /// 成交價
        /// </summary>
        public string intDealPrice { get; set; }

        /// <summary>
        /// 總成交金額
        /// </summary>
        public string uintTotalDealAmt { get; set; }

        /// <summary>
        /// 單量內外盤標記
        /// </summary>
        public string bytVolFlag { get; set; }

        /// <summary>
        /// 單量
        /// </summary>
        public string uintVol { get; set; }

        /// <summary>
        /// 總成交量
        /// </summary>
        public string uintTotalVol { get; set; }

        /// <summary>
        /// 定價量
        /// </summary>
        public string uintFixedPriceVol { get; set; }

        /// <summary>
        /// 未平倉量
        /// </summary>
        public string uintReserveVol { get; set; }

        /// <summary>
        /// 結算價
        /// </summary>
        public string intSettlementPrice { get; set; }

        /// <summary>
        /// 合約高價
        /// </summary>
        public string intHiContractPrice { get; set; }

        /// <summary>
        /// 合約低價
        /// </summary>
        public string intLoContractPrice { get; set; }

        /// <summary>
        /// 委託買進總筆數
        /// </summary>
        public string uintOrderBuyCount { get; set; }

        /// <summary>
        /// 委託買進總口數
        /// </summary>
        public string uintOrderBuyQty { get; set; }

        /// <summary>
        /// 委託賣出總筆數
        /// </summary>
        public string uintOrderSellCount { get; set; }

        /// <summary>
        /// 委託賣出總口數
        /// </summary>
        public string uintOrderSellQty { get; set; }

        /// <summary>
        /// 累計買進成交筆數
        /// </summary>
        public string uintDealBuyCount { get; set; }

        /// <summary>
        /// 累計賣出成交筆數
        /// </summary>
        public string uintDealSellCount { get; set; }

        /// <summary>
        /// 波動率
        /// </summary>
        public string uintVolatility { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        public string struTime { get; set; }

        /// <summary>
        /// 時差
        /// </summary>
        public string sbytTimeDiff { get; set; }

        /// <summary>
        /// 屬性2
        /// </summary>
        public string byStkType2 { get; set; }

        /// <summary>
        /// 未平倉量增減
        /// </summary>
        public string uintReserveVolDiff { get; set; }

        /// <summary>
        /// 所屬產業分類碼
        /// </summary>
        public string abyBelongCode { get; set; }

        /// <summary>
        /// 產業類股名稱
        /// </summary>
        public string abyIndustryName { get; set; }

        /// <summary>
        /// 市值(%)
        /// </summary>
        public string intPrincipalPercent { get; set; }

        /// <summary>
        /// 連續漲跌(天)
        /// </summary>
        public string shtUpDownDay { get; set; }

        /// <summary>
        /// 第一買量
        /// </summary>
        public string uintBidQty { get; set; }

        /// <summary>
        /// 第一賣量
        /// </summary>
        public string uintAskQty { get; set; }

        /// <summary>
        /// 瞬間價格趨勢
        /// </summary>
        public string byPriceTrends { get; set; }

        /// <summary>
        /// 盤前揭露價
        /// </summary>
        public string intEstDealPrice { get; set; }

        /// <summary>
        /// 盤前揭露量
        /// </summary>
        public string uintEstDealVol { get; set; }

        /// <summary>
        /// 盤前揭露量內外盤標記
        /// </summary>
        public string byEstDealVolFlag { get; set; }
    }
}
