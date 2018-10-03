select t.* from accounts t where
	t.Name = @accountName and t.accountToken = @accountToken and 
	t.state in (1, 2)