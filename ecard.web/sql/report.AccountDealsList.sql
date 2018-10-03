select lg.SerialNo, convert(nvarchar(50), lg.submittime, 120) submittime, lg.AccountName, 
	isnull((select s.displayname from shops s where s.shopid = acc.shopid), '系统总部') AccountShopName,
	(select accType.displayName from accounts acc inner join accountTypes accType on acc.AccountTypeId = accType.accountTypeId and acc.AccountId = lg.AccountId) displayName, 
	lg.SourcePosName, lg.Amount 
	from deallogs lg inner join accounts acc on lg.accountid = acc.accountid
	where lg.state = 1 and lg.dealtype in (1, 4,8)
	and (@start is null or lg.submittime >= @start) 
	and (@end is null or lg.submittime < @end)
	and (@shopId is null or lg.shopId = @shopId)
	and (@accountShopId is null or acc.shopid = @accountShopId)
