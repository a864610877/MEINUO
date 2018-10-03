using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models
{
    public class AlipayConfig
    {
        /// <summary>
        /// 支付宝网关
        /// </summary>
        public const string URL = "https://openapi.alipay.com/gateway.do";
        public const string APPID = "2017061907519441";
        /// <summary>
        /// 开发者应用私钥，由开发者自己生成
        /// </summary>
        public const string APP_PRIVATE_KEY = "MIICXQIBAAKBgQDCZb95DBLqRbHAV2Uhi4yQULOX48n76iDIhPBC+mxITk3Net0wN7JGjFbvfgDqjFghCN+1pcElY9W5moAYVeKsPn/j8yC1uPOL7/xoDhkuNs16MSdV3ozGdAhkRQyg3NlTSpzGUxdIYhFzKjDgI8RVWZ45JcJ9sNXsF2O5C90NsQIDAQABAoGAFQD7XC/Sx186YmbO9X3ndRxTG0EwbLiSTDgY4ZO/KVzUiTQSPAh4iajWJ9A8dxss1nzn9u9u3ARablBkMLzu3a8nHi2T5t5ooDs/yFpkbmByTD83Gfh2YDe4J+0+R4MSITwhRBY2WheBIlsSEjmRF0PKSSOX6RFPBsv75SpvFPECQQDry5wh8B2uQZWxKf3SyHXaYjh6/JD/WJBmFiZr4W3db/+eW6EDAh+MLTWupS9q9MXKBWnOmJ2phIFaaoLqb00VAkEA0w4JZEs4++1EFmb7fQym+c4cPQqoiyvxk7w93rqXWJPwtmU3/JbdpG0MaluGEwmXsx2RDZcbGEU6wz01wdS9LQJBALBtLGmIS+zyTZq9nJl2PBgmnbQH/kXQclqwABeAGMAy6MQIMzUZBZnQyfXeytfwRX2fB0f5kR4hctfAEixvEvECQGw1WSlby+aWen9F45D0qLORMjc1vL5GFIDbVZlZb3lRuGu7r53It/CynFf3fuFJ3MZP1WvzWkfyTrOFMYekjC0CQQDiyhzogNeh+vcLYIfPlmBaNOpfsHGv/Mwvfg5QcWf3YliBQt7yOKceRlj62ccNdyf8qIWxhaadpdFxuSQL4EUC";
        // public const string APP_PUBLIC_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCt9dCiST0o7l9tjoLNNm0hkpl9eEdxQa2RDHDSxrJUZVIw09wgRVIqm5hLYk7rm6soLg93M7O1D6HMxudNpIgJZkOMzRkLqNQRB/UPee0QirIqXAFtUbTPZ0x/TVAkm04V33ZczDIK61QIoXOx/+rlgOpZZivMOoGCKeFK6jQIhQIDAQAB";

        public const string CHARSET = "UTF-8";
        /// <summary>
        /// 支付宝公钥，由支付宝生成
        /// </summary>
        public const string ALIPAY_PUBLIC_KEY = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";

        public const string NOTIFY_URL = "http://wx.fislive.com/NotifyUrl/AlipayNotifyUrl.aspx";
        /// <summary>
        /// 合作伙伴身份（PID）
        /// </summary>
        public const string PARTNER = "2088721127410283";
        /// <summary>
        /// 帐号
        /// </summary>
        public const string SELLER_ID = "suixingbao0518@163.com";
    }
}