select t.* from PosEndPoints t where 
(@State is null or t.State = @State)
and (@ShopId is null or t.ShopId = @ShopId) 
and (@Name is null or t.Name = @Name) 
and (@NameWith is null or t.Name Like '%' + @NameWith + '%' ) 