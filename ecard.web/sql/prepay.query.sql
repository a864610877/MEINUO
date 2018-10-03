select t.* from prepays t where
(@state is null or t.state = @state)
and (@shopId is null or t.shopId = @shopId)
and (@AccountId is null or t.AccountId = @AccountId )