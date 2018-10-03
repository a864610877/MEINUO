select t.* from accounts t where
(t.Name = @Name 
or exists(select * from users u where t.OwnerId = u.UserId and u.Mobile = @OwnerMobile)
or exists(select * from users u where t.OwnerId = u.UserId and u.DisplayName = @OwnerDisplayName))
and State in (@states)