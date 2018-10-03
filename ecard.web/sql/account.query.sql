select t.* from accounts t left join Users u on t.OwnerId=u.UserId where
(@Name is null or t.Name = @Name)
and (@State is null or t.State = @State)
and (@ShopId is null or t.ShopId = @ShopId)
and (@AccountToken is null or t.AccountToken = @AccountToken)
and (@States is null or t.State  in (@States))
and (@Ids is null or t.AccountId in (@Ids))
and (@NameWith is null or t.Name like '%'+ @NameWith + '%') 
and (@AccountTypeId is null or t.AccountTypeId = @AccountTypeId) 
and (@IsMobileAvailable is null or 
	(@IsMobileAvailable = 1 and exists(select * from users u where t.ownerId = u.userId and u.IsMobileAvailable = 1)) or
	(@IsMobileAvailable = 0 and not exists(select * from users u where t.ownerId = u.userId and u.IsMobileAvailable = 1))
	) 
and	
(@Content is null or u.Mobile like '%'+@Content+ '%')

