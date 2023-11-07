using System;
using System.Runtime.InteropServices; //使用Marsha等類別操作所需的namespace
using System.Diagnostics;			//使用EventLog等類別所需的namespace	 
using System.Text;					//使用StringBuilder等類別所需的namespace	
using System.IO;				   //使用MemoryStream等類別所需的namespace	
using System.Net;					//使用IPAddress等類別所需的namespace


namespace YuantaShareFunction
{

	/// <summary>
	/// 提供各種供元件(TCBUS架構)的工具函式。
	/// </summary>
	public class ShareFunctionClass
	{


		#region 私有工具函式
        [DllImport("kernel32.dll", EntryPoint = "LCMapStringA")]
        public static extern int LCMapString(int Locale, int dwMapFlags, byte[] lpSrcStr, int cchSrc, byte[] lpDestStr, int cchDest);
        const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        /// <summary>
        /// 將繁體中文轉為簡體中文
        /// </summary>
        /// <param name="lines">繁體中文字串</param>
        /// <returns></returns>
        public byte[] ConvertToSC(String lines)
        {
            Encoding gb2312 = Encoding.GetEncoding(936);
            byte[] src = gb2312.GetBytes(lines);
            byte[] dest = new byte[src.Length];
            LCMapString(0x0804, LCMAP_SIMPLIFIED_CHINESE, src, -1, dest, src.Length);
            return dest;
        }
        /// <summary>
        /// 過濾結束字元\0
        /// </summary>
        /// <param name="abyData">欲過濾的字串資料</param>
        /// <param name="Encoder">語系</param>
        /// <returns></returns>
        public string FilterBreakChar(byte[] abyData, Encoding enc)
        {
            int intCharLen = abyData.Length;
            int indexCharData = intCharLen;
            for (int i = 0; i < intCharLen; i++)
            {
                if (Convert.ToChar(abyData.GetValue(i)) == 0)
                {
                    indexCharData = i;
                    break;
                }
            }
            return enc.GetString(abyData, 0, indexCharData);
        }
        /// <summary>
        /// 過濾結束字元\0
        /// </summary>
        /// <param name="strFilterData">欲過濾的字串資料</param>
        /// <returns></returns>
        public string FilterBreakChar(byte[] abyFilterData, string strEncode)
        {
            Encoding enc = Encoding.GetEncoding(strEncode);//提供gb2312的編解碼
            int intCharLen = abyFilterData.Length;
            int indexCharData = intCharLen;
            for (int i = 0; i < intCharLen; i++)
            {
                if (Convert.ToChar(abyFilterData.GetValue(i)) == 0)
                {
                    indexCharData = i;
                    break;
                }
            }
            return enc.GetString(abyFilterData, 0, indexCharData);
        }

        public string FillASCIIZero(string strData, int intLen)
        {
            Encoding enc = Encoding.GetEncoding("ASCII");
            int intDataLen = enc.GetByteCount(strData);
            if (intDataLen < intLen)
            {
                StringBuilder strRtnData = new StringBuilder(strData);
                strRtnData.Append('\0', intLen - strData.Length);
                return strRtnData.ToString();
            }
            else
            {
                return strData;
            }
        }

		/// <summary>
		/// 將錯誤資訊寫入EventLog
		/// </summary>
		/// <param name="strComName">元件名稱</param>
		/// <param name="strExcetion">欲寫入的錯誤訊息</param>
		public void WriteEventLog(string strComName,string strExcetion)
		{
            try
            {
                EventLog EventLog1 = new EventLog("Application");
                EventLog1.Source = strComName;
                EventLog1.WriteEntry(strExcetion, EventLogEntryType.Error);
                EventLog1.Close();
                EventLog1.Dispose();
            }
            catch (System.Net.Sockets.SocketException s)
            {
                Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
                byte[] abyEx = enc.GetBytes(s.ToString());
                FileStream objFile = new FileStream("c:\\FutOrderSocketExcetion.txt", FileMode.Create);
                objFile.Write(abyEx, 0, abyEx.Length);
                objFile.Close();

            }
            catch(Exception ex)
            {
                Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
                byte[] abyEx = enc.GetBytes(ex.ToString());
                FileStream objFile = new FileStream("c:\\FutOrderExcetion.txt", FileMode.Create);
                objFile.Write(abyEx, 0, abyEx.Length);
                objFile.Close();

            }
		}
        /// <summary>
        /// 將錯誤資訊寫入EventLog
        /// </summary>
        /// <param name="strComName">元件名稱</param>
        /// <param name="strExcetion">欲寫入的錯誤訊息</param>
        /// <param name="ExcetionType">訊息種類</param>
        public void WriteEventLog(string strComName, string strExcetion, EventLogEntryType objExcetionType)
        {
            EventLog EventLog1 = new EventLog("Application");
            EventLog1.Source = strComName;
            EventLog1.WriteEntry(strExcetion, objExcetionType);
            EventLog1.Close();
            EventLog1.Dispose();
        }
		/// <summary>
		/// 將字串開頭補 "0"字元
		/// </summary>
		/// <param name="strData">欲處理的字串</param>
		/// <param name="intLen">欲達到的Bytes</param>
		/// <returns></returns>
		public string FillZero(string strData,int intLen)
		{
			
			Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
			int intDataLen = enc.GetByteCount(strData);
			if (intDataLen < intLen)
			{
				StringBuilder strRtnData = new StringBuilder(strData);
				strRtnData.Insert(0,"0",intLen-strData.Length);
				return strRtnData.ToString();
			}
			else
			{
				return strData;
			}
		}
		/// <summary>
		/// 將字串結尾補空白字元
		/// </summary>
		/// <param name="strData">欲處理的字串</param>
		/// <param name="intLen">欲達到的Bytes</param>
		/// <returns></returns>
		public string FillSpace(string strData,int intLen)
		{
			Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
			int intDataLen = enc.GetByteCount(strData);
			if (intDataLen < intLen)
			{
				StringBuilder strRtnData = new StringBuilder(strData);
				strRtnData.Append(' ',intLen-strData.Length);
				return strRtnData.ToString();
			}
			else
			{
				return strData;
			}
		}
		/// <summary>
		/// 釋放 Unmanaged 物件的資源
		/// </summary>
		/// <param name="o">欲釋放資源的物件</param>
		public void ReleaseComObject(object o)
		{
			try
			{
				Marshal.ReleaseComObject(o);
			}
			catch 
            {
            
            }
			finally
			{
				o = null;
			}
		}
		/// <summary>
		/// 篩選重覆的資料內容
		/// </summary>
		/// <param name="abySource">欲篩選的資料陣列</param>
		/// <param name="intRowLength">每筆資料的長度</param>
		/// <returns></returns>
		public byte[] RepeatFilter(string[] abySource,int intRowLength)
		{
			byte[] abyResult=null;
			try
			{
				if((abySource.Length>0)&&(abySource[0].Length==intRowLength)) //確認字串陣列有內容並且確認第一筆的內容長度合法
				{
					int intResultCount=1;	//結果至少會有一筆
					int intSourceLen = abySource.Length;
					Encoding enc = Encoding.GetEncoding("Big5");//提供big5的編解碼
					Array.Sort(abySource);//先行排序字串陣列的內容
					MemoryStream memResult_tmp = new MemoryStream(Marshal.SizeOf(intResultCount.GetType())+(intSourceLen*intRowLength));	//計算MemoryStream的大小 
					memResult_tmp.Position=Marshal.SizeOf(intResultCount.GetType());//先跳過筆數
					memResult_tmp.Write(enc.GetBytes(abySource[0]),0,abySource[0].Length); //先將第一筆寫入MemoryStream
					for (int i=0;i<intSourceLen;i++)
					{
						if (((i+1)<intSourceLen)&&(abySource[i+1]!=abySource[i]))	//判斷下一筆是否為最後一筆並且
						{
							if (abySource[i].Length==intRowLength)//確認字串陣列有內容並且確認內容長度合法
							{
								intResultCount=intResultCount+1;	//記錄筆數
								memResult_tmp.Write(enc.GetBytes(abySource[i+1]),0,abySource[i+1].Length); //將結果寫入MemoryStream
							}
							else
							{
								return null;
							}
						}

					}
					memResult_tmp.Position=0;	
					memResult_tmp.Write(BitConverter.GetBytes((uint)(intResultCount)),0,Marshal.SizeOf(intResultCount.GetType()));//最後將帳號筆數寫入MemoryStream
					memResult_tmp.Capacity = Marshal.SizeOf(intResultCount.GetType())+(intResultCount*intRowLength);//在重新設定MemoryStream Buffer的大小
					abyResult = memResult_tmp.GetBuffer();
					memResult_tmp.Close();
				}
				else
				{
					abyResult = null ;
				}
			}
			catch 
			{
				abyResult = null ;

			}
			return abyResult ;
		}
        /// <summary>
        /// 將密碼字串結尾補結束字元(\0)
        /// </summary>
        /// <param name="strData">欲處理的字串</param>
        /// <param name="intLen">欲達到的Bytes</param>
        /// <returns></returns>
        public string FillBreakChar(string strData, int intLen)
        {
            Encoding enc = Encoding.GetEncoding("Big5");//提供Big5的編解碼
            int intDataLen = enc.GetByteCount(strData);
            if (intDataLen < intLen)
            {
                StringBuilder strRtnData = new StringBuilder(strData);
                strRtnData.Append((char)0, intLen - strData.Length);
                return strRtnData.ToString();
            }
            else
            {
                return strData;
            }
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
			for (int i=0;i<intCharLen;i++)
			{
				if (Convert.ToChar(tmp_bytearyData.GetValue(i))==0)
				{
					indexCharData = i;
					break;
				}
			}
			return enc.GetString(tmp_bytearyData,0,indexCharData);
		}
		/// <summary>
		/// 過濾結束字元\0
		/// </summary>
		/// <param name="abyData">欲過濾的字串資料</param>
		/// <returns></returns>
		public string FilterBreakChar(byte[] abyData)
		{
			string strRtn;
			GCHandle handle = GCHandle.Alloc(abyData,GCHandleType.Pinned);
			strRtn = Marshal.PtrToStringAnsi(handle.AddrOfPinnedObject());
			handle.Free();//將分配的記憶體做釋放的動作 
			return strRtn;
		}
		#endregion
	}
}
