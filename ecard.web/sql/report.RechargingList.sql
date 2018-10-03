select dbo.PadLeft(Convert(nvarchar(50), t.SystemDealLogId), '0', 12) SystemDealLogId, 
       acc.Name, 
	   isnull((select s.displayname from shops s where s.shopid = acc.shopid), '系统总部') AccountShopName,
	   Convert(nvarchar(50), t.submittime, 120 ) submittime, t.howtodeal, t.amount, t.UserName 
from systemdeallogs t left join accounts acc on t.addin = acc.accountid
where dealtype = 3 and (@start is null or submittime >= @start) and (@end is null or submittime < @end)
	and (@accountShopId is null or acc.shopid = @accountShopId)
	and t.state = 1