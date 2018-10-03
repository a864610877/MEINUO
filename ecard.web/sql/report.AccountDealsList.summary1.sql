select '' SerialNo, '' submittime, '' AccountName,'' AccountShopName, '' displayName, 
	'' SourcePosName, isnull(sum(lg.Amount), 0.0) amount
	from deallogs lg inner join accounts acc on lg.accountid = acc.accountid
	where lg.state = 1 and lg.dealtype in (1, 4,8)
	and (@start is null or lg.submittime >= @start) 
	and (@end is null or lg.submittime < @end)
	and (@shopId is null or lg.shopId = @shopId)
	and (@accountShopId is null or acc.shopid = @accountShopId)
