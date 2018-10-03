select '过期卡' as accountType, isnull(sum(amount), 0) amount, isnull(sum(depositAmount), 0) depositAmount from accounts where (state = 1 or state = 3) and expiredDate < getdate() 
union 
select tt.DisplayName as accountType, isnull(sum(t1.amount), 0) amount, isnull(sum(t1.depositAmount), 0) depositAmount from accountTypes tt left join accounts t1 on t1.accountTypeId = tt.accountTypeId and (t1.state = 1 or t1.state = 3) and t1.expiredDate > getdate() group by tt.displayName 
union  
select '无效卡' as accountType, isnull(sum(t1.amount), 0) amount, isnull(sum(t1.depositAmount), 0) depositAmount from accounts t1 where (t1.state <> 1 and t1.state <> 3)