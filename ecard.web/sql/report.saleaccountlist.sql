select dbo.PadLeft(convert(nvarchar(10), logid), '0', 12) as logid, acc.Name, 
	isnull((select s.displayname from shops s where s.shopid = acc.shopid), '系统总部') AccountShopName,
	accType.displayName, 
	lg.UserName
	from logs lg left join accounts acc on lg.addin = acc.accountid left join accountTypes accType on accType.accounttypeid = acc.accounttypeid
	where LogType = @LogType 
		and (@start is null or @start <= submittime) 
		and (@end is null or @end > submittime)
	and (@accountShopId is null or acc.shopid = @accountShopId)
	