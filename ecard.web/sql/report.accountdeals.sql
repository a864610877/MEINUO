select s.Name accountName, 
isnull((select sh.displayname from shops sh where sh.shopid = s.shopid), '系统总部') AccountShopName,
isnull(l.payAmount, 0.0) payAmount, 
--isnull(l.payCount, 0.0) payCount, 
isnull(l.cancelAmount, 0.0) cancelAmount, 
--isnull(l.cancelCount, 0.0) cancelCount, 
isnull(l.donePrepayAmount, 0.0) donePrepayAmount, 
--isnull(l.donePrepayCount, 0.0) donePrepayCount, 
isnull(l.cancelDonePrepayAmount, 0.0) cancelDonePrepayAmount, 
--isnull(l.cancelDonePrepayCount, 0.0) cancelDonePrepayCount, 
isnull(l.payAmount + l.donePrepayAmount, 0.0) totalDealAmount,
isnull(l.cancelAmount + l.cancelDonePrepayAmount, 0.0) totalCancelAmount,
isnull(l.payAmount + l.donePrepayAmount + l.cancelAmount + l.cancelDonePrepayAmount, 0.0) amount,
isnull(l.payCount + l.donePrepayCount + l.cancelCount + l.cancelDonePrepayCount, 0) [count],
isnull(l.UnPayCount, 0) UnPayCount
from accounts s left join 

		(select accountId, 
				isnull(sum(payAmount), 0.0) payAmount, 
				isnull(sum(payCount), 0.0) payCount, 
				isnull(sum(cancelAmount), 0.0) cancelAmount, 
				isnull(sum(cancelCount), 0.0) cancelCount, 
				isnull(sum(donePrepayAmount), 0.0) donePrepayAmount, 
				isnull(sum(donePrepayCount), 0.0) donePrepayCount, 
				isnull(sum(cancelDonePrepayAmount), 0.0) cancelDonePrepayAmount ,
				isnull(sum(cancelDonePrepayCount), 0.0) cancelDonePrepayCount,
				isnull(sum(UnPayCount), 0.0) UnPayCount
				from reportaccountdeals where 
				(@DayMin is null or [submitdate] >= @DayMin) 
				and (@DayMax is null or [submitdate] <= @DayMax) 
				and (@accountName is null or accountName = @accountName)
				group by accountId) l 
on s.accountId = l.accountId
where s.state in (1,2)
	and (@accountShopId is null or s.shopid = @accountShopId)
 