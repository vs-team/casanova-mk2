data Result a = Exception | Value a
showS Exception = "Nothing"
showS (Value x) = "Value(" ++ show x ++ ")"

instance Show a => Show (Result a) where
	show t = showS t

instance Monad Result where
    return          = Value
    Exception >>= f = Exception
    (Value x) >>= f = f x

throw = Exception

main0 = 
	let v = Exception --Value("Ciao!")
	in do x <- v
	      return x



newtype StateTrans s a = ST(s -> (s,a))

instance Monad (StateTrans s) where
	(ST p) >>= k = ST(\s0 -> let (s1,x) = p s0 in let ST(k1) = k x in k1 s1)
	return x = ST(\s -> (s,x))

v1 = ST(\(x,y) -> ((x,y),x))
v2 = ST(\(x,y) -> ((x,y),y))
set1 v = ST(\(x,y) -> ((v,y),()))
set2 v = ST(\(x,y) -> ((x,v),()))

stm = 
	do x <- v1
	   y <- v2
	   set1 0
	   z <- v1
	   return (x+y*z)

main1 = let ST(p) = stm in let (_,res) = p (1,2) in res