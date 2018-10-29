create table PayOrder--����֧��
(
  id int identity(1,1) primary key,
  orderNo nvarchar(100),--������
  userId int,
  orderType int,--�������� 1��Ա���� 
  orderState int,--1 δ���� 2�Ѹ��� 3��ȡ��
  item nvarchar(100) default(''),--��Ŀ member--�����Ա shopowner����곤 shopkeeper�������
  amount decimal(18,2) default(0),--���
  payAmount decimal(18,2) default(0),--֧�����
  payTime datetime,--֧��ʱ��
  IsRebate bit,--�Ƿ���
  remark nvarchar(200),--��ע
  submitTime datetime --�µ�ʱ��
)
select top 1000 r.rebateId,r.reateAmount,r.[type],u.DisplayName,r.submitTime from fz_Rebate r
 join fz_Orders o on r.orderDetailId=o.orderId join Users u on u.UserId=o.userId where r.accountId='1' 
 union  all
select top 1000 r1.rebateId,r1.reateAmount,r1.[type],u1.DisplayName,r1.submitTime from fz_Rebate r1
 join PayOrder o1 on r1.orderDetailId=o1.id join Users u1 on u1.UserId=o1.userId where r1.accountId='1'