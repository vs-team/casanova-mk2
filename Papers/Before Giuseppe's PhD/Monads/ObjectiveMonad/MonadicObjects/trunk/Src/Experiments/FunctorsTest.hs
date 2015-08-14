{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

type ObjectInternals = ( Integer, (()->((),Object)) )
data Object = Object ObjectInternals

mk_object cnt =
  Object(
      cnt,
      (\() -> ((),(mk_object (cnt+1))))
    )

x (Object (x, f)) = x
f (Object (x, f)) = f

this0 = mk_object 0
((),this1) = f this0 ()
((),this2) = f this1 ()

class RecMethod s_obj s where
  rec_method :: (a -> (b, s_obj)) -> (a -> (b, s))

instance RecMethod Object ObjectInternals where
  rec_method m =
    \x -> 
      let (y,Object k) = m x
      in (y,k)

--(<=) :: RecMethod s_obj s => Field a s -> a!n = (b -> (c,s)) -> Method 

data WR a = WR a
data RR a = RR a

class Reference r where
  get :: r a -> a

instance Reference WR where
  get (WR x) = x

instance Reference RR where
  get (RR x) = x

bind :: (Reference r1, Reference r2) => r1 a -> (a -> r2 b) -> r2 b
bind e k =
  let x = get e
  in k x