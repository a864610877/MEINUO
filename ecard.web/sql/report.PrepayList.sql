select t.serialNo, t.accountName, AccountShopName, convert(nvarchar(20), t.PrePaySubmitTime, 120) PrePaySubmitTime, t.Amount, convert(nvarchar(20), t.DoneSubmitTime, 120 ) DoneSubmitTime, t.DoneAmount from (
	select 
		(select top 1 lg.SerialNo from deallogs lg where lg.addin = pp.PrePayId and dealtype = 3 and state = 1) serialNo, 
		acc.Name accountName, 
		isnull((select s.displayname from shops s where s.shopid = acc.shopid), '系统总部') AccountShopName,
		(select top 1 lg.SubmitTime from deallogs lg where lg.addin = pp.PrePayId and dealtype = 3 and state = 1) PrePaySubmitTime,
		pp.Amount, 
		(select top 1 lg.SubmitTime from deallogs lg where lg.addin = pp.PrePayId and dealtype = 4 and state = 1) DoneSubmitTime,
		(select top 1 lg.Amount from deallogs lg where lg.addin = pp.PrePayId and dealtype = 4 and state = 1) DoneAmount
	from prepays pp left join accounts acc on pp.accountId = acc.accountId
		where (@accountShopId is null or acc.shopid = @accountShopId)
	) t
where 
	(@start is null or PrePaySubmitTime >= @start) and (@end is null or PrePaySubmitTime < @end) 