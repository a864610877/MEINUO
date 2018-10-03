using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Com.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：1.0
    /// 修改日期：2016-06-06
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class Config
    {

        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://openhome.alipay.com/platform/keyManage.htm?keyType=partner
        public static string partner = "2088721127410283";

        // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public static string seller_id = partner;
		
		//商户的私钥,原始格式，RSA公私钥生成：https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.nBDxfy&treeId=58&articleId=103242&docType=1
        public static string private_key = "MIICXQIBAAKBgQDCZb95DBLqRbHAV2Uhi4yQULOX48n76iDIhPBC+mxITk3Net0wN7JGjFbvfgDqjFghCN+1pcElY9W5moAYVeKsPn/j8yC1uPOL7/xoDhkuNs16MSdV3ozGdAhkRQyg3NlTSpzGUxdIYhFzKjDgI8RVWZ45JcJ9sNXsF2O5C90NsQIDAQABAoGAFQD7XC/Sx186YmbO9X3ndRxTG0EwbLiSTDgY4ZO/KVzUiTQSPAh4iajWJ9A8dxss1nzn9u9u3ARablBkMLzu3a8nHi2T5t5ooDs/yFpkbmByTD83Gfh2YDe4J+0+R4MSITwhRBY2WheBIlsSEjmRF0PKSSOX6RFPBsv75SpvFPECQQDry5wh8B2uQZWxKf3SyHXaYjh6/JD/WJBmFiZr4W3db/+eW6EDAh+MLTWupS9q9MXKBWnOmJ2phIFaaoLqb00VAkEA0w4JZEs4++1EFmb7fQym+c4cPQqoiyvxk7w93rqXWJPwtmU3/JbdpG0MaluGEwmXsx2RDZcbGEU6wz01wdS9LQJBALBtLGmIS+zyTZq9nJl2PBgmnbQH/kXQclqwABeAGMAy6MQIMzUZBZnQyfXeytfwRX2fB0f5kR4hctfAEixvEvECQGw1WSlby+aWen9F45D0qLORMjc1vL5GFIDbVZlZb3lRuGu7r53It/CynFf3fuFJ3MZP1WvzWkfyTrOFMYekjC0CQQDiyhzogNeh+vcLYIfPlmBaNOpfsHGv/Mwvfg5QcWf3YliBQt7yOKceRlj62ccNdyf8qIWxhaadpdFxuSQL4EUC";

        //支付宝的公钥，查看地址：https://b.alipay.com/order/pidAndKey.htm 
        public static string alipay_public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";
        
        // 签名方式
        public static string sign_type = "RSA";

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public static string log_path = HttpRuntime.AppDomainAppPath.ToString() + "log/";

        // 字符编码格式 目前支持 gbk 或 utf-8
        public static string input_charset = "utf-8";

        // 支付类型 ，无需修改
        public static string payment_type = "1";

        // 调用的接口名，无需修改
        public static string service = "mobile.securitypay.pay";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

    }
}