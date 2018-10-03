
select '' accountName, 
'' AccountShopName,
isnull(sum(payAmount), 0.0) payAmount, 
isnull(sum(cancelAmount), 0.0) cancelAmount, 
isnull(sum(donePrepayAmount), 0.0) donePrepayAmount, 
isnull(sum(cancelDonePrepayAmount), 0.0) cancelDonePrepayAmount, 
isnull(sum(payAmount) + sum(donePrepayAmount), 0.0) totalDealAmount ,
isnull(sum(cancelAmount) + sum(cancelDonePrepayAmount), 0.0) totalCancelAmount ,
isnull(sum(payAmount) + sum(donePrepayAmount) + sum(cancelAmount) + sum(cancelDonePrepayAmount), 0.0) amount,
isnull(sum(payCount) + sum(donePrepayCount) + sum(cancelCount) + sum(cancelDonePrepayCount),0) [count],
isnull(sum(unpaycount),0) unpaycount
 from reportaccountdeals where 
	(@DayMin is null or [submitdate] >= @DayMin) 
	and (@DayMax is null or [submitdate] <= @DayMax) 
	and (@accountShopId is null or exists(select * from accounts acc where acc.accountid = accountid and acc.shopid = @accountShopId))

 