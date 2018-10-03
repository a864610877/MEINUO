
select s.Name ShopName, s.DisplayName ShopDisplayName, 
isnull((select sum(l.payAmount) from reportshopdeals l where l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) payAmount, 
isnull((select sum(l.cancelAmount) from reportshopdeals l where  l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) cancelAmount, 
isnull((select sum(l.donePrepayAmount) from reportshopdeals l where  l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) donePrepayAmount, 
isnull((select sum(l.cancelDonePrepayAmount) from reportshopdeals l where  l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) cancelDonePrepayAmount, 
isnull(s.ShopDealLogChargeRate, (select top 1 ShopDealLogChargeRate from sites) ) ShopDealLogChargeRate,
isnull((select sum(l.ShopDealLogDoneAmount) from reportshopdeals l where  l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) ShopDealLogDoneAmount, 
isnull((select sum(l.ShopDealLogChargeAmount) from reportshopdeals l where  l.shopid = s.shopid and (@DayMin is null or [submitdate] >= @DayMin) and (@DayMax is null or [submitdate] < @DayMax) ), 0.0) ShopDealLogChargeAmount
from shops s  
where (@ShopDisplayName is null or s.DisplayName like '%' + @ShopDisplayName + '%') and (@ShopName is null or s.Name = @ShopName)
 