select t.* from users t where 
(@ShopIds is null or t.ShopId in (@ShopIds))
and (@State is null or t.State = @State) 
and (@shopRole is null or t.shopRole = @shopRole) 