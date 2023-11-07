using System;
using System.Runtime.InteropServices;
using YuantaShareStructList;


namespace StoStoreSummaryGroupIX
{
	/// <summary>
	/// StructList ���K�n�y�z�C
	/// </summary>
	
	//--------------------
	//�����c(Input)
	[StructLayout(LayoutKind.Sequential,Pack=1)]
	public struct ParentStruct_In
	{
		public uint uintCount ;		//��J����
	}
	//--------------------
	//�l���c(Input)
	[StructLayout(LayoutKind.Sequential,Pack=1)]
	public struct ChildStruct_In
	{
		public TByte22 abyAccount;
	}
	//--------------------
    //�����c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct1_Out
    {
        public uint uintCount;		//��X����
    }
    //--------------------
    //�l���c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct1_Out	//�Ѳ��w�s���c
    {
        public TByte22 abyAccount;       //�b��
        public short shtTradeKind;       //�������
        public byte byMarketNo;          //�����N�X
        public TByte30 abyMarketName;    //�����W��
        public TByte12 abyStkCode;       //�Ѳ��N�X
        public TByte30 abyStkName;       //�Ѳ��W��
        public long lngStockNos;	     //�Ѽ�
        public long lngPrice;	         //���槡��
        public long lngCost;		     //��������
        public long lngInterest;		 //�w���Q��
        public int intBuyNotInNos;       //�R�i���J�b�Ѽ�
        public int intSellNotInNos;      //��X���J�b�Ѽ�
        public long lngCanOrderQty;      //����i�U��Ѽ�
        public long lngLoan;		     //��O�Ҫ�/���O���~
        public int intTaxRate;           //����|�v
        public uint uintLotSize;         //������
        public int intMarketPrice;       //����
        public short shtDecimal;         //�p�Ʀ��
        public byte byStkType1;          //�ݩ�1
        public byte byStkType2;          //�ݩ�2
        public int intBuyPrice;          //�R��
        public int intSellPrice;         //���
        public int intUpStopPrice;       //������
        public int intDownStopPrice;     //�^����
        public uint uintPriceMultiplier; //�p������
        public TByte3 abyTradeCurrency;  //�������O
        public long lngCDQTY;            //�ɶU�Ѽ�
        public long lngCanOrderOddQty;   //�s�ѥi�U��Ѽ�
    }
	//�����c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ParentStruct2_Out
	{
		public uint uintCount ;		//��X����
	}
	//--------------------
	//�l���c(Output)
	[StructLayout(LayoutKind.Sequential,Pack=1)]
	public struct ChildStruct2_Out	//�Ѳ��w�s���c
	{
        public TByte22 abyAccount;           //�b��
        public TByte3 abyCurrencyType;       //���O
        public byte byMarketNo;              //�����N�X
        public TByte30 abyMarketName;        //�����W��
        public TByte12 abyStkCode;           //�Ѳ��N�X
        public TByte30 abyStkName;           //�Ѳ��W��
        public TByte60 abyStkFullName;       //�Ѳ����W
        public long lngStockQty;             //�w�s�Ѽ�
        public long lngTradingQty;           //�i����Ѽ�
        public long lngPrice;                //���槡��
        public long lngCost;                 //��������
        public int intCloseRate;             //�ײv
        public byte byRateKind;              //�ײv�B��Ҧ�
        public uint uintLotSize;             //������
        public int intMarketPrice;           //����
        public short shtDecimal;             //�p�Ʀ��
        public int intBuyPrice;              //�R��
        public int intSellPrice;             //���        
	}
}
