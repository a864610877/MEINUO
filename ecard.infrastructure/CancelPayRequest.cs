using System.Runtime.Serialization;

namespace Ecard.Infrastructure
{
    /// <summary>
    /// ������������
    /// </summary>
    [DataContract]
    public class CancelPayRequest
    {
        public CancelPayRequest(string accountName, string password, string posName, decimal amount, string serialNo, string oldSerialNo, string userToken, string shopName)
        {
            AccountName = accountName;
            Password = password;
            PosName = posName;
            Amount = amount;
            OldSerialNo = oldSerialNo;
            SerialNo = serialNo;
            UserToken = userToken;
            ShopName = shopName;
        }
        /// <summary>
        /// ��Ա����
        /// </summary>
        [DataMember]
        public string AccountName { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// �����ն˺�
        /// </summary>
        [DataMember]
        public string PosName { get; set; }
        /// <summary>
        /// ������� 
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// ���������׵���ˮ��
        /// </summary>
        [DataMember]
        public string OldSerialNo { get; set; }
        /// <summary>
        /// ���ν�����ˮ��
        /// </summary>
        [DataMember]
        public string SerialNo { get; set; }
        /// <summary>
        /// ��Ա������
        /// </summary>
        [DataMember]
        public string UserToken { get; set; }
        /// <summary>
        /// �����̻���
        /// </summary>
        [DataMember]
        public string ShopName { get; set; }
        /// <summary>
        /// ���ܷ��̻���
        /// </summary>
        [DataMember]
        public string ShopNameTo { get; set; }
        /// <summary>
        /// �Ƿ� ϵͳǿ�Ʒ���
        /// </summary>
        [DataMember]
        public bool IsForce { get; set; }
    }
}