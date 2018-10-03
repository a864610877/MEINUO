select '' SystemDealLogId, 
		'' Name, 
		'' AccountShopName,
		'' submittime, '' howtodeal, isnull(sum(t.amount), 0.0) amount, '' UserName 
from systemdeallogs t left join accounts acc on t.addin = acc.accountid
where dealtype = 3 
	and (@start is null or submittime >= @start) and (@end is null or submittime < @end)
	and (@accountShopId is null or acc.shopid = @accountShopId)
	and t.state = 1