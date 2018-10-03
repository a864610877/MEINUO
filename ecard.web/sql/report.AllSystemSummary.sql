select '系统余额' dealType, amount amountIn, 0 amountOut, 1 orderNum from siteaccounts
union all (select '商户余额' dealType, 0 , sum (amount), 2 from shops) 
union all (select '商户手续费' dealType,  0 ,sum (rechargingamount), 3 from shops) 
union all (select '帐户余额' dealType, 0 , sum (Amount), 4 from accounts where state in (1,2, 14)) 
union all (select '帐户冰结余额' dealType, 0,sum (FreezeAmount), 5 from accounts where state in (1,2, 14)) 
union all (select '帐户押金' dealType,  0 , sum (DepositAmount), 6 from accounts where state in (1,2, 14)) 
union all (select '帐户手续费支付' dealType,  0 , sum (ChargingAmount), 7 from accounts where state in (1,2, 13,14, 15)) 

union all	(select '汇总' dealType, 
			(select amount from siteaccounts) ,
		   (select sum (amount) from shops) 
		  + (select sum (rechargingamount) from shops) 
		  + (select sum (Amount) from accounts where state in (1,2, 14)) 
		  + (select sum (FreezeAmount) from accounts where state in (1,2, 14)) 
		  + (select sum (DepositAmount) from accounts where state in (1,2, 14)) 
		  + (select sum (ChargingAmount) from accounts where state in (1,2, 13,14, 15)) 
		 ,100)