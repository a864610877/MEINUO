create table PayOrder--订单支付
(
  id int identity(1,1) primary key,
  orderNo nvarchar(100),--订单号
  userId int,
  orderType int,--订单类型 1会员升级 
  orderState int,--1 未付款 2已付款 3已取消
  item nvarchar(100) default(''),--项目 member--购买会员 shopowner购买店长 shopkeeper购买店主
  amount decimal(18,2) default(0),--金额
  payAmount decimal(18,2) default(0),--支付金额
  payTime datetime,--支付时间
  IsRebate bit,--是否返利
  remark nvarchar(200),--备注
  submitTime datetime --下单时间
)
select top 1000 r.rebateId,r.reateAmount,r.[type],u.DisplayName,r.submitTime from fz_Rebate r
 join fz_Orders o on r.orderDetailId=o.orderId join Users u on u.UserId=o.userId where r.accountId='1' 
 union  all
select top 1000 r1.rebateId,r1.reateAmount,r1.[type],u1.DisplayName,r1.submitTime from fz_Rebate r1
 join PayOrder o1 on r1.orderDetailId=o1.id join Users u1 on u1.UserId=o1.userId where r1.accountId='1'