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
select * from PayOrder