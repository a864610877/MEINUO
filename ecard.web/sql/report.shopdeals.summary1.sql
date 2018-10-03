
select '' shopName,  '' shopdisplayName,
isnull(sum(payAmount), 0.0) payAmount, 
isnull(sum(cancelAmount), 0.0) cancelAmount, 
isnull(sum(donePrepayAmount), 0.0) donePrepayAmount, 
isnull(sum(cancelDonePrepayAmount), 0.0) cancelDonePrepayAmount,
0.0 ShopDealLogChargeRate, 
isnull(sum(ShopDealLogDoneAmount), 0.0) ShopDealLogDoneAmount, 
isnull(sum(ShopDealLogChargeAmount), 0.0) ShopDealLogChargeAmount 
 from reportshopdeals where 
(@DayMin is null or [submitdate] >= @DayMin) 
and (@DayMax is null or [submitdate] <= @DayMax) 
and (@ShopName is null or shopName = @ShopName)
and exists (select * from shops s where s.shopId = shopid and (@ShopDisplayName is null or s.DisplayName like '%' + @ShopDisplayName + '%'))
 