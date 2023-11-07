using System;
using System.Runtime.InteropServices; //�ϥ�Marsha�����O�ާ@�һݪ�namespace
using System.Diagnostics;			//�ϥ�EventLog�����O�һݪ�namespace	 
using System.Text;					//�ϥ�StringBuilder�����O�һݪ�namespace	
using System.IO;				   //�ϥ�MemoryStream�����O�һݪ�namespace	
using System.Net;					//�ϥ�IPAddress�����O�һݪ�namespace


namespace YuantaShareFunction
{

	/// <summary>
	/// ���ѦU�بѤ���(TCBUS�[�c)���u��禡�C
	/// </summary>
	public class ShareFunctionClass
	{


		#region �p���u��禡
        [DllImport("kernel32.dll", EntryPoint = "LCMapStringA")]
        public static extern int LCMapString(int Locale, int dwMapFlags, byte[] lpSrcStr, int cchSrc, byte[] lpDestStr, int cchDest);
        const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        /// <summary>
        /// �N�c�餤���ର²�餤��
        /// </summary>
        /// <param name="lines">�c�餤��r��</param>
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
        /// �L�o�����r��\0
        /// </summary>
        /// <param name="abyData">���L�o���r����</param>
        /// <param name="Encoder">�y�t</param>
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
        /// �L�o�����r��\0
        /// </summary>
        /// <param name="strFilterData">���L�o���r����</param>
        /// <returns></returns>
        public string FilterBreakChar(byte[] abyFilterData, string strEncode)
        {
            Encoding enc = Encoding.GetEncoding(strEncode);//����gb2312���s�ѽX
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
		/// �N���~��T�g�JEventLog
		/// </summary>
		/// <param name="strComName">����W��</param>
		/// <param name="strExcetion">���g�J�����~�T��</param>
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
                Encoding enc = Encoding.GetEncoding("Big5");//����big5���s�ѽX
                byte[] abyEx = enc.GetBytes(s.ToString());
                FileStream objFile = new FileStream("c:\\FutOrderSocketExcetion.txt", FileMode.Create);
                objFile.Write(abyEx, 0, abyEx.Length);
                objFile.Close();

            }
            catch(Exception ex)
            {
                Encoding enc = Encoding.GetEncoding("Big5");//����big5���s�ѽX
                byte[] abyEx = enc.GetBytes(ex.ToString());
                FileStream objFile = new FileStream("c:\\FutOrderExcetion.txt", FileMode.Create);
                objFile.Write(abyEx, 0, abyEx.Length);
                objFile.Close();

            }
		}
        /// <summary>
        /// �N���~��T�g�JEventLog
        /// </summary>
        /// <param name="strComName">����W��</param>
        /// <param name="strExcetion">���g�J�����~�T��</param>
        /// <param name="ExcetionType">�T������</param>
        public void WriteEventLog(string strComName, string strExcetion, EventLogEntryType objExcetionType)
        {
            EventLog EventLog1 = new EventLog("Application");
            EventLog1.Source = strComName;
            EventLog1.WriteEntry(strExcetion, objExcetionType);
            EventLog1.Close();
            EventLog1.Dispose();
        }
		/// <summary>
		/// �N�r��}�Y�� "0"�r��
		/// </summary>
		/// <param name="strData">���B�z���r��</param>
		/// <param name="intLen">���F�쪺Bytes</param>
		/// <returns></returns>
		public string FillZero(string strData,int intLen)
		{
			
			Encoding enc = Encoding.GetEncoding("Big5");//����big5���s�ѽX
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
		/// �N�r�굲���ɪťզr��
		/// </summary>
		/// <param name="strData">���B�z���r��</param>
		/// <param name="intLen">���F�쪺Bytes</param>
		/// <returns></returns>
		public string FillSpace(string strData,int intLen)
		{
			Encoding enc = Encoding.GetEncoding("Big5");//����big5���s�ѽX
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
		/// ���� Unmanaged ���󪺸귽
		/// </summary>
		/// <param name="o">������귽������</param>
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
		/// �z�ﭫ�Ъ���Ƥ��e
		/// </summary>
		/// <param name="abySource">���z�諸��ư}�C</param>
		/// <param name="intRowLength">�C����ƪ�����</param>
		/// <returns></returns>
		public byte[] RepeatFilter(string[] abySource,int intRowLength)
		{
			byte[] abyResult=null;
			try
			{
				if((abySource.Length>0)&&(abySource[0].Length==intRowLength)) //�T�{�r��}�C�����e�åB�T�{�Ĥ@�������e���צX�k
				{
					int intResultCount=1;	//���G�ܤַ|���@��
					int intSourceLen = abySource.Length;
					Encoding enc = Encoding.GetEncoding("Big5");//����big5���s�ѽX
					Array.Sort(abySource);//����ƧǦr��}�C�����e
					MemoryStream memResult_tmp = new MemoryStream(Marshal.SizeOf(intResultCount.GetType())+(intSourceLen*intRowLength));	//�p��MemoryStream���j�p 
					memResult_tmp.Position=Marshal.SizeOf(intResultCount.GetType());//�����L����
					memResult_tmp.Write(enc.GetBytes(abySource[0]),0,abySource[0].Length); //���N�Ĥ@���g�JMemoryStream
					for (int i=0;i<intSourceLen;i++)
					{
						if (((i+1)<intSourceLen)&&(abySource[i+1]!=abySource[i]))	//�P�_�U�@���O�_���̫�@���åB
						{
							if (abySource[i].Length==intRowLength)//�T�{�r��}�C�����e�åB�T�{���e���צX�k
							{
								intResultCount=intResultCount+1;	//�O������
								memResult_tmp.Write(enc.GetBytes(abySource[i+1]),0,abySource[i+1].Length); //�N���G�g�JMemoryStream
							}
							else
							{
								return null;
							}
						}

					}
					memResult_tmp.Position=0;	
					memResult_tmp.Write(BitConverter.GetBytes((uint)(intResultCount)),0,Marshal.SizeOf(intResultCount.GetType()));//�̫�N�b�����Ƽg�JMemoryStream
					memResult_tmp.Capacity = Marshal.SizeOf(intResultCount.GetType())+(intResultCount*intRowLength);//�b���s�]�wMemoryStream Buffer���j�p
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
        /// �N�K�X�r�굲���ɵ����r��(\0)
        /// </summary>
        /// <param name="strData">���B�z���r��</param>
        /// <param name="intLen">���F�쪺Bytes</param>
        /// <returns></returns>
        public string FillBreakChar(string strData, int intLen)
        {
            Encoding enc = Encoding.GetEncoding("Big5");//����Big5���s�ѽX
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
		/// �L�o�����r��\0
		/// </summary>
		/// <param name="strFilterData">���L�o���r����</param>
		/// <returns></returns>
		public string FilterBreakChar(string strFilterData)
		{
			Encoding enc = Encoding.GetEncoding("Big5");//����Big5���s�ѽX
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
		/// �L�o�����r��\0
		/// </summary>
		/// <param name="abyData">���L�o���r����</param>
		/// <returns></returns>
		public string FilterBreakChar(byte[] abyData)
		{
			string strRtn;
			GCHandle handle = GCHandle.Alloc(abyData,GCHandleType.Pinned);
			strRtn = Marshal.PtrToStringAnsi(handle.AddrOfPinnedObject());
			handle.Free();//�N���t���O���鰵���񪺰ʧ@ 
			return strRtn;
		}
		#endregion
	}
}
