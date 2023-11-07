using System;
using System.Runtime.InteropServices;
using YuantaShareStructList;

namespace SgnAPILogin
{
	/// <summary>
	/// �����c(Input)
	/// </summary>
	[StructLayout(LayoutKind.Sequential,Pack=1)]
	public struct ParentStruct_In
	{
        public TByte22 abyAccount;      //�b��	TByte22
        public TByte20 abyPWD;          //�K�X	TByte20
	}
	/// <summary>
	/// ��X�������c
	/// </summary>
	[StructLayout(LayoutKind.Sequential,Pack=1)]
	public struct ParentStruct_Out
	{
        public TByte5 abyMsgCode;	    //�T���N�X	TByte5
        public TByte50 abyMsgContent;	//����T��	TByte50
        public uint uintCount;	        //����	Uint
	}

	/// <summary>
	/// �{�f�l���c(Output)
	/// </summary>
	[StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct ChildStruct_Out
	{
		public TByte22 abyAccount;	    //�b��	TByte22
        public TByte12 abySubAccName;	//����W��	TByte12
        public TByte14 abyInvesotrID;	//�����Ҧr��/�νs	TByte14
        public short shtSellerNo;	    //��~���N�X	short
	}

}
