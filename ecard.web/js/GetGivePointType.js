(function ($) {
    $.extend($, {

        GetType: function (data) {
            if (data.length <= 0) {
                return null;
            }
            var a = { 1: "推荐会员/商家", 2: "会员注册", 3: "网络管理费", 4: "积分激活", 5: "提现管理费", 6: "提现", 7: "取消提现", 8: "积分消费", 9: "撤销积分消费", 10: "撤销会员消费返利", 11: "锁定商家交易返利", 12: "撤销锁定商家交易返利",
                13: "会员积分消费", 14: "消费", 15: "锁定会员消费返利", 16: "提现手续费", 17: "增加积分操作", 18: "减少积分操作"
            };
            return a[data];
           
        }

    })

    $.extend($, {
        getDealTypesStr: function (data) {
            if (data.length <= 0) {
                return null;
            }
            var a = { pos: "银联pos刷卡", shop: "商城消费", posPoint: "pos积分消费", shopPoint: "商城积分消费", Cancel: "撤销", Rollback: "冲正"
            };
            return a[data];
        }

    })
    $.extend($, {
        getExStateStr: function (data) {
            if (data.length <= 0) {
                return null;
            }
            var a = { 1: "等待确认收货", 2: "交易完成", 3: "已撤销", 4: "已冲正"
            };
            return a[data];
        }

    })
    $.extend($, {
        getOperatorType: function (data) {
            if (data.length <= 0) {
                return null;
            }
            var a = { 1: "提成设置", 2: "积分增减", 3: "短信模板", 4: "短信参数", 5: "商户管理", 6: "代理商管理", 7: "提现管理", 8: "贷款管理", 9: "招商员管理", 10: "会员管理" };
            return a[data];
        }
    })

    $.extend($, {
        getAccountType: function (data) {
            if (data.length <= 0) {
                return null;
            }
            var a = { MemberUser: "会员", ShopUser: "商户", PAgentUser: "省代理商", CAgentUser: "市代理商", DAgentUser: "区县代理商", SuperAdmin: "超级管理员", WebSite: "网点", Attract: "招商员", AdminUser: "子管理员(总)", SubAdminUser: "子管理员(非总)" };
            return a[data];
        }
    })

} (jQuery));