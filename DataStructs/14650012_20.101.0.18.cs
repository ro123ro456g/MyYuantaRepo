using System;
using System.Runtime.InteropServices;
using YuantaShareStructList;
using System.ComponentModel;

namespace RptOrderTradeReportGroupIV
{
    /// <summary>
    /// �����c(Input)-Count
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct_In
    {
        public TByte abyNoListCancel;			//�O�_�C�X���P��
        public uint uintCount;
    }

    /// <summary>
    /// �{�f�l���c(Input)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct_In
    {
        public TByte22 abyAccount;
    }

    /// <summary>
    /// ������c(Input)-Count
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct1_Out
    {
        public uint uintCount;
    }

    /// <summary>
    /// �{�f�l���c(Output)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct1_Out
    {
        public TByte22 abyAccount;	        //�b��
        public TYuantaDate struTradeYMD;	//�����
        public byte byMarketNo;             //�����N�X
        public TByte30 abyMarketName;		//�����W��
        public TByte12 abyCompanyNo;		//�Ѳ��N�X
        public TByte30 abyStkName;			//�Ѳ��W��
        public short shtOrderType;			//�e�U����
        public TByte abyBS;					//�R��O
        public long lngPrice;				//����
        public TByte abyPriceFlag;			//����e�U���� char(1) (M-����, L-����)        
        public int intBeforeQty;			//�e�@���e�U�q
        public int intAfterQty;			    //�ثe�e�U�q
        public int intOkQty;				//����q
        public short shtOrderStatus;		//���A�X
        public TYuantaDate struAcceptDate;	//������
        public TYuantaTime struAcceptTime;	//����ɶ�
        public TByte5 abyOrderNo;			//�e�U��s��
        public TByte5 abyOrderErrorNo;		//���~�X
        public TByte120 abyEmError;			//���~�T��
        public short shtSeller;				//��~���s��
        public TByte3 abyChannel;			//Channel
        public short shtAPCode;				//APCode
        public int intOTax;					//O_TAX
        public int intOCharge;				//O_Charge
        public int intODueAmt;				//�����I
        public TByte abyCancelFlag;			//�i����Flag
        public TByte abyReduceFlag;			//�i��qFlag
        public TByte abyTraditionFlag;      //�ǲγ�Flag	TByte	1
        public TByte10 abyBasketNo;
        public TByte3 abyTradeCurrency;     // ���O
        public TByte abyTime_in_Force;      //�e�U�Ĵ� char(1) R:��馳�� I:IOC F:FOK
        public TByte abyOrder_Success;      //�e�U���\�X�� CHAR(1) 
        public TByte abyReduce_Flag;        //���e�U�U��O�_�Q��q CHAR(1) 
        public TByte abyChg_Prz_Flag;       //���e�U�U��O�_�i���� CHAR(1) 
        public TByte abyTSE_Cancel;         //���e�U�U��O�_�Q����ҥD�ʧR�� CHAR(1)
        public int intCancelQty;		    //�����ƶq
        public int intOR_Qty;   		    //��e�U�q        
        public TYuantaDate struUpdateDate;	//��s���
        public TYuantaTime struUpdateTime;	//��s�ɶ�
    }
    /// <summary>
    /// ������c(Input)-Count
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct2_Out
    {
        public uint uintCount;
    }

    /// <summary>
    /// �{�f�l���c(Output)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct2_Out
    {
        public TByte22 abyAccount;	        // �b��
        public byte byMarketNo;             // �����N�X
        public TByte30 abyMarketName;       // �����W��
        public TByte12 abyCompanyNo;        // �Ѳ��N�X
        public TByte30 abyStkName;			// �Ѳ��W��
        public short shtOrderType;			// �e�U����
        public TByte abyBS;					// �R��O
        public int intOKStkNos;				// ����q
        public long lngOPrice;				// �e�U��
        public long lngSPrice;				// �����
        public TYuantaDateTime struDateTime;// ����~���ɤ���
        public TByte5 abyOrderNo;			// �e�U��s��
        public TByte3 abyTradeCurrency;     // ���O
        public TByte abyPrice_Flag;         // �e�U����Flag
        public short shtExchange_Code;      // �e�U�O 0-�@��e�U 1-�d�B 2-�s�� 4-�L��w�� 5 �L���s��
    }
    //�����c(Output)
    public struct ParentStruct3_Out
    {
        public uint uintCount; //��X����
    }
    //--------------------
    //�l���c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct3_Out
    {
        public TByte22 abyAccount;          //�b��
        public TYuantaDate struTradeDate;   //������
        public byte byMarketNo;             //�����N�X
        public TByte30 abyMarketName;		//�����W��
        public TByte7 abyCommodityID1;      //�ӫ~�W��1
        public int intSettlementMonth1;     //�ӫ~���1
        public int intStrikePrice1;         //�i����1
        public TByte abyBuySellKind1;       //�R��O1
        public TByte7 abyCommodityID2;      //�ӫ~�W��2
        public int intSettlementMonth2;     //�ӫ~���2
        public int intStrikePrice2;         //�i����2
        public TByte abyBuySellKind2;       //�R��O2
        public TByte abyOpenOffsetKind;     //�s/����
        public TByte abyOrderCondition;	    //�e�U����	
        public TByte10 abyOrderPrice;       //�e�U��
        public int intBeforeQty;            //��q�e�e�U�f��
        public int intAfterQty;             //��q��e�U�f��
        public int intOKQty;                //����f��
        public short shtStatus;             //�e�U���A
        public TYuantaDate struAcceptDate;	//������
        public TYuantaTime struAcceptTime;	//������
        public TByte10 abyErrorNo;          //���~�N�X
        public TByte120 abyErrorMessage;    //���~�T��
        public TByte5 abyOrderNO;           //�e�U�渹
        public TByte abyProductType;        //�ӫ~����
        public ushort ushtSeller;           //��~���N�X
        public long lngTotalMatFee;         //����O
        public long lngTotalMatExchTax;     //�ҥ�|
        public long lngTotalMatPremium;     //�_�������I
        public TByte abyDayTradeID;         //��R���O
        public TByte abyCancelFlag;			//�i����Flag
        public TByte abyReduceFlag;			//�i��qFlag
        public TByte30 abyStkName1;			//�ӫ~�W��1
        public TByte30 abyStkName2;			//�ӫ~�W��2
        public TByte abyTraditionFlag;      //�ǲγ�Flag	TByte	1
        public TByte20 abyTRID;	            //�ӫ~�N�X
        public TByte3 abyCurrencyType;      //������O
        public TByte3 abyCurrencyType2;     //��ι��O
        public TByte10 abyBasketNo;         //BasketNo
        public byte byMarketNo1;            //�����N�X1
        public TByte12 abyStkCode1;         //�污�Ѳ��N�X1
        public byte byMarketNo2;            //�����N�X2
        public TByte12 abyStkCode2;         //�污�Ѳ��N�X2
    }
    //�����c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct4_Out
    {
        public uint uintCount; //��X����
    }
    //--------------------
    //�l���c(Output)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct4_Out
    {
        public TByte22 abyAccount;              //�b��
        public byte byMarketNo;                 //�����N�X
        public TByte30 abyMarketName;			//�����W��
        public TByte7 abyCommodityID1;          //�ӫ~�W��1
        public int intSettlementMonth1;         //�ӫ~���1
        public TByte abyBuySellKind1;           //�R��O1
        public int intMatchQty;                 //����f��
        public long lngMatchPrice1;             //�����1
        public long lngMatchPrice2;             //�����2
        public TYuantaTime struMatchTime;       //����ɶ�
        public TYuantaDate struMatchDate;       //������
        public TByte5 abyOrderNo;               //�e�U�渹
        public int intStrikePrice1;             //�i����1
        public TByte7 abyCommodityID2;          //�ӫ~�W��2
        public int intSettlementMonth2;         //�ӫ~���2
        public TByte abyBuySellKind2;           //�R��O2
        public int intStrikePrice2;             //�i����2
        public TByte abyRecType;                //�榡��/�Ʀ���
        public TByte abyProductType;            //�ӫ~����
        public long intOrderPrice;              //�e�U��
        public TByte30 abyStkName1;				//�ӫ~�W��1
        public TByte30 abyStkName2;				//�ӫ~�W��2
        public TByte abyDayTradeID;             //��R���O
        public long lngSprMatchPrice;           //�Ʀ��榨���
        public TByte20 abyTRID;	                //�ӫ~�N�X
        public TByte3 abyCurrencyType;          //������O
        public TByte3 abyCurrencyType2;         //��ι��O
        public TByte abySub_No;                 //�l����Ǹ�
    }
    /// <summary>
    /// ������c(Input)-Count
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct5_Out
    {
        public uint uintCount;
    }

    /// <summary>
    /// �{�f�l���c(Output)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct5_Out
    {
        public TByte22 abyAccount;	                //�b��
        public TYuantaDate struTradeYMD;		    //�����
        public byte byMarketNo;                     //�����N�X
        public TByte30 abyMarketName;			    //�����W��
        public TByte12 abyCompanyNo;			    //�Ѳ��N�X
        public TByte30 abyStkName;			        //�Ѳ��W��
        public TByte abyBS;					        //�R��O
        public TByte3 abyCurrencyType;              //������O
        public long lngPrice;				        //����
        public TByte3 abyPriceType;			        //���櫬�A
        public int intOrderQty;				        //�e�U�q
        public int intMatchQty;				        //����q
        public short shtOrderStatus;		        //���A�X
        public TYuantaDateTime struOrderTime;		//�e�U�ɶ�
        public TByte3 abyOrderType;			        //�e�U�櫬�A
        public TByte7 abyOrderNo;			        //�e�U��s��
        public int intFee;                          //����O
        public long lngAMT;                         //�����I���B
        public TByte8 abyOrderErrorNo;		        //���~�X
        public TByte180 abyEmError;			        //���~�T��
        public TByte3 abyCurrencyType2;             //��ι��O
        public TByte abyCancelFlag;					//�i����Flag
        public TByte abyReduceFlag;					//�i��qFlag
        public TByte abyTraditionFlag;              //�ǲγ�Flag	TByte	1
        public TByte abySettleType;                 //��Τ覡
        public TByte10 abyBasketNo;                 //BasketNo
    }
    /// <summary>
    /// ������c(Input)-Count
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct6_Out
    {
        public uint uintCount;
    }

    /// <summary>
    /// �{�f�l���c(Output)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct6_Out
    {
        public TByte22 abyAccount;	//�b��
        public byte byMarketNo;                     //�����N�X
        public TByte30 abyMarketName;			    //�����W��
        public TByte12 abyCompanyNo;			    //�Ѳ��N�X
        public TByte30 abyStkName;			        //�Ѳ��W��
        public TByte abyBS;					        //�R��O
        public TByte3 abyCurrencyType;              //������O
        public int intMatchQty;				        //����q
        public long lngOrderPrice;				    //����
        public long lngMatchPrice;				    //����
        public TYuantaDateTime struDateTime;        //����ɶ� 
        public int intFee;                          //����O
        public TByte7 abyOrderNo;			        //�e�U��s��
        public long lngSettlementAMT;               //������B
        public TByte3 abyCurrencyType2;             //��ι��O
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct7_Out
    {
        public uint uintCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct7_Out
    {
        public TByte22 abyAccount;              //�b��
        public TYuantaDate struTradeDate;       //������
        public byte byMarketNo;                 //�����N�X
        public TByte30 abyMarketName;			//�����W��
        public TByte7 abyCommodityID;           //�ӫ~�W��
        public int intSettlementMonth;          //�ӫ~���
        public TByte30 abyStkName;              //�ӫ~�W��
        public TByte abyBuySell;                //�R��O
        public TByte3 abyOrderType;             //�e�U�覡
        public TByte14 abyOdrPrice;             //�e�U��
        public TByte14 abyTouchPrice;           //���l�����
        public int intOrderQty;                 //�e�U�f��
        public int intMatchQty;                 //����f��
        public short shtOrderStatus;            //���A�X
        public TYuantaDate struAcceptDate;		//������
        public TYuantaTime struAcceptTime;		//������
        public TByte10 abyErrorNo;		        //���~�X
        public TByte120 abyErrorMessage;	    //���~�T��
        public TByte8 abyOrderNo;               //�e�U�ѽs��
        public TByte abyDayTradeID;             //��R���O
        public TByte abyCancelFlag;             //�i����Flag
        public TByte abyReduceFlag;             //�i��qFlag
        public long lngUtPrice;                 //�e�U�����Ʀ�
        public int intUtPrice2;                 //�e�U������l
        public int intMinPrice2;                //�e�U�������
        public long lngUtPrice4;                //���l�������Ʀ�
        public int intUtPrice5;                 //���l���������l
        public int intUtPrice6;                 //���l����������
        public TByte abyTraditionFlag;          //�ǲγ�Flag	TByte	1
        public TByte10 abyBasketNo;             //Basketno
        public byte byMarketNo1;                //�����N�X1
        public TByte12 abyStkCode1;             //�污�Ѳ��N�X1
        public TByte3 abyCurrencyType;          //������O
        public TByte3 abyCurrencyType2;         //��ι��O
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParentStruct8_Out
    {
        public uint uintCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChildStruct8_Out
    {
        public TByte22 abyAccount;              //�b��
        public byte byMarketNo;                 //�����N�X
        public TByte30 abyMarketName;			//�����W��
        public TByte7 abyCommodityID;           //�ӫ~�W��
        public int intSettlementMonth;          //�ӫ~���
        public TByte30 abyStkName;              //�ӫ~�W��
        public TByte abyBuySell;                //�R��O
        public int intMatchQty;                 //����f��
        public TByte14 abyOdrPrice;             //�e�U��
        public TByte14 abyMatchPrice;           //�����
        public TYuantaDate struMatchDate;		//������
        public TYuantaTime struMatchTime;		//������
        public TByte8 abyOrderNo;               //�e�U�ѽs��
        public TByte3 abyCurrencyType;          //������O
        public TByte3 abyCurrencyType2;         //��ι��O
    }
}
