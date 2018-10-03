select t.* from users t where  1=1
and(@Discriminator is null or t.Discriminator = @Discriminator) 
and IsSale=1